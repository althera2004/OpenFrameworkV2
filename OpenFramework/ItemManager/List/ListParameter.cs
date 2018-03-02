// --------------------------------
// <copyright file="ListParameter.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.Core.ItemManager.List
{
    using Newtonsoft.Json;

    /// <summary>Implements parameters for list</summary>
    public sealed class ListParameter
    {
        /// <summary>Gets or sets parameter name</summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>Gets or sets parameter value</summary>
        [JsonProperty("Value")]
        public string Value { get; set; }
    }
}