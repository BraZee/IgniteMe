using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Ignite.DAL;

namespace Ignite.Users
{
    public partial class UserGroupOperations : System.Web.UI.Page
    {
        private DataProvider dataProvider = DataProvider.GetInstance();
        private string userToken = Utilities.userToken;
        private int userGroupId_edit;
        private string userGroup_edit = Utilities.userGroup_edit;
        Dictionary<int, string> rights = new Dictionary<int, string>();
        List<string> rightsList = new List<string>();
        SysParameterDataSet userGroupParams;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageUserGroups";
            removeInjectedScript();

            if (Session["userGroupParams"] == null)
            {
                Response.Redirect("/LandingPage.aspx");
            }
            else
            {
                userGroupParams = (SysParameterDataSet)Session["userGroupParams"];
                userGroup.Text = "to '" + userGroupParams.SysParameter[0].Name + "'";
                userGroupId_edit = userGroupParams.SysParameter[0].Id;
            }

            if (!IsPostBack)
            {
               
                LoadOperationsCB();
            }
            DisplayOperationsInGrid();
            
        }

        private void LoadOperationsCB()
        {
            var source = dataProvider.GetAllOperations(userToken);
            cbOperations.DataSource = source;
            cbOperations.DataBind();
            cbOperations.DataTextField = "OperationName";
            cbOperations.DataValueField = "OperationId";
            cbOperations.DataBind();
        }

        private void DisplayOperationsInGrid()
        {
            GetOperations();
            dgvOperations.DataSource = rights;
            dgvOperations.DataBind();
        }

        private void GetOperations()
        {
            var operations = dataProvider.GetAuthorizedOperations(userToken, userGroupId_edit);
            //Dictionary<int,string> rights = new Dictionary<int, string>();

            foreach (var operation in operations.Operation)
            {
                if (!rights.ContainsKey(operation.OperationId))
                {
                    rights.Add(operation.OperationId, operation.OperationName);
                }
            }
        }

        protected void btnAdd_OnClick(object sender, EventArgs e)
        {
            //userGroupId_edit = _id;
            Session["userGroupParams"] = Session["userGroupParams"];
           
            
            if (!Utilities.CheckGridForText(dgvOperations,cbOperations.SelectedItem.ToString()))
            {
                rights.Add(int.Parse(cbOperations.SelectedValue), cbOperations.SelectedItem.ToString());

                foreach (int right in rights.Keys)
                {
                    rightsList.Add(right.ToString());
                }
                
                int result = dataProvider.SetRightsForUserGroup(userGroupId_edit, rightsList);

                if (result != -1)
                {
                    SetToast("success", "Operation added.");
                }
                else
                {
                    SetToast("error", "An error occured, try again.");
                }
       
            }
            else
            {
                SetToast("info","Operation already added.");
            }

            DisplayOperationsInGrid();
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

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            if (!(rights.Count > 0))
            {
                SetToast("error","Please select at least one operation.");
                return;
            }
//           foreach (int right in rights.Keys)
//            {
//                rightsList.Add(right.ToString());
//            }


            if (Utilities.editId != -1)
            {

                int result = dataProvider.UpdateUserGroup(userToken, userGroupParams);

                if (result != -1)
                {
                    //SetToast("success", "User group updated succesfully.");
                    Utilities.editId = -1;
                    Session.RemoveAll();
                    Session["success"] = "User group updated succesfully.";
                    Response.Redirect("/Users/UserGroupsSetup.aspx");
                }
                else
                {
                    //SetToast("error", "An error has occurred, try again.");
                    Utilities.editId = -1;
                    Session.RemoveAll();
                    Session["error"] = "A user group with that name exists already";
                    Response.Redirect("/Users/UserGroupsSetup.aspx");
                }
            }
            else
            {
                Utilities.editId = -1;
                Session.RemoveAll();
                Session["success"] = "User group created succesfully.";
                Response.Redirect("/Users/UserGroupsSetup.aspx");
            }
//            else
//            {
//                int result = dataProvider.AddUserGroup(userToken, userGroupDS,rightsList, orgId);
//
//                    if (result != -1)
//                    {
//                        Utilities.editId = -1;
//                        //SetToast("success", "User group added succesfully.");
//                        Session.RemoveAll();
//                        Session["success"] = "User group added succesfully.";
//                        Response.Redirect("/Users/UserGroupsSetup.aspx");
//                        
//                    }
//                    else
//                    {
//                        SetToast("error", "An error has occured, try again.");
//                    }
                
            //}
        }

        protected void dgvOperations_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            List<string> rightsList = new List<string>();

            rights.Remove(int.Parse(dgvOperations.DataKeys[e.RowIndex].Value.ToString()));
            
            foreach (int right in rights.Keys)
            {
                rightsList.Add(right.ToString());
            }

            int result = dataProvider.SetRightsForUserGroup(userGroupId_edit, rightsList);

            if (result != -1)
            {
                SetToast("success", "Operation removed.");
            }
            else
            {
                SetToast("error", "An error occured, try again.");
            }

            DisplayOperationsInGrid();
        }
    }
}