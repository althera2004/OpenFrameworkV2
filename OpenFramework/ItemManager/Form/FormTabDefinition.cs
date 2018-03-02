// --------------------------------
// <copyright file="FormTabDefinition.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.Core.ItemManager.Form
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;
    using OpenFramework.ItemManager.Form;

    /// <summary>
    /// Implements form tab definition
    /// </summary>
    public sealed class FormTabDefinition
    {
        /// <summary>Definition of the rows that will be contained inside of the tab</summary>
        [JsonProperty("Rows")]
        private List<FormRowDefinition> rows;

        /// <summary>Size of the tab (unused)</summary>
        [JsonProperty("Size")]
        private int size;

        /// <summary>Checks if the current tab requires an item instance to make it selectable</summary>
        [JsonProperty("ItemRequired")]
        private bool? itemRequired;

        /// <summary>Gets or sets the tab label</summary>
        [JsonProperty("Label")]
        public string Label { get; set; }

        /// <summary>Gets a value indicating whether is read only</summary>
        [JsonIgnore]
        public bool ItemRequired
        {
            get
            {
                // By default return false
                if (this.itemRequired == null)
                {
                    return false;
                }

                return (bool)this.itemRequired;
            }
        }

        /*/// <summary>Gets a value indicating the row size (unused)</summary>
        [JsonIgnore]
        public int Size
        {
            get
            {
                // By default, return 12
                if (this.size == 0)
                {
                    return 12;
                }

                return this.size;
            }
        }*/

        /// <summary>Gets the rows of form</summary>
        [JsonIgnore]
        public ReadOnlyCollection<FormRowDefinition> Rows
        {
            get
            {
                if (this.rows == null || this.rows.Count == 0)
                {
                    return new ReadOnlyCollection<FormRowDefinition>(new List<FormRowDefinition>());
                }

                return new ReadOnlyCollection<FormRowDefinition>(this.rows);
            }
        }
    }
}
