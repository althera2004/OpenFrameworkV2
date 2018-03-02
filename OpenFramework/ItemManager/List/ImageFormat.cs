// --------------------------------
// <copyright file="ImageFormat.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.Core.ItemManager.List
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;

    /// <summary>Implements image format in list</summary>
    public sealed class ImageFormat
    {
        /// <summary>Extension permitted for image</summary>
        [JsonProperty("Extensions")]
        private object[] extensions;

        /// <summary>Gets or sets the maximum length of image</summary>
        [JsonProperty("MaxLength")]
        public long MaxLength { get; set; }

        /// <summary>Gets or sets the image size</summary>
        [JsonProperty("Size")]
        public int Size { get; set; }

        /// <summary>Gets or sets the image miniature</summary>
        [JsonProperty("Thumbnail")]
        public int Thumbnail { get; set; }

        /// <summary>Gets or sets the image of "no image"</summary>
        [JsonProperty("NoImage")]
        public string NoImage { get; set; }

        /// <summary>Gets the lists of permitted extensions</summary>
        [JsonIgnore]
        public ReadOnlyCollection<string> Extensions
        {
            get
            {
                List<string> res = new List<string>();
                //// object[] valueArray = new object[this.extensions.Length];
                for (int x = 0; x < this.extensions.Length; x++)
                {
                    res.Add(this.extensions[x] as string);
                }

                return new ReadOnlyCollection<string>(res);
            }
        }
    }
}