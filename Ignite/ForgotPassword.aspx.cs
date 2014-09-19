using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ignite.DAL;

namespace Ignite
{
    public partial class ForgotPassword : System.Web.UI.Page
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

        protected void SetToast(string type, string errorMessage)
        {
            var jsHelper = (Label)Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>toastr." + type + "('" + errorMessage + "', '');</script>";  //if not null inject js to show modal
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

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            bool result = dataProvider.ResetPassword(txtEmail.Text, int.Parse(cbSecurityQuestions.SelectedValue),
                txtSecurityAnswer.InnerText,0);

            if (result)
            {
                Session["success"] = "Password reset, new password sent to your email.";
                Response.Redirect("/Default.aspx");
            }
            else
            {
                SetToast("error","Invalid details, check and try again.");
            }
        }
    }
}