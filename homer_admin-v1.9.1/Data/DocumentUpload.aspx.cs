﻿// --------------------------------
// <copyright file="DocumentUpload.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using OpenFramework;
using OpenFramework.DataAccess;
using OpenFramework.Customer;

public partial class DataDocumentUpload : Page
{
    private CustomerFramework instance;

    protected void Page_Load(object sender, EventArgs e)
    {
        var res = ActionResult.NoAction;
        string path = Request.PhysicalApplicationPath;
        try
        {
            if (this.Request.Files != null && this.Request.Files.Count > 0)
            {
                long itemId = Convert.ToInt64(this.Request.Form["itemId"]);
                string itemName = this.Request.Form["itemName"];
                string instanceName = this.Request.Form["InstanceName"];

                bool name = this.Request.Form["name"].Equals("1", StringComparison.OrdinalIgnoreCase);
                bool normalize = this.Request.Form["normalize"].Equals("1", StringComparison.OrdinalIgnoreCase);
                bool serialize = this.Request.Form["serialize"].Equals("1", StringComparison.OrdinalIgnoreCase);
                bool replace = this.Request.Form["replace"].Equals("1", StringComparison.OrdinalIgnoreCase);

                this.instance = CustomerFramework.Load(instanceName);
                var file = this.Request.Files[0];
                if (file != null)
                {
                    if (!path.EndsWith("\\"))
                    {
                        path += "\\";
                    }

                    string fileName = string.Format(
                        CultureInfo.InvariantCulture,
                        "{0}_{1}",
                        itemId,
                        file.FileName);

                    if (normalize)
                    {
                        string extension = Path.GetExtension(fileName);
                        if (name)
                        {
                            fileName = string.Format(
                            CultureInfo.InvariantCulture,
                            "{0}_{1}_{2}{3}",
                            itemName.ToUpperInvariant(),
                            itemId,
                            this.Request.Form["fieldName"].ToUpperInvariant(),
                            extension);
                        }
                        else
                        {
                            fileName = string.Format(
                            CultureInfo.InvariantCulture,
                            "{0}_{1}{2}",
                            itemName.ToUpperInvariant(),
                            itemId,
                            extension);
                        }
                    }

                    if (serialize)
                    {
                        string serializePath = string.Format(
                           CultureInfo.InvariantCulture,
                           @"{0}\Instances\{1}\Data\{2}\",
                           path,
                           instance.Name,
                           itemName);
                        fileName = Serialize(serializePath, fileName, replace);
                    }

                    if (replace)
                    {
                        if (normalize)
                        {
                            string removePath = string.Format(
                                CultureInfo.InvariantCulture,
                                @"{0}\Instances\{1}\Data\{2}\",
                                path,
                                instance.Name,
                                itemName);
                            Remove(removePath, fileName);
                        }
                    }

                    path = string.Format(
                        CultureInfo.InvariantCulture,
                        @"{0}\Instances\{1}\Data\{2}\{3}",
                        path,
                        instance.Name,
                        itemName,
                        fileName);

                    file.SaveAs(path);

                    string fieldNameDB = "Archivo";
                    if (name)
                    {
                        fieldNameDB = this.Request.Form["fieldName"].ToUpperInvariant();
                    }

                    var query = new ExecuteQuery()
                    {
                        ConnectionString = instance.Config.ConnectionString,
                        QueryText = string.Format(
                            CultureInfo.InvariantCulture,
                            "UPDATE Item_{2} SET {3} = '{0}' WHERE Id = {1}",
                            fileName,
                            itemId,
                            itemName,
                            fieldNameDB)
                    };

                    res = query.ExecuteCommand;
                    if (res.Success)
                    {
                        res.ReturnValue = fileName;
                        ActionLog.Trace(itemName.ToUpperInvariant() + "_", itemId, fieldNameDB + ":" + fileName, this.instance.Name, this.instance.Config.ConnectionString);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            res.SetFail(ex);
        }

        this.Response.Clear();
        this.Response.Write(res.Json);
        this.Response.Flush();
        this.Response.SuppressContent = true;
        this.ApplicationInstance.CompleteRequest();
    }

    private void Remove(string path, string filename)
    {
        string extension = Path.GetExtension(filename);
        string res = filename.Replace(extension, string.Empty);
        var files = Directory.GetFiles(path, res + "*.*").OrderBy(f => f).ToList();
        if (files.Count > 0)
        {
           foreach(string file in files)
            {
                File.Delete(file);
            }
        }
    }

    private string Serialize(string path, string filename, bool replace)
    {
        string extension = Path.GetExtension(filename);
        string res = filename.Replace(extension, string.Empty);
        string counter = "0001";
        var files = Directory.GetFiles(path, res + "*.*").OrderBy(f => f).ToList();
        if(files.Count > 0)
        {
            counter = string.Format(CultureInfo.InvariantCulture, @"{0:0000}", files.Count + 1);

            if (replace)
            {
                counter = string.Format(CultureInfo.InvariantCulture, @"{0:0000}", files.Count);
            }
        }

        res += "_" + counter +  extension;
        return res;
    }
}