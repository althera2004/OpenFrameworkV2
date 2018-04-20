// --------------------------------
// <copyright file="ItemDataBase.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using OpenFramework;
using OpenFramework.Customer;
using OpenFramework.DataAccess;
using OpenFramework.ItemManager;
using OpenFramework.Security;

/// <summary>Data base actions for AMPA instance</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public partial class CustomersFramework_AMPA_Data_ItemDataBase : Page
{
    /// <summary>Playmobil instance</summary>
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
        string action = this.Request.QueryString["GetScript"].Trim().ToUpperInvariant();
        string variable = this.Request.QueryString["var"].Trim();
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
        var listId = this.Request.QueryString["ListId"];
        var itemName = this.Request.QueryString["ItemName"];
        var item = new ItemBuilder(itemName, "Playmobil");
        var parameters = Request.QueryString.Keys.Cast<string>().ToDictionary(k => k, v => Request.QueryString[v]);
        parameters = parameters.Where(p => p.Key != "ApplicationUserId" && p.Key != "_" && p.Key != "Action" && p.Key != "ItemName" && p.Key != "ListId").ToDictionary(v => v.Key, v => v.Value);
        var query = ItemTools.QueryByListId(item, parameters, listId);
        var instance = CustomerFramework.Load("Playmobil");
        return SqlStream.GetByQuery(query, instance.Config.ConnectionString);
    }

    /// <summary>Gets JSON array</summary>
    private void GoAction()
    {
        var d0 = DateTime.Now;
        string res = string.Empty;
        string action = this.Request.QueryString["Action"].Trim().ToUpperInvariant();

        switch (action.ToUpperInvariant())
        {
            case "GETLIST":
                res = this.GetList();
                break;
            default:
                res = SqlStream.GetSqlStreamNoParams(action, this.instance.Config.ConnectionString);
                break;
        }

        var d1 = DateTime.Now;
        this.Response.Clear();
        this.Response.Write(@"{""data"":");
        this.Response.Write(res);
        var d2 = DateTime.Now;
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
        var customer = CustomerFramework.Load("playmobil");
        var actualUser = ApplicationUser.GetById(applicationUserId, customer.Config.ConnectionString);
        using (var cmd = new SqlCommand("Item_Pieza_AddToCollection"))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(DataParameter.Input("@PiezaId", piezaId));
            cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
            using (var cnn = new SqlConnection(customer.Config.ConnectionString))
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