using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFramework.ItemManager
{
    [Serializable]
    public sealed class Condition
    {
        [JsonProperty("Type")]
        private string type { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }

        [JsonIgnore]
        public ConditionType Type
        {
            get
            {
                if (string.IsNullOrEmpty(this.type))
                {
                    return ConditionType.Field;
                }

                switch (this.type.ToUpperInvariant())
                {
                    case "GLOBAL":
                        return ConditionType.Global;
                    default:
                        return ConditionType.Field;
                }
            }
        }
    }
}
