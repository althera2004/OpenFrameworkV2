// --------------------------------
// <copyright file="ItemDataBase.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using OpenFramework;
using OpenFramework.Core;
using OpenFramework.DataAccess;
using OpenFramework.ItemManager;
using OpenFramework.Security;
using OpenFramework.Customer;

/// <summary>Data base actions for AMPA instance</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public partial class CustomersFramework_AMPA_Data_ItemDataBase : Page
{

    /// <summary>AMPA instance</summary>
    private CustomerFramework instance;

    /// <summary>Page load event</summary>
    /// <param name="sender">This page</param>
    /// <param name="e">Event arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Request.QueryString["InstanceName"] != null)
        {
            this.instance = new CustomerFramework { Name = this.Request.QueryString["InstanceName"] };
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

    private string GetList()
    {
        var res = new StringBuilder();
        var listId = this.Request.QueryString["ListId"];
        var itemName = this.Request.QueryString["ItemName"];
        var item = new ItemBuilder(itemName, "AMPA");
        var parameters = Request.QueryString.Keys.Cast<string>().ToDictionary(k => k, v => Request.QueryString[v]);
        parameters = parameters.Where(p => p.Key != "ApplicationUserId" && p.Key != "_" && p.Key != "Action" && p.Key != "ItemName" && p.Key != "ListId").ToDictionary(v => v.Key, v => v.Value);
        var query = ItemTools.QueryByListId(item, parameters, listId);
        var instance = CustomerFramework.Load("AMPA");
        return SqlStream.GetByQuery(query, instance.Config.ConnectionString);
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

    /// <summary>Gets JSON array</summary>
    private void GoAction()
    {
        var d0 = DateTime.Now;
        string res = string.Empty;
        string action = this.Request.QueryString["Action"].Trim().ToUpperInvariant();

        switch (action)
        {
            case "GETLIST":
                res = this.GetList();
                break;
            case "ITEM_ALUMNO_GETBYCURSO":
                res = this.Item_Alumno_GetByCurso();
                break;
            case "ITEM_ACTIVIDAD_GETBYALUMNO":
                res = this.Item_Actividad_GetByAlumno();
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

    private string Item_Actividad_GetByAlumno()
    {
        var instance = CustomerFramework.Load("AMPA");
        string res = "[]";
        var parameters = new List<SqlParameter>
        {
            DataParameter.Input("@AlumnoId", Convert.ToInt64(this.Request.QueryString["AlumnoId"]))
        };

        try
        {
            res = SqlStream.GetSqlStream("Item_Actividad_GetByAlumno", new ReadOnlyCollection<SqlParameter>(parameters), instance.Config.ConnectionString);
        }
        catch (Exception ex)
        {
            ExceptionManager.Trace(ex, "Item_Actividad_GetByAlumno(" + Convert.ToInt64(this.Request.QueryString["AlumnoId"]) + ")");
        }

        return res;
    }

    private string Item_Alumno_GetByCurso()
    {
        var instance = CustomerFramework.Load("AMPA");
        string res = "[]";
        var parameters = new List<SqlParameter>
        {
            DataParameter.Input("@CursoId", Convert.ToInt64(this.Request.QueryString["CursoId"]))
        };

        try
        {
            res = SqlStream.GetSqlStream("Item_Alumno_GetByCurso", new ReadOnlyCollection<SqlParameter>(parameters), instance.Config.ConnectionString);
        }
        catch (Exception ex)
        {
            ExceptionManager.Trace(ex, "Item_Alumno_GetByCurso(" + Convert.ToInt64(this.Request.QueryString["CursoId"]) + ")");
        }

        return res;
    }
}