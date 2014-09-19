using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ignite.DAL;

namespace Ignite.Users
{
    public partial class UserProfileSecurity : System.Web.UI.Page
    {
        private DataProvider dataProvider = DataProvider.GetInstance();
        private string userToken = Utilities.userToken;
        private int userId = Utilities.userId;


        protected void Page_Load(object sender, EventArgs e)
        {
            removeInjectedScript();
            if (!IsPostBack)
            {
                LoadSecurityQuestions();
                initForm();
            }
            
        }

        private void LoadSecurityQuestions()
        {
            var questions = dataProvider.GetAllSecurityQuestions();
            cbSecurityQuestions.DataSource = questions;
            cbSecurityQuestions.DataBind();
            cbSecurityQuestions.DataTextField = "SecurityQuestionName";
            cbSecurityQuestions.DataValueField = "SecurityQuestionId";
            cbSecurityQuestions.DataBind();
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            int securityQuestionId = int.Parse(cbSecurityQuestions.SelectedValue);
            bool result = dataProvider.UpdateUserSecurity(userToken,userId,txtPassword.Text,securityQuestionId,txtSecurityAnswer.InnerText);

            if (result)
            {
                SetToast("success", "Profile security updated succesfully!");
                Utilities.statusId = 1;
                initForm();
            }
            else
            {
                SetToast("error","An error occured, try again.");
            }

            
        }

        protected void SetToast(string type, string errorMessage)
        {
            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>toastr." + type + "('" + errorMessage + "', '');</script>";  //if not null inject js to show modal
            }
        }

        private void removeInjectedScript()
        {
            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper");
            if (jsHelper != null)
            {
                jsHelper.Text = "";
            }
        }

        private void initForm()
        {
            txtPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
            
        }
    }
}