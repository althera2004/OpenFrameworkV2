// --------------------------------
// <copyright file="ItemDefinitionLayout.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager
{
    using Newtonsoft.Json;

    /// <summary>Definition for item layout</summary>
    public sealed class ItemDefinitionLayout
    {
        /// <summary>Edition type</summary>
        [JsonProperty("EditionType")]
        private string editionType;

        /// <summary>Gets an empty instance of an ItemDefinitionLayout class</summary>
        [JsonIgnore]
        public static ItemDefinitionLayout Empty
        {
            get
            {
                return new ItemDefinitionLayout()
                {
                    Icon = string.Empty,
                    Label = string.Empty,
                    LabelPlural = string.Empty
                };
            }
        }

        /// <summary>Gets or sets the icon that represents the item</summary>
        [JsonProperty("Icon")]
        public string Icon { get; set; }

        /// <summary>Gets or sets the text form item label</summary>
        [JsonProperty("Label")]
        public string Label { get; set; }

        /// <summary>Gets or sets the text for item plural label</summary>
        [JsonProperty("LabelPlural")]
        public string LabelPlural { get; set; }

        /// <summary>Gets or sets the descriptions that represents the item</summary>
        [JsonProperty("Description")]
        public ItemDescription Description { get; set; }

        /// <summary>Gets the item's edition type</summary>
        [JsonIgnore]
        public EditionType EditionType
        {
            get
            {
                if (string.IsNullOrEmpty(this.editionType))
                {
                    return EditionType.CustomForm;
                }

                switch (this.editionType.ToUpperInvariant())
                {
                    case "POPUP":
                        return EditionType.Popup;
                    case "INLINE":
                        return EditionType.Inline;
                    case "CUSTOMFORM":
                    default:
                        return EditionType.CustomForm;
                }
            }
        }
    }
}
