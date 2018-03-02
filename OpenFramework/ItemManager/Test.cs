// --------------------------------
// <copyright file="Test.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón -  jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.ItemManager
{
    using Newtonsoft.Json;

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
