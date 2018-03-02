// --------------------------------
// <copyright file="Sorting.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager.List
{
    using Newtonsoft.Json;

    /// <summary>Implements sorting configuration of column</summary>
    public sealed class Sorting
    {
        /// <summary>Gets or sets sorting type</summary>
        [JsonProperty("SortingType")]
        public string SortingType { get; set; }

        /// <summary>Gets or sets the field/column name</summary>
        [JsonProperty("Index")]
        public int Index { get; set; }

        /*/// <summary>Gets column sorting type</summary>
        [JsonIgnore]
        public SortingType SortingType
        {
            get
            {
                if (this.sortingType == null)
                {
                    return SortingType.Unsorting;
                }

                switch (this.sortingType.ToUpperInvariant())
                {
                    case ConstantValue.SortAscendent:
                    case ConstantValue.SortAscendentShort:
                        return List.SortingType.Ascendent;
                    case ConstantValue.SortDescendent:
                    case ConstantValue.SortDescendentShort:
                        return List.SortingType.Descendent;
                    default:
                        return List.SortingType.Unsorting;
                }
            }
        }*/
    }
}