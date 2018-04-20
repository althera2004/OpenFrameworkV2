// --------------------------------
// <copyright file="FKList.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón -  jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;

    public class FKList
    {
        [JsonIgnore]
        private List<ItemField> realFields;

        [JsonProperty("Fields")]
        private List<string> fields;

        [JsonProperty("Fiter")]
        private Filter[] filter;

        [JsonIgnore]
        public ReadOnlyCollection<Filter> Filter
        {
            get
            {
                if (this.filter == null)
                {
                    return new ReadOnlyCollection<Filter>(new List<Filter>());
                }

                return new ReadOnlyCollection<Filter>(this.filter);
            }
        }

        [JsonIgnore]
        public ReadOnlyCollection<string> Fields
        {
            get
            {
                if(this.fields == null)
                {
                    this.fields = new List<string>();
                }

                return new ReadOnlyCollection<string>(this.fields);
            }
        }

        [JsonIgnore]
        public static FKList Empty
        {
            get
            {
                return new FKList
                {
                    fields = new List<string>(),
                    filter = null
                };
            }
        }
    }
}