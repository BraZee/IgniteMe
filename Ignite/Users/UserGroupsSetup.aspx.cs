using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ignite.DAL;

namespace Ignite.Users
{
    public partial class UserGroupsSetup : System.Web.UI.Page
    {
        private DataProvider dataProvider = DataProvider.GetInstance();
        private string userToken = Utilities.userToken;
        private int orgId = Utilities.companyId;

        protected void Page_Load(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageUserGroups";
            if (Session["success"] != null)
            {
                SetToast("success",Session["success"].ToString());  
                Session.RemoveAll();
            }
            else if(Session["error"] != null)
            {
                SetToast("error", Session["error"].ToString());
                Session.RemoveAll();
            }
            else
            {
                removeInjectedScript();   
            }
            if (!IsPostBack)
            {
                DisplayUserGroupsInGrid();
            }

            //removeInjectedScript();
        }

        private void DisplayUserGroupsInGrid()
        {
            var userGroupDS = dataProvider.GetAllUserGroups(Utilities.userToken, Utilities.companyId);
            dgvUserGroups.DataSource = userGroupDS;
            dgvUserGroups.DataBind();
        }

        protected void dgvUserGroups_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (bool.Parse(dgvUserGroups.Rows[e.RowIndex].Cells[3].Text))
            {
                //Utilities.editId = int.Parse(dgvUserGroups.DataKeys[e.RowIndex].Value.ToString());
                //dataProvider.
            }
            else
            {
                SetToast("info","User Group already deactivated");
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

        protected void dgvUserGroups_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Delete" && e.CommandName != "Edit")
            {
                int rowIndex = int.Parse(e.CommandArgument.ToString());
                GridViewRow row = (GridViewRow) dgvUserGroups.Rows[rowIndex];
                Session["userGroup_edit"] = row.Cells[1].Text;
                //Utilities.userGroupId_edit = int.Parse(row.Cells[1].Text);
                DataKey rowDK = (DataKey)dgvUserGroups.DataKeys[rowIndex];
                Session["userGroupId_edit"] = int.Parse(rowDK.Value.ToString());
                
               Response.Redirect("/Users/UserGroupOperations.aspx");

            }
        }

        protected void dgvUserGroups_OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            e.Cancel = true;
            Utilities.editId = int.Parse(dgvUserGroups.DataKeys[e.NewEditIndex].Value.ToString());
            txtName.Text = dgvUserGroups.Rows[e.NewEditIndex].Cells[1].Text;
            txtDescription.InnerText = dgvUserGroups.Rows[e.NewEditIndex].Cells[2].Text;
            chkActive.Checked = bool.Parse(dgvUserGroups.Rows[e.NewEditIndex].Cells[3].Text);
            chkActive.Enabled = true;

            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>$('#myModal').modal('show')</script>"; //if not null inject js to show modal
            }
        }

        protected void btnCloseModal_OnClick(object sender, EventArgs e)
        {
            txtName.Text = string.Empty;
            txtDescription.InnerText = string.Empty;
            chkActive.Checked = false;
            chkActive.Enabled = false;
            Utilities.editId = -1;
        }

        protected void btnSaveUserGroup_OnClick(object sender, EventArgs e)
        {
            if (Utilities.editId == -1)
            {
               var userGroupDS = new SysParameterDataSet();
                    userGroupDS.SysParameter.AddSysParameterRow(Utilities.editId, txtName.Text, txtDescription.InnerText,
                        chkActive.Checked);

                List<string> list = new List<string>();
                
                if (Utilities.CheckGridForText(dgvUserGroups, txtName.Text) && Utilities.editId == -1)
                {
                    SetToast("info", "User group exists already.");
                    return;
                }
                else
                {
                    try
                    {
                        Utilities.editId = dataProvider.AddUserGroup(userToken, userGroupDS, list, orgId);
                    }
                    catch (Exception ex)
                    {
                        SetToast("error","An error occured, try again.");
                    }
                    
                }
                
            }

            var userGroupDS2 = new SysParameterDataSet();
                userGroupDS2.SysParameter.AddSysParameterRow(Utilities.editId, txtName.Text, txtDescription.InnerText,
                    chkActive.Checked);

                Session["userGroupParams"] = userGroupDS2;
                Utilities.pageName = "btnManageUserGroups";
                Utilities.editId = -1;
                Response.Redirect("/Users/UserGroupOperations.aspx");

            
        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>$('#myModal').modal('show')</script>"; //if not null inject js to show modal
            }
            Utilities.editId = -1;
            btnCloseModal_OnClick(sender, e);
            chkActive.Checked = true;
        }
    }
}