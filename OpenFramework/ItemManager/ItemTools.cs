// --------------------------------
// <copyright file="ItemTools.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager
{
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using OpenFramework.ItemManager;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System;

    public static class ItemTools
    {
        public static string SelectQueryBasic(ItemBuilder item)
        {
            if (item == null)
            {
                return string.Empty;
            }

            StringBuilder query = new StringBuilder("SELECT Id,CompanyId,");
            foreach (ItemField field in item.Definition.Fields.Where(f => f.DataType != FieldDataType.Image))
            {
                if (!item.Definition.ForeignValues.Any(l => !string.IsNullOrEmpty(l.LinkField) && l.LocalName == field.Name))
                {
                    query.AppendFormat(CultureInfo.InvariantCulture, @"[{0}],", field.Name);
                }
            }

            query.AppendFormat("Active FROM ITEM_{0} WITH(NOLOCK)", item.ItemName);
            return query.ToString();
        }

        public static string SelectQueryAdapter(ItemBuilder item)
        {
            if (item == null)
            {
                return string.Empty;
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"EXEC {0}",
                item.Definition.DataAdapter.GetAll.StoredName);
        }

        public static string SelectQuery(ItemBuilder item)
        {
            if (item == null)
            {
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(item.Definition.DataAdapter.GetAll.StoredName))
            {
                return ItemTools.SelectQueryAdapter(item);
            }
            else
            {
                return ItemTools.SelectQueryBasic(item);
            }
        }

        public static ReadOnlyCollection<ItemField> FieldsDescription(ItemBuilder item)
        {
            List<ItemField> res = new List<ItemField>();
            if (item.Definition.Layout.Description != null)
            {
                foreach (ItemDescriptionField field in item.Definition.Layout.Description.Fields)
                {
                    foreach(var fieldData in item.Definition.Fields)
                    {
                        if(fieldData.Name == field.Name)
                        {
                            res.Add(fieldData);
                            break;
                        }
                    }
                }
            }

            return new ReadOnlyCollection<ItemField>(res);
            //return new ReadOnlyCollection<ItemField>(item.Definition.Fields.Where(f => item.Definition.Layout.Description.Fields.Select(a => a.Name).Contains(f.Name) == true).ToList());
        }

        public static ReadOnlyCollection<ItemField> FieldForeingLines(ItemBuilder item)
        {
            List<ItemField> fields = item.Definition.Fields.Where(i => item.Definition.FKList.Fields.Contains(i.Name)).ToList();
            return new ReadOnlyCollection<ItemField>(fields.Where(f => FieldsDescription(item).Select(a => a.Name).ToList().Contains(f.Name) == false).ToList());
        }

        public static string QueryFKList(ItemBuilder item)
        {
            var descriptionFields = FieldsDescription(item);
            var fieldLines = FieldForeingLines(item);
            var res = new StringBuilder("");

            if (item.Definition.Fields != null)
            {
                foreach (var field in fieldLines)
                {
                    res.Append(field.SqlFieldExtractor);
                    res.Append(" +");
                }
            }

            var descriptionFieldLine = item.Definition.Layout.Description.Pattern;
            var descriptionFieldList = new List<string>();
            foreach (var field in descriptionFields)
            {
                descriptionFieldList.Add(field.SqlFieldExtractorValue);
            }

            var nfield = item.Definition.Layout.Description.Pattern.ToCharArray().Count(c => c.Equals('{'));
            for (var x = 0; x < nfield; x++)
            {
                descriptionFieldLine = descriptionFieldLine.Replace("{" + x.ToString() + "}", "#" + x.ToString() + " ");
            }

            var parts = descriptionFieldLine.Split(' ');
            descriptionFieldLine = string.Empty;
            foreach(var part in parts)
            {
                if (part.StartsWith("#"))
                {
                    descriptionFieldLine += " + " + part;
                }
                else
                {
                    descriptionFieldLine += " + '" + part + "'";
                }
            }

            for (var x = 0; x < nfield; x++)
            {
                descriptionFieldLine = descriptionFieldLine.Replace("#" + x.ToString(), descriptionFieldList[x]);
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"SELECT 
		              '{{""Id"":' +  CAST(Item.Id AS  nvarchar(20)) + ',' +
                      '""Description"":""' {1} + '"",' + 
                      {2}
                      '""Active"":' + CASE WHEN Item.Active = 0 THEN 'false' ELSE 'true' END + '}}' AS Data
                      FROM Item_{0} Item WITH(NOLOCK)",
                item.ItemName,
                descriptionFieldLine,
                res);
        }

        public static string QueryByListId(ItemBuilder item, string listId)
        {
            var list = item.Definition.Lists.Where(l => l.Id.Equals(listId, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if(list == null)
            {
                return QueryGetAll(item);
            }

            var res = new StringBuilder("");
            var innerJoin = new StringBuilder("");
            var joinCount = 1;

            foreach (var column in list.Columns)
            {
                var field = item.Definition.Fields.Where(f => f.Name.Equals(column.DataProperty, StringComparison.OrdinalIgnoreCase)).First();
                var referedField = ItemField.GetReferedField(item, field);

                var queryField = string.Empty;
                if (referedField != null)
                {
                    var joinClause = string.Format(
                        CultureInfo.InvariantCulture,
                        @"LEFT JOIN Item_{0} J{1} WITH(NOLOCK) ON J{1}.Id = Item.{0}Id{2}",
                        referedField.ItemName,
                        joinCount,
                        Environment.NewLine);
                    innerJoin.Append(joinClause);

                    var referedFieldQuery = referedField.SqlFieldExtractor.Replace("Item.", "J" + joinCount.ToString() + ".");

                    if (!string.IsNullOrEmpty(column.ReplacedBy))
                    {
                        referedFieldQuery = referedFieldQuery.Replace(@"""" + referedField.Name + @""":", @"""" + column.ReplacedBy + @""":");
                    }

                    res.Append(referedFieldQuery);
                    joinCount++;
                }
                else
                {
                    if (string.IsNullOrEmpty(column.ReplacedBy))
                    {
                        res.Append(field.SqlFieldExtractor);
                    }
                    else
                    {
                        res.Append(field.SqlFieldExtractorReplace(column.ReplacedBy));
                    }
                }

                res.Append(" +");
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"SELECT 
		              '""Id"":' +  CAST(Item.Id AS  nvarchar(20)) + ',' +
		              {1}
                      '""Active"":' + CASE WHEN Item.Active = 0 THEN 'false' ELSE 'true' END
                      FROM Item_{0} Item WITH(NOLOCK){2}",
                item.ItemName,
                res,
                innerJoin);
        }

        public static string QueryGetAll(ItemBuilder item)
        {
            var descriptionFields = FieldsDescription(item);
            var res = new StringBuilder("");

            foreach (var field in item.Definition.Fields)
            {
                res.Append(field.SqlFieldExtractor);
                res.Append(" +");
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"SELECT 
		              {1}
                      '""Active"":' + CASE WHEN Item.Active = 0 THEN 'false' ELSE 'true' END
                      FROM Item_{0} Item WITH(NOLOCK)",
                item.ItemName,
                res);
        }

        public static string QueryById(ItemBuilder item, long id)
        {
            var descriptionFields = FieldsDescription(item);
            var res = new StringBuilder("");

            foreach(var field in item.Definition.Fields)
            {
                res.Append(field.SqlFieldExtractor);
                res.Append(" +");
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"SELECT 
		              {2}
                      '""Active"":' + CASE WHEN Item.Active = 0 THEN 'false' ELSE 'true' END
                      FROM Item_{0} Item WITH(NOLOCK) WHERE Id = {1}",
                item.ItemName,
                id,
                res);
        }
    }
}
