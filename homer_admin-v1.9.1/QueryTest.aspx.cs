using OpenFramework.CRUD;
using OpenFramework.ItemManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class QueryTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ItemBuilder item = new ItemBuilder("Pieza", "Playmobil");
        this.LtById.Text = ItemTools.QueryById(item, 2);
        this.LtByIdData.Text = Read.JsonById(2, item.ItemName, "Playmobil");
    }
}