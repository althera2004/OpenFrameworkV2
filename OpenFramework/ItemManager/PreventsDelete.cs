// --------------------------------
// <copyright file="PreventsDelete.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager
{
    using System;
    using Newtonsoft.Json;

    /// <summary>Prevents delete item by dependency</summary>
    [Serializable]
    public sealed class PreventsDelete
    {
        /// <summary>Item name that depens delete option</summary>
        [JsonProperty("ItemName")]
        public string ItemName { get; set; }

        /// <summary>Foreign field that depens delete option</summary>
        [JsonProperty("Foreign")]
        public string ForeignField { get; set; }
    }
}
