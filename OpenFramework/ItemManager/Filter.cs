// --------------------------------
// <copyright file="Filter.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón -  jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager
{ 
    using System;
    using Newtonsoft.Json;

    [Serializable]
    public sealed class Filter
    {
        /// <summary>Name of field</summary>
        [JsonProperty("FieldName")]
        public string FieldName;

        /// <summary>condition of filter</summary>
        [JsonProperty("Condition")]
        public Condition Condition { get; set; }
    }
}