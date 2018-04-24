// --------------------------------
// <copyright file="FKScript.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using OpenFramework.DataAccess;
using OpenFramework.ItemManager;
using OpenFramework.Customer;
using System.Collections.Generic;

/// <summary>Implements retrieve data for foreing keyssummary>
public partial class DataFKScript : Page
{
    /// <summary>Page load event</summary>
    /// <param name="sender">This page</param>
    /// <param name="e">Event arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        var d0 = DateTime.Now;
        string itemName = this.Request.QueryString["ItemName"];
        string instanceName = this.Request.QueryString["InstanceName"];
        var instance = CustomerFramework.Load(instanceName);
        instance.LoadConfig();
        var item = new ItemBuilder(itemName, instanceName);
        this.Response.Clear();
        this.Response.ContentType = "application/json";

        if (this.Request.QueryString["r"] == null)
        {
            this.Response.Write("FK.");
            this.Response.Write(itemName);
            this.Response.Write(" = ");
        }
        else
        {
            this.Response.Write("{\"ItemName\":\"");
            this.Response.Write(itemName);
            this.Response.Write("\",\"Data\":");
        }

        if (itemName.Equals("user", StringComparison.OrdinalIgnoreCase))
        {
            this.Response.Write(ItemBuilder.ListItemJson(ItemBuilder.GetActive(itemName, instance.Name, instance.Config.ConnectionString)));
        }
        else
        {
            var parameters = Request.QueryString.Keys.Cast<string>().ToDictionary(k => k, v => Request.QueryString[v]);
            parameters = parameters.Where(p => p.Key != "ac" && p.Key != "r" && p.Key != "itemName" && p.Key != "InstanceName").ToDictionary(v => v.Key, v => v.Value);            
            this.Response.Write(SqlStream.GetFKStream(item, parameters, instance.Config.ConnectionString));
        }

        if (this.Request.QueryString["r"] == null)
        {
            this.Response.Write(";");
        }
        else
        {
			this.Response.Write(",\"Duration\":");
			this.Response.Write(string.Format(CultureInfo.InvariantCulture, "{0:#0.000}", (DateTime.Now - d0).TotalMilliseconds));
            this.Response.Write("}");
        }

        this.Response.Flush();
        this.Response.SuppressContent = true;
        this.ApplicationInstance.CompleteRequest();
    }
}