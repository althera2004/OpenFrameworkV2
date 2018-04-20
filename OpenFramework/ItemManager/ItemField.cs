// --------------------------------
// <copyright file="ItemField.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Newtonsoft.Json;
    using OpenFramework.ItemManager.List;

    /// <summary>Implements class for item fields</summary>
    [Serializable]
    public sealed class ItemField
    {
        /// <summary>Name of default field type</summary>
        [JsonIgnore]
        public const string DefaultTypeName = "TEXT";

        /// <summary>Maximum length</summary>
        [JsonProperty("Length")]
        private string length;

        /// <summary>Column sorting</summary>
        [JsonProperty("Sorting")]
        private string sorting;

        /// <summary>Field label from JSON item file</summary>
        [JsonProperty("Label")]
        private string label;

        /// <summary>Column data format</summary>
        [JsonProperty("DataFormat")]
        private ColumnDataFormat columnDataFormat;

        /// <summary>Value of field</summary>
        [JsonIgnore]
        private object fieldValue;

        /// <summary>Gets an empty field</summary>
        [JsonIgnore]
        public static ItemField Empty
        {
            get
            {
                return new ItemField()
                {
                    Name = string.Empty,
                    fieldValue = string.Empty,
                    label = string.Empty,
                    TypeName = "text"
                };
            }
        }

        /// <summary>Gets a default description field</summary>
        [JsonIgnore]
        public static ItemField DefaultDescription
        {
            get
            {
                return new ItemField()
                {
                    Name = "Description",
                    fieldValue = string.Empty,
                    TypeName = "text"
                };
            }
        }

        /// <summary>Gets or sets a value indicating whether field is only a reference</summary>
        [JsonProperty("Referencial")]
        public bool Referencial { get; set; }

        /// <summary>Gets column sorting</summary>
        [JsonIgnore]
        public string Sorting
        {
            get
            {
                if (string.IsNullOrEmpty(this.sorting))
                {
                    return string.Empty;
                }

                return this.sorting;
            }
        }

        /// <summary>Gets or sets the column data type</summary>
        [JsonProperty("ColumnDataType")]
        public string ColumnDataType { get; set; }

        /// <summary>Gets or sets a value indicating whether if field is showed in list including Id field</summary>
        [JsonProperty("ShowInList")]
        public bool ShowInList { get; set; }

        /// <summary>Gets or sets the name of linked field</summary>
        [JsonProperty("VinculatedTo")]
        public string VinculatedTo { get; set; }

        /// <summary>Gets or sets the item name</summary>
        [JsonIgnore]
        public string ItemName { get; set; }

        /// <summary>Gets or sets the name of field</summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>Gets or sets code sequence for automatic codes fields</summary>
        [JsonProperty("CodeSequence")]
        public string CodeSequence { get; set; }

        /// <summary>Gets or sets a value indicating whether data is required</summary>
        [JsonProperty("Required")]
        public bool Required { get; set; }

        /// <summary>Gets or sets the fixed list identifier</summary>
        [JsonProperty("FixedListId")]
        public string FixedListId { get; set; }

        /// <summary>Gets the maximum length of field</summary>
        [JsonIgnore]
        public int? Length
        {
            get
            {
                if (string.IsNullOrEmpty(this.length))
                {
                    return null;
                }

                return Convert.ToInt32(this.length, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>Gets form of column data</summary>
        [JsonIgnore]
        public ColumnDataFormat ColumnDataFormat
        {
            get
            {
                if (this.columnDataFormat != null)
                {
                    return this.columnDataFormat;
                }

                if (this.TypeName.ToUpperInvariant() == "MONEY")
                {
                    return ColumnDataFormat.MoneyFormat;
                }

                if (this.TypeName.ToUpperInvariant() == "GPS")
                {
                    return ColumnDataFormat.GPS;
                }

                if (this.TypeName.ToUpperInvariant() == "DECIMAL")
                {
                    return ColumnDataFormat.DecimalFormat;
                }

                return null;
            }
        }
        
        /// <summary>Gets or sets the label of field in lists and forms</summary>
        [JsonIgnore]
        public string Label
        {
            get
            {
                if (string.IsNullOrEmpty(this.label))
                {
                    return this.Name;
                }

                return this.label;
            }

            set
            {
                this.label = value;
            }
        }

        /// <summary>Gets label of files specify for forms</summary>
        [JsonIgnore]
        public string LabelExcel
        {
            get
            {
                if (this.Required)
                {
                    return string.Format(CultureInfo.GetCultureInfo("en-us"), "{0}*", this.Label);
                }

                return this.Label;
            }
        }

        /// <summary>Gets label of files specify for forms</summary>
        [JsonIgnore]
        public string LabelForm
        {
            get
            {
                if (this.Required)
                {
                    return string.Format(CultureInfo.InvariantCulture, @"{0}<span style=""color:#ff0000;"">*</span>", this.Label);
                }

                return this.Label;
            }
        }
        
        /// <summary>Gets data type of field</summary>
        [JsonIgnore]
        public FieldDataType DataType
        {
            get
            {
                switch (this.TypeName.ToUpperInvariant())
                {
                    case "TEXT":
                        return FieldDataType.Text;
                    case "LONG":
                        if (this.Required == false)
                        {
                            return FieldDataType.NullableLong;
                        }

                        return FieldDataType.Long;
                    case "INTEGER":
                    case "INT":
                        if (this.Required == false)
                        {
                            return FieldDataType.NullableInteger;
                        }

                        return FieldDataType.Integer;
                    case "BOOLEAN":
                    case "BOOL":
                        if (this.Required == false)
                        {
                            return FieldDataType.NullableBoolean;
                        }

                        return FieldDataType.Boolean;
                    case "DATETIME":
                        if (this.Required == false)
                        {
                            return FieldDataType.NullableDateTime;
                        }

                        return FieldDataType.DateTime;
                    case "TIME":
                        if (this.Required == false)
                        {
                            return FieldDataType.NullableTime;
                        }

                        return FieldDataType.Time;
                    case "MONEY":
                    case "DECIMAL":
                        if (this.Required == false)
                        {
                            return FieldDataType.NullableDecimal;
                        }

                        return FieldDataType.Decimal;
                    case "TEXTAREA":
                        return FieldDataType.Textarea;                        
                    case "EMAIL":
                        return FieldDataType.Email;
                    case "URL":
                        return FieldDataType.Url;
                    case "FIXEDLIST":
                        return FieldDataType.FixedList;
                    case "FIXEDLISTLINKED":
                        return FieldDataType.FixedListLinked;
                    case "IMAGE":
                        return FieldDataType.Image;
                    case "CDNIMAGE":
                        return FieldDataType.CDNImage;
                    case "IMAGEGALLERY":
                    case "PHOTOGALLERY":
                        return FieldDataType.ImageGallery;
                    case "DOCUMENTGALLERY":
                    case "DOCUMENTFILE":
                    case "DOCUMENT":
                    case "FILE":
                        return FieldDataType.DocumentFile;
                    case "FOLDER":
                        return FieldDataType.Folder;
                }

                return FieldDataType.Undefined;
            }
        }

        /// <summary>Gets or sets the value of field</summary>
        [JsonIgnore]
        public object Value
        {
            get
            {
                switch (this.DataType)
                {
                    case FieldDataType.Undefined:
                    case FieldDataType.Text:
                        return (string)this.fieldValue;
                    case FieldDataType.Integer:
                        return (int)this.fieldValue;
                    case FieldDataType.NullableInteger:
                        return (int?)this.fieldValue;
                    case FieldDataType.Decimal:
                        return (decimal)this.fieldValue;
                    case FieldDataType.NullableDecimal:
                        return (decimal?)this.fieldValue;
                    default:
                        return (string)this.fieldValue;
                }
            }

            set
            {
                switch (this.DataType)
                {
                    case FieldDataType.Undefined:
                    case FieldDataType.Text:
                        this.fieldValue = (string)value;
                        break;
                    case FieldDataType.Integer:
                        this.fieldValue = (int)value;
                        break;
                    case FieldDataType.NullableInteger:
                        this.fieldValue = (int?)value;
                        break;
                    case FieldDataType.Decimal:
                        this.fieldValue = (decimal)value;
                        break;
                    case FieldDataType.NullableDecimal:
                        this.fieldValue = (decimal?)value;
                        break;
                    default:
                        this.fieldValue = value;
                        break;
                }
            }
        }

        /// <summary>Gets data type text</summary>
        [JsonIgnore]
        public string DataTypeLabel
        {
            get
            {
                return Enum.GetName(typeof(FieldDataType), this.DataType);
            }
        }

        /// <summary>Gets data type description for user interface</summary>
        [JsonIgnore]
        public string DataTypeInterfaceLabel
        {
            get
            {
                switch (this.DataType)
                {
                    case FieldDataType.Text:
                        return "Texto";
                    case FieldDataType.Textarea:
                        return "Texto largo";
                    case FieldDataType.DateTime:
                    case FieldDataType.NullableDateTime:
                        return "Fecha/hora";
                    case FieldDataType.Decimal:
                    case FieldDataType.NullableDecimal:
                        return "Decimal";
                    case FieldDataType.Integer:
                    case FieldDataType.NullableInteger:
                        return "Número entero";
                    case FieldDataType.Long:
                    case FieldDataType.NullableLong:
                        return "Número entero largo";
                    case FieldDataType.Boolean:
                    case FieldDataType.NullableBoolean:
                        return "Sí/No";
                    case FieldDataType.Image:
                        return "Imagen";
                    case FieldDataType.ImageGallery:
                        return "Galería de imágenes";
                    case FieldDataType.CDNImage:
                        return "Imagen CDN";
                    case FieldDataType.DocumentFile:
                        return "Documento";
                    case FieldDataType.FixedList:
                        return "Lista fija";
                }

                return "No definido";
            }
        }

        /// <summary>Gets or sets the data type of field</summary>
        [JsonProperty("Type")]
        public string TypeName { get; set; }

        /// <summary>Gets or sets path for CDN images</summary>
        [JsonProperty("CDNImagePath")]
        public string CDNImagePath { get; set; }

        /// <summary>Gets or sets mark for CDN images</summary>
        [JsonProperty("CDNImageMark")]
        public string CDNImageMark { get; set; }

        /// <summary>Gets SQL sentence to extract value of field and label</summary>
        [JsonIgnore]
        public string SqlFieldExtractor
        {
            get
            {
                string res = string.Empty;
                switch (this.DataType)
                {
                    case FieldDataType.Integer:
                    case FieldDataType.Long:
                    case FieldDataType.Decimal:
                    case FieldDataType.Float:
                    case FieldDataType.NullableInteger:
                    case FieldDataType.NullableLong:
                    case FieldDataType.NullableDecimal:
                    case FieldDataType.NullableFloat:
                        res = string.Format(
                            CultureInfo.InvariantCulture,
                            @"'""{0}"":' + ISNULL(CAST(Item.{0} AS nvarchar(20)),'null') + ','",
                            this.Name);
                        break;
                    case FieldDataType.DateTime:
                    case FieldDataType.NullableDateTime:
                        res = string.Format(
                            CultureInfo.InvariantCulture,
                            @"'""{0}"":' + ISNULL('""' + CONVERT(varchar(11),Item.{0},102) + '""', 'null') + '"",')",
                            this.Name);
                        break;
                    case FieldDataType.Boolean:
                    case FieldDataType.NullableBoolean:
                        res = string.Format(
                            CultureInfo.InvariantCulture,
                            @"'""{0}"":' +  CASE WHEN Item.{0} = 1 THEN 'true' ELSE 'false' END + ','",
                            this.Name);
                        break;
                    case FieldDataType.Image:
                        res = string.Format(
                            CultureInfo.InvariantCulture,
                            @"'""{0}"":' + CASE WHEN Item.{0} IS NULL THEN 'null,' ELSE '""' + Item.{0} + '"",' END",
                            this.Name);
                        break;
                    default:
                        res = string.Format(
                            CultureInfo.InvariantCulture,
                            @"'""{0}"":""' +  ISNULL(Item.{0},'') + '"",'",
                            this.Name);
                        break;
                }

                return res;
            }
        }

        /// <summary>Gets part of SQL sentence to extract value of field</summary>
        [JsonIgnore]
        public string SqlFieldExtractorValue
        {
            get
            {
                string res = string.Empty;
                switch (this.DataType)
                {
                    case FieldDataType.Integer:
                    case FieldDataType.Long:
                    case FieldDataType.Decimal:
                    case FieldDataType.Float:
                    case FieldDataType.NullableInteger:
                    case FieldDataType.NullableLong:
                    case FieldDataType.NullableDecimal:
                    case FieldDataType.NullableFloat:
                        res = string.Format(
                            CultureInfo.InvariantCulture,
                            @"ISNULL(CAST(Item.{0} AS nvarchar(20)),'null')",
                            this.Name);
                        break;
                    case FieldDataType.DateTime:
                    case FieldDataType.NullableDateTime:
                        res = string.Format(
                            CultureInfo.InvariantCulture,
                            @"ISNULL('""' + CONVERT(varchar(11),Item.{0},102) + '""', 'null')",
                            this.Name);
                        break;
                    case FieldDataType.Boolean:
                    case FieldDataType.NullableBoolean:
                        res = string.Format(
                            CultureInfo.InvariantCulture,
                            @"CASE WHEN Item.{0} = 1 THEN 'true' ELSE 'false' END",
                            this.Name);
                        break;
                    case FieldDataType.Image:
                        res = string.Format(
                            CultureInfo.InvariantCulture,
                            @"CASE WHEN Item.{0} IS NULL THEN 'null' ELSE '""' + Item.{0} + '""' END",
                            this.Name);
                        break;
                    default:
                        res = string.Format(
                            CultureInfo.InvariantCulture,
                            @"ISNULL(Item.{0},'')",
                            this.Name);
                        break;
                }

                return res;
            }
        }

        /// <summary>Gets field refered by a foreign key</summary>
        /// <param name="item">Item source</param>
        /// <param name="field">Field source</param>
        /// <returns>Field refered by a foreign key</returns>
        public static ItemField GetReferedField(ItemBuilder item, ItemField field)
        {
            if (field.Name.EndsWith("id", StringComparison.OrdinalIgnoreCase))
            {
                var foreignItem = field.Name.Substring(0, field.Name.Length - 2);
                var foreingRelation = item.Definition.ForeignValues.FirstOrDefault(fv => fv.ItemName.Equals(foreignItem));
                if (foreingRelation != null)
                {
                    var referedItem = new ItemBuilder(foreignItem, item.InstanceName);
                    var descriptionField = referedItem.Definition.Layout.Description.Fields.First();
                    var referedField = referedItem.Definition.Fields.First(f => f.Name.Equals(descriptionField.Name, StringComparison.OrdinalIgnoreCase));
                    if (referedField != null)
                    {
                        return referedField;
                    }
                }
            }

            return null;
        }

        /// <summary>Sets format of column data</summary>
        /// <param name="columnFormat">Column data format</param>
        public void SetColumnDataFormat(ColumnDataFormat columnFormat)
        {
            this.columnDataFormat = columnFormat;
        }

        /// <summary>Sets text for field label</summary>
        /// <param name="text">Text for field label</param>
        public void SetLabel(string text)
        {
            this.label = text;
        }

        /// <summary>
        /// Gets field value in order to insert in item trace
        /// </summary>
        /// <param name="value">Field value</param>
        /// <returns>String representation of value</returns>
        public string TraceValue(object value)
        {
            switch (this.DataType)
            {
                case FieldDataType.DateTime:
                    var dateValue = (DateTime)value;
                    return string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", dateValue);
                case FieldDataType.Boolean:
                    bool boolValue = (bool)value;
                    return boolValue ? "true" : "false";
                default:
                    return value.ToString();
            }
        }

        /// <summary>Gets SQL sentence to extract value of field and label for ReplacedBy configuration</summary>
        /// <param name="replacedBy">ReplacedBy value configuration</param>
        /// <returns>SQL sentence to extract value of field and label for ReplacedBy configuration</returns>
        public string SqlFieldExtractorReplace(string replacedBy)
        {
            string res = string.Empty;
            switch (this.DataType)
            {
                case FieldDataType.Integer:
                case FieldDataType.Long:
                case FieldDataType.Decimal:
                case FieldDataType.Float:
                case FieldDataType.NullableInteger:
                case FieldDataType.NullableLong:
                case FieldDataType.NullableDecimal:
                case FieldDataType.NullableFloat:
                    res = string.Format(
                        CultureInfo.InvariantCulture,
                        @"'""{1}"":' + ISNULL(CAST(Item.{0} AS nvarchar(20)),'null') + ','",
                        this.Name,
                        replacedBy);
                    break;
                case FieldDataType.DateTime:
                case FieldDataType.NullableDateTime:
                    res = string.Format(
                        CultureInfo.InvariantCulture,
                        @"'""{1}"":' + ISNULL('""' + CONVERT(varchar(11),Item.{0},102) + '""', 'null') + '"",')",
                        this.Name,
                        replacedBy);
                    break;
                case FieldDataType.Boolean:
                case FieldDataType.NullableBoolean:
                    res = string.Format(
                        CultureInfo.InvariantCulture,
                        @"'""{1}"":' +  CASE WHEN Item.{0} = 1 THEN 'true' ELSE 'false' END + ','",
                        this.Name,
                        replacedBy);
                    break;
                default:
                    res = string.Format(
                        CultureInfo.InvariantCulture,
                        @"'""{1}"":""' +  ISNULL(Item.{0},'') + '"",'",
                        this.Name,
                        replacedBy);
                    break;
            }

            return res;
        }
    }
}