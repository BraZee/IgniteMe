using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Ignite.DAL;


namespace Ignite
{
    public partial class SiteChild : System.Web.UI.MasterPage
    {
        private DataProvider dataProvider = DataProvider.GetInstance();
        private List<string> controls;
        OperationDataSet operations;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utilities.userToken == "token")
            {
                Utilities.pageName = "Default";
                Response.Redirect("/Default.aspx");
                return;
            }
            
            if (Utilities.statusId != 1 && HttpContext.Current.Request.Url.AbsolutePath != "/Users/UserProfileSecurity.aspx")
            {
                Utilities.pageName = "User Profile Security";
                Response.Redirect("/Users/UserProfileSecurity.aspx");
                return;
            }

            
                controls = new List<string>();
                operations = dataProvider.GetAuthorizedOperations(Utilities.userToken, Utilities.userGroupId);
                int roleId;
                foreach (var operation in operations.Operation)
                {
                    roleId = operation.OperationId;
                    controls.AddRange(dataProvider.GetControlListByOperation(Utilities.userToken, roleId));
                }
                controls.Add("checker");

                foreach (var control in controls)
                {
                    var a = MenuS.FindControl(control);
                    if (a != null)
                    {
                        a.Visible = true;
                        var parent = (HtmlControl) a.Parent;
                        parent.Visible = true;
                    }
                }
                var logout = (HtmlControl) Master.FindControl("divLogin");
                logout.Visible = true;
                var showSidebar = (HtmlControl) Master.FindControl("btnShowSidebar");
                showSidebar.Visible = true;
                var username = (Label) Master.FindControl("lblUserFullName");
                username.Text = Utilities.userFullName;

                
            
            if (Utilities.pageName != string.Empty)
            {
                if (!controls.Contains(Utilities.pageName))
                {
                    Utilities.pageName = string.Empty;
                    Response.Redirect("/LandingPage.aspx");
                }

            }
            
        }

        protected void btnManageCategories_Click(object sender, EventArgs e)
        {
            //btnManageCategories.CssClass = "active";
            Utilities.pageName = "btnManageCategories";
            Utilities.editId = -1;
            Response.Redirect("/Admin/ManageCategories.aspx");
        }

        protected void btnManageAdminUsers_Click(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageUsers";
            Utilities.editId = -1;
            Response.Redirect("/Users/ManageUserAccounts.aspx");
        }

        protected void btnManageOrganisations_Click(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageOrganisations";
            Utilities.editId = -1;
            Response.Redirect("/Admin/ManageOrganisations.aspx");
        }


        protected void btnManageFinancials_Click(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageFinancials";
            Utilities.editId = -1;
            Response.Redirect("/Admin/ManageFinancials.aspx");
        }

        protected void btnManageLocations_Click(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageLocations";
            Utilities.editId = -1;
            Response.Redirect("/Organisations/ManageLocations.aspx");
        }

        protected void btnManageEvents_Click(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageEvents";
            Utilities.editId = -1;
            Response.Redirect("/Organisations/Events/ManageEvents.aspx");
        }

        
        protected void btnManageUserGroups_OnClick(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageUserGroups";
            Utilities.editId = -1;
            Response.Redirect("/Users/UserGroupsSetup.aspx");
        }

        protected void btnPushSMS_OnClick(object sender, EventArgs e)
        {
            Utilities.pageName = "btnPushSMS";
            Utilities.editId = -1;
            Response.Redirect("/SendSMS.aspx");
        }

        protected void btnPushEmail_OnClick(object sender, EventArgs e)
        {
            Utilities.pageName = "btnPushEmail";
            Utilities.editId = -1;
            Response.Redirect("/SendEmail.aspx");
        }

        protected void btnManageInterests_OnClick(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageInterests";
            Utilities.editId = -1;
            Response.Redirect("/Admin/ManageInterests.aspx");
        }

        protected void btnManageCurrencies_OnClick(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageCurrencies";
            Utilities.editId = -1;
            Response.Redirect("/Organisations/ManageCurrency.aspx");
        }
    }
}