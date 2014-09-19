using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace IgniteWeb
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var login = (HtmlControl) Master.FindControl("login");
            var register = (HtmlControl)Master.FindControl("register");

            if (login != null)
            {
                login.Visible = true;
            }
            if (register != null)
            {
                register.Visible = true;
            }

        }
    }
}