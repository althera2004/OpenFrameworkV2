// --------------------------------
// <copyright file="Condition.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager
{
    using System;
    using Newtonsoft.Json;

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