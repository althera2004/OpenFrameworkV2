// --------------------------------
// <copyright file="ItemBuilder.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Web;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using OpenFramework.Core;
    using OpenFramework.CRUD;
    using OpenFramework.ItemManager.Form;
    using OpenFramework.ItemManager.List;
    using OpenFramework.Customer;

    /// <summary>Implements ItemBuilder class based on ICollection class</summary>
    [Serializable]
    public sealed class ItemBuilder : IDictionary<string, object>, ICollection
    {
        public const bool NotResolveForeignKeys = false;
        public const bool NotFromImport = false;

        /// <summary>Values of item</summary>
        private Dictionary<string, object> source;

        /// <summary>Synchronization of item values</summary>
        private object syncRoot;

        /// <summary>List of primary keys</summary>
        private Dictionary<string, ReadOnlyCollection<PrimaryKey>> primaryKeyValue;

        /// <summary>Gets the name of instance</summary>
        public string InstanceName { get; private set; }

        /// <summary>Initializes a new instance of the ItemBuilder class</summary>
        /// <param name="itemName">Item name</param>
        /// <param name="definition">Item definition</param>
        /// <param name="instanceName">Instance Name</param>
        public ItemBuilder(string itemName, ItemDefinition definition, string instanceName)
        {
            this.ItemName = itemName;
            this.Definition = definition;
            this.primaryKeyValue = new Dictionary<string, ReadOnlyCollection<PrimaryKey>>();
            this.source = new Dictionary<string, object>();
            this.InstanceName = instanceName;
        }

        /// <summary>Initializes a new instance of the ItemBuilder class</summary>
        /// <param name="itemName">Item name</param>
        /// <param name="instanceName">Instance Name</param>
        public ItemBuilder(string itemName, string instanceName)
        {
            this.ItemName = itemName;
            this.Definition = ItemDefinition.Load(itemName, instanceName);
            this.primaryKeyValue = new Dictionary<string, ReadOnlyCollection<PrimaryKey>>();
            this.source = new Dictionary<string, object>();
            this.InstanceName = instanceName;
        }

        public ReadOnlyCollection<ItemField> HumanFields
        {
            get
            {
                var res = new List<ItemField>();
                foreach (var field in this.Definition.Fields)
                {
                    if (field.Name != "Id" && !this.Definition.LinkedField(field.Name))
                    {
                        res.Add(field);
                    }
                }

                return new ReadOnlyCollection<ItemField>(res);
            }
        }

        public static ReadOnlyCollection<ItemBuilder> GetActive(string itemName,string instanceName, string connectionString)
        {
            throw new NotImplementedException();   
        }

        public static string ItemsDefinitonPathExternal(string instanceName)
        {
            if (string.IsNullOrEmpty(instanceName))
            {
                return string.Empty;
            }

            string path = string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                ConfigurationManager.AppSettings["ItemsDefinitionPath"],
                instanceName);

            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}\", path);
            }
            else
            {
                path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}", path);
            }

            return path.ToUpperInvariant();
        }

        /*public static string ItemsDefinitonPath
        {
            get
            {
                //CustomerFramework customer = Basics.ActualInstance;
                //string path = string.Format(
                //CultureInfo.GetCultureInfo("en-us"),
                //ConfigurationManager.AppSettings["ItemsDefinitionPath"].ToString(),
                //customer.Name);

                //if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
                //{
                //    path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}\", path);
                //}
                //else
                //{
                //    path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}", path);
                //}

                //return path.ToUpperInvariant();
                return Basics.ActualInstance.DefinitionPath;
            }
        }*/

        public static string ItemsInstancePathExternal(string instanceName)
        {
            if (string.IsNullOrEmpty(instanceName))
            {
                return string.Empty;
            }

            var customer = CustomerFramework.Load(instanceName);
            if (customer == null)
            {
                return string.Empty;
            }

            string path = string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                ConfigurationManager.AppSettings["ItemsDefinitionPath"],
                customer.Name);

            path = path.ToUpperInvariant().Replace("ITEMDEFINITION", string.Empty);
            path = path.Replace(@"\\", @"\");

            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}\", path);
            }
            else
            {
                path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}", path);
            }

            return path.ToUpperInvariant();
        }

        public static string ItemsInstancePath(string instanceName)
        {
            string path = string.Format(
                CultureInfo.InvariantCulture,
                ConfigurationManager.AppSettings["ItemsDefinitionPath"],
                instanceName);

            path = path.ToUpperInvariant().Replace("ITEMDEFINITION", string.Empty);
            path = path.Replace(@"\\", @"\");

            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}\", path);
            }
            else
            {
                path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}", path);
            }

            return path.ToUpperInvariant();
        }

        public int Count
        {
            get { return this.source.Count; }
        }

        public ICollection<string> Keys
        {
            get { return this.source.Keys; }
        }

        public ICollection<object> Values
        {
            get { return this.source.Values; }
        }

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly
        {
            get { return false; }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get
            {
                if (this.syncRoot == null)
                {
                    if (this.source is ICollection collection)
                    {
                        this.syncRoot = collection.SyncRoot;
                    }
                    else
                    {
                        Interlocked.CompareExchange(ref this.syncRoot, new object(), null);
                    }
                }

                return this.syncRoot;
            }
        }

        /// <summary>Gets the path of item's definitions folder on server</summary>
        public string PathDefinition
        {
            get
            {
                string path = ConfigurationManager.AppSettings["ItemsDefinitionPath"];
                if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
                {
                    path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}\{1}.item", path, this.ItemName);
                }
                else
                {
                    path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}{1}.item", path, this.ItemName);
                }

                return path;
            }
        }

        /// <summary>Gets a value indicating whether an Item has to show Simple List</summary>
        public bool SimpleList
        {
            get
            {
                if (this.Definition.Lists == null)
                {
                    return true;
                }

                if (this.Definition.Lists.Count == 0)
                {
                    return true;
                }

                if (!this.Definition.Lists.Any(f => f.Columns.Count > 0))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>Gets a value indicating whether the item has lists</summary>
        public bool HasLists
        {
            get
            {
                if (this.Definition.Lists == null)
                {
                    return false;
                }

                if (this.Definition.Lists.Count == 0)
                {
                    return false;
                }

                if (!this.Definition.Lists.Any(f => f.Columns.Count > 0))
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>Gets a value indicating whether an Item has Custom Forms</summary>
        public bool HasForms
        {
            get
            {
                if (this.Definition.Forms == null)
                {
                    return false;
                }

                if (this.Definition.Forms.Count == 0)
                {
                    return false;
                }

                if (this.Definition.Forms.Any(f => f.Tabs.Count == 0))
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>Gets the ListDefinition of the first Custom list if it exists</summary>
        public ListDefinition ListCustom
        {
            get
            {
                return this.GetListById("CUSTOM");
            }
        }

        /// <summary>Gets the ListDefinition of the first InForm list if it exists</summary>
        public ListDefinition ListInForm
        {
            get
            {
                return this.GetListById("INFORM");
            }
        }

        /// <summary>Gets the FormDefinition of the first Popup form if it exists</summary>
        public FormDefinition FormPopup
        {
            get
            {
                return this.GetFormByType(EditMode.Popup);
            }
        }

        /// <summary>Gets the FormDefinition of the first Custom form if it exists</summary>
        public FormDefinition FormCustom
        {
            get
            {
                return this.GetFormByType(EditMode.Custom);
            }
        }

        /// <summary>Gets the FormDefinition of the first InForm form if it exists</summary>
        public FormDefinition FormInForm
        {
            get
            {
                return this.GetFormByType(EditMode.Inform);
            }
        }

        /// <summary>Gets a value indicating whether an Item has a Custom List</summary>
        /// <param name="listId">List identifier</param>
        /// <returns>A value indicating whether an Item has a Custom List</returns>
        public bool HasListById(string listId)
        {
            if (this.Definition.Lists == null)
            {
                return false;
            }

            if (this.Definition.Lists.Count == 0)
            {
                return false;
            }

            if (string.IsNullOrEmpty(listId))
            {
                return false;
            }

            return this.Definition.Lists.Any(l => l.Id.ToUpperInvariant() == listId.ToUpperInvariant());
        }

        /// <summary>Retrieves a list with matching Id (if exists)</summary>
        /// <param name="id">Identifier of the list</param>
        /// <returns>Definition of a list</returns>
        public ListDefinition GetListById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return ListDefinition.Empty;
            }

            if (this.Definition.Lists.Any(li => li.Id.ToUpperInvariant() == id.ToUpperInvariant()))
            {
                return this.Definition.Lists.First(li => li.Id.ToUpperInvariant() == id.ToUpperInvariant());
            }

            return ListDefinition.Empty;
        }

        /// <summary>Retrieves a form by identifier if exists</summary>
        /// <param name="id">Identifier of the form</param>
        /// <returns>Definition of a list</returns>
        public FormDefinition GetFormById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return FormDefinition.Empty;
            }

            if (this.Definition.Forms.Any(f => f.Id.ToUpperInvariant() == id.ToUpperInvariant()))
            {
                return this.Definition.Forms.First(f => f.Id.ToUpperInvariant() == id.ToUpperInvariant());
            }

            return FormDefinition.Empty;
        }

        /// <summary>Retrieves a form with matching type (the first if any exists)
        /// </summary>
        /// <param name="type">Type of the form</param>
        /// <returns>Definition of a form</returns>
        public FormDefinition GetFormByType(EditMode type)
        {
            if (this.Definition.Forms.Any(f => f.FormType == type))
            {
                return this.Definition.Forms.First(f => f.FormType == type);
            }

            return FormDefinition.Empty;
        }

        public string JsonList
        {
            get
            {
                this.Add("Status", "Unchanged");
                var res = new Dictionary<string, object>();
                foreach (KeyValuePair<string, object> itemData in this)
                {
                    if (itemData.Value == null)
                    {
                        res.Add(itemData.Key, string.Empty);
                    }
                    else
                    {
                        res.Add(itemData.Key, itemData.Value);
                    }
                }

                return JsonConvert.SerializeObject(res);
            }
        }

        /// <summary>Gets or sets the name of item</summary>
        public string ItemName { get; set; }

        /// <summary>Gets or sets the item definition of item</summary>
        public ItemDefinition Definition { get; set; }

        /// <summary>Gets the item identifier</summary>
        public long Id
        {
            get
            {
                // Coge de los values el que tenga la Key "Id"
                if (this.ContainsKey("Id"))
                {
                    return Convert.ToInt64(this["Id"].ToString(), CultureInfo.GetCultureInfo("en-us"));
                }

                return 0;
            }
        }

        /// <summary>Gets a value indicating whether item is active</summary>
        public bool Active
        {
            get
            {
                if (this.ContainsKey("Active"))
                {
                    return Convert.ToBoolean(this["Active"], CultureInfo.InvariantCulture);
                }

                return true;
            }
        }

        public static ItemBuilder FromJsonObject(string itemName, dynamic data, string instanceName)
        {
            var res = new ItemBuilder(itemName, instanceName);
            if (data != null)
            {
                var values = data.ToObject<Dictionary<string, object>>();
                foreach (var pair in values)
                {
                    var finalData = pair;
                    if (pair.Value == null)
                    {
                        if (res.Definition.SqlMappings.Any(s => s.ItemField == pair.Key))
                        {
                            finalData = new KeyValuePair<string, object>(pair.Key, res.Definition.SqlMappings.First(s => s.ItemField == pair.Key).DefaultValue);
                        }
                    }

                    res.Add(finalData);
                }
            }

            return res;
        }



        /// <summary>Gets the company identifier</summary>
        public long CompanyId
        {
            get
            {
                if (this.ContainsKey("CompanyId"))
                {
                    return Convert.ToInt64(this["CompanyId"], CultureInfo.InvariantCulture);
                }

                return 0;
            }
        }

        public ReadOnlyDictionary<string, ReadOnlyCollection<PrimaryKey>> PrimaryKeyValue
        {
            get
            {
                if (this.primaryKeyValue == null)
                {
                    this.primaryKeyValue = new Dictionary<string, ReadOnlyCollection<PrimaryKey>>();
                }

                return new ReadOnlyDictionary<string, ReadOnlyCollection<PrimaryKey>>(this.primaryKeyValue);
            }
        }

        public string PrimaryKeyData
        {
            get
            {
                var primaryFields = this.Definition.PrimaryFields;
                if (primaryFields == null)
                {
                    return string.Empty;
                }

                if (primaryFields.Count == 0)
                {
                    return string.Empty;
                }

                string res = string.Empty;
                foreach (string fieldName in primaryFields)
                {
                    if (this.ContainsKey(fieldName))
                    {
                        if (!string.IsNullOrEmpty(res))
                        {
                            res += "|";
                        }

                        if (this[fieldName] != null)
                        {
                            string data = this[fieldName].ToString();
                            if (data.StartsWith("{\r\n  \"Id\": ", StringComparison.OrdinalIgnoreCase))
                            {
                                data = data.Substring(11).Split(',')[0].Trim();
                            }

                            res += data;
                        }
                    }
                    else if (this.ContainsKey(fieldName + "Id"))
                    {
                        if (!string.IsNullOrEmpty(res))
                        {
                            res += "|";
                        }

                        if (this[fieldName + "Id"] != null)
                        {
                            res += this[fieldName + "Id"].ToString();
                        }
                    }
                }

                res = res.Replace("||", "|");
                if (res.EndsWith("|", StringComparison.OrdinalIgnoreCase))
                {
                    if (res.Length > 1)
                    {
                        res = res.Substring(0, res.Length - 1);
                    }
                    else
                    {
                        res = string.Empty;
                    }
                }

                if (res.StartsWith("|", StringComparison.OrdinalIgnoreCase))
                {
                    if (res.Length > 1)
                    {
                        res = res.Substring(1, res.Length - 1);
                    }
                    else
                    {
                        res = string.Empty;
                    }
                }

                return res;
            }
        }

        public string Xml
        {
            get
            {
                var res = new StringBuilder();
                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"    <{0} Id=""{1}"">{2}",
                    this.Definition.Layout.Label.Replace(" ", "_"),
                    this.Id,
                    Environment.NewLine);

                foreach (var data in this.source)
                {
                    if (!string.IsNullOrEmpty(data.Value as string))
                    {
                        res.AppendFormat(
                            CultureInfo.InvariantCulture,
                            @"        <{0}>{1}</{0}>{2}",
                            data.Key,
                            data.Value,
                            Environment.NewLine);
                    }
                }

                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "    </{0}>{1}",
                    this.Definition.Layout.Label.Replace(" ", "_"),
                    Environment.NewLine);
                return res.ToString();
            }
        }

        public string TraceData
        {
            get
            {
                var res = new StringBuilder();
                res.AppendFormat(CultureInfo.InvariantCulture, "\tId ==> {0}{1}", this.Id, Environment.NewLine);
                res.AppendFormat(CultureInfo.InvariantCulture, "\tDescription ==> {0}{1}", this.Description, Environment.NewLine);
                foreach (ItemField field in this.Definition.Fields.Where(f => !f.Name.Equals("Id", StringComparison.OrdinalIgnoreCase)))
                {
                    if (this.ContainsKey(field.Name))
                    {
                        if (this[field.Name] != null)
                        {
                            res.AppendFormat(
                                CultureInfo.InvariantCulture,
                                "\t{0} ==> {1}{2}",
                                field.Label,
                                field.TraceValue(this[field.Name]),
                                Environment.NewLine);
                        }
                    }
                }

                return res.ToString();
            }
        }

        public string Json
        {
            get
            {
                // TODO: Check why this dirtyhack is needed
                this["Description"] = this.Description;
                return JsonConvert.SerializeObject(this);
            }
        }

        public static string CsvHeader(string itemName, ItemDefinition definition, string instanceName)
        {
            var item = new ItemBuilder(itemName, definition, instanceName);
            var res = new StringBuilder();
            res.Append("Id;Description;");

            foreach (ItemField field in item.Definition.Fields)
            {
                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"{0};",
                    field.Label);
            }

            return res.ToString();
        }

        public string Csv
        {
            get
            {
                var res = new StringBuilder();
                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"{0};",
                    this.Id);
                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"{0};",
                    this.Description);

                foreach (string key in this.Keys)
                {
                    res.AppendFormat(
                        CultureInfo.InvariantCulture,
                        @"{0};",
                        this[key] as string);
                }

                return res.ToString();
            }
        }

        public string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"{{""Id"":{0},""Description"":""{1}"",""Active"":{2}}}",
                    this.Id,
                    ToolsJson.JsonCompliant(this.Description),
                    ConstantValue.Value(this.Active));
            }
        }

        public string Description
        {
            get
            {
                var dataList = new List<object>();

                // Coge la composicion de la definicion
                /*  "Description": {
                    "Pattern": "{0}-{1}-{2}",
                    "Fields": [ { "Name": "FabricanteId" }, { "Name": "Code" }, { "Name": "ColorId" } ]
                } */

                // Y por cada campo de la lista Fields, coge su valor y monta el string final siguiente el pratón "Pattern"

                // Es una forma unificada para que todo item tenga ID/Description en toda la aplicación
                foreach (ItemDescriptionField field in this.Definition.Layout.Description.Fields)
                {
                    if (this.ContainsKey(field.Name))
                    {
                        var obj = this[field.Name];

                        if (obj == null)
                        {
                            obj = this[field.Name + "Id"];
                        }

                        if (obj != null)
                        {
                            if (obj is JObject)
                            {
                                obj = ((JObject)obj)["Description"];
                            }

                            dataList.Add(obj);
                        }
                        else
                        {
                            dataList.Add(null);
                        }
                    }
                    else
                    {
                        dataList.Add(null);
                    }
                }

                string value = string.Empty;

                if (dataList.Any(obj => obj != null))
                {
                    value = string.Format(
                        CultureInfo.GetCultureInfo("en-us"),
                        this.Definition.Layout.Description.Pattern,
                        dataList.ToArray());
                }

                if (string.IsNullOrEmpty(value) && this.ContainsKey("Description"))
                {
                    value = this["Description"] as string;
                }

                return value;
            }
        }

        /// <summary>Gets formatted string with item data</summary>
        public string Data
        {
            get
            {
                var res = new StringBuilder();
                foreach (string key in this.Keys)
                {
                    res.Append(this[key] as string).Append('|');
                }

                return res.ToString();
            }
        }

        /// <summary>Gets the KeyValuePair with the solicited key if exists</summary>
        /// <param name="key">Searched key</param>
        /// <returns>KeyValuePair with the solicited key if exist</returns>
        public object this[string key]
        {
            get
            {
                if (this.source.ContainsKey(key))
                {
                    return this.source[key];
                }

                return null;
            }

            set
            {
                this.source[key] = value;
            }
        }
        
        /// <summary>Gets a JSON structure with basic data of an ItemBuilder</summary>
        /// <param name="value">Object that contains ItemBuilder data</param>
        /// <returns>JSON structure with basic data of an ItemBuilder</returns>
        public static string ItemValueJson(IDictionary<string, object> value)
        {
            if (value == null)
            {
                return ConstantValue.Null;
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"{{""Id"":{0},""Description"":""{1}"",""Active"":{2}}}",
                value["Id"],
                value["Description"],
                ConstantValue.Value((bool)value["Active"]));
        }

        public static string GetPathDefinition(string itemName, string instanceName)
        {
            string path = string.Format(
                CultureInfo.InvariantCulture,
                ConfigurationManager.AppSettings["ItemsDefinitionPath"],
                instanceName);

            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}\{1}.item", path, itemName);
            }
            else
            {
                path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}{1}.item", path, itemName);
            }

            return path;
        }

        public static string GetPathScripts(ItemBuilder item)
        {
            if (item == null)
            {
                return string.Empty;
            }

            return GetPathScripts(item.InstanceName, item.ItemName);
        }

        public static string GetPathScriptsList(ItemBuilder item)
        {
            if (item == null)
            {
                return string.Empty;
            }

            return GetPathScriptsList(item.InstanceName, item.ItemName);
        }

        public static string GetPathScripts(string instanceName, string itemName)
        {
            if (string.IsNullOrEmpty(instanceName) || string.IsNullOrEmpty(itemName))
            {
                return string.Empty;
            }

            string path = string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                ConfigurationManager.AppSettings["ItemsDefinitionPath"].Replace("ItemDefinition", "Scripts"),
                instanceName);

            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}\{1}.js", path, itemName);
            }
            else
            {
                path = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}{1}.js", path, itemName);
            }

            return path;
        }

        public static string GetPathScriptsList(string instanceName, string itemName)
        {
            if (string.IsNullOrEmpty(instanceName) || string.IsNullOrEmpty(itemName))
            {
                return string.Empty;
            }

            string path = string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                ConfigurationManager.AppSettings["ItemsDefinitionPath"].Replace("ItemDefinition", "Scripts"),
                instanceName);

            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}List.js", path, itemName);
            }
            else
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}{1}List.js", path, itemName);
            }

            return path;
        }

        public static ItemBuilder Empty(string itemName, bool withDefaults, string instanceName)
        {
            var res = new ItemBuilder(itemName, instanceName)
            {
                { "Id", (long)0 },
                { "Description", string.Empty }
            };

            if (withDefaults)
            {
                var properties = res.Definition.Fields.Where(p => p.Name != "Id" && p.Name != "Description").ToList();
                foreach (var property in properties)
                {
                    string label = property.Name;
                    switch (property.DataType)
                    {
                        case FieldDataType.Boolean:
                            res.Add(label, false);
                            break;
                        case FieldDataType.Integer:
                        case FieldDataType.Decimal:
                            res.Add(label, 0);
                            break;
                        default:
                            res.Add(label, null);
                            break;
                    }
                }
            }

            return res;
        }

        public static ActionResult Inactive(long itemId, string itemName, long userId, string userDescription, string instanceName, string stringConnection)
        {
            var customerConfig = CustomerFramework.Load(instanceName);            
            if (customerConfig.Config.DeleteAction == DeleteAction.Delete)
            {
                return OpenFramework.CRUD.Save.Delete(itemId, userId, userDescription, itemName, instanceName, stringConnection);
            }
            else
            {
                return OpenFramework.CRUD.Save.Inactive(itemId, userId, userDescription, itemName, instanceName, stringConnection);
            }
        }

        public string FieldAsString(string fieldName)
        {
            string paramValue = string.Empty;
            if (this.ContainsKey(fieldName))
            {
                paramValue = (this[fieldName] ?? string.Empty).ToString();
            }

            paramValue = paramValue.Replace(",", ".");
            return paramValue;
        }

        public void FromDataPersistence(ItemBuilder item)
        {
            if (item == null)
            {
                return;
            }

            // Se ha creado una colección intermedia para evitar un "InvalidOperationException" debido a unna modificación en item durante el foreach
            var values = new List<KeyValuePair<string, object>>();

            foreach (var pair in item)
            {
                values.Add(pair);
            }

            foreach (KeyValuePair<string, object> pair in values)
            {
                this.Add(pair);
            }
        }

        public void ReadById(long value, string connectionString)
        {
            this.ReadById(value, NotResolveForeignKeys, connectionString);
        }

        public void ReadById(long value, bool resolveForeignKeys, string connectionString)
        {
            throw new NotImplementedException();
        }

        public string GetKeyValue
        {
            get 
            {
                return ItemValueJson(this);
            }
        }

        public ActionResult Save(string instanceName, long userId, bool fromImport = false)
        {
            this.PrepareToSave();
            if (this.Id > 0)
            {
                return OpenFramework.CRUD.Save.Update(this, instanceName, userId, fromImport);
            }
            else
            {
                return OpenFramework.CRUD.Save.Insert(this, instanceName, userId, fromImport);
            }
        }

        public void Add(string key, object value)
        {
            if (this.source.ContainsKey(key))
            {
                this.source[key] = value;
            }
            else
            {
                this.source.Add(key, value);
            }
        }

        public void Add(KeyValuePair<string, object> value)
        {
            if (this.source.ContainsKey(value.Key))
            {
                this.source[value.Key] = value.Value;
            }
            else
            {
                this.source.Add(value.Key, value.Value);
            }
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            ICollection<KeyValuePair<string, object>> collection = this.source;
            collection.CopyTo(array, arrayIndex);
        }

        public bool ContainsKey(string key)
        {
            return this.source.ContainsKey(key);
        }

        /// <summary>
        /// Remove a KeyValuePair from item's value
        /// </summary>
        /// <param name="key">Key of KeyValuePair</param>
        /// <returns>Indicates if action finish with exit</returns>
        public bool Remove(string key)
        {
            return this.source.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return this.source.TryGetValue(key, out value);
        }

        /// <summary>Add a KeyValuePair into item's values</summary>
        /// <param name="item">KeyValuePair to add</param>
        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            ICollection<KeyValuePair<string, object>> collection = this.source;
            collection.Add(item);
        }

        /// <summary>Clear values of item</summary>
        void ICollection<KeyValuePair<string, object>>.Clear()
        {
            this.source.Clear();
        }

        /// <summary>Retrieve if key is in item's values</summary>
        /// <param name="item">KeyValuePair to find</param>
        /// <returns>Key is in item's values</returns>
        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            ICollection<KeyValuePair<string, object>> collection = this.source;
            return collection.Contains(item);
        }

        /// <summary>Remove a KeyValuePair from item's value</summary>
        /// <param name="item">KeyValuePair to remove</param>
        /// <returns>Indicates if action finish with exit</returns>
        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            ICollection<KeyValuePair<string, object>> collection = this.source;
            return collection.Remove(item);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ICollection collection = new List<KeyValuePair<string, object>>(this.source);
            collection.CopyTo(array, index);
        }

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            IEnumerable<KeyValuePair<string, object>> enumerator = this.source;
            return enumerator.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.source.GetEnumerator();
        }

        public static string ListItemJsonKeyValue(ReadOnlyCollection<ItemBuilder> items)
        {
            if (items == null)
            {
                return ToolsJson.EmptyJsonList;
            }

            var res = new StringBuilder("[");
            bool first = true;
            foreach (ItemBuilder item in items)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(Environment.NewLine);
                res.Append(item.JsonKeyValue);
            }

            res.Append("]");
            return res.ToString();
        }

        public static string ListItemJson(ReadOnlyCollection<ItemBuilder> items)
        {
            if (items == null)
            {
                return ToolsJson.EmptyJsonList;
            }

            var res = new StringBuilder("[");
            bool first = true;
            foreach (ItemBuilder item in items.OrderBy(i => i.Description))
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(Environment.NewLine);
                res.Append(item.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public static string ListItemPKJson(ReadOnlyCollection<ItemBuilder> items)
        {
            if (items == null)
            {
                return ToolsJson.EmptyJsonList;
            }

            var res = new StringBuilder("[");
            bool first = true;
            foreach (ItemBuilder item in items.OrderBy(i => i.Description))
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(Environment.NewLine);
                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"""{0}""",
                    item.PrimaryKeyData);
            }

            res.Append("]");
            return res.ToString();
        }

        public bool Equals(ItemBuilder comparative)
        {
            if (comparative == null)
            {
                return false;
            }

            string currentKey = this.PrimaryKeyData;
            string comparativeKey = comparative.PrimaryKeyData;

            return currentKey.Equals(comparativeKey, StringComparison.OrdinalIgnoreCase);
            /*
            bool res = false;
            foreach (string primaryField in this.Definition.PrimaryFields)
            {
                ReadOnlyCollection<string> pks = this.Definition.PrimaryKeys[primaryField];
                foreach (string pk in pks)
                {
                    if (this.ContainsKey(pk) && !comparative.ContainsKey(pk))
                    {
                        return false;
                    }
                    else if (!this.ContainsKey(pk) && comparative.ContainsKey(pk))
                    {
                        return false;
                    }
                    else if (this.ContainsKey(pk) && comparative.ContainsKey(pk))
                    {
                        if (this[pk].Equals(comparative[pk]))
                        {
                            return true;
                        }
                    }
                }
            }

            return res;
             * */
        }

        public string HasSearchedData(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            text = text.ToUpperInvariant();
            foreach (string key in this.Keys)
            {
                if (this.Definition.Fields.Any(f => f.Name == key))
                {
                    var dataType = this.Definition.Fields.First(f => f.Name == key).DataType;
                    if (dataType == FieldDataType.Textarea || dataType == FieldDataType.Text)
                    {
                        if (this[key] != null && (this[key].ToString().ToUpperInvariant()).IndexOf(text, StringComparison.OrdinalIgnoreCase) != -1)
                        {
                            return key;
                        }
                    }
                }
            }

            return string.Empty;
        }

        private string DataAdapterGetKeyValue()
        {
            string query = string.Empty;
            if (this.Definition.DataAdapter.GetKeyValue.StoredName == "#_getParams")
            {
                var result = new StringBuilder("SELECT ");
                bool first = true;
                foreach (StoredParameter param in this.Definition.DataAdapter.GetAll.Parameters)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        result.Append(",");
                    }

                    result.AppendFormat(CultureInfo.GetCultureInfo("en-us"), "[{0}]", param.Parameter);
                }

                result.AppendFormat(" FROM Item_{0}", this.ItemName);
                query = result.ToString();
            }
            else if (this.Definition.DataAdapter.GetKeyValue.StoredName != "#_getAll")
            {
                query = this.Definition.DataAdapter.GetKeyValue.StoredName;
            }
            else if (!string.IsNullOrEmpty(this.Definition.DataAdapter.GetKeyValue.StoredName))
            {
                query = this.Definition.DataAdapter.GetKeyValue.StoredName;
            }

            return query;
        }

        private string DataAdapterGetAll()
        {
            string query = string.Empty;
            if (this.Definition.DataAdapter.GetAll.StoredName == "#_getParams")
            {
                var result = new StringBuilder("SELECT ");
                bool first = true;
                foreach (StoredParameter param in this.Definition.DataAdapter.GetAll.Parameters)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        result.Append(",");
                    }

                    result.AppendFormat(CultureInfo.GetCultureInfo("en-us"), "[{0}]", param.Parameter);
                }

                result.AppendFormat(" FROM Item_{0}", this.ItemName);
                query = result.ToString();
            }
            else if (this.Definition.DataAdapter.GetAll.StoredName != "#_getAll")
            {
                query = this.Definition.DataAdapter.GetAll.StoredName;
            }

            return query;
        }

        /*
        private string DataAdapterGetById()
        {
            string query = string.Empty;
            if (this.Definition.DataAdapter.GetById.StoredName == "#_getParams")
            {
                StringBuilder result = new StringBuilder("SELECT ");
                bool first = true;
                foreach (StoredParameter param in this.Definition.DataAdapter.GetById.Parameters)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        result.Append(",");
                    }

                    result.AppendFormat("[{0}]", param.Parameter);
                }

                result.AppendFormat(CultureInfo.GetCultureInfo("en-us"), " FROM Item_{0} WHERE ", this.ItemName);
                query = result.ToString();
            }
            else if (this.Definition.DataAdapter.GetById.StoredName != "#_getById")
            {
                query = this.Definition.DataAdapter.GetById.StoredName;
            }

            return query;
        }
        */

        private ItemDefinition GetDefinition(string instanceName)
        {
            if (this.Definition != null)
            {
                return this.Definition;
            }

            return ItemDefinition.Load(this.ItemName, instanceName);
        }

        /// <summary>Initialize the basics properties of an ItemBuilder instance</summary>
        /// <param name="itemName">Item name</param>
        /// <param name="resolveForeign">Indicates if must resolve foreign keys</param>
        /// <param name="instanceName">Name of instance</param>
        private void Initialize(string itemName, string instanceName, string connectionString)
        {
            this.ItemName = itemName;
            this.InstanceName = instanceName;
            this.source = new Dictionary<string, object>();
            if (HttpContext.Current.Session["Items"] is Dictionary<string, ItemBuilder> items && items.ContainsKey(itemName.ToUpperInvariant()))
            {
                var found = items[itemName.ToUpperInvariant()];
                this.Definition = found.Definition;

                foreach (KeyValuePair<string, object> data in found.Values)
                {
                    this.Add(data);
                }
            }
            else
            {
                this.Definition = this.GetDefinition(this.InstanceName);
            }
        }

        /// <summary>Gets data differences beetween two items</summary>
        /// <param name="old">Old item</param>
        /// <param name="actual">Actual item</param>
        /// <returns>Strign description of differences beetween two items</returns>
        public static string Differences(ItemBuilder old, ItemBuilder actual)
        {
            var res = new StringBuilder();
            foreach(var field in actual.Definition.Fields)
            {
                string oldValue = string.Empty;
                string actualValue = string.Empty;

                switch (field.DataType)
                {
                    case FieldDataType.Boolean:
                        if (old.ContainsKey(field.Name) && old[field.Name] != null)
                        {
                            oldValue = ConstantValue.Value((bool)old[field.Name]);
                        }

                        if (actual.ContainsKey(field.Name) && actual[field.Name] != null)
                        {
                            actualValue = ConstantValue.Value((bool)actual[field.Name]);
                        }

                        break;
                    case FieldDataType.Long:
                        if (old.ContainsKey(field.Name) && old[field.Name] != null)
                        {
                            oldValue = Convert.ToInt64(old[field.Name], CultureInfo.InvariantCulture).ToString();
                        }

                        if (actual.ContainsKey(field.Name) && actual[field.Name] != null)
                        {
                            actualValue = Convert.ToInt64(actual[field.Name], CultureInfo.InvariantCulture).ToString();
                        }
                        break;
                    case FieldDataType.Integer:
                        if (old.ContainsKey(field.Name) && old[field.Name] != null)
                        {
                            oldValue = Convert.ToInt32(old[field.Name], CultureInfo.InvariantCulture).ToString();
                        }

                        if (actual.ContainsKey(field.Name) && actual[field.Name] != null)
                        {
                            actualValue = Convert.ToInt32(actual[field.Name], CultureInfo.InvariantCulture).ToString();
                        }
                        break;
                    case FieldDataType.NullableDateTime:
                    case FieldDataType.DateTime:
                        if (old.ContainsKey(field.Name) && old[field.Name] != null)
                        {
                            oldValue = Convert.ToDateTime(old[field.Name], CultureInfo.InstalledUICulture).ToString();
                        }

                        if (actual.ContainsKey(field.Name) && actual[field.Name] != null && !string.IsNullOrEmpty(actual[field.Name].ToString()))
                        {
                            actualValue = Convert.ToDateTime(actual[field.Name], CultureInfo.InstalledUICulture).ToString();
                        }
                        break;
                    case FieldDataType.Decimal:
                    case FieldDataType.NullableDecimal:
                        if (old.ContainsKey(field.Name) && old[field.Name] != null)
                        {
                            oldValue = string.Format("{0:F10}", old[field.Name]);
                        }

                        if (actual.ContainsKey(field.Name) && actual[field.Name] != null)
                        {
                            actualValue = string.Format("{0:F10}", actual[field.Name]);
                        }
                        break;
                    default:
                        if (old.ContainsKey(field.Name) && old[field.Name] != null)
                        {
                            oldValue = old[field.Name].ToString().Trim();
                        }

                        if (actual.ContainsKey(field.Name) && actual[field.Name] != null)
                        {
                            actualValue = actual[field.Name].ToString().Trim();
                        }
                        break;
                }


                if (!actualValue.Equals(oldValue, StringComparison.InvariantCulture))
                {
                    res.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "\t{0}: {1} ==> {2}{3}",
                        field.Label,
                        oldValue,
                        actualValue,
                        Environment.NewLine);
                }
            }

            return res.ToString();
        }

        public void PrepareToSave()
        {
            // Prepara los campos vinculados
            if (this.Definition.Fields.Any(f => !string.IsNullOrEmpty(f.VinculatedTo)))
            {
                var vinculados = this.Definition.Fields.Where(f => !string.IsNullOrEmpty(f.VinculatedTo)).ToList();
                foreach (var vinculado in vinculados)
                {
                    var value = this[vinculado.VinculatedTo];
                    if (value == null)
                    {
                        this[vinculado.Name] = 0;
                    }
                }
            }
            /* WEKE: Se trabaja directamente con el valor del campo referencia
            // Prepara los foreing key
            foreach (KeyValuePair<string, object> data in this.source.Where(s => s.Key.EndsWith("Id", StringComparison.OrdinalIgnoreCase)).ToList())
            {
                string fieldName = Basics.Left(data.Key, data.Key.Length - 2);
                if (this.source.ContainsKey(fieldName))
                {
                    object entityReference = this.source[fieldName];
                    if (entityReference != null)
                    {
                        if (entityReference.GetType().Name.ToUpperInvariant() == "JOBJECT")
                        {
                            string entityReferenceText = entityReference.ToString();
                            JObject x2 = entityReference as JObject;
                            if (x2["Description"] != null)
                            {
                                if (x2["Id"] == null)
                                {
                                    this.source[data.Key] = null;
                                    continue;
                                }

                                entityReferenceText = x2["Id"].ToString();
                            }
                            else 
                            {
                                if (x2["Id"] != null)
                                {
                                    entityReferenceText = x2["Id"].ToString();
                                }
                            }

                            if (string.IsNullOrEmpty(entityReferenceText))
                            {
                                this.source[data.Key] = null;
                            }
                            else if (entityReferenceText.Equals("null", StringComparison.OrdinalIgnoreCase))
                            {
                                this.source[data.Key] = null;
                            }
                            else
                            {
                                this.source[data.Key] = Convert.ToInt64(entityReferenceText, CultureInfo.InvariantCulture);
                            }
                        }
                    }
                }
            }*/
        }
    }
}