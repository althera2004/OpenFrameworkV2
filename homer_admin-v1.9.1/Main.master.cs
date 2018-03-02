using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Main : System.Web.UI.MasterPage
{
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    public string ActualUser
    {
        get
        {
            return @"{
                ""Id"": 1,
                ""FullName"": ""Juan Castilla"",
                ""JobPosition"": ""MegaMaster""
            }";
        }
    }

    public string PageTitle
    {
        get
        {
            return "El titulo";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.RenderAlerts();
    }

    private void RenderAlerts()
    {
        string res = @"<ul class=""dropdown-menu hdropdown notification animated flipInX"">
                          <li>  
                              <a>  
                                  <span class=""label label-success"">NEW</span> It is a long established.   
                               </a>
                        </li>
                        <li>
                            <a>
                                <span class=""label label-warning"">WAR</span> There are many variations.
                            </a>
                        </li>
                        <li>
                            <a>
                                <span class=""label label-danger"">ERR</span> Contrary to popular belief.
                            </a>
                        </li>
                        <li>
                            <a>
                                <span class=""label label-danger"">ERR</span> Contrary to popular belief.
                            </a>
                        </li>
                        <li>
                            <a>
                                <span class=""label label-danger"">ERR</span> Contrary to popular belief.
                            </a>
                        </li>
                        <li>
                            <a>
                                <span class=""label label-danger"">ERR</span> Contrary to popular belief.
                            </a>
                        </li>
                        <li>
                            <a>
                                <span class=""label label-danger"">ERR</span> Contrary to popular belief.
                            </a>
                        </li>
                        <li class=""summary""><a href = ""#"" > See all notifications</a></li>
                    </ul>";
        this.LtAlerts.Text = res;
    }
}
