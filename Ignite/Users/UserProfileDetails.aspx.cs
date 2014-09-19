using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ignite.DAL;


namespace Ignite.Users
{
    public partial class UserProfileDetails : System.Web.UI.Page
    {
        private DataProvider dataProvider = DataProvider.GetInstance();
        private string userToken = Utilities.userToken;
        private int userId = Utilities.userId;
        private int userGroupId = Utilities.userGroupId;
        private int statusId = Utilities.statusId;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initForm();
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
            UserDataSet userDS = dataProvider.GetUserById(userToken, userId);

            if (userDS.User.Count > 0)
            {
                txtLastName.Text = userDS.User[0].LastName;
                txtFirstName.Text = userDS.User[0].FirstName;
                txtEmail.Text = userDS.User[0].Email;
                txtUsername.Text = userDS.User[0].Logon;
            }
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            int result = dataProvider.UpdateUserWithoutPassword(userToken, userId, txtFirstName.Text, txtLastName.Text, txtEmail.Text, txtUsername.Text,userGroupId, statusId);
           
            if (result != -1)
            {
                SetToast("success","Profile details updated succesfully!");
            
            }
        }
    }
}