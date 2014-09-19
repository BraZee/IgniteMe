using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ignite
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogout_OnClick(object sender, EventArgs e)
        {
            Utilities.userToken = "token";
            Session.RemoveAll();
            Response.Redirect("/Default.aspx");
        }

        protected void btnUserProfileDetails_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("/Users/UserProfileDetails.aspx");
        }

        protected void btnUserProfileSecurity_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("/Users/UserProfileSecurity.aspx");
        }

        protected void btnIgnite_OnServerClick(object sender, EventArgs e)
        {
            Session.RemoveAll();
            
        }
    }
}