// --------------------------------
// <copyright file="DashBoard.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using OpenFramework.Core;
using OpenFramework.Customer;

/// <summary>
/// Implements DashBoard page
/// </summary>
public partial class CustomersFramework_AMPA_Pages_DashBoard : Page
{
    private List<string> files;

    /// <summary>Gets a random string for anticache purposses</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    public string TableItems
    {
        get
        {
            StringBuilder res = new StringBuilder(Environment.NewLine);
            int cont = 0;
            foreach (string itemName in this.files)
            {
                if (cont == 0)
                {
                    res.Append("<tr>");
                }
                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"<div style=""padding:8px;"" class=""col col-sm-3""><span id=""Item_{0}_Status""><i class=""fa fa-sign-out"" style=""color:#777;""></i></span>&nbsp;{0}<span id=""Count_{0}""></span></div>",
                    itemName);

                cont++;

                if (cont == 4)
                {
                    res.Append("</tr>");
                    cont = 0;
                }
            }

            if (cont != 0)
            {
                res.Append("</tr>");
            }

            return res.ToString();
        }
    }

    public string FKScripts
    {
        get
        {
            StringBuilder res = new StringBuilder("var itemsToLoad=[");
            bool first = true;
            foreach (string itemName in this.files)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(", ");
                }

                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"""{0}""",
                    itemName);

            }

            res.AppendFormat(
                CultureInfo.InvariantCulture,
                @"];{1}totalItems = {0};",
                files.Count,
                Environment.NewLine);
            return res.ToString();
        }
    }

    /// <summary>Event of page load</summary>
    /// <param name="sender">Page loaded</param>
    /// <param name="e">Arguments of event</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        CustomerFramework instance = CustomerFramework.Load("AMPA");
        string path = instance.DefinitionPath;
        var filesPath = Directory.GetFiles(path, "*.item").ToList();
        this.files = new List<string>();

        foreach (string fileName in filesPath)
        {
            files.Add(Path.GetFileNameWithoutExtension(fileName));
        }
    }
}