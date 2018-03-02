// --------------------------------
// <copyright file="ItemDataBase.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using OpenFramework;
using OpenFramework.Customer;
using OpenFramework.DataAccess;
using OpenFramework.ItemManager;
using OpenFramework.Security;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;

/// <summary>
/// Data base actions for AMPA instance
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public partial class CustomersFramework_AMPA_Data_ItemDataBase : Page
{
    private CustomerFramework instance;

    /// <summary>Page load event</summary>
    /// <param name="sender">This page</param>
    /// <param name="e">Event arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if(this.Request.QueryString["InstanceName"] != null)
        {
            this.instance = new CustomerFramework() { Name = this.Request.QueryString["InstanceName"].ToString() };
            this.instance.LoadConfig();
        }

        if (this.Request.QueryString["Action"] != null)
        {
            this.GoAction();
        }

        if (this.Request.QueryString["GetScript"] != null)
        {
            this.GoScript();
        }
    }

    /// <summary>Gets JavaScript variable</summary>
    private void GoScript()
    {
        string res = string.Empty;
        string action = this.Request.QueryString["GetScript"].ToString().Trim().ToUpperInvariant();
        string variable = this.Request.QueryString["var"].ToString().Trim();
        switch (action)
        {
        }

        this.Response.Clear();
        this.Response.Write("var " + variable + "=");
        this.Response.Write(res);
        this.Response.Write(";");
        this.Response.Flush();
        this.Response.SuppressContent = true;
        this.ApplicationInstance.CompleteRequest();
    }

    private string GetList()
    {
        var res = new StringBuilder();
        var listId = this.Request.QueryString["ListId"].ToString();
        var itemName = this.Request.QueryString["ItemName"].ToString();

        ItemBuilder item = new ItemBuilder(itemName, "Playmobil");
        var query = ItemTools.QueryByListId(item, listId);

        CustomerFramework instance = CustomerFramework.Load("Playmobil");

        return SqlStream.GetByQuery(query, instance.Config.ConnectionString);
    }

    /// <summary>Gets JSON array</summary>
    private void GoAction()
    {
        DateTime d0 = DateTime.Now;
        string res = string.Empty;
        string action = this.Request.QueryString["Action"].ToString().Trim().ToUpperInvariant();

        switch (action.ToUpperInvariant())
        {
            case "GETLIST":
                res = this.GetList();
                break;
            default:
                res = SqlStream.GetSqlStreamNoParams(action, this.instance.Config.ConnectionString);
                break;
        }

        DateTime d1 = DateTime.Now;
        this.Response.Clear();
        this.Response.Write(@"{""data"":");
        this.Response.Write(res);
        DateTime d2 = DateTime.Now;
        this.Response.Write(string.Format(CultureInfo.GetCultureInfo("en-us"), @",""Time"":""{0:#,##0} - {1:#,##0}""}}", (d1 - d0).TotalMilliseconds, (d2 - d1).TotalMilliseconds));
        this.Response.Flush();
        this.Response.SuppressContent = true;
        this.ApplicationInstance.CompleteRequest();
    }
	
	[WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult AddToCollection(long piezaId, long applicationUserId)
    {
        var res = ActionResult.NoAction;
        CustomerFramework customer = new CustomerFramework() { Name = "playmobil" };
        customer.LoadConfig();
        ApplicationUser actualUser = ApplicationUser.GetById(applicationUserId, customer.Config.ConnectionString);
        using (SqlCommand cmd = new SqlCommand("Item_Pieza_AddToCollection"))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(DataParameter.Input("@PiezaId", piezaId));
            cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
            using (SqlConnection cnn = new SqlConnection(customer.Config.ConnectionString))
            {
                cmd.Connection = cnn;
                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess();
				}
                catch (Exception ex)
                {
                    res.SetFail(ex);
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
        }

        return res;
    }
}