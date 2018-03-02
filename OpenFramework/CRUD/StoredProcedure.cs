// --------------------------------
// <copyright file="StoredProcedure.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.CRUD
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;

    /// <summary>Implements stored procedure class</summary>
    [Serializable]
    public sealed class StoredProcedure
    {
        /// <summary>Stored parameters</summary>
        [JsonProperty("Parameters")]
        private StoredParameter[] parameters;

        /// <summary>Gets an empty stored procedure</summary>
        [JsonIgnore]
        public static StoredProcedure Empty
        {
            get
            {
                return new StoredProcedure()
                {
                    StoredName = string.Empty
                };
            }
        }

        /// <summary>Gets or sets the name of stored procedure</summary>
        [JsonProperty("Stored")]
        public string StoredName { get; set; }

        /// <summary>Gets the stored parameters</summary>
        [JsonIgnore]
        public ReadOnlyCollection<StoredParameter> Parameters
        {
            get
            {
                if (this.parameters == null || this.parameters.Length == 0)
                {
                    return new ReadOnlyCollection<StoredParameter>(new List<StoredParameter>());
                }

                return new ReadOnlyCollection<StoredParameter>(this.parameters);
            }
        }
    }
}