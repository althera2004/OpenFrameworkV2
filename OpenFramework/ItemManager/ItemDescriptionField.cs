// --------------------------------
// <copyright file="ItemDescriptionField.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón -  jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager
{    
    using Newtonsoft.Json;

    /// <summary>Description field of item</summary>
    public sealed class ItemDescriptionField
    {
        /// <summary>Gets or sets the name of item description field</summary>
        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}
