// --------------------------------
// <copyright file="GalleryActions.aspx.cs" company="Sbrinna">
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

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public partial class Data_GalleryActions : Page
{
    [WebMethod]
    [ScriptMethod]
    public static ActionResult SaveImage(string instanceName, string galleryFolder, string imageData, string imageFileName)
    {
        var res = ActionResult.NoAction;
        byte[] bytes = Convert.FromBase64String(imageData);
        /* ALTER PROCEDURE [dbo].[Core_PhotoGallery_Insert]
         *   @Id bigint output,
         *   @ItemName nvarchar(50),
         *   @ItemField nvarchar(50),
         *   @ItemId bigint,
         *   @Date datetime,
         *   @Description nvarchar(50),
         *   @GPS nvarchar(50),
         *   @ApplicationUserId bigint
         */

        string path = HttpContext.Current.Request.PhysicalApplicationPath;
        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = path + "\\";
        }     

        path = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}CustomersFramework\\{1}\\Data\PhotoGallery\{2}\{3}",
            path,
            instanceName,
            galleryFolder,
            imageFileName);

        string url = string.Format(
            CultureInfo.InvariantCulture,
            @"/CustomersFramework/{0}/Data/PhotoGallery/{1}/{2}.jpg",
            instanceName,
            galleryFolder,
            imageFileName);

        File.WriteAllBytes(path, bytes);
        //imageBits.Save(path, ImageFormat.Png);

        string result = string.Format(
            CultureInfo.InvariantCulture,
            @"{{""path"":""{0}"",""url"":""{1}?{2}""}}",
            path,
            url,
            Guid.NewGuid().ToString().ToUpperInvariant());

        res.SetSuccess(result);
        return res;
    }
}