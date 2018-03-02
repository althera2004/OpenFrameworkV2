// --------------------------------
// <copyright file="FormDefinition.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager.Form
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;
    using OpenFramework.Core.ItemManager.Form;
    using OpenFramework.ItemManager.List;

    /// <summary>Implements form definition</summary>
    public sealed class FormDefinition
    {
        /// <summary>Identifier of the form</summary>
        [JsonProperty("Id")]
        private string id;

        /// <summary>Form type</summary>
        [JsonProperty("FormType")]
        private string formType;

        /// <summary>Checks if the form must have an accept button on the footer</summary>
        [JsonProperty("Footer")]
        private bool? footer;

        /// <summary>List of tabs of the form</summary>
        [JsonProperty("Tabs")]
        private List<FormTabDefinition> tabs;

        /// <summary>Gets an empty form definition</summary>
        [JsonIgnore]
        public static FormDefinition Empty
        {
            get
            {
                return new FormDefinition();
            }
        }

        /// <summary>Gets form identifier</summary>
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

        /// <summary>Gets form type</summary>
        [JsonIgnore]
        public EditMode FormType
        {
            get
            {
                if (string.IsNullOrEmpty(this.formType))
                {
                    return EditMode.Undefined;
                }

                switch (this.formType.ToUpperInvariant())
                {
                    case "POPUP":
                        return EditMode.Popup;
                    case "CUSTOM":
                        return EditMode.Custom;
                    case "INFORM":
                        return EditMode.Inform;
                    default:
                        return EditMode.Undefined;
                }
            }
        }

        /// <summary>Gets form tabs</summary>
        [JsonIgnore]
        public ReadOnlyCollection<FormTabDefinition> Tabs
        {
            get
            {
                if (this.tabs == null)
                {
                    return new ReadOnlyCollection<FormTabDefinition>(new List<FormTabDefinition>());
                }

                return new ReadOnlyCollection<FormTabDefinition>(this.tabs);
            }
        }

        /// <summary>Gets form label</summary>
        [JsonProperty("Label")]
        public string Label { get; private set; }

        /// <summary>Gets a value indicating whether form footer</summary>
        [JsonIgnore]
        public bool Footer
        {
            get
            {
                // By default, return true
                if (this.footer == null)
                {
                    return true;
                }

                return (bool)this.footer;
            }
        }
    }
}
