// --------------------------------
// <copyright file="ListDefinition.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager.List
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;
    using OpenFramework.Core.ItemManager.List;

    /// <summary>Implements ListDefinition class</summary>
    public sealed class ListDefinition
    {
        /// <summary>Edit action</summary>
        [JsonProperty("GridMode")]
        private string gridMode;

        /// <summary>Edit action</summary>
        [JsonProperty("EditAction")]
        private string editAction;

        /// <summary>List's columns</summary>
        [JsonProperty("Columns")]
        private Column[] columns;

        /// <summary>Configuration of sorting of list's columns</summary>
        [JsonProperty("Sorting")]
        private Sorting[] sorting;

        [JsonProperty("Parameters")]
        private ListParameter[] parameters;

        /// <summary>List buttons</summary>
        [JsonProperty("Buttons")]
        private List<ListButtonDefinition> buttons;

        /// <summary>List layout</summary>
        [JsonProperty("Layout")]
        private string layout;

        /// <summary>Data origin</summary>
        [JsonProperty("DataOrigin")]
        private string dataOrigin;

        /// <summary>Indicates if list is readonly</summary>
        [JsonProperty("ReadOnly")]
        private string readOnly;

        /// <summary>List form identifier</summary>
        [JsonProperty("FormId")]
        private string formId;

        /// <summary>Indicates if list has gelolocation</summary>
        [JsonProperty("GeoLocation")]
        private string geoLocation;

        /// <summary>Gets an empty list definition</summary>
        [JsonIgnore]
        public static ListDefinition Empty
        {
            get
            {
                return new ListDefinition()
                {
                    Id = string.Empty,
                    columns = null,
                    sorting = null,
                    editAction = string.Empty,
                    layout = string.Empty
                };
            }
        }

        /// <summary>Gets a value indicating whether list is readonly</summary>
        [JsonIgnore]
        public bool ReadOnly
        {
            get
            {
                if (string.IsNullOrEmpty(this.readOnly))
                {
                    return false;
                }

                if (this.readOnly.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>Gets data origin</summary>
        [JsonIgnore]
        public DataOrigin Type
        {
            get
            {
                if (string.IsNullOrEmpty(this.dataOrigin))
                {
                    return DataOrigin.Native;
                }

                switch (this.dataOrigin.ToUpperInvariant())
                {
                    case ConstantValue.OriginNative: return DataOrigin.Native;
                    case ConstantValue.OriginNavision: return DataOrigin.Navision;
                    case ConstantValue.OriginCRM: return DataOrigin.CRM;
                    case ConstantValue.OriginSAP: return DataOrigin.SAP;
                }

                return DataOrigin.Undefined;
            }
        }

        /// <summary>Gets a value indicating whether if list has geolocation</summary>
        [JsonIgnore]
        public bool GeoLocation
        {
            get
            {
                if (string.IsNullOrEmpty(this.geoLocation))
                {
                    return false;
                }

                return this.geoLocation.Equals("true", StringComparison.OrdinalIgnoreCase);
            }
        }

        /// <summary>Gets the buttons of list</summary>
        [JsonIgnore]
        public ReadOnlyCollection<ListButtonDefinition> Buttons
        {
            get
            {
                if (this.buttons == null || this.buttons.Count == 0)
                {
                    return new ReadOnlyCollection<ListButtonDefinition>(new List<ListButtonDefinition>());
                }

                return new ReadOnlyCollection<ListButtonDefinition>(this.buttons);
            }
        }

        /// <summary>Gets or sets list identifier</summary>
        [JsonProperty("Id")]
        public string Id { get; set; }

        /// <summary>Gets or sets a custom AJAX source</summary>
        [JsonProperty("CustomAjaxSource")]
        public string CustomAjaxSource { get; set; }

        [JsonIgnore]
        public bool GridMode
        {
            get
            {
                if (string.IsNullOrEmpty(this.gridMode))
                {
                    return false;
                }

                if(this.gridMode.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>Gets the form identifier</summary>
        [JsonIgnore]
        public string FormId
        {
            get
            {
                if (string.IsNullOrEmpty(this.formId))
                {
                    return this.Id;
                }

                return this.formId;
            }
        }

        /// <summary>Gets the edit action</summary>
        [JsonIgnore]
        public EditMode EditAction
        {
            get
            {
                if (string.IsNullOrEmpty(this.editAction))
                {
                    return EditMode.Undefined;
                }

                switch (this.editAction.ToUpperInvariant())
                {
                    case "1":
                        return EditMode.Popup;
                    case "2":
                        return EditMode.Inline;
                    case "3":
                        return EditMode.Custom;
                    case "4":
                        return EditMode.Inform;
                    case "5":
                        return EditMode.Capture;
                    case "6":
                        return EditMode.ReadOnly;
                    default:
                        return EditMode.Undefined;
                }
            }
        }

        /// <summary>Gets the list's layout</summary>
        [JsonIgnore]
        public ListLayout Layout
        {
            get
            {
                if (this.layout == null)
                {
                    return ListLayout.Editable;
                }

                switch (this.layout)
                {
                    case "3":
                        return ListLayout.NoButton;
                    case "2":
                        return ListLayout.Linkable;
                    case "1":
                    default:
                        return ListLayout.Editable;
                    case "0":
                        return ListLayout.ReadOnly;
                }
            }
        }

        /*[JsonIgnore]
        public ListPosition Position
        {
            get
            {
                if (string.IsNullOrEmpty(this.position))
                {
                    return ListPosition.Embedded;
                }

                switch (this.position)
                {
                    case "1":
                        return ListPosition.Widget;
                    case "0":
                    default:
                        return ListPosition.Embedded;
                }
            }
        }*/

        /// <summary>Gets the columns of list</summary>
        [JsonIgnore]
        public ReadOnlyCollection<Column> Columns
        {
            get
            {
                if (this.columns == null)
                {
                    return new ReadOnlyCollection<Column>(new List<Column>());
                }

                return new ReadOnlyCollection<Column>(this.columns);
            }
        }

        /// <summary> Gets the sorting configuration of columns</summary>
        [JsonIgnore]
        public ReadOnlyCollection<Sorting> Sorting
        {
            get
            {
                if (this.sorting == null)
                {
                    return new ReadOnlyCollection<Sorting>(new List<Sorting>());                    
                }

                return new ReadOnlyCollection<Sorting>(this.sorting);
            }
        }

        /// <summary> Gets the parameters of list</summary>
        [JsonIgnore]
        public ReadOnlyCollection<ListParameter> Parameters
        {
            get
            {
                if (this.parameters == null)
                {
                    return new ReadOnlyCollection<ListParameter>(new List<ListParameter>());
                }

                return new ReadOnlyCollection<ListParameter>(this.parameters);
            }
        }
    }
}
