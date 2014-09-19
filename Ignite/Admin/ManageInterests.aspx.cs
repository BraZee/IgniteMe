using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ignite.DAL;

namespace Ignite.Admin
{
    public partial class ManageInterests : System.Web.UI.Page
    {
        private DataProvider dataProvider = DataProvider.GetInstance();
        private string userToken = Utilities.userToken;

        protected void Page_Load(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageInterests";

            removeInjectedScript();

            if (!IsPostBack)
            {
                DisplayInterestsInGrid();
            }
        }

        
        private void DisplayInterestsInGrid()
        {
            var categories = dataProvider.GetAllInterests(userToken);
            dgvInterests.DataSource = categories;
            dgvInterests.DataBind();
        }

        private void removeInjectedScript()
        {
            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper");
            if (jsHelper != null)
            {
                jsHelper.Text = "";
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

        protected void btnCloseModal_OnClick(object sender, EventArgs e)
        {
            txtName.Text = string.Empty;
            txtDescription.InnerText = string.Empty;
            chkActive.Checked = false;
            chkActive.Enabled = false;
            Utilities.editId = -1;
        }

       
        protected void btnSaveInterest_OnClick(object sender, EventArgs e)
        {
            var intDS = new SysParameterDataSet();
            intDS.SysParameter.AddSysParameterRow(Utilities.editId, txtName.Text, txtDescription.InnerText,
                chkActive.Checked);

            if (Utilities.editId != -1)
            {

                int result = dataProvider.UpdateInterest(userToken, intDS);

                    if (result != -1)
                    {
                        Utilities.editId = -1;
                        btnCloseModal_OnClick(sender, e);
                        SetToast("success", "Interest updated succesfully.");
                    }
                    else
                    {
                        SetToast("error", "An Interest with that name exists already.");
                    }
            }
            else
            {
                if (!Utilities.CheckGridForText(dgvInterests, txtName.Text))
                {
                    int result = dataProvider.AddInterest(userToken, intDS);

                    if (result != -1)
                    {
                        Utilities.editId = -1;
                        btnCloseModal_OnClick(sender,e);
                        SetToast("success", "Interest created succesfully.");
                    }
                    else
                    {
                        SetToast("error", "An error occured, try again.");
                    }
                }
                else
                {
                    SetToast("info", "Interest exists already");
                }
            }

            DisplayInterestsInGrid();
        }

        protected void dgvInterests_OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            e.Cancel = true;
            Utilities.editId = int.Parse(dgvInterests.DataKeys[e.NewEditIndex].Value.ToString());

            txtName.Text = dgvInterests.Rows[e.NewEditIndex].Cells[1].Text;
            txtDescription.InnerText = dgvInterests.Rows[e.NewEditIndex].Cells[2].Text;
            chkActive.Enabled = true;
            chkActive.Checked = bool.Parse(dgvInterests.Rows[e.NewEditIndex].Cells[3].Text);
            
            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>$('#myModal').modal('show')</script>"; //if not null inject js to show modal
            }

        }

        protected void dgvInterests_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (dgvInterests.Rows[e.RowIndex].Cells[3].Text != "False")
            {
                Utilities.editId = int.Parse(dgvInterests.DataKeys[e.RowIndex].Value.ToString());
                string name = dgvInterests.Rows[e.RowIndex].Cells[1].Text;
                string desc = dgvInterests.Rows[e.RowIndex].Cells[2].Text;
                
                var intDS = new SysParameterDataSet();
                intDS.SysParameter.AddSysParameterRow(Utilities.editId, name, desc,false);
                dataProvider.UpdateInterest(userToken,intDS);

                SetToast("success", "Interest deactivated succesfully");
            }
            else
            {
                SetToast("info", "Interest already deactivated");
            }
            DisplayInterestsInGrid();
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