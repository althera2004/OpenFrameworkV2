using OpenFramework;
using OpenFramework.Customer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DataDocumentGalleryUpload : Page
{
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
                string userDescription = this.Request.Form["UserDescription"];
                bool name = this.Request.Form["name"].Equals("1", StringComparison.OrdinalIgnoreCase);
                bool normalize = this.Request.Form["normalize"].Equals("1", StringComparison.OrdinalIgnoreCase);
                bool serialize = this.Request.Form["serialize"].Equals("1", StringComparison.OrdinalIgnoreCase);
                bool replace = this.Request.Form["replace"].Equals("1", StringComparison.OrdinalIgnoreCase);

                var instance = CustomerFramework.Load(instanceName);
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
                            file.FileName.ToUpperInvariant(),
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
                           @"{0}\CustomersFramework\{1}\Data\{2}\",
                           path,
                           instance.Name,
                           itemName);
                        fileName = Serialize(serializePath, fileName, replace);
                    }

                    path = string.Format(
                        CultureInfo.InvariantCulture,
                        @"{0}\CustomersFramework\{1}\Data\DocumentGallery\{2}\{3}",
                        path,
                        instance.Name,
                        itemName,
                        fileName);

                    file.SaveAs(path);

                    if (res.Success)
                    {
                        res.ReturnValue = fileName;
                        ActionLog.Trace(itemName.ToUpperInvariant() + "_", itemId, "DocumentGallery:" + fileName, instance.Config.ConnectionString, userDescription);
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
            foreach (string file in files)
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
        if (files.Count > 0)
        {
            counter = string.Format(CultureInfo.InvariantCulture, @"{0:0000}", files.Count + 1);

            if (replace)
            {
                counter = string.Format(CultureInfo.InvariantCulture, @"{0:0000}", files.Count);
            }
        }

        res += "_" + counter + extension;
        return res;
    }
}