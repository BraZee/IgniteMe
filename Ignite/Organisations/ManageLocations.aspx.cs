using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ignite.DAL;

namespace Ignite.Organisations
{
    public partial class ManageLocations : System.Web.UI.Page
    {
        private DataProvider dataProvider = DataProvider.GetInstance();
        private string userToken = Utilities.userToken;
        private int orgId = Utilities.companyId;


        protected void Page_Load(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageLocations";

            removeInjectedScript();

            if (!IsPostBack)
            {
                DisplayLocationsInGrid(); //Populate datagridview
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

        private void DisplayLocationsInGrid()
        {
            var org = dataProvider.GetLocationsByOrgId(Utilities.userToken, orgId);
            dgvLocations.DataSource = org.Location; //bind result to datagridview
            dgvLocations.DataBind();
        }


        protected void dgvLocations_OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            Utilities.editId = int.Parse(dgvLocations.DataKeys[e.NewEditIndex].Value.ToString());
            //UserDataSet LocationDS = dataProvider.GetLocationById(Utilities.userToken, Utilities.editId);

            e.Cancel = true;
            txtName.Text = dgvLocations.Rows[e.NewEditIndex].Cells[1].Text;
            txtLatitude.Text = dgvLocations.Rows[e.NewEditIndex].Cells[2].Text;
            txtlongitude.Text = dgvLocations.Rows[e.NewEditIndex].Cells[3].Text;
            txtCountry.Text = dgvLocations.Rows[e.NewEditIndex].Cells[4].Text;
            txtCity.Text = dgvLocations.Rows[e.NewEditIndex].Cells[5].Text;
            txtTown.Text = dgvLocations.Rows[e.NewEditIndex].Cells[6].Text;
            chkActive.Checked = bool.Parse(dgvLocations.Rows[e.NewEditIndex].Cells[7].Text);
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
            txtLatitude.Text = string.Empty;
            txtlongitude.Text = string.Empty;
            txtCountry.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtTown.Text = string.Empty;
            chkActive.Checked = true;
            chkActive.Enabled = false;
            Utilities.editId = -1;
        }


        protected void btnSaveLocation_OnClick(object sender, EventArgs e)
        {
            var LocationDS = new LocationDataSet();

            LocationDS.Location.AddLocationRow(Utilities.editId,txtName.Text, chkActive.Checked, double.Parse(txtlongitude.Text),
                double.Parse(txtLatitude.Text),
                txtCountry.Text, txtCity.Text, txtTown.Text);

            if (Utilities.editId != -1)
            {
                int result = dataProvider.UpdateLocation(Utilities.userToken, LocationDS, orgId);

                if (result != -1)
                {
                    SetToast("success", "Location has been succesfully updated");
                    Utilities.editId = -1;
                    btnCloseModal_OnClick(sender, e);
                }
                else
                {
                    SetToast("error", "Location already exists");
                }
            }
            else
            {
                if (!Utilities.CheckGridForText(dgvLocations, txtName.Text))
                {
                    int result = dataProvider.AddLocation(Utilities.userToken, LocationDS, orgId);

                    if (result != -1)
                    {
                        Utilities.editId = -1;
                        SetToast("success", "Location was created successfully.");
                    }
                    else
                    {
                        SetToast("error", "An error has occured, try again.");
                    }
                }
                else
                {
                    SetToast("info", "Location already exists");
                }

                DisplayLocationsInGrid();
            }

            DisplayLocationsInGrid();
        }

        protected void SetToast(string type, string errorMessage)
        {
            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>toastr." + type + "('" + errorMessage + "', '');</script>";  //if not null inject js to show modal
            }
        }

        protected void dgvLocations_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (dgvLocations.Rows[e.RowIndex].Cells[7].Text != "False")
            {
                Utilities.editId = int.Parse(dgvLocations.DataKeys[e.RowIndex].Value.ToString());

                string _txtName = dgvLocations.Rows[e.RowIndex].Cells[1].Text;
                string _txtLong = dgvLocations.Rows[e.RowIndex].Cells[2].Text;
                string _txtLat = dgvLocations.Rows[e.RowIndex].Cells[3].Text;
                string _txtCountry = dgvLocations.Rows[e.RowIndex].Cells[4].Text;
                string _txtCity = dgvLocations.Rows[e.RowIndex].Cells[5].Text;
                string _txtTown = dgvLocations.Rows[e.RowIndex].Cells[6].Text;
                
                var curr = new LocationDataSet();
                curr.Location.AddLocationRow(Utilities.editId,_txtName, false, double.Parse(_txtLong), double.Parse(_txtLat),
                    _txtCountry,
                    _txtCity, _txtTown);

                dataProvider.UpdateLocation(userToken, curr, orgId);
                SetToast("success", "Location deactivated succesfully");
            }
            else
            {
                SetToast("info", "Location already deactivated");
            }
            DisplayLocationsInGrid();
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
        }
    }
}