// --------------------------------
// <copyright file="Read.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace OpenFramework.CRUD
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using OpenFramework.Customer;
    using OpenFramework.DataAccess;
    using OpenFramework.ItemManager;

    public static class Read
    {
        public static ItemBuilder ById(long id, ItemDefinition definition, string instanceName)
        {
            return ItemBuilder.Empty(definition.ItemName, ItemBuilder.NotResolveForeignKeys, instanceName);
        }

        public static ReadOnlyCollection<ItemBuilder> Active(ItemDefinition definition, string instanceName)
        {
            var res = new List<ItemBuilder>();

            return new ReadOnlyCollection<ItemBuilder>(res);
        }

        public static ReadOnlyCollection<ItemBuilder> All(ItemDefinition definition, string instanceName)
        {
            var res = new List<ItemBuilder>();

            return new ReadOnlyCollection<ItemBuilder>(res);
        }

        public static string JsonById(long id, string itemName, string instanceName)
        {
            var instance = CustomerFramework.Load(instanceName);
            var item = new ItemBuilder(itemName, instanceName);
            return SqlStream.GetByQuerySimple(ItemTools.QueryById(item, id), instance.Config.ConnectionString);
        }

        public static string JsonActive(ItemDefinition definition, string instanceName)
        {
            return string.Empty;
        }

        public static string JsonAll(ItemDefinition definition, string instanceName)
        {
            return string.Empty;
        }
    }
}