// --------------------------------
// <copyright file="ItemFieldRules.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón -  jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.Core
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>Implements validations rules for fields data</summary>
    public struct ItemFieldRules
    {
        /// <summary>Values of validation control</summary>
        [JsonProperty("Value")]
        private object[] value;

        /// <summary>Custom name of rule</summary>
        [JsonProperty("CustomName")]
        private string customName;

        /// <summary>Name of fields affected by rule</summary>
        [JsonProperty("FieldNames")]
        private List<string> fieldNames;

        /// <summary>Gets an empty rule</summary>
        [JsonIgnore]
        public static ItemFieldRules Empty
        {
            get
            {
                return new ItemFieldRules()
                {
                    fieldNames = new List<string>(),
                    RuleName = string.Empty,
                    value = new object[] { }
                };
            }
        }

        /// <summary>Gets or sets the name of rule</summary>
        [JsonProperty("Rule")]
        public string RuleName { get; set; }

        /// <summary>Gets the name of fields affected by rule</summary>
        [JsonIgnore]
        public ReadOnlyCollection<string> FieldNames
        {
            get
            {
                if (this.fieldNames == null)
                {
                    return new ReadOnlyCollection<string>(new List<string>());
                }

                return new ReadOnlyCollection<string>(this.fieldNames);
            }
        }

        /// <summary>Gets rule type</summary>
        [JsonIgnore]
        public ValidatorRule Rule
        {
            get
            {
                switch (this.RuleName.ToUpperInvariant())
                {
                    case "MINVALUE":
                        return ValidatorRule.MinValue;
                    case "MAXVALUE":
                        return ValidatorRule.MaxValue;
                    case "RANGEVALUE":
                        return ValidatorRule.RangeValue;
                    case "MINDATE":
                        return ValidatorRule.MinDate;
                    case "MAXDATE":
                        return ValidatorRule.MaxDate;
                    case "RANGEDATE":
                        return ValidatorRule.RangeDate;
                    case "MINTODAY":
                        return ValidatorRule.MinToday;
                    case "MAXTODAY":
                        return ValidatorRule.MaxToday;
                    case "MINSELECTED":
                        return ValidatorRule.MinSelected;
                    case "MAXSELECTED":
                        return ValidatorRule.MaxSelected;
                    case "ONLYNUMBERS":
                        return ValidatorRule.OnlyNumbers;
                    case "EMAIL":
                        return ValidatorRule.Email;
                    case "MINLENGTH":
                        return ValidatorRule.MinLength;
                    case "MAXLENGTH":
                        return ValidatorRule.MaxLength;
                    case "DATELESSTHANFIELD":
                        return ValidatorRule.DateLessThanField;
                    case "DATEMORETHANFIELD":
                        return ValidatorRule.DateMoreThanField;
                    case "NUMBERLESSTHANFIELD":
                        return ValidatorRule.NumberMinorThanField;
                    case "NUMBERMORETHANFIELD":
                        return ValidatorRule.NumberMajorThanField;
                    case "MINLENGTHDECIMAL":
                        return ValidatorRule.MinLengthDecimal;
                    case "MAXLENGTHDECIMAL":
                        return ValidatorRule.MaxLengthDecimal;
                }

                return ValidatorRule.Undefined;
            }
        }
        
        /// <summary>Gets values for validation control</summary>
        [JsonIgnore]
        public ReadOnlyCollection<object> Value
        {
            get
            {                
                object[] valueArray = new object[this.value.Length];
                for (int x = 0; x < this.value.Length; x++)
                {
                    /*string dataType = this.value[x].GetType().ToString().ToUpperInvariant();
                    switch (dataType)
                    {
                        case "SYSTEM.STRING":
                            valueArray[x] = (string)this.value[x];
                            break;
                        case "SYSTEM.INT32":
                            valueArray[x] = (int)this.value[x];
                            break;
                        case "SYSTEM.INT64":
                            valueArray[x] = (long)this.value[x];
                            break;
                        case "SYSTEM.FLOAT":
                            valueArray[x] = (float)this.value[x];
                            break;
                        case "SYSTEM.DOUBLE":
                            valueArray[x] = (double)this.value[x];
                            break;
                        default:
                            valueArray[x] = (string)this.value[x];
                            break;
                    }*/

                    valueArray[x] = this.ObtainValue(this.value[x]);
                }

                return new ReadOnlyCollection<object>(valueArray); // .ToList();
            }
        }
        
        /// <summary>Gets JavaScript representation of rule definition</summary>
        [JsonIgnore]
        public string ValidatorRules
        {
            get
            {
                switch (this.Rule)
                {
                    case ValidatorRule.MinValue:
                        return string.Format(CultureInfo.InvariantCulture, "min: {0}", this.value[0].ToString().Replace(",", "."));
                    case ValidatorRule.MaxValue:
                        return string.Format(CultureInfo.InvariantCulture, "max: {0}", this.value[0].ToString().Replace(",", "."));
                    case ValidatorRule.RangeValue:
                        return string.Format(CultureInfo.InvariantCulture, "range: [{0}, {1}]", this.value[0].ToString().Replace(",", "."), this.value[1].ToString().Replace(",", "."));
                    case ValidatorRule.MinDate:
                        return string.Format(CultureInfo.InvariantCulture, @"minDate: ""{0}""", this.value[0]);
                    case ValidatorRule.MaxDate:
                        return string.Format(CultureInfo.InvariantCulture, @"maxDate: ""{0}""", this.value[0]);
                    case ValidatorRule.RangeDate:
                        return string.Format(CultureInfo.InvariantCulture, @"rangeDate: [""{0}"",""{1}""]", this.value[0], this.value[1]);
                    case ValidatorRule.MinToday:
                        return @"minDate: ""TODAY""";
                    case ValidatorRule.MaxToday:
                        return @"maxDate: ""TODAY""";
                    case ValidatorRule.MinSelected:
                        return string.Format(CultureInfo.InvariantCulture, "minSelected: [{0}, {1}]", this.value[0], this.RenderFieldNames());
                    case ValidatorRule.MaxSelected:
                        return string.Format(CultureInfo.InvariantCulture, "maxSelected: [{0}, {1}]", this.value[0], this.RenderFieldNames());
                    case ValidatorRule.OnlyNumbers:
                        return "number:true";
                    case ValidatorRule.Email:
                        return "emailValidator: true";
                    case ValidatorRule.MinLength:
                        return string.Format(CultureInfo.InvariantCulture, "minlength: {0}", this.value[0]);
                    case ValidatorRule.MaxLength:
                        return string.Format(CultureInfo.InvariantCulture, "maxlength: {0}", this.value[0]);
                    case ValidatorRule.DateLessThanField:
                        return string.Format(CultureInfo.InvariantCulture, @"maxDate: ['FIELD', '{0}']", this.value[0]);
                    case ValidatorRule.DateMoreThanField:
                        return string.Format(CultureInfo.InvariantCulture, @"minDate: ['FIELD', '{0}']", this.value[0]);
                    case ValidatorRule.NumberMinorThanField:
                        return string.Format(CultureInfo.InvariantCulture, @"numberLessThanField: '{0}'", this.value[0]);
                    case ValidatorRule.NumberMajorThanField:
                        return string.Format(CultureInfo.InvariantCulture, @"numberMoreThanField: '{0}'", this.value[0]);
                    case ValidatorRule.MinLengthDecimal:
                        return string.Format(CultureInfo.InvariantCulture, @"minLengthDecimal: [{0}, {1}]", this.value[0], this.value[1]);
                    case ValidatorRule.MaxLengthDecimal:
                        return string.Format(CultureInfo.InvariantCulture, @"maxLengthDecimal: [{0}, {1}]", this.value[0], this.value[1]);
                }

                return string.Empty;
            }
        }

        /// <summary>Gets a JavaScript representation of validator error message</summary>
        [JsonIgnore]
        public string ValidatorMessages
        {
            get
            {
                switch (this.Rule)
                {
                    case ValidatorRule.MinValue:
                        return string.Format(CultureInfo.InvariantCulture, @"min: ""El valor ha de ser mayor a {0}""", this.value[0]);
                    case ValidatorRule.MaxValue:
                        return string.Format(CultureInfo.InvariantCulture, @"max: ""El valor ha de ser menor a {0}""", this.value[0]);
                    case ValidatorRule.RangeValue:
                        return string.Format(CultureInfo.InvariantCulture, @"range: ""El valor ha de estar entre {0} y {1} ambos incluidos""", this.value[0], this.value[1]);
                    case ValidatorRule.MinDate:
                        return string.Format(CultureInfo.InvariantCulture, @"minDate: ""La fecha ha de ser superior a {0}""", this.value[0]);
                    case ValidatorRule.MaxDate:
                        return string.Format(CultureInfo.InvariantCulture, @"maxDate: ""La fecha ha de ser inferior a {0}""", this.value[0]);
                    case ValidatorRule.RangeDate:
                        return string.Format(CultureInfo.InvariantCulture, @"rangeDate: ""La fecha ha de ser mayor a {0} y menor a {1}""", this.value[0], this.value[1]);
                    case ValidatorRule.MinToday:
                        return @"minDate: ""La fecha ha de ser superior a hoy""";
                    case ValidatorRule.MaxToday:
                        return @"maxDate: ""La fecha ha de ser inferior a hoy""";
                    case ValidatorRule.MinSelected:
                        return string.Format(CultureInfo.InvariantCulture, @"minSelected: ""Se han de seleccionar al menos {0} opciones""", this.value[0]);
                    case ValidatorRule.MaxSelected:
                        return string.Format(CultureInfo.InvariantCulture, @"maxSelected: ""Se han de seleccionar {0} opciones o menos""", this.value[0]);
                    case ValidatorRule.OnlyNumbers:
                        return @"number: ""Sólo números (0-9)""";
                    case ValidatorRule.Email:
                        return @"emailValidator: ""Debe introducirse una dirección de email válida""";
                    case ValidatorRule.MinLength:
                        return string.Format(CultureInfo.InvariantCulture, @"minlength: ""Se deben usar {0} o más carácteres""", this.value[0]);
                    case ValidatorRule.MaxLength:
                        return string.Format(CultureInfo.InvariantCulture, @"maxlength: ""Se deben usar {0} carácteres o menos""", this.value[0]);
                    case ValidatorRule.DateLessThanField:
                        return string.Format(CultureInfo.InvariantCulture, @"maxDate: ""La fecha debe ser inferior a {0}""", this.value[0]);
                    case ValidatorRule.DateMoreThanField:
                        return string.Format(CultureInfo.InvariantCulture, @"minDate: ""La fecha debe ser superior a {0}""", this.value[0]);
                    case ValidatorRule.NumberMinorThanField:
                        return string.Format(CultureInfo.InvariantCulture, @"numberLessThanField: ""El valor ha de ser inferior a {0}""", this.value[0]);
                    case ValidatorRule.NumberMajorThanField:
                        return string.Format(CultureInfo.InvariantCulture, @"numberMoreThanField: ""El valor ha de ser superior a {0}""", this.value[0]);
                    case ValidatorRule.MinLengthDecimal:
                        return string.Format(CultureInfo.InvariantCulture, @"minLengthDecimal: ""{0} dígitos en la parte real y {1} en la decimal mínimo""", this.value[0], this.value[1]);
                    case ValidatorRule.MaxLengthDecimal:
                        return string.Format(CultureInfo.InvariantCulture, @"maxLengthDecimal: ""{0} dígitos en la parte real y {1} en la decimal máximo""", this.value[0], this.value[1]);
                }

                return string.Empty;
            }
        }

        /// <summary>Gets groups of validation</summary>
        [JsonIgnore]
        public string ValidatorGroups
        {
            get
            {
                StringBuilder groupName = new StringBuilder();
                StringBuilder groupValue = new StringBuilder("\"");
                bool first = true;

                foreach (string name in this.fieldNames)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        groupValue.Append(" ");
                    }

                    groupName.AppendFormat(CultureInfo.GetCultureInfo("en-us"), @"{0}", name);
                    groupValue.AppendFormat(CultureInfo.GetCultureInfo("en-us"), @"(ITEMNAME)Txt{0}", name);
                }

                groupValue.Append("\"");
                switch (this.Rule)
                {
                    case ValidatorRule.MinSelected:
                    case ValidatorRule.MaxSelected:
                        return string.Format(CultureInfo.InvariantCulture, @"{0}: {1}", groupName.ToString(), groupValue.ToString());
                }

                return string.Empty;
            }
        }

        /// <summary>Gets placement of error for validation groups</summary>
        [JsonIgnore]
        public string ErrorPlacementGroups
        {
            get
            {                
                switch (this.Rule)
                {
                    case ValidatorRule.MinSelected:
                    case ValidatorRule.MaxSelected:
                        StringBuilder group = new StringBuilder("else if(");
                        bool first = true;
                        foreach (string name in this.fieldNames)
                        {
                            if (first)
                            {
                                first = false; 
                            }
                            else
                            {
                                group.Append("||");
                            }

                            group.AppendFormat(CultureInfo.GetCultureInfo("en-us"), @"element[0].id === ""(ITEMNAME)Txt{0}""", name);
                        }

                        group.AppendFormat(CultureInfo.GetCultureInfo("en-us"), @"){{ $(""#{0}"").append('<br \>').append(error);}}", this.Name, Environment.NewLine);
                        return group.ToString();
                }

                return string.Empty;
            }
        }

        /// <summary>Gets the rule name</summary>
        [JsonIgnore]
        public string Name
        {
            get
            {
                StringBuilder res = new StringBuilder();
                if (this.fieldNames.Count > 0 && !string.IsNullOrEmpty(this.RuleName))
                {
                    res.Append(this.RuleName);
                    foreach (string name in this.fieldNames)
                    {
                        res.Append(name);
                    }
                }

                return res.ToString();
            }
        }

        /// <summary>Gets custom name of rule</summary>
        [JsonIgnore]
        public string CustomName
        {
            get
            {
                if (string.IsNullOrEmpty(this.customName))
                {
                    return this.Name;
                }

                return this.customName;
            }
        }

        /// <summary>Gets typed values for validation control</summary>
        /// <param name="value">Object to get typed value</param>
        /// <returns>Typed value</returns>
        private object ObtainValue(object value)
        {
            if (value == null)
            {
                return null;
            }

            string dataType = value.GetType().ToString().ToUpperInvariant();
            switch (dataType)
            {
                case "SYSTEM.STRING":
                    return (string)value;
                case "SYSTEM.INT32":
                    return (int)value;
                case "SYSTEM.INT64":
                    return (long)value;
                case "SYSTEM.FLOAT":
                    return (float)value;
                case "SYSTEM.DOUBLE":
                    return (double)value;
                default:
                    return (string)value;
            }
        }

        /// <summary>Creates a JSON list of the names of fields affected by rule</summary>
        /// <returns>A JSON list of the names of fields affected by rule</returns>
        private string RenderFieldNames()
        {
            StringBuilder renderNames = new StringBuilder();
            renderNames.Append("[ ");
            bool first = true;
            foreach (string name in this.fieldNames)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    renderNames.Append(",");
                }

                renderNames.AppendFormat(CultureInfo.GetCultureInfo("en-us"), @"""{0}""", name);            
            }

            renderNames.Append(" ]");
            return renderNames.ToString();
        }
    }
}