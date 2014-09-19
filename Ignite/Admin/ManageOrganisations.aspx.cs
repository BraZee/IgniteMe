using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ignite.DAL;


namespace Ignite.Admin
{
    public partial class ManageOrganisations : System.Web.UI.Page
    {
        private DataProvider dataProvider = DataProvider.GetInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageOrganisations";

            if (Session["success"] != null)
            {
                SetToast("success", Session["success"].ToString());
                Session.RemoveAll();
            }
            else if (Session["error"] != null)
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
                DisplayOrganisationsInGrid();
            }
            
        }

        protected void DisplayOrganisationsInGrid()
        {
            var organisations = dataProvider.GetAllOwnerOrganisations();  //connect to db, get all wishes
            dgvOrganisations.DataSource = organisations.Organisation; //bind result to datagridview
            dgvOrganisations.DataBind();
        }

        private void removeInjectedScript()
        {
            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper");
            if (jsHelper != null)
            {
                jsHelper.Text = "";
            }
        }

        protected void dgvOrganisations_OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            Utilities.editId = int.Parse(dgvOrganisations.DataKeys[e.NewEditIndex].Value.ToString());
            OrganisationDataSet orgDS = dataProvider.GetOwnerOrganisationByID(Utilities.userToken,Utilities.editId);
            Session.RemoveAll();
            e.Cancel = true;
            txtOrgName.Text = orgDS.Organisation[0].Name; //get text from selected row
            txtOrgCode.Text = orgDS.Organisation[0].SMSName; 
            txtOrgAddress1.Text = orgDS.Organisation[0].OfficeAddress1; 
            txtOrgAddress2.Text = orgDS.Organisation[0].OfficeAddress2; 
            txtOrgAddress3.Text = orgDS.Organisation[0].OfficeAddress3; 
            txtOrgPOBox1.Text = orgDS.Organisation[0].PostAddress1; 
            txtOrgPOBox2.Text = orgDS.Organisation[0].PostAddress2; 
            txtOrgPOBox3.Text = orgDS.Organisation[0].PostAddress3; 
            txtOrgTelephone.Text = orgDS.Organisation[0].Telephone; 
            txtOrgFax.Text = orgDS.Organisation[0].Fax; 
            txtOrgEmail.Text = orgDS.Organisation[0].Email; 
            txtOrgFacebook.Text = orgDS.Organisation[0].Facebook; 
            txtOrgTwitter.Text = orgDS.Organisation[0].Twitter; 
            txtOrgGoogle.Text = orgDS.Organisation[0].Google;
            chkOrgActive.Checked = orgDS.Organisation[0].Active;
            chkOrgActive.Enabled = true;

            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>$('#myModal').modal('show')</script>";  //if not null inject js to show modal
            }
        }

        protected void dgvOrganisations_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (bool.Parse(dgvOrganisations.Rows[e.RowIndex].Cells[5].Text))
            {
                Utilities.editId = int.Parse(dgvOrganisations.DataKeys[e.RowIndex].Value.ToString());
                int result = dataProvider.DeactivateOwnerOrganisation(Utilities.editId, Utilities.userToken);
                if (result != -1)
                {
                    SetToast("success", "Organisation Deactivated");
                }
                else
                {
                    SetToast("error", "Deactivate failed.");
                }
                DisplayOrganisationsInGrid();
            }
            else
            {
                SetToast("info","Organisation already deactivated.");
            }

        }

        protected void btnCloseModal_OnClick(object sender, EventArgs e)
        {
            txtOrgName.Text = string.Empty;
            txtOrgCode.Text = string.Empty;
            txtOrgAddress1.Text = string.Empty;
            txtOrgAddress2.Text = string.Empty;
            txtOrgAddress3.Text = string.Empty;
            txtOrgPOBox1.Text = string.Empty;
            txtOrgPOBox2.Text = string.Empty;
            txtOrgPOBox3.Text = string.Empty;
            txtOrgTelephone.Text = string.Empty;
            txtOrgFax.Text = string.Empty;
            txtOrgEmail.Text = string.Empty;
            txtOrgFacebook.Text = string.Empty;
            txtOrgTwitter.Text = string.Empty;
            txtOrgGoogle.Text = string.Empty;
            chkOrgActive.Checked = false;
            chkOrgActive.Enabled = false;
            Utilities.editId = -1;

        }


        protected void btnsavefacility_OnClick(object sender, EventArgs e)
        {
            if (Utilities.editId == -1)
            {
                var orgDS = new OrganisationDataSet();
                orgDS.Organisation.AddOrganisationRow(Utilities.editId, txtOrgName.Text, "",
                    txtOrgAddress1.Text,
                    txtOrgAddress2.Text,
                    txtOrgAddress3.Text,
                    txtOrgAddress1.Text + "," + txtOrgAddress2.Text + "," + txtOrgAddress3.Text,
                    txtOrgPOBox1.Text,
                    txtOrgPOBox2.Text,
                    txtOrgPOBox3.Text,
                    txtOrgPOBox1.Text + "," + txtOrgPOBox2.Text + "," + txtOrgPOBox3.Text,
                    txtOrgTelephone.Text,
                    txtOrgFax.Text,
                    txtOrgEmail.Text,
                    chkOrgActive.Checked,
                    txtOrgFacebook.Text,
                    txtOrgTwitter.Text,
                    txtOrgGoogle.Text, txtOrgCode.Text);

                if (Utilities.CheckGridForText(dgvOrganisations, txtOrgName.Text))
                {
                    SetToast("info", "Organisation exists already.");
                    return;
                }
                else
                {
                    Utilities.editId = dataProvider.AddOwnerOrganiasation(Utilities.userToken, orgDS);
                }

                    
            }

            var orgDS2 = new OrganisationDataSet();
            orgDS2.Organisation.AddOrganisationRow(Utilities.editId, txtOrgName.Text, "",
                txtOrgAddress1.Text,
                txtOrgAddress2.Text,
                txtOrgAddress3.Text,
                txtOrgAddress1.Text + "," + txtOrgAddress2.Text + "," + txtOrgAddress3.Text,
                txtOrgPOBox1.Text,
                txtOrgPOBox2.Text,
                txtOrgPOBox3.Text,
                txtOrgPOBox1.Text + "," + txtOrgPOBox2.Text + "," + txtOrgPOBox3.Text,
                txtOrgTelephone.Text,
                txtOrgFax.Text,
                txtOrgEmail.Text,
                chkOrgActive.Checked,
                txtOrgFacebook.Text,
                txtOrgTwitter.Text,
                txtOrgGoogle.Text, txtOrgCode.Text);

                Session["orgParams"] = orgDS2;
                Session["catType"] = true;
                Utilities.pageName = "btnManageOrganisations";
                Utilities.editId = -1;
                Response.Redirect("/Admin/OrganisationCategories.aspx");
            

            /*if (Utilities.editId != -1)
            {
                int result = dataProvider.UpdateOwnerOrganiasations(Utilities.userToken,orgDS);

                if (result != -1)
                {
                    SetToast("success","The Organisation has been succesfully updated!");
                    Utilities.editId = -1;
                    btnCloseModal_OnClick(sender,e);
                }
                else
                {
                    SetToast("error","An error has occured, please try again.");
                }
            }
            else
            {
                if (!Utilities.CheckGridForText(dgvOrganisations, txtOrgName.Text))
                {
                   int result = dataProvider.AddOwnerOrganiasation(Utilities.userToken, orgDS);

                    if (result != -1)
                    {
                        Utilities.editId = -1;
                        SetToast("success","Organisation has been created succesfully.");
                    }
                    else
                    {
                        SetToast("error","An error has occurred, please try again.");
                    }

                }
                else
                {
                    SetToast("info","Organisation already exists!");
                }

                DisplayOrganisationsInGrid();
            }

           
            DisplayOrganisationsInGrid();*/
        }

        protected void SetToast(string type, string errorMessage)
        {
            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>toastr."+type+"('" + errorMessage + "', '');</script>";  //if not null inject js to show modal
            }
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
            chkOrgActive.Checked = true;
        }
    }

     
}