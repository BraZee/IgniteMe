using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Ignite.DAL;

namespace Ignite
{
    public partial class NestedMasterPage1 : System.Web.UI.MasterPage
    {
        private DataProvider dataProvider = DataProvider.GetInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utilities.userToken == "token")
            {
                Response.Redirect("/Default.aspx");
                return;
            }

            List<string> controls = new List<string>();
            var operations = dataProvider.GetAuthorizedOperations(Utilities.userToken, Utilities.userGroupId);
            int roleId;
            foreach (var operation in operations.Operation)
            {
                roleId = operation.OperationId;
                controls.AddRange(dataProvider.GetControlListByOperation(Utilities.userToken,roleId));
            }

            foreach (var control in controls)
            {
                var a = Menu.FindControl(control);
                a.Visible = true;
                var parent = (HtmlControl) a.Parent;
                parent.Visible = true;
            }
            var logout = (LinkButton) Master.FindControl("btnLogout");
            var userprofiledet = (LinkButton)Master.FindControl("btnUserProfileDetails");
            var userprofilesec = (LinkButton)Master.FindControl("btnUserProfileSecurity");
            logout.Visible = true;
            userprofiledet.Visible = true;
            userprofilesec.Visible = true;


        }

        protected void btnManageCategories_Click(object sender, EventArgs e)
        {
            //btnManageCategories.CssClass = "active";
           Response.Redirect("/Admin/ManageCategories.aspx");
        }

        protected void btnManageAdminUsers_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Users/ManageUserAccounts.aspx");
        }

        protected void btnManageOrganisations_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Admin/ManageOrganisations.aspx");
        }

        
        protected void btnManageFinancials_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Admin/ManageFinancials.aspx");
        }

        protected void btnManageLocations_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Organisations/ManageLocations.aspx");
        }

        protected void btnManageEvents_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Organisations/ManageEvents.aspx");
        }

        protected void btnManageOrganisationsUsers_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Organisations/ManageOrganisationUsers.aspx");
        }

        protected void btnManageUserGroups_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("/Users/UserGroupsSetup.aspx");
        }
    }
}