// --------------------------------
// <copyright file="FormFilterFieldDefinition.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.Core.ItemManager.Form
{
    using Newtonsoft.Json;

    /// <summary>Implements definition for form fields</summary>
    public sealed class FormFilterFieldDefinition
    {
        /// <summary>Field value</summary>
        [JsonProperty("Value")]
        private string value;

        /// <summary>Local field value</summary>
        [JsonProperty("LocalValue")]
        private string localValue;

        /// <summary>Gets the name of field</summary>
        [JsonProperty("Field")]
        public string Field { get; private set; }

        /// <summary>Gets field value</summary>
        [JsonIgnore]
        public string Value
        {
            get
            {
                if (string.IsNullOrEmpty(this.value))
                {
                    return string.Empty;
                }

                return this.value;
            }
        }

        /// <summary>Gets local field value</summary>
        [JsonIgnore]
        public string LocalValue
        {
            get
            {
                if (string.IsNullOrEmpty(this.localValue))
                {
                    return string.Empty;
                }

                return this.localValue;
            }
        }
    }
}