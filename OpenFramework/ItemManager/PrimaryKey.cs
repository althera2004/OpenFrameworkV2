// --------------------------------
// <copyright file="PrimaryKey.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.Core
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Newtonsoft.Json;
    using OpenFramework.Core.ItemManager;

    public sealed class PrimaryKey
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Values")]
        private string[] itemFields;

        [JsonIgnore]
        public ReadOnlyCollection<string> ItemFields
        {
            get
            {
                if (this.itemFields == null)
                {
                    return new ReadOnlyCollection<string>(new List<string>());
                }

                return new ReadOnlyCollection<string>(this.itemFields.ToList());
            }
        }

        [JsonIgnore]
        public static PrimaryKey Empty
        {
            get
            {
                return new PrimaryKey()
                {
                    Id = string.Empty
                };
            }
        }
    }
}
