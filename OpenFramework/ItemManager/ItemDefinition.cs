// --------------------------------
// <copyright file="ItemDefinition.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Newtonsoft.Json;
    using OpenFramework.Core;
    using OpenFramework.CRUD;
    using OpenFramework.ItemManager.Form;
    using OpenFramework.ItemManager.List;

    /// <summary>Implements item definition</summary>
    [Serializable]
    public sealed class ItemDefinition
    {
        /// <summary>Indicates if item is master</summary>
        [JsonProperty("MasterTable")]
        private string masterTable;

        /// <summary>Foreign keys of item</summary>
        [JsonProperty("ForeignValues")]
        private ForeignList[] foreignValues;

        [JsonProperty("NeedFK")]
        private string[] needFK;

        [JsonProperty("Removes")]
        private string[] removes;

        [JsonProperty("GeoLocation")]
        private string geoLocation;

        /// <summary>Primary keys of item</summary>
        [JsonProperty("PrimaryKeys")]
        private PrimaryKey[] primaryKeys;

        /// <summary>Parent item</summary>
        [JsonProperty("ParentItem")]
        private string parentItem;

        [JsonProperty("InheritedItems")]
        private List<string> inheritedItems;

        /// <summary>Mapping of database fields</summary>
        [JsonProperty("SqlMappings")]
        private SqlMapping[] sqlMappings;

        /// <summary>Gets the fields</summary>
        [JsonProperty("Fields")]
        private List<ItemField> fields;

        /// <summary>Gets the list</summary>
        [JsonProperty("Lists")]
        private List<ListDefinition> lists;

        /// <summary>Gets the forms for the item</summary>
        [JsonProperty("Forms")]
        private List<FormDefinition> forms;

        /// <summary>Item data adapter for SQL server</summary>
        [JsonProperty("DataAdapter")]
        private DataAdapter dataAdapter;

        /// <summary>Congiruation of foreing list of item</summary>
        [JsonProperty("FKList")]
        private FKList fklist;

        /// <summary>Special validation rules</summary>
        [JsonProperty("SpecialRules")]
        private List<ItemFieldRules> itemRules;

        /// <summary>Gets an empty definition</summary>
        [JsonIgnore]
        public static ItemDefinition Empty
        {
            get
            {
                return new ItemDefinition()
                {
                    fields = new List<ItemField>(),
                    foreignValues = null,
                    ItemName = string.Empty,
                    lists = new List<ListDefinition>(),
                    Layout = ItemDefinitionLayout.Empty
                };
            }
        }

        [JsonIgnore]
        public ReadOnlyCollection<string> Removes
        {
            get
            {
                if (this.removes == null)
                {
                    return new ReadOnlyCollection<string>(new List<string>());
                }

                return new ReadOnlyCollection<string>(this.removes.ToList());
            }
        }

        [JsonIgnore]
        public ReadOnlyCollection<string> NeedFK
        {
            get
            {
                if (this.needFK == null)
                {
                    return new ReadOnlyCollection<string>(new List<string>());
                }

                return new ReadOnlyCollection<string>(this.needFK.ToList());
            }
        }

        /// <summary> Gets a value indicating whether if item supports geolocation</summary>
        [JsonIgnore]
        public bool GeoLocation
        {
            get
            {
                if (string.IsNullOrEmpty(this.geoLocation))
                {
                    return false;
                }

                return this.geoLocation.Equals("true", StringComparison.OrdinalIgnoreCase);
            }
        }

        /// <summary> Gets or sets a value indicating whether if item is master</summary>
        [JsonProperty("MustReload")]
        public bool MustReload { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if item is master
        /// </summary>
        [JsonProperty("ListId")]
        public string ListId { get; set; }

        /// <summary>
        /// Gets a value indicating whether if item is master
        /// </summary>
        [JsonIgnore]
        public bool MasterTable
        {
            get
            {
                if (string.IsNullOrEmpty(this.masterTable))
                {
                    return false;
                }

                return this.masterTable.Equals("1", StringComparison.OrdinalIgnoreCase);
            }
        }

        /// <summary>Gets or sets the JSON structure of item definition</summary>
        public string Json { get; set; }

        /// <summary>Gets or sets the item name</summary>
        public string ItemName { get; set; }

        /// <summary>Gets the lists to show items</summary>
        [JsonIgnore]
        public ReadOnlyCollection<ListDefinition> Lists
        {
            get
            {
                if (this.lists == null)
                {
                    return new ReadOnlyCollection<ListDefinition>(new List<ListDefinition>());
                }

                return new ReadOnlyCollection<ListDefinition>(this.lists);
            }
        }

        /// <summary>Gets the forms to edit items</summary>
        [JsonIgnore]
        public ReadOnlyCollection<FormDefinition> Forms
        {
            get
            {
                if (this.forms == null)
                {
                    return new ReadOnlyCollection<FormDefinition>(new List<FormDefinition>());
                }

                return new ReadOnlyCollection<FormDefinition>(this.forms);
            }
        }

        /// <summary>Gets the mapping of database fields</summary>
        [JsonIgnore]
        public ReadOnlyCollection<SqlMapping> SqlMappings
        {
            get
            {
                if (this.sqlMappings == null)
                {
                    return new ReadOnlyCollection<SqlMapping>(new List<SqlMapping>());
                }

                return new ReadOnlyCollection<SqlMapping>(this.sqlMappings);
            }
        }

        /// <summary>Gets or sets the layout of item definition</summary>
        [JsonProperty("Layout")]
        public ItemDefinitionLayout Layout { get; set; }

        /// <summary>Gets parent item</summary>
        [JsonIgnore]
        public string ParentItem
        {
            get
            {
                if (string.IsNullOrEmpty(this.parentItem))
                {
                    return string.Empty;
                }

                return this.parentItem;
            }
        }

        [JsonIgnore]
        public ReadOnlyCollection<string> InheritedItems
        {
            get
            {
                if (this.inheritedItems == null)
                {
                    return new ReadOnlyCollection<string>(new List<string>());
                }

                if (this.inheritedItems.Count == 0)
                {
                    return new ReadOnlyCollection<string>(new List<string>());
                }

                return new ReadOnlyCollection<string>(this.inheritedItems);
            }
        }

        /// <summary>Gets the foreign values</summary>
        [JsonIgnore]
        public ReadOnlyCollection<ForeignList> ForeignValues
        {
            get
            {
                if (this.foreignValues == null)
                {
                    return new ReadOnlyCollection<ForeignList>(new List<ForeignList>());
                }

                return new ReadOnlyCollection<ForeignList>(this.foreignValues);
            }
        }

        /// <summary>Gets the primary keys</summary>
        [JsonIgnore]
        public ReadOnlyDictionary<string, ReadOnlyCollection<string>> PrimaryKeys
        {
            get
            {
                Dictionary<string, ReadOnlyCollection<string>> res = new Dictionary<string, ReadOnlyCollection<string>>();
                if (this.primaryKeys != null)
                {
                    foreach (PrimaryKey pk in this.primaryKeys)
                    {
                        res.Add(pk.Id, pk.ItemFields);
                    }
                }

                return new ReadOnlyDictionary<string, ReadOnlyCollection<string>>(res);
            }
        }

        [JsonIgnore]
        public ReadOnlyCollection<string> PrimaryFields
        {
            get
            {
                if (this.PrimaryKeys == null)
                {
                    return new ReadOnlyCollection<string>(new List<string>());
                }

                List<string> res = new List<string>();
                foreach (ReadOnlyCollection<string> fields in this.PrimaryKeys.Values)
                {
                    foreach (string fieldName in fields)
                    {
                        res.Add(fieldName);
                    }
                }

                return new ReadOnlyCollection<string>(res);
            }
        }

        /// <summary>Gets the name of the foreign keys</summary>
        [JsonIgnore]
        public ReadOnlyCollection<string> ForeignListNames
        {
            get
            {
                List<string> res = new List<string>();
                if (this.foreignValues != null)
                {
                    foreach (ForeignList foreigList in this.foreignValues)
                    {
                        if (!res.Contains(foreigList.ItemName))
                        {
                            res.Add(foreigList.ItemName);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(foreigList.RemoteItem) && !res.Contains(foreigList.RemoteItem))
                            {
                                res.Add(foreigList.RemoteItem);
                            }
                        }
                    }
                }

                return new ReadOnlyCollection<string>(res);
            }
        }

        /// <summary>Gets the item data adapter for SQL server</summary>
        [JsonIgnore]
        public DataAdapter DataAdapter
        {
            get
            {
                if (this.dataAdapter == null)
                {
                    return DataAdapter.Empty;
                }

                return this.dataAdapter;
            }
        }

        /// <summary>Gets configuration of foreign list of item</summary>
        [JsonIgnore]
        public FKList FKList
        {
            get
            {
                if (this.fklist == null)
                {
                    return FKList.Empty;
                }

                return this.fklist;
            }
        }

        /// <summary>Gets the fields</summary>
        [JsonIgnore]
        public ReadOnlyCollection<ItemField> Fields
        {
            get
            {
                if (this.fields == null)
                {
                    return new ReadOnlyCollection<ItemField>(new List<ItemField>());
                }

                return new ReadOnlyCollection<ItemField>(this.fields);
            }
        }

        /// <summary>Gets the validation rules for item data</summary>
        [JsonIgnore]
        public ReadOnlyCollection<ItemFieldRules> ItemRules
        {
            get
            {
                if (this.itemRules == null)
                {
                    return new ReadOnlyCollection<ItemFieldRules>(new List<ItemFieldRules>());
                }

                return new ReadOnlyCollection<ItemFieldRules>(this.itemRules);
            }
        }

        public ReadOnlyCollection<ForeignList> ImportReferences
        {
            get
            {
                List<ForeignList> res = new List<ForeignList>();
                foreach (ForeignList fl in this.ForeignValues)
                {
                    if (!string.IsNullOrEmpty(fl.ImportReference))
                    {
                        if (!fl.ImportReference.Equals(fl.FieldRetrieved, StringComparison.OrdinalIgnoreCase))
                        {
                            res.Add(fl);
                        }
                    }
                }

                return new ReadOnlyCollection<ForeignList>(res);
            }
        }

        /* <summary>Load a item definition from file</summary>
        /// <param name="itemName">Item name</param>
        /// <returns>Item definition for item</returns>
        public static ItemDefinition Load(string instanceName, string itemName)
        {
            CustomerFramework customer = InstanceMode.Instance;// HttpContext.Current.Session["FrameworkCustomer"] as CustomerFramework;
            return Load(instanceName, itemName);
        }*/

        /// <summary>Load a item definition from file</summary>
        /// <param name="itemName">Item name</param>
        /// <param name="instanceName">Instance name</param>
        /// <returns>Item definition for item</returns>
        public static ItemDefinition Load(string itemName, string instanceName)
        {
            if (string.IsNullOrEmpty(itemName) || string.IsNullOrEmpty(instanceName))
            {
                return ItemDefinition.Empty;
            }

            string path = ItemBuilder.GetPathDefinition(itemName, instanceName);
            string jsonDefinition = string.Empty;

            using (StreamReader input = new StreamReader(path))
            {
                jsonDefinition = input.ReadToEnd();
            }

            if (!string.IsNullOrEmpty(jsonDefinition))
            {
                ItemDefinition res = ItemDefinition.Empty;
                try
                {
                    res = JsonConvert.DeserializeObject<ItemDefinition>(jsonDefinition);
                    foreach (ItemField field in res.fields)
                    {
                        field.ItemName = itemName.ToUpperInvariant();
                    }

                    string path2 = HttpContext.Current.Request.PhysicalApplicationPath;
                    if (!path2.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
                    {
                        path2 = string.Format(CultureInfo.InvariantCulture, @"{0}\", path2);
                    }

                    res.Json = jsonDefinition;
                    string templateFileName = string.Format(CultureInfo.InvariantCulture, @"{0}js\Template.js", path2);
                    string destinationFileName = string.Format(
                        CultureInfo.InvariantCulture,
                        @"{0}CustomersFramework\{1}\Scripts\{2}.js",
                        path2,
                        instanceName,
                        itemName);

                    if (!File.Exists(destinationFileName))
                    {
                        File.Copy(templateFileName, destinationFileName);
                    }
                }
                catch (IOException ex)
                {
                    ExceptionManager.LogException(ex as Exception, string.Format(CultureInfo.InvariantCulture, "IOException -- ItemDefinition::Load({0})", itemName));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.LogException(ex as Exception, string.Format(CultureInfo.InvariantCulture, "NullReferenceException -- ItemDefinition::Load({0})", itemName));
                }
                catch (JsonException ex)
                {
                    ExceptionManager.LogException(ex as Exception, string.Format(CultureInfo.InvariantCulture, "JsonException -- ItemDefinition::Load({0})", itemName));
                }
                catch (NotSupportedException ex)
                {
                    ExceptionManager.LogException(ex as Exception, string.Format(CultureInfo.InvariantCulture, "NotSupportedException -- ItemDefinition::Load({0})", itemName));
                }

                return res;
            }

            return null;
        }

        /// <summary>Load a item definition from file</summary>
        /// <param name="itemName">Item name</param>
        /// <param name="instanceName">Instance name</param>
        /// <returns>Item definition for item</returns>
        public static ItemDefinition OldLoad(string itemName, string instanceName)
        {
            if (string.IsNullOrEmpty(itemName))
            {
                return ItemDefinition.Empty;
            }

            string path = ItemBuilder.GetPathDefinition(itemName, instanceName);
            string jsonDefinition = string.Empty;

            using (StreamReader input = new StreamReader(path))
            {
                jsonDefinition = input.ReadToEnd();
            }

            if (!string.IsNullOrEmpty(jsonDefinition))
            {
                ItemDefinition res = ItemDefinition.Empty;
                try
                {
                    res = JsonConvert.DeserializeObject<ItemDefinition>(jsonDefinition);

                    foreach (ItemField field in res.fields)
                    {
                        field.ItemName = itemName.ToUpperInvariant();
                    }

                    res.Json = jsonDefinition;
                    string customJavaScriptPath = ConfigurationManager.AppSettings["ItemsDefinitionPath"].ToString();
                    customJavaScriptPath = string.Format(CultureInfo.InvariantCulture, customJavaScriptPath, instanceName).Replace("ItemDefinition", "Scripts");
                    string sourceFileName = @"C:\GESE-AIO\GESE\Web\js\Template.js";
                    string targetFileName = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}.js", customJavaScriptPath, itemName);
                    /*if (!File.Exists(@"C:\GESE\Web\CustomersFramework\" + instanceName + "\\Scripts\\" + itemName + ".js"))
                    {
                        File.Copy(@"C:\GESE\Web\js\Template.js", @"C:\GESE\Web\CustomersFramework\" + instanceName + "\\Scripts\\" + itemName + ".js");
                    }*/
                    if (!File.Exists(targetFileName))
                    {
                        File.Copy(sourceFileName, targetFileName);
                    }
                }
                catch (IOException ex)
                {
                    ExceptionManager.LogException(ex as Exception, string.Format(CultureInfo.InvariantCulture, "IOException -- ItemDefinition::Load({0})", itemName));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.LogException(ex as Exception, string.Format(CultureInfo.InvariantCulture, "NullReferenceException -- ItemDefinition::Load({0})", itemName));
                }
                catch (JsonException ex)
                {
                    ExceptionManager.LogException(ex as Exception, string.Format(CultureInfo.InvariantCulture, "JsonException -- ItemDefinition::Load({0})", itemName));
                }
                catch (NotSupportedException ex)
                {
                    ExceptionManager.LogException(ex as Exception, string.Format(CultureInfo.InvariantCulture, "NotSupportedException -- ItemDefinition::Load({0})", itemName));
                }

                return res;
            }

            return null;
        }

        /// <summary>Load a item definition from file</summary>
        /// <param name="fileName">Item's name</param>
        /// <returns>Item definition for item</returns>
        public static ItemDefinition LoadFromFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return ItemDefinition.Empty;
            }

            string jsonDefinition = string.Empty;
            using (StreamReader input = new StreamReader(fileName))
            {
                jsonDefinition = input.ReadToEnd();
            }

            // Extract item name from file name
            string[] path = fileName.Split('\\');
            string itemName = path[path.Length - 1].ToUpperInvariant().Replace(".ITEM", string.Empty);
            if (!string.IsNullOrEmpty(jsonDefinition))
            {
                ItemDefinition res = ItemDefinition.Empty;
                try
                {
                    res = JsonConvert.DeserializeObject<ItemDefinition>(jsonDefinition);

                    foreach (ItemField field in res.fields)
                    {
                        field.ItemName = itemName;
                    }

                    res.Json = jsonDefinition;
                }
                catch (IOException ex)
                {
                    ExceptionManager.LogException(ex as Exception, string.Format(CultureInfo.InvariantCulture, "IOException -- ItemDefinition::Load({0})", fileName));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.LogException(ex as Exception, string.Format(CultureInfo.InvariantCulture, "NullReferenceException -- ItemDefinition::Load({0})", fileName));
                }
                catch (JsonException ex)
                {
                    ExceptionManager.LogException(ex as Exception, string.Format(CultureInfo.InvariantCulture, "JsonException -- ItemDefinition::Load({0})", fileName));
                }
                catch (NotSupportedException ex)
                {
                    ExceptionManager.LogException(ex as Exception, string.Format(CultureInfo.InvariantCulture, "NotSupportedException -- ItemDefinition::Load({0})", fileName));
                }

                return res;
            }

            return null;
        }

        public bool LinkedField(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                return false;
            }

            if (this.foreignValues == null || this.foreignValues.Length == 0)
            {
                return false;
            }

            foreach (ForeignList foreignList in this.foreignValues.Where(fl => !string.IsNullOrEmpty(fl.LinkField)))
            {
                if (foreignList.LocalName.Equals(fieldName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}