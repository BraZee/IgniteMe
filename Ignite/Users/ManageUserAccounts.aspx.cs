using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Ignite.DAL;



namespace Ignite.Users
{
    public partial class ManageUserAccounts : System.Web.UI.Page
    {
        private DataProvider dataProvider = DataProvider.GetInstance();
        private int statusId;
        private string status;
        private bool active;

        protected void Page_Load(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageUsers";

            removeInjectedScript();

            if (!IsPostBack)
            {
                DisplayUsersInGrid(); //Populate datagridview
                LoadUserGroupCB();
           }
            
        }

       
        private void removeInjectedScript()
        {
            var jsHelper = (Label) Master.Master.FindControl("LblJsHelper");
            if (jsHelper != null)
            {
                jsHelper.Text = "";
            }
        }

        private void DisplayUsersInGrid()
        {
            var org = dataProvider.GetAllUsersByOrg(Utilities.userToken,Utilities.companyId);
            dgvUsers.DataSource = org.User; //bind result to datagridview
            dgvUsers.DataBind();
        }


        protected void dgvUsers_OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            Utilities.editId = int.Parse(dgvUsers.DataKeys[e.NewEditIndex].Value.ToString());
            UserDataSet userDS = dataProvider.GetUserById(Utilities.userToken, Utilities.editId);
            //statusId = int.Parse(dgvUsers.Rows[e.NewEditIndex].Cells[7].Text);
            active = bool.Parse(dgvUsers.Rows[e.NewEditIndex].Cells[8].Text);
           
            e.Cancel = true;
            txtLastName.Text = userDS.User[0].LastName;
            txtFirstName.Text = userDS.User[0].FirstName;
            txtEmail.Text = userDS.User[0].Email;
            txtUsername.Text = userDS.User[0].Logon;
            chkActive.Enabled = true;

            if (userDS.User[0].Status == "Active")
            {
                chkActive.Checked = true;
            }
            else
            {
                chkActive.Checked = false;
            }
           
            cbUsergroup.Text = userDS.User[0].UserGroupId.ToString();
            var jsHelper = (Label) Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>$('#myModal').modal('show')</script>"; //if not null inject js to show modal
            }
        }

        private void LoadUserGroupCB()
        {
            var userGroup = dataProvider.GetActiveUserGroups(Utilities.userToken, Utilities.companyId);
            cbUsergroup.DataSource = userGroup;
            cbUsergroup.DataBind();
            cbUsergroup.DataTextField = "Name";
            cbUsergroup.DataValueField = "Id";
            cbUsergroup.DataBind();
        }


        protected void btnCloseModal_OnClick(object sender, EventArgs e)
        {
            txtLastName.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtUsername.Text = string.Empty;
            chkActive.Checked = false;
            chkActive.Enabled = false;
            Utilities.editId = -1;

        }

        protected void btnSaveUser_OnClick(object sender, EventArgs e)
        {

            if (chkActive.Checked)
            {
                statusId = 1;
            }
            else
            {
                statusId = 2;
            }

            UserDataSet userDS = new UserDataSet();


            userDS.User.AddUserRow(Utilities.editId, txtFirstName.Text, txtLastName.Text, txtEmail.Text,
                txtUsername.Text,
                "", "", active, statusId, "",
                int.Parse(cbUsergroup.SelectedValue), "", "");

            if (Utilities.editId != -1)
            {
                int result = dataProvider.UpdateUser(Utilities.userToken, userDS);

                if (result != -1)
                {
                    SetToast("success", "The user has been succesfully updated");
                    Utilities.editId = -1;
                    btnCloseModal_OnClick(sender, e);
                }
                else
                {
                    SetToast("error", "Username already exists");
                }
            }
            else
            {
                if (!Utilities.CheckGridForText(dgvUsers, txtUsername.Text))
                {
                    int result = dataProvider.CreateUser(Utilities.userToken, userDS, Utilities.companyId);

                    if (result != -1)
                    {
                        Utilities.editId = -1;
                        SetToast("success", "The user was created successfully.");
                    }
                    else
                    {
                        SetToast("error", "An error has occured, try again.");
                    }
                }
                else
                {
                    SetToast("info","Username already exists");
                }

                DisplayUsersInGrid();
            }

            DisplayUsersInGrid();
        }

        protected void SetToast(string type, string errorMessage)
        {
            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>toastr." + type + "('" + errorMessage + "', '');</script>";  //if not null inject js to show modal
            }
        }

        protected void dgvUsers_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (dgvUsers.Rows[e.RowIndex].Cells[6].Text != "Deleted")
            {
                Utilities.editId = int.Parse(dgvUsers.DataKeys[e.RowIndex].Value.ToString());
                dataProvider.DeleteUser(Utilities.userToken, Utilities.editId);
                SetToast("success", "User deactivated succesfully");
            }
            else
            {
                SetToast("info","User already deactivated");
            }
            DisplayUsersInGrid();
        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>$('#myModal').modal('show')</script>"; //if not null inject js to show modal
            }
            Utilities.editId = -1;
            btnCloseModal_OnClick(sender,e);
        }
    }
}