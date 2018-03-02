// --------------------------------
// <copyright file="FixedList.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.Core.ItemManager.List
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>Implements fixed lists of framework</summary>
    public sealed class FixedList
    {
        /// <summary>List data</summary>
        [JsonProperty("Data")]
        private FixedListItem[] data;

        /// <summary>Gets or sets the list identifier</summary>
        [JsonProperty("Id")]
        public string Id { get; set; }

        /// <summary>Gets the data of list</summary>
        [JsonIgnore]
        public ReadOnlyCollection<FixedListItem> Data
        {
            get
            {
                List<FixedListItem> res = new List<FixedListItem>();

                if (this.data != null && this.data.Length > 0)
                {
                    res = this.data.OrderBy(l => l.Description).ToList();
                }

                return new ReadOnlyCollection<FixedListItem>(res);
            }
        }

        /// <summary>Gets a JSON structure of list including data</summary>
        [JsonIgnore]
        public string Json
        {
            get
            {
                StringBuilder res = new StringBuilder("{");
                res.AppendFormat(
                   CultureInfo.InvariantCulture,
                   @"""Id"":""{0}"", ""Data"":[",
                   this.Id);

                bool first = true;
                foreach (FixedListItem item in this.Data)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(item.Json);
                }

                res.Append("]}");
                return res.ToString();
            }
        }
    }
}
