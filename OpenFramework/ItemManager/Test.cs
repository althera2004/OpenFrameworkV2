using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFramework.ItemManager
{
    public static class Test
    {
        public static ItemDefinition TestItemDefinitionFromJson()
        {
            ItemDefinition definition = JsonConvert.DeserializeObject(@"{
    ""ItemName"": ""CountryCurrency"",
    ""ForeingValues"":
    [
        ""Country"",""Currency""
    ],
	""Fields"": 
		[
            {""Name"":""Id"", ""Type"":""long""},
            {""Name"":""Description"", ""Type"":""long""},
            {""Name"":""Position"", ""Type"":""GeoPosition""}
        ]
}") as ItemDefinition;

            return definition;
        }
    }
}
