// --------------------------------
// <copyright file="FKList.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón -  jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
                return new FKList()
                {
                    fields = new List<string>(),
                    filter = null
                };
            }
        }
    }
}