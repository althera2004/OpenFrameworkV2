// --------------------------------
// <copyright file="ItemDataBase.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using Newtonsoft.Json;
using OpenFramework;
using OpenFramework.ItemManager;
using OpenFramework.Customer;
using OpenFramework.CRUD;
using System.Configuration;
using System.Globalization;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public partial class DataItemDataBase : Page
{
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetByIdJson(string itemName, long itemId, string instanceName)
    {
        return Read.JsonById(itemId, itemName, instanceName);
    }

    [WebMethod]
    [ScriptMethod]
    public static string GetPhotoGalleryByItemId(string itemName, long itemId, string instanceName)
    {
        var instance = new CustomerFramework { Name = instanceName };
        instance.LoadConfig();
        return PhotoGallery.JsonList(PhotoGallery.GetByItemId(itemName, itemId, instance.Config.ConnectionString));
    }

    [WebMethod]
    [ScriptMethod]
    public static string GetPhotoGalleryByItemFieldId(string itemName, string itemFieldName, long itemId, string instanceName)
    {
        var instance = new CustomerFramework { Name = instanceName };
        instance.LoadConfig();
        return PhotoGallery.JsonList(PhotoGallery.GetByItemFieldId(itemName, itemFieldName, itemId, instance.Config.ConnectionString));
    }    

    [WebMethod]
    [ScriptMethod]
    public static ActionResult DeleteImage(string itemName,long itemId, string fieldName, string instanceName, long applicationUserId)
    {
        var res = ActionResult.NoAction;
        var query = string.Format(
            CultureInfo.InvariantCulture,
            "UPDATE Item_{0} SET {1} = NULL WHERE Id = {2}",
            itemName,
            fieldName,
            itemId);
        var instance = CustomerFramework.Load(instanceName);
        using (var cmd = new SqlCommand(query))
        {
            cmd.CommandType = CommandType.Text;
            using(var cnn = new SqlConnection(instance.Config.ConnectionString))
            {
                cmd.Connection = cnn;
                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess();
                }
                catch(Exception ex)
                {
                    res.SetFail(ex);
                }
                finally
                {
                    if(cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
        }

        return res;
    }

    [WebMethod]
    [ScriptMethod]
    public static ActionResult Save(string itemName, string itemData,string instanceName, long applicationUserId)
    {
        var res = ActionResult.NoAction;
        itemData = itemData.Replace('^', '{');
        dynamic data = JsonConvert.DeserializeObject(itemData);
        ItemBuilder itemBuilder = ItemBuilder.FromJsonObject(itemName, data, instanceName);
        return itemBuilder.Save(instanceName, applicationUserId, ItemBuilder.NotFromImport);
    }

    [WebMethod]
    [ScriptMethod]
    public static ActionResult Inactive(long itemId, string itemName, string instanceName, long applicationUserId, string userDescription)
    {
        var instance = new CustomerFramework { Name = instanceName };
        instance.LoadConfig();
        return ItemBuilder.Inactive(itemId, itemName, applicationUserId, userDescription, instanceName, instance.Config.ConnectionString);
    }

    [WebMethod]
    [ScriptMethod]
    public static ItemBuilder GetById(long id, string itemName, string instanceName)
    {
        return Read.ById(id, ItemDefinition.Load(itemName, instanceName), instanceName);
    }

    [WebMethod]
    [ScriptMethod]
    public static ReadOnlyCollection<ItemBuilder> GetAll(string itemName, string instanceName)
    {
        return Read.All(ItemDefinition.Load(itemName, instanceName), instanceName);
    }

    [WebMethod]
    [ScriptMethod]
    public static ReadOnlyCollection<ItemBuilder> GetActive(string itemName,string instanceName)
    {
        return Read.Active(ItemDefinition.Load(itemName, instanceName), instanceName);
    }

    [WebMethod]
    [ScriptMethod]
    public static ActionResult CreateItemFromFields(string itemName, string[] fieldNames, object[] values, string instanceName, long applicationUserId, string userDescription)
    {
        var res = ActionResult.NoAction;

        if (string.IsNullOrEmpty(itemName))
        {
            return res;
        }

        var item = new ItemBuilder(itemName, instanceName);
        for (var i = 0; i < fieldNames.Length; i++)
        {
            item[fieldNames[i]] = values[i];
        }

        if (item.ContainsKey("Id"))
        {
            item["Id"] = 0;
        }

        res = item.Save(instanceName, applicationUserId, ItemBuilder.NotFromImport);

        return res;
    }

    [WebMethod]
    [ScriptMethod]
    public static ActionResult SqlToJson(string query, string instanceName, bool stored)
    {
        var res = ActionResult.NoAction;
        var customerConfig = CustomerFramework.Load(instanceName);
        var dt = new DataTable();
        using (SqlConnection con = new SqlConnection(customerConfig.Config.ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.CommandType = stored ? CommandType.StoredProcedure : CommandType.Text;
                con.Open();
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                //convert datatable to list using LINQ. Input datatable is "dt"
                var lst = dt.AsEnumerable()
                    .Select(r => r.Table.Columns.Cast<DataColumn>()
                            .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
                           ).ToDictionary(z => z.Key, z => z.Value)
                    ).ToList();
                //now serialize it
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                var d1 = DateTime.Now;
                serializer.MaxJsonLength = 500000000;
                res.SetSuccess(serializer.Serialize(lst));
            }
        }

        return res;
    }
}