using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ignite.DAL;


namespace Ignite.Admin
{
    public partial class ManageCategories : System.Web.UI.Page
    {
        private DataProvider dataProvider = DataProvider.GetInstance();
        private string userToken = Utilities.userToken;

        protected void Page_Load(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageCategories";

            removeInjectedScript();

            if (!IsPostBack)
            {
                DisplayCategoriesInGrid();
                LoadTypesCB();
            }
        }

        private void LoadTypesCB()
        {
            Dictionary<string, string> types = new Dictionary<string, string>();
            types.Add("True","Organisation");
            types.Add("False","Event");
            cbTypes.DataSource = types;
            cbTypes.DataBind();
            cbTypes.DataValueField = "Key";
            cbTypes.DataTextField = "Value";
            cbTypes.DataBind();
        }

        private void DisplayCategoriesInGrid()
        {
            var categories = dataProvider.GetAllCategoriesByType(userToken, bool.Parse(catList.SelectedItem.Value));//get from chk
            dgvCategories.DataSource = categories;
            dgvCategories.DataBind();
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

        protected void catList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayCategoriesInGrid();
        }

        protected void btnSaveCategory_OnClick(object sender, EventArgs e)
        {
            var catDS = new SysParameterDataSet();
            catDS.SysParameter.AddSysParameterRow(Utilities.editId, txtName.Text, txtDescription.InnerText,
                chkActive.Checked);

            if (Utilities.editId != -1)
            {

                int result = dataProvider.UpdateCategory(userToken, catDS,bool.Parse(cbTypes.SelectedValue));

                if (result != -1)
                {
                    Utilities.editId = -1;
                    btnCloseModal_OnClick(sender,e);
                    SetToast("success","Category updated succesfully.");
                }
                else
                {
                    SetToast("error","A category with that name exists already.");
                }
            }
            else
            {
                if (!Utilities.CheckGridForText(dgvCategories, txtName.Text))
                {
                    int result = dataProvider.AddCategory(userToken, catDS, bool.Parse(cbTypes.SelectedValue));

                    if (result != -1)
                    {
                        Utilities.editId = -1;
                        btnCloseModal_OnClick(sender,e);
                        SetToast("success","Category created succesfully.");
                    }
                    else
                    {
                        SetToast("error","An error occured, try again.");
                    }
                }
                else
                {
                    SetToast("info","Category exists already");
                }
            }

            DisplayCategoriesInGrid();
        }

        protected void dgvCategories_OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            e.Cancel = true;
            Utilities.editId = int.Parse(dgvCategories.DataKeys[e.NewEditIndex].Value.ToString());

            txtName.Text = dgvCategories.Rows[e.NewEditIndex].Cells[1].Text;
            txtDescription.InnerText = dgvCategories.Rows[e.NewEditIndex].Cells[2].Text;
            chkActive.Enabled = true;
            chkActive.Checked = bool.Parse(dgvCategories.Rows[e.NewEditIndex].Cells[3].Text);
            cbTypes.Text = catList.SelectedValue;

            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>$('#myModal').modal('show')</script>"; //if not null inject js to show modal
            }

        }

        protected void dgvCategories_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (dgvCategories.Rows[e.RowIndex].Cells[3].Text != "False")
            {
                Utilities.editId = int.Parse(dgvCategories.DataKeys[e.RowIndex].Value.ToString());
                dataProvider.DeactivateCategory(Utilities.editId, userToken);
                SetToast("success", "Category deactivated succesfully");
            }
            else
            {
                SetToast("info", "Category already deactivated");
            }
            DisplayCategoriesInGrid();
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