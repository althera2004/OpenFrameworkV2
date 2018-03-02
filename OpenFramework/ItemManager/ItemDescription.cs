// --------------------------------
// <copyright file="ItemDescription.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;

    /// <summary>
    /// Implements item description Class
    /// </summary>
    public sealed class ItemDescription
    {
        /// <summary>List of fields that composes description</summary>
        [JsonProperty("Fields")]
        private ItemDescriptionField[] fields;

        /// <summary>Gets or sets the pattern to build description</summary>
        [JsonProperty("Pattern")]
        public string Pattern { get; set; }

        /// <summary>Gets the list of fields that composes description</summary>
        [JsonIgnore]
        public ReadOnlyCollection<ItemDescriptionField> Fields
        {
            get
            {
                if (this.fields == null)
                {
                    return new ReadOnlyCollection<ItemDescriptionField>(new List<ItemDescriptionField>());
                }

                return new ReadOnlyCollection<ItemDescriptionField>(this.fields);
            }
        }
    }
}
