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

public partial class Data_LogActions : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    [WebMethod]
    [ScriptMethod]
    public static ActionResult ByItemId(string instanceName, string itemName, long itemId)
    {
        var res = ActionResult.NoAction;
        try
        {
            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            if(!path.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, "{0}\\", path);
            }

            path = string.Format(CultureInfo.InvariantCulture, "{0}Log\\", path);

            //Constraula_CENTRO_217.log
            string fileName = string.Format(
                CultureInfo.InvariantCulture,
                "{0}_{1}_{2}.log",
                instanceName,
                itemName,
                itemId);

            string finalPath = string.Format(CultureInfo.InvariantCulture, "{0}{1}", path, fileName);

            if (!File.Exists(finalPath))
            {
                res.ReturnValue = "No traces found";
            }
            else
            {
                using(StreamReader input = new StreamReader(finalPath))
                {
                    res.ReturnValue = input.ReadToEnd();
                }
            }

            res.SetSuccess();
        }
        catch (Exception ex)
        {
            res.SetFail(ex);
        }

        return res;
    }
}