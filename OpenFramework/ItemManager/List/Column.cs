// --------------------------------
// <copyright file="Column.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager.List
{
    using Newtonsoft.Json;

    /// <summary>Implements Column class that represents a DataTable column for show ItemBuilder data in lists pages</summary>
    [JsonObject("Columns")]
    public sealed class Column
    {
        /// <summary>Gets or sets de CSS class of column content</summary>
        [JsonProperty("ClassName")]
        public string ClassName { get; set; }

        /// <summary>Gets or sets a value indicating whether if column is orderable</summary>
        [JsonProperty("Orderable")]
        public bool Orderable { get; set; }

        /// <summary>Gets or sets a value indicating whether if column is orderable</summary>
        [JsonProperty("HiddenMobile")]
        public bool HiddenMobile { get; set; }

        /// <summary>Gets or sets de data column</summary>
        [JsonProperty("Data")]
        public object Data { get; set; }

        /// <summary>Gets or sets de default content of column if data no exists</summary>
        [JsonProperty("DefaultContent")]
        public string DefaultContent { get; set; }

        /// <summary>Gets or sets the width of column</summary>
        [JsonProperty("Width")]
        public int Width { get; set; }

        /// <summary>Gets or sets a value indicating whether if column can be hide</summary>
        [JsonProperty("HideShowExclude")]
        public bool HideShowExclude { get; set; }

        /// <summary>Gets or sets de field identifier of source data to extract column content</summary>
        [JsonProperty("DataProperty")]
        public string DataProperty { get; set; }

        /// <summary>Gets or sets a function to render column content</summary>
        [JsonProperty("Render")]
        public string Render { get; set; }

        /// <summary>Gets or sets a field to replace actual</summary>
        [JsonProperty("ReplacedBy")]
        public string ReplacedBy { get; set; }

        /// <summary>Gets or sets specif type of column data</summary>
        [JsonProperty("DataType")]
        public string DataType { get; set; }

        /// <summary>Gets or sets the column title</summary>
        [JsonProperty("Title")]
        public string Title { get; set; }
    }
}