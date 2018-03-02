// --------------------------------
// <copyright file="ListButtonDefinition.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.Core.ItemManager.List
{
    using System.Globalization;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>Implements buttons for list</summary>
    public sealed class ListButtonDefinition
    {
        /// <summary>Button's function</summary>
        [JsonProperty("Function")]
        private string function;

        /// <summary>Values for button function</summary>
        [JsonProperty("FunctionValues")]
        private object[] functionValues;

        /// <summary>Gets the button label</summary>
        [JsonProperty("Label")]
        public string Label { get; private set; }

        /// <summary>Gets the name of button</summary>
        [JsonProperty("Name")]
        public string Name { get; private set; }

        /// <summary>Gets the function attached to button</summary>
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

                        string value = val.GetType().ToString().ToUpperInvariant() == "SYSTEM.STRING" ? string.Format(CultureInfo.InvariantCulture, @"'{0}'", val) : val.ToString();
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
    }
}