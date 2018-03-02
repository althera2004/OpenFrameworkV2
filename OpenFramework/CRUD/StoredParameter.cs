// --------------------------------
// <copyright file="StoredParameter.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.CRUD
{
    using System;
    using Newtonsoft.Json;

    /// <summary>Implements StoredParameter class</summary>
    [Serializable]
    public sealed class StoredParameter
    {
        /// <summary>Parameter name</summary>
        [JsonProperty("Parameter")]
        private string parameter;

        /// <summary>Name of field</summary>
        [JsonProperty("Field")]
        private string field;

        /// <summary>Gets the name of parameter</summary>
        [JsonIgnore]
        public string Parameter
        {
            get
            {
                if (string.IsNullOrEmpty(this.parameter))
                {
                    return string.Empty;
                }

                return this.parameter;
            }
        }

        /// <summary>Gets the name of item field</summary>
        [JsonIgnore]
        public string Field
        {
            get
            {
                if (string.IsNullOrEmpty(this.field))
                {
                    return string.Empty;
                }

                return this.field;
            }
        }
    }
}