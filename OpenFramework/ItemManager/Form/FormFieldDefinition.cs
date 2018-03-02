// --------------------------------
// <copyright file="FormFieldDefinition.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.Core.ItemManager.Form
{
    using System.Globalization;
    using System.Text;
    using Newtonsoft.Json;
    using OpenFramework.ItemManager;

    /// <summary>Implements class for form fields</summary>
    public sealed class FormFieldDefinition
    {
        /// <summary>Size of the field (unused)</summary>
        [JsonProperty("Size")]
        private int size;

        /// <summary>Checks if it's read-only</summary>
        [JsonProperty("ReadOnly")]
        private bool readOnly;

        /// <summary>Type of the field</summary>
        [JsonProperty("Type")]
        private int type;

        /// <summary>Text representation of type field</summary>
        [JsonProperty("TypeField")]
        private string typeField;

        /// <summary>Function (if the field is a button)</summary>
        [JsonProperty("Function")]
        private string function;

        /// <summary>Values to send to the function (if the field is a button)</summary>
        [JsonProperty("FunctionValues")]
        private object[] functionValues;

        /// <summary>Checks if the field required an item instance</summary>
        [JsonProperty("ItemRequired")]
        private bool? itemRequired;

        /// <summary>Gets the name of the field</summary>
        [JsonProperty("Name")]
        public string Name { get; private set; }

        /// <summary>Gets the label of the field (used if the field is not a regular input)</summary>
        [JsonProperty("Label")]
        public string Label { get; private set; }

        /// <summary>Gets a value indicating whether public access to itemRequired</summary>
        [JsonIgnore]
        public bool ItemRequired
        {
            get
            {
                if (!this.itemRequired.HasValue)
                {
                    return false;
                }

                return (bool)this.itemRequired;
            }
        }

        /// <summary>Gets code for the function (if the field is a button)</summary>
        [JsonIgnore]
        public string Function
        {
            get
            {
                if (string.IsNullOrEmpty(this.function))
                {
                    return string.Empty;
                }

                StringBuilder res = new StringBuilder();
                
                // If there are any values defined...
                if (this.functionValues != null)
                {
                    bool first = true;
                    foreach (object val in this.functionValues)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            res.Append(",");
                        }

                        // If the value is a string, format as string
                        string value = val.GetType().ToString().ToUpperInvariant() == "SYSTEM.STRING" ? string.Format(CultureInfo.InvariantCulture, @"'{0}'", val) : val.ToString();
                        
                        // Append the value
                        res.Append(value);
                    }
                }

                return string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}({1})",
                    this.function,
                    res);                
            }
        }

        /// <summary>Gets the type of the field</summary>
        [JsonIgnore]
        public FormFieldType Type
        {
            get
            {
                return (FormFieldType)this.type;
            }
        }

        /// <summary>Gets the type of the field</summary>
        [JsonIgnore]
        public FieldDataType TypeField
        {
            get
            {
                if (string.IsNullOrEmpty(this.typeField))
                {
                    return FieldDataType.Undefined;
                }

                switch (this.typeField.ToUpperInvariant().Trim())
                {
                    case "DATEPICKER":
                        return FieldDataType.DateTime;
                    case "SELECT":
                        return FieldDataType.FixedListLinked;
                    case "NUMERIC":
                        return FieldDataType.Decimal;
                    case "CHECKBOX":
                        return FieldDataType.Boolean;
                    case "TEXT":
                    default:
                        return FieldDataType.Text;
                }
            }
        }

        /// <summary>Gets field size</summary>
        [JsonIgnore]
        public int Size
        {
            get
            {
                if (this.size == 0)
                {
                    return 12;
                }

                return this.size;
            }
        }

        /// <summary>Gets a value indicating whether field is read only</summary>
        [JsonIgnore]
        public bool ReadOnly
        {
            get
            {
                return this.readOnly;
            }
        }
    }
}
