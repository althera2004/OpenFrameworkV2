// --------------------------------
// <copyright file="DataTableGetData.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón -  jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using OpenFramework.ItemManager;
using OpenFramework.CRUD;
using OpenFramework.Customer;

public partial class Data_DataTableGetData : Page
{
    /// <summary>Page load event</summary>
    /// <param name="sender">This page</param>
    /// <param name="e">Event arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Go();
    }

    /// <summary>Continues PageLoad execution if session is alive</summary>
    private void Go()
    {        
        var d0 = DateTime.Now;
        string itemName = this.Request.QueryString["ItemName"];
        string instanceName = this.Request.QueryString["InstanceName"];
        ReadOnlyCollection<ItemBuilder> data;

        bool external = false;
        CustomerFramework instance = new CustomerFramework() { Name = instanceName };
        instance.LoadConfig();

        ItemDefinition definition = ItemDefinition.Load(itemName, instanceName);

        data = Read.Active(definition, instance.Name);
        if (this.Request.QueryString.AllKeys.Count() > 3)
        {
            var temporalData = data.ToList();
            if (this.Request.QueryString.AllKeys.Any(k => k == "InstanceName"))
            {
                external = true;
                foreach (string key in this.Request.QueryString.AllKeys.Where(k => k != "ItemName"))
                {
                    if (key.Equals("ItemName", StringComparison.OrdinalIgnoreCase))
                    {
                        var localKey = key.Remove('_');
                        object query = this.Request.QueryString[localKey];
                        if (query.ToString().Contains("EXCLUDE"))
                        {
                            long val = Convert.ToInt64(query.ToString().Split('/')[1]);
                            temporalData = temporalData.Where(d => Convert.ToInt64(d[localKey]) != val).ToList();
                        }
                        else if (query.GetType().ToString().ToUpperInvariant() == "SYSTEM.STRING")
                        {
                            if (query.ToString().Trim().ToUpperInvariant().Equals("NULL", StringComparison.OrdinalIgnoreCase) || query.ToString().Trim().ToUpperInvariant().Equals("0", StringComparison.OrdinalIgnoreCase))
                            {
                                temporalData = temporalData.Where(d => d[localKey] == null || d[localKey].ToString().Trim().Equals("0", StringComparison.OrdinalIgnoreCase)).ToList();
                            }
                            else
                            {
                                temporalData = temporalData.Where(d => d[localKey] != null && d[localKey].ToString().Equals(query.ToString())).ToList();
                            }
                        }
                        else
                        {
                            temporalData = temporalData.Where(d => d[localKey] != null && Convert.ToInt64(d[localKey]) == Convert.ToInt64(query)).ToList();
                        }
                    }
                }

                data = new ReadOnlyCollection<ItemBuilder>(temporalData);
            }
            else
            {
                foreach (string key in this.Request.QueryString.AllKeys.Where(k => k != "ItemName"))
                {
                    var localKey = key;

                    if (localKey == "callback")
                    {
                        external = true;
                    }

                    if (localKey == "format" || localKey == "InstanceName" || localKey == "callback" || localKey == "_" || string.IsNullOrEmpty(localKey))
                    {
                        continue;
                    }

                    if (key.Equals(itemName + "Id"))
                    {
                        localKey = "Id";
                    }

                    localKey = localKey.Replace("_", string.Empty);

                    object query = this.Request.QueryString[key];
                    if (query.ToString().Contains("EXCLUDE"))
                    {
                        long val = Convert.ToInt64(query.ToString().Split('/')[1]);
                        temporalData = temporalData.Where(d => Convert.ToInt64(d[localKey]) != val).ToList();
                    }
                    else if (query.GetType().ToString().ToUpperInvariant() == "SYSTEM.STRING")
                    {
                        if (query.ToString().Trim().ToUpperInvariant().Equals("NULL", StringComparison.OrdinalIgnoreCase) || query.ToString().Trim().ToUpperInvariant().Equals("0", StringComparison.OrdinalIgnoreCase))
                        {
                            temporalData = temporalData.Where(d => d[localKey] == null || d[localKey].ToString().Trim().Equals("0", StringComparison.OrdinalIgnoreCase)).ToList();
                        }
                        else
                        {
                            temporalData = temporalData.Where(d => d[localKey] != null && d[localKey].ToString().Equals(query.ToString())).ToList();
                        }
                    }
                    else
                    {
                        temporalData = temporalData.Where(d => d[localKey] != null && Convert.ToInt64(d[localKey]) == Convert.ToInt64(query)).ToList();
                    }
                }

                data = new ReadOnlyCollection<ItemBuilder>(temporalData);
            }
        }

        DateTime d1 = DateTime.Now;
        this.Response.Clear();
        this.Response.ContentType = "application/json";
        DateTime d2 = DateTime.Now;
        if (external)
        {
            string res = string.Format(
                CultureInfo.InvariantCulture,
                @"{0}",
                ItemBuilder.ListItemJson(data).Replace("\n",string.Empty).Replace("\r",string.Empty).Replace("\\",string.Empty).Replace(@"""""""""",@""""""));
            this.Response.Write(res);
        }
        else
        {
            this.Response.Write(@"{""data"":");
            this.Response.Write(ItemBuilder.ListItemJson(data));
            this.Response.Write(string.Format(CultureInfo.GetCultureInfo("en-us"), @",""Time"":""{0:#,##0} - {1:#,##0}""}}", (d1 - d0).TotalMilliseconds, (d2 - d1).TotalMilliseconds));
        }

        this.Response.Flush();
        this.Response.SuppressContent = true;
        this.ApplicationInstance.CompleteRequest();
    }
}