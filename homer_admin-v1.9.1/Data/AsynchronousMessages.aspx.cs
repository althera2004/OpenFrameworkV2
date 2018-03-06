// --------------------------------
// <copyright file="AsynchronousMessages.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Linq;
using OpenFramework;
using System.Net;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using OpenFramework.Customer;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public partial class Data_AsynchronousMessages : Page
{
    [WebMethod]
    [ScriptMethod]
    public static ActionResult SaveImage(string instanceName, string itemName, string itemField, long itemId, string image)
    {
        var res = ActionResult.NoAction;
        byte[] bytes = Convert.FromBase64String(image);

        string path = HttpContext.Current.Request.PhysicalApplicationPath;
        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = path + "\\";
        }

        path = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}Instances\\{1}\\Data\img\{2}_{3}_{4}.png",
            path,
            instanceName,
            itemName,
            itemField,
            itemId);

        var url = string.Format(
            CultureInfo.InvariantCulture,
            @"/Instances/{0}/Data/img/{1}_{2}_{3}.png",
            instanceName,
            itemName,
            itemField,
            itemId);

        File.WriteAllBytes(path, bytes);

        var result = string.Format(
            CultureInfo.InvariantCulture,
            @"{{""path"":""{0}"",""url"":""{1}?{2}""}}",
            path,
            url,
            Guid.NewGuid().ToString().ToUpperInvariant());

        var query = string.Format(
            CultureInfo.InvariantCulture,
            "UPDATE Item_{0} SET {1} = '{2}' WHERE Id = {3}",
            itemName,
            itemField,
            url,
            itemId);

        var instance = CustomerFramework.Load(instanceName);
        using(var command = new SqlCommand(query))
        {
            command.CommandType = CommandType.Text;
            using(var cnn = new SqlConnection(instance.Config.ConnectionString))
            {
                command.Connection = cnn;
                try
                {
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    res.SetSuccess();
                }
                catch(Exception ex)
                {
                    res.SetFail(ex);
                }
                finally
                {
                    if(command.Connection.State != ConnectionState.Closed)
                    {
                        command.Connection.Close();
                    }
                }
            }
        }

        if (res.Success)
        {
            res.SetSuccess(result);
        }
        return res;
    }

    [WebMethod]
    [ScriptMethod]
    public static ActionResult SaveFile(string instanceName, string itemName, string fileName, long itemId, string data)
    {
        var res = ActionResult.NoAction;
        byte[] bytes = Convert.FromBase64String(data);

        string path = HttpContext.Current.Request.PhysicalApplicationPath;
        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = path + "\\";
        }

        path = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}CustomersFramework\\{1}\\Data\{2}\{3}_{4}",
            path,
            instanceName,
            itemName,
            itemId,
            fileName);

        File.WriteAllBytes(path, bytes);
        //imageBits.Save(path, ImageFormat.Png);

        string result = string.Format(
            CultureInfo.InvariantCulture,
            @"{{""path"":""{0}""}}",
            path);

        res.SetSuccess(result);
        return res;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static ActionResult RequestData()
    {
        var res = ActionResult.NoAction;

        var IPAddress = string.Empty;
        IPHostEntry Host = default(IPHostEntry);
        string Hostname = null;
        Hostname = System.Environment.MachineName;
        Host = Dns.GetHostEntry(Hostname);
        foreach (IPAddress IP in Host.AddressList)
        {
            if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                IPAddress = Convert.ToString(IP);
            }
        }

        res.SetSuccess(IPAddress);

        return res;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat= ResponseFormat.Json)]
    public static ActionResult GetAllItems(string instanceName)
    {
        var res = ActionResult.NoAction;
		try
		{
            CustomerFramework instance = CustomerFramework.Load(instanceName);
			string path = instance.DefinitionPath;
			var filesPath = Directory.GetFiles(path, "*.item").ToList();
			var files = new List<string>();

			foreach (string fileName in filesPath)
			{
				files.Add(Path.GetFileNameWithoutExtension(fileName));
			}

			res.SetSuccess(files);
		}
		catch(Exception ex)
		{
			res.SetFail(ex);
		}

        return res;
    }
}