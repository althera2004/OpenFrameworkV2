// --------------------------------
// <copyright file="SqlServerWrapper.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.CRUD
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using OpenFramework.ItemManager;

    /// <summary>Generates SQL sentences for insert and update items into database</summary>
    public static class SqlServerWrapper
    {
        /// <summary>Deactivates an item</summary>
        /// <param name="itemId">Item identifier</param>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <param name="itemName">Item name</param>
        /// <param name="instanceName">Name of actual instance</param>
        /// <returns>Deactivate query for item</returns>
        public static string InactiveQuery(long itemId, long applicationUserId, string itemName, string instanceName)
        {
            string res = string.Empty;
            ItemDefinition definition = ItemDefinition.Load(itemName, instanceName);
            if (!string.IsNullOrEmpty(definition.DataAdapter.Inactive.StoredName))
            {
                res = string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    "{0} @Id = {1}, @ModifiedBy = {2}", 
                    definition.DataAdapter.Inactive.StoredName, 
                    itemId, 
                    applicationUserId);
            } 
            else
            {
                res = DefaultInactiveQuery(itemId, applicationUserId, itemName);
            }

            return res;
        }

        /// <summary>Delete an item</summary>
        /// <param name="itemId">Item identifier</param>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <param name="itemName">Item name</param>
        /// <param name="instanceName">Name of actual instance</param>
        /// <returns>Deactivate query for item</returns>
        public static string DeleteQuery(long itemId, long applicationUserId, string itemName, string instanceName)
        {
            string res = string.Empty;
            ItemDefinition definition = ItemDefinition.Load(itemName, instanceName);
            if (!string.IsNullOrEmpty(definition.DataAdapter.Inactive.StoredName))
            {
                res = string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    "{0} @Id = {1}, @ModifiedBy = {2}",
                    definition.DataAdapter.Inactive.StoredName,
                    itemId,
                    applicationUserId);
            }
            else
            {
                res = DefaultDeleteQuery(itemId, itemName);
            }

            return res;
        }

        /// <summary>Creates update query for item</summary>
        /// <param name="item">Item instance</param>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <returns>Update query for item</returns>
        public static string UpdateQuery(ItemBuilder item, long applicationUserId)
        {
            if (item == null)
            {
                return string.Empty;
            }

            string res = string.Empty;
            if (!string.IsNullOrEmpty(item.Definition.DataAdapter.Update.StoredName))
            {
                res = DataAdapterUpdateQuery(item, applicationUserId);
            }
            else
            {
                res = DefaultUpdateQuery(item, applicationUserId);
            }

            return res;
        }

        /// <summary>Creates insert query for item</summary>
        /// <param name="item">Item instance</param>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <returns>Insert query for item</returns>
        public static string InsertQuery(ItemBuilder item, long applicationUserId)
        {
            if (item == null)
            {
                return string.Empty;
            }

            string res = string.Empty;
            if (!string.IsNullOrEmpty(item.Definition.DataAdapter.Insert.StoredName))
            {
                res = DataAdapterInsertQuery(item, applicationUserId);
            }
            else
            {
                res = DefaultInsertQuery(item, applicationUserId);
            }

            return res;
        }

        /// <summary>Creates activate for item</summary>
        /// <param name="itemId">Item identifier</param>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <param name="itemName">Item Name</param>
        /// <param name="instanceName">Name of actual instance</param>
        /// <returns>Activate query for item</returns>
        public static string ActiveQuery(long itemId, long applicationUserId, string itemName, string instanceName)
        {
            string res = string.Empty;
            ItemDefinition definition = ItemDefinition.Load(itemName, instanceName);
            if (!string.IsNullOrEmpty(definition.DataAdapter.Active.StoredName))
            {
                res = string.Format(
                    CultureInfo.GetCultureInfo("en-us"), 
                    "{0} @Id = {1}", 
                    definition.DataAdapter.Active.StoredName,
                    itemId);
            }
            else
            {
                res = DefaultActiveQuery(itemId, applicationUserId, itemName);
            }

            return res;
        }

        /// <summary>Creates default (all fields) insert query for item</summary>
        /// <param name="item">Item instance</param>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <returns>Insert query for item</returns>
        private static string DefaultInsertQuery(ItemBuilder item, long applicationUserId)
        {
            StringBuilder fields = new StringBuilder();
            StringBuilder values = new StringBuilder();
            bool first = true;

            foreach (ItemField field in item.Definition.Fields.Where(f => f.Referencial != true))
            {
                /*if (field.Name != "Id" && string.IsNullOrEmpty(foreignData.LinkField))
                {
                    string realField = string.Empty;
                    if (item.Definition.SqlMappings.Any(m => m.ItemField == field.Name))
                    {
                        SqlMapping mapping = item.Definition.SqlMappings.Where(m => m.ItemField == field.Name).First();
                        realField = mapping.TableField;
                    }
                    else
                    {
                        //realField = GetRealField(item, field.Name);
                    }

                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        fields.Append(",");
                        values.Append(",");
                    }

                    fields.AppendFormat(@"[{0}]", realField);
                    values.AppendFormat(@"{0}", SqlValue.Value(field, item[field.Name]));
                }*/
            }

            fields.Append(InsertQueryFieldsEnd());
            values.Append(InsertQueryValuesEnd(applicationUserId));

            // TODO: Pasar usuario a CreatedBy
            StringBuilder res = new StringBuilder("INSERT INTO Item_").Append(item.ItemName);
            res.Append("(");
            res.Append(fields);
            res.Append(") VALUES(");
            res.Append(values);
            res.Append("); SELECT SCOPE_IDENTITY();");
            return res.ToString().Replace("\"\"", string.Empty);
        }

        /// <summary>Creates custom (defined in item) insert query for item</summary>
        /// <param name="item">Item instance</param>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <returns>Insert query for item</returns>
        private static string DataAdapterInsertQuery(ItemBuilder item, long applicationUserId)
        {
            if (item.Definition.DataAdapter.Insert.StoredName == "#_insert")
            {
                StringBuilder fields = new StringBuilder();
                StringBuilder values = new StringBuilder();
                bool first = true;

                foreach (StoredParameter param in item.Definition.DataAdapter.Insert.Parameters)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        fields.Append(","); 
                        values.Append(",");
                    }

                    fields.AppendFormat(CultureInfo.GetCultureInfo("en-us"), "[{0}]", param.Parameter);
                    values.Append(SqlValue.Value(item.Definition.Fields.Where(f => f.Name == param.Field).First(), item[param.Field]));
                }

                fields.Append(InsertQueryFieldsEnd());
                values.Append(InsertQueryValuesEnd(applicationUserId));
                StringBuilder res = new StringBuilder().AppendFormat("INSERT INTO Item_{0} ({1}) VALUES({2})", item.ItemName, fields.ToString(), values.ToString());
                return res.ToString();
            }
            else
            {
                StringBuilder values = new StringBuilder();

                foreach (StoredParameter param in item.Definition.DataAdapter.Insert.Parameters)
                {
                    values.AppendFormat(CultureInfo.GetCultureInfo("en-us"), "@{0} = {1},", param.Parameter, SqlValue.Value(item.Definition.Fields.Where(f => f.Name == param.Field).First(), item[param.Field]));
                }

                values.AppendFormat("@CreatedBy = {0}, @ModifiedBy = {0}", applicationUserId);
                StringBuilder res = new StringBuilder();
                res.AppendFormat(CultureInfo.GetCultureInfo("en-us"), "EXEC [{0}] {1}", item.Definition.DataAdapter.Insert.StoredName, values);
                return res.ToString();
            }
        }

        /// <summary>Creates default (all fields) update query for item</summary>
        /// <param name="item">Item instance</param>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <returns>Update query for item</returns>
        private static string DefaultUpdateQuery(ItemBuilder item, long applicationUserId)
        {
            StringBuilder values = new StringBuilder();
            bool first = true;

            foreach (ItemField field in item.Definition.Fields)
            {
                /*ForeignList foreignData = item.ForeignListName(field.Name);

                if (field.Name != "Id" && string.IsNullOrEmpty(foreignData.LinkField))
                {
                    string realField = string.Empty;
                    if (item.Definition.SqlMappings.Any(m => m.ItemField == field.Name))
                    {
                        SqlMapping mapping = item.Definition.SqlMappings.Where(m => m.ItemField == field.Name).First();
                        realField = mapping.TableField;
                    }
                    else
                    {
                        realField = GetRealField(item, field.Name);
                    }

                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        values.Append(",");
                    }

                    values.AppendFormat(@"[{0}] = {1}", realField, SqlValue.Value(field, item[field.Name]));
                }*/
            }

            values.Append(UpdateQueryValuesEnd(applicationUserId));

            StringBuilder res = new StringBuilder("UPDATE Item_").Append(item.ItemName);
            res.Append(" SET ");
            res.Append(values);
            res.AppendFormat(" WHERE Id = {0}", item["Id"]);
            return res.ToString().Replace("\"\"", string.Empty);
        }

        /// <summary>Creates custom (defined in item) update query for item</summary>
        /// <param name="item">Item instance</param>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <returns>Update query for item</returns>
        private static string DataAdapterUpdateQuery(ItemBuilder item, long applicationUserId)
        {
            if (item.Definition.DataAdapter.Update.StoredName == "#_update")
            {
                StringBuilder values = new StringBuilder();
                bool first = true;

                foreach (StoredParameter param in item.Definition.DataAdapter.Update.Parameters)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        values.Append(",");
                    }

                    values.AppendFormat(CultureInfo.GetCultureInfo("en-us"), "[{0}] = {1}", param.Parameter, SqlValue.Value(item.Definition.Fields.Where(f => f.Name == param.Field).First(), item[param.Field]));
                }

                values.Append(UpdateQueryValuesEnd(applicationUserId));
                StringBuilder res = new StringBuilder();
                res.AppendFormat(CultureInfo.GetCultureInfo("en-us"), "UPDATE Item_{0} SET{1} WHERE Id = {2}", item.ItemName, values, item["Id"]);
                return res.ToString();
            }
            else
            {
                StringBuilder values = new StringBuilder();
                bool first = true;
                foreach (StoredParameter param in item.Definition.DataAdapter.Update.Parameters)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        values.Append(",");
                    }

                    values.AppendFormat(CultureInfo.GetCultureInfo("en-us"), "@{0} = {1}", param.Parameter, SqlValue.Value(item.Definition.Fields.Where(f => f.Name == param.Field).First(), item[param.Field]));
                }

                values.AppendFormat(
                    CultureInfo.InvariantCulture,
                    ", @ModifiedBy = {0}",
                    applicationUserId);
                StringBuilder res = new StringBuilder();
                res.AppendFormat(CultureInfo.GetCultureInfo("en-us"), "EXEC [{0}] {1}", item.Definition.DataAdapter.Update.StoredName, values);
                return res.ToString();
            }
        }

        /// <summary>Creates default deactivation for item</summary>
        /// <param name="itemId">Item identifier</param>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <param name="itemName">Item name</param>
        /// <returns>Deactivate query for item</returns>
        private static string DefaultInactiveQuery(long itemId, long applicationUserId, string itemName)
        {
            StringBuilder res = new StringBuilder();
            res.AppendFormat(CultureInfo.GetCultureInfo("en-us"), "UPDATE Item_{0} SET Active = 0, ModifiedBy = {1}, ModifiedOn = GETDATE() WHERE Id = {2}", itemName, applicationUserId, itemId);
            return res.ToString();
        }

        /// <summary>Creates default deletion for item</summary>
        /// <param name="itemId">Item identifier</param>
        /// <param name="itemName">Item name</param>
        /// <returns>Deactivate query for item</returns>
        private static string DefaultDeleteQuery(long itemId, string itemName)
        {
            StringBuilder res = new StringBuilder();
            res.AppendFormat(CultureInfo.GetCultureInfo("en-us"), "DELETE FROM Item_{0} WHERE Id = {1}", itemName, itemId);
            return res.ToString();
        }

        /// <summary>Creates default activation for item</summary>
        /// <param name="itemId">Item Identifier</param>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <param name="itemName">Item name</param>
        /// <returns>Activate query for item</returns>
        private static string DefaultActiveQuery(long itemId, long applicationUserId, string itemName)
        {
            StringBuilder res = new StringBuilder();
            res.AppendFormat(CultureInfo.GetCultureInfo("en-us"), "UPDATE Item_{0} SET Active = 1, ModifiedBy = {1} WHERE Id = {2}", itemName, applicationUserId, itemId);
            return res.ToString();
        }

        /// <summary>Creates last fields for insert query</summary>
        /// <returns>String with last fields for insert query</returns>
        private static string InsertQueryFieldsEnd()
        {
            return ",[CompanyId],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn],[Active]";
        }

        /// <summary>Creates last values for insert query</summary>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <returns>String with the last values for insert query</returns>
        private static string InsertQueryValuesEnd(long applicationUserId)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                ",1,{0},GETDATE(),{0},GETDATE(),1",
                applicationUserId);
        }

        /// <summary>Creates last fields for update query</summary>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <returns>String with the last values for update query</returns>
        private static string UpdateQueryValuesEnd(long applicationUserId)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                ",[ModifiedBy] = {0}, [ModifiedOn] = GETDATE()",
                applicationUserId);
        }
    }
}