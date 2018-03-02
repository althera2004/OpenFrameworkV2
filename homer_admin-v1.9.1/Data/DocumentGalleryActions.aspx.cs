// --------------------------------
// <copyright file="DocumentGalleryActions.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using OpenFramework;
using System.Text;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public partial class Data_DocumentGalleryActions : Page
{
    [WebMethod]
    [ScriptMethod]
    public static ActionResult SaveDocument(string instanceName, string galleryFolder, string documentData, string documentFileName)
    {
        var res = ActionResult.NoAction;
        byte[] bytes = Convert.FromBase64String(documentData);

        string path = HttpContext.Current.Request.PhysicalApplicationPath;
        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = path + "\\";
        }

        path = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}CustomersFramework\\{1}\\Data\DocumentGallery\{2}\{3}",
            path,
            instanceName,
            galleryFolder,
            documentFileName);

        string url = string.Format(
            CultureInfo.InvariantCulture,
            @"/CustomersFramework/{0}/Data/DocumentGallery/{1}/{2}.jpg",
            instanceName,
            galleryFolder,
            documentFileName);

        File.WriteAllBytes(path, bytes);

        string result = string.Format(
            CultureInfo.InvariantCulture,
            @"{{""path"":""{0}"",""url"":""{1}?{2}""}}",
            path,
            url,
            Guid.NewGuid().ToString().ToUpperInvariant());

        res.SetSuccess(result);
        return res;
    }

    [WebMethod]
    [ScriptMethod]
    public static ActionResult GetDocumentGallery(string instanceName, string galleryFolder, string documentPattern)
    {
        var res = ActionResult.NoAction;
        try
        {
            StringBuilder list = new StringBuilder("[");
            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = path + "\\";
            }

            path = string.Format(
               CultureInfo.InvariantCulture,
               @"{0}CustomersFramework\\{1}\\Data\DocumentGallery\{2}\",
               path,
               instanceName,
               galleryFolder);

            if (!documentPattern.EndsWith("*.*", StringComparison.OrdinalIgnoreCase))
            {
                documentPattern += "*.*";
            }

            string[] filesName = Directory.GetFiles(path, documentPattern, SearchOption.TopDirectoryOnly);
            bool first = true;
            foreach (string fileName in filesName)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    list.Append(",");
                }

                list.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"""{0}?{1}""",
                    Path.GetFileName(fileName),
                    Guid.NewGuid());
            }

            list.Append("]");
            res.SetSuccess();
            res.ReturnValue = list.ToString();
        }
        catch(Exception ex)
        {
            res.SetFail(ex);
        }

        return res;
    }
}