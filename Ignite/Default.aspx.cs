using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ignite.DAL;
using Ignite.WS;
using LoginReturn = Ignite.SecurityManager.LoginReturn;


namespace Ignite
{
    public partial class Default : System.Web.UI.Page
    {
       private DataProvider dataProvider = DataProvider.GetInstance();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["success"] != null)
            {
                SetToast("success",Session["success"].ToString());
                Session.RemoveAll();
            }
            else
            {
                removeInjectedScript();
            }
            
            txtPassword.Attributes.Add("autocomplete", "off");

            if (!IsPostBack)
            {
                txtUsername.Focus();
                BindOrgCB();
                
            }


        }

        protected void SetToast(string type,string errorMessage)
        {
            var jsHelper = (Label)Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>toastr." + type + "('" + errorMessage + "', '');</script>";  //if not null inject js to show modal
            }
        }


        protected void BindOrgCB()
        {
            var source = dataProvider.GetActiveOwnerOrganisations(Utilities.userToken);

            cbOrganisations.DataSource = source;
            cbOrganisations.DataBind();
            cbOrganisations.DataTextField = "Name";
            cbOrganisations.DataValueField = "Id";
            cbOrganisations.DataBind();

        }


        protected void btnLogin_OnClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrEmpty(txtUsername.Text))
            {
                SetToast("error","Please enter your Username and Password.");
                return;
            }

            try
            {
                int companyId = int.Parse(cbOrganisations.SelectedValue);
                string companyName = cbOrganisations.SelectedItem.ToString();

                var result = dataProvider.Authenticate(companyId, txtUsername.Text, txtPassword.Text);//Authenticate user
                var returnCode = result.ReturnCode; //get return code


                if (result.Success)//if success, init user details
                {
                    Utilities.userId = int.Parse(result.UserId);
                    Utilities.userToken = result.UserToken; 
                    Utilities.userFullName = result.UserFullName;
                    Utilities.companyId = companyId;
                    Utilities.companyName = companyName;

                    var user = dataProvider.GetUserById(Utilities.userToken, Utilities.userId);
                    //Utilities.userExpiryDate = user.User[0].ExpiryDate;
                    Utilities.userGroupId = user.User[0].UserGroupId;
                    Utilities.isSystemAdmin = Utilities.userGroupId == 1;
                    Utilities.userActive = user.User[0].Active;
                    Utilities.status = user.User[0].Status;
                    Utilities.statusId = user.User[0].StatusId;
                    Utilities.companyId = companyId;
                    Utilities.pageName = "checker";
                    

                    Session["ssiAccountId"] = -1;
//                    Session["Operations"] = dataProvider.GetAuthorizedOperations(Utilities.userToken,
//                        Utilities.userGroupId);

                    Response.Redirect("/LandingPage.aspx");
                        
                }
                else
                {
                    switch (returnCode)
                    {
                        case DAL.LoginReturn.DeletedUser:
                            SetToast("error", "Access Denied. Account Deactivated");
                            break;

                       case DAL.LoginReturn.ExpiredCredentials:
                            SetToast("error", "Access Denied. Account Expired.");
                            break;

                        case DAL.LoginReturn.InactiveUser:
                            //SetToast("error","Send this guy to profile security to set his password.");
                            Utilities.userId = int.Parse(result.UserId);
                            Utilities.userToken = result.UserToken; 
                            Utilities.userFullName = result.UserFullName;
                            Utilities.companyId = companyId;
                            Utilities.companyName = companyName;

                            var user = dataProvider.GetUserById(Utilities.userToken, Utilities.userId);
                            //Utilities.userExpiryDate = user.User[0].ExpiryDate;
                            Utilities.userGroupId = user.User[0].UserGroupId;
                            Utilities.isSystemAdmin = Utilities.userGroupId == 1;
                            Utilities.userActive = user.User[0].Active;
                            Utilities.status = user.User[0].Status;
                            Utilities.statusId = user.User[0].StatusId;
                            Utilities.companyId = companyId;
                            Utilities.pageName = "checker";
                            Session["ssiAccountId"] = -1;
                            Response.Redirect("Users/UserProfileSecurity.aspx");
                            break;

                        default:
                            SetToast("error", "Access Denied. Wrong Credentials");
                            txtPassword.Focus();
                            break;
                    } 
                }
            }
            catch (Exception ex)
            {
                
            }


        }

        private void removeInjectedScript()
        {
            var jsHelper = (Label)Master.FindControl("LblJsHelper");
            if (jsHelper != null)
            {
                jsHelper.Text = "";
            }
        }

        protected void btnForgotPassword_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("/ForgotPassword.aspx");
        }
    }
}