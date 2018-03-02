// --------------------------------
// <copyright file="FormRowDefinition.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager.Form
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Newtonsoft.Json;
    using OpenFramework.Core.ItemManager.Form;
    using OpenFramework.ItemManager;

    /// <summary>Implements form wor definition</summary>
    public sealed class FormRowDefinition
    {
        /// <summary>Checks if the row is collapsible</summary>
        [JsonProperty("Id")]
        private string id;

        /// <summary>Label of row</summary>
        [JsonProperty("Label")]
        private string label;

        /// <summary>List of fields contained in this row</summary>
        [JsonProperty("Fields")]
        private List<FormFieldDefinition> fields;

        /// <summary>Checks if the row is collapsible</summary>
        [JsonProperty("Collapsible")]
        private bool collapsible;

        /// <summary>Name of the item to render a list (if current row is a list)</summary>
        [JsonProperty("ItemList")]
        private string itemList;

        /// <summary>List of filters for the list (if current row is a list)</summary>
        [JsonProperty("FilterFields")]
        private List<FormFilterFieldDefinition> filterFields;

        /// <summary>Id of the list (if the current row is a list)</summary>
        [JsonProperty("ListId")]
        private string listId;

        /// <summary>Name of the item of the form to render (if the current row is a form inform)</summary>
        [JsonProperty("ItemForm")]
        private string itemForm;

        /// <summary>Public access to label</summary>
        [JsonIgnore]
        public string Id
        {
            get
            {
                if (string.IsNullOrEmpty(this.id))
                {
                    return string.Empty;
                }

                return this.id;
            }
        }

        /// <summary>Gets label of row</summary>
        [JsonIgnore]
        public string Label
        {
            get
            {
                if (string.IsNullOrEmpty(this.label))
                {
                    return string.Empty;
                }

                return this.label;
            }
        }

        /// <summary>Gets a value indicating whether row is collapsible</summary>
        [JsonIgnore]
        public bool Collapsible
        {
            get
            {
                return this.collapsible;
            }
        }

        /// <summary>Custom AJAX source for the list (if the current row is a list)</summary>
        [JsonProperty("CustomAjaxSource")]
        public string CustomAjaxSource { get; set; }

        [JsonProperty("TableAlias")]
        public string TableAlias { get; set; }

        /// <summary>Gets list identifier</summary>
        [JsonIgnore]
        public string ListId
        {
            get
            {
                if (string.IsNullOrEmpty(this.listId))
                {
                    return "INFORM";
                }

                return this.listId;
            }
        }

        /// <summary>Gets filter fields</summary>
        [JsonIgnore]
        public ReadOnlyCollection<FormFilterFieldDefinition> FilterFields
        {
            get
            {
                if (this.filterFields == null)
                {
                    return new ReadOnlyCollection<FormFilterFieldDefinition>(new List<FormFilterFieldDefinition>());
                }

                return new ReadOnlyCollection<FormFilterFieldDefinition>(filterFields);
            }
        }

        /// <summary>Gets a value indicating the name of the dependencies to populate list</summary>
        [JsonProperty("LocalData")]
        public string LocalData { get; set; }

        /// <summary>Gets item builder of list</summary>
        [JsonIgnore]
        public string ItemList
        {
            get
            {
                if (string.IsNullOrEmpty(this.itemList))
                {
                    return string.Empty;
                }

                return this.itemList;
            }
        }

        /// <summary>Checks if the current row is a list</summary>
        [JsonIgnore]
        public bool IsItemList
        {
            get
            {
                return !string.IsNullOrEmpty(this.itemList);
            }
        }

        /// <summary>Public access to itemForm</summary>
        [JsonIgnore]
        public string ItemForm
        {
            get
            {
                if (string.IsNullOrEmpty(this.itemForm))
                {
                    return string.Empty;
                }

                return this.itemForm;
            }
        }

        /// <summary>Checks if the current row is a form inform</summary>
        [JsonIgnore]
        public bool IsItemForm
        {
            get
            {
                if (string.IsNullOrEmpty(this.itemForm))
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>Size of the row (unused)</summary>
        [JsonProperty("Size")]
        private int size;

        /// <summary>Public access to size (unused)</summary>
        [JsonIgnore]
        public int Size
        {
            get
            {
                return this.size;
            }
        }

        /// <summary>Function that returns the ItemField objects corresponding to the fields of this row</summary>
        /// <param name="ItemFields">List of fields (of the item)</param>
        /// <returns>Dictionary <field, field definition></returns>
        public ReadOnlyDictionary<ItemField, FormFieldDefinition> GetFields(ReadOnlyCollection<ItemField> ItemFields)
        {
            if (this.fields == null || ItemFields == null)
            {
                return new ReadOnlyDictionary<ItemField, FormFieldDefinition>(new Dictionary<ItemField, FormFieldDefinition>());
            }

            Dictionary<ItemField, FormFieldDefinition> res = new Dictionary<ItemField, FormFieldDefinition>();
            
            // For all fields in the row
            foreach (FormFieldDefinition fieldDefinition in this.fields)
            {
                // If any of the ItemFields matches the current field...
                if (ItemFields.Any(f => f.Name == fieldDefinition.Name))
                {
                    // Add the field to the list
                    res.Add(ItemFields.Where(f => f.Name == fieldDefinition.Name).First(), fieldDefinition);
                }
                else
                {
                    // Create an empty field
                    ItemField field = ItemField.Empty;

                    // Add the name and the label from the definition
                    if (!string.IsNullOrEmpty(fieldDefinition.Name) && !string.IsNullOrEmpty(fieldDefinition.Label))
                    {
                        field.Name = fieldDefinition.Name;
                        field.Label = fieldDefinition.Label;
                    }

                    // Add the new field
                    res.Add(field, fieldDefinition);
                }
            }

            return new ReadOnlyDictionary<ItemField, FormFieldDefinition>(res);
        }
    }
}
