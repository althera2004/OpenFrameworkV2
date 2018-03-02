using OpenFramework.Customer;
using OpenFramework.DataAccess;
using OpenFramework.ItemManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFramework.CRUD
{
    public static class Read
    {
        public static ItemBuilder ById(long id, ItemDefinition definition, string instanceName)
        {
            ItemBuilder res = ItemBuilder.Empty(definition.ItemName, false, instanceName);

            return res;
        }

        public static ReadOnlyCollection<ItemBuilder> Active(ItemDefinition definition, string instanceName)
        {
            List<ItemBuilder> res = new List<ItemBuilder>();

            return new ReadOnlyCollection<ItemBuilder>(res);
        }

        public static ReadOnlyCollection<ItemBuilder> All(ItemDefinition definition, string instanceName)
        {
            List<ItemBuilder> res = new List<ItemBuilder>();

            return new ReadOnlyCollection<ItemBuilder>(res);
        }

        public static string JsonById(long id, string itemName, string instanceName)
        {
            CustomerFramework instance = CustomerFramework.Load(instanceName);
            ItemBuilder item = new ItemBuilder(itemName, instanceName);
            return SqlStream.GetByQuerySimple(ItemTools.QueryById(item, id), instance.Config.ConnectionString);
        }

        public static string JsonActive(ItemDefinition definition, string instanceName)
        {
            string res = string.Empty;

            return res;
        }

        public static string JsonAll(ItemDefinition definition, string instanceName)
        {
            string res = string.Empty;

            return res;
        }
    }
}
