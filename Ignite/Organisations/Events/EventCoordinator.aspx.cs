using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ignite.DAL;

namespace Ignite.Organisations.Events
{
    public partial class EventCoordinator : System.Web.UI.Page
    {
        private DataProvider dataProvider = DataProvider.GetInstance();
        private string userToken = Utilities.userToken;
        private int eventId;

        private string eventName;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Utilities.editId = -1;
            

            if (Session["eventName"] == null && Session["eventId"] == null)
            {
                Response.Redirect("/Organisations/Events/ManageEvents.aspx");
            }
            else
            {
                lblEventName.Text = "'"+Session["eventName"].ToString()+"'";
                Utilities.pageName = "btnManageEvents";
                eventId = int.Parse(Session["eventId"].ToString());
                eventName = Session["eventName"].ToString();
                btnEventCoordinator.ForeColor = Color.Black;
                btnEventCoordinator.BackColor = Color.White;

            }

            if (bool.Parse(Session["hasSession"].ToString()))
                btnEventSessions.Visible = true;

            removeInjectedScript();

            if (!IsPostBack)
            {
                DisplayEventCoordinatorsInGrid();
                
            }
        }

        
        private void DisplayEventCoordinatorsInGrid()
        {
            var source = dataProvider.GetEventCoordinatorsByEventId(userToken, eventId);
            dgvEventCoordinator.DataSource = source.SysParameter;
            dgvEventCoordinator.DataBind();
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

        protected void btnSaveEventCoordinator_OnClick(object sender, EventArgs e)
        {
            SysParameterDataSet sysParamDS = new SysParameterDataSet();

            sysParamDS.SysParameter.AddSysParameterRow(Utilities.editId,txtName.Text,txtEmail.Text,chkActive.Checked);

            if (Utilities.editId != -1)
            {
                
                    int result = dataProvider.UpdateEventCoordinator(Utilities.userToken, sysParamDS, eventId);

                    if (result != -1)
                    {
                        SetToast("success", "Event Coordinator has been succesfully updated");
                        Utilities.editId = -1;
                        btnCloseModal_OnClick(sender, e);
                    }
                    else
                    {
                        SetToast("error", "An Event Coordinator with that email already exists!.");
                    }
               
            }
            else
            {
                if (!Utilities.CheckGridForText(dgvEventCoordinator, txtEmail.Text))
                {
                    int result = dataProvider.AddEventCoordinator(Utilities.userToken, sysParamDS, eventId);

                    if (result != -1)
                    {
                        Utilities.editId = -1;
                        SetToast("success", "Event Coordinator was created successfully.");
                        btnCloseModal_OnClick(sender, e);
                    }
                    else
                    {
                        SetToast("error", "Event Coordinator exists already!");
                    }


                }
                else
                {
                    SetToast("info", "An Event Coordinator with that email already exists!");
                }

                DisplayEventCoordinatorsInGrid();
            }

            DisplayEventCoordinatorsInGrid();
        }

        protected void btnCloseModal_OnClick(object sender, EventArgs e)
        {
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            chkActive.Checked = true;
            chkActive.Enabled = false;
            Utilities.editId = -1;
        }

        protected void dgvEventCoordinators_OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            Utilities.editId = int.Parse(dgvEventCoordinator.DataKeys[e.NewEditIndex].Value.ToString());

            e.Cancel = true;
            txtEmail.Text = dgvEventCoordinator.Rows[e.NewEditIndex].Cells[2].Text;
            txtName.Text = dgvEventCoordinator.Rows[e.NewEditIndex].Cells[1].Text;
            chkActive.Enabled = true;
            chkActive.Checked = bool.Parse(dgvEventCoordinator.Rows[e.NewEditIndex].Cells[3].Text);

            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>$('#myModal').modal('show')</script>"; //if not null inject js to show modal
            }
        }

        protected void dgvEventCoordinators_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            if (dgvEventCoordinator.Rows[e.RowIndex].Cells[3].Text != "False")
            {
                Utilities.editId = int.Parse(dgvEventCoordinator.DataKeys[e.RowIndex].Value.ToString());
                SysParameterDataSet sysDS = new SysParameterDataSet();
                string _name = dgvEventCoordinator.Rows[e.RowIndex].Cells[1].Text;
                String _email = dgvEventCoordinator.Rows[e.RowIndex].Cells[2].Text;
                sysDS.SysParameter.AddSysParameterRow(Utilities.editId,_name ,_email,false);

                dataProvider.UpdateEventCoordinator(userToken, sysDS, eventId);
                SetToast("success", "Event Coordinator deactivated succesfully");
            }
            else
            {
                SetToast("info", "Event Coordinator already deactivated");
            }
            DisplayEventCoordinatorsInGrid();
        }

        protected void btnBacktoEvents_OnClick(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageEvents";
            Response.Redirect("/Organisations/ManageEvents.aspx");
        }

        protected void btnAddSocialMedia_OnClick(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageEvents";
            Session["eventName"] = Session["eventName"];
            Session["eventId"] = Session["eventId"];
            Utilities.editId = -1;
            Response.Redirect("/Organisations/EventSocialMediaPage.aspx");
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

        protected void btnEventCategories_OnClick(object sender, EventArgs e)
        {
            Session["eventName"] = Session["eventName"];
            Session["eventId"] = Session["eventId"];
            Session["timeBeginCheck"] = Session["timeBeginCheck"];
            Session["timeEndCheck"] = Session["timeEndCheck"];
            Session["beginDate"] = Session["beginDate"];
            Session["endDate"] = Session["endDate"];
            Session["hasSession"] = Session["hasSession"];
            Session["location"] = Session["location"];
            Utilities.editId = -1;
            Utilities.pageName = "btnManageEvents";
            Response.Redirect("/Organisations/Events/EventCategories.aspx");
        }

        protected void btnEventCoordinator_OnClick(object sender, EventArgs e)
        {
            Session["eventName"] = Session["eventName"];
            Session["eventId"] = Session["eventId"];
            Session["timeBeginCheck"] = Session["timeBeginCheck"];
            Session["timeEndCheck"] = Session["timeEndCheck"];
            Session["beginDate"] = Session["beginDate"];
            Session["endDate"] = Session["endDate"];
            Session["hasSession"] = Session["hasSession"];
            Session["location"] = Session["location"];
            Utilities.editId = -1;
            Utilities.pageName = "btnManageEvents";
            Response.Redirect("/Organisations/Events/EventCoordinator.aspx");
        }

        protected void btnEventInterests_OnClick(object sender, EventArgs e)
        {
            Session["eventName"] = Session["eventName"];
            Session["eventId"] = Session["eventId"];
            Session["timeBeginCheck"] = Session["timeBeginCheck"];
            Session["timeEndCheck"] = Session["timeEndCheck"];
            Session["beginDate"] = Session["beginDate"];
            Session["endDate"] = Session["endDate"];
            Session["hasSession"] = Session["hasSession"];
            Session["location"] = Session["location"];
            Utilities.editId = -1;
            Utilities.pageName = "btnManageEvents";
            Response.Redirect("/Organisations/Events/EventInterests.aspx");
        }

        protected void btnEventSessions_OnClick(object sender, EventArgs e)
        {
            Session["eventName"] = Session["eventName"];
            Session["eventId"] = Session["eventId"];
            Session["timeBeginCheck"] = Session["timeBeginCheck"];
            Session["timeEndCheck"] = Session["timeEndCheck"];
            Session["beginDate"] = Session["beginDate"];
            Session["endDate"] = Session["endDate"];
            Session["hasSession"] = Session["hasSession"];
            Session["location"] = Session["location"];
            Utilities.editId = -1;
            Utilities.pageName = "btnManageEvents";
            Response.Redirect("/Organisations/Events/EventSessions.aspx");
        }

        protected void btnEventSocialMediaPage_OnClick(object sender, EventArgs e)
        {
            Session["eventName"] = Session["eventName"];
            Session["eventId"] = Session["eventId"];
            Session["timeBeginCheck"] = Session["timeBeginCheck"];
            Session["timeEndCheck"] = Session["timeEndCheck"];
            Session["beginDate"] = Session["beginDate"];
            Session["endDate"] = Session["endDate"];
            Session["hasSession"] = Session["hasSession"];
            Session["location"] = Session["location"];
            Utilities.editId = -1;
            Utilities.pageName = "btnManageEvents";
            Response.Redirect("/Organisations/Events/EventSocialMediaPage.aspx");
        }

        protected void btnScheduleAlerts_OnClick(object sender, EventArgs e)
        {
            Session["eventName"] = Session["eventName"];
            Session["eventId"] = Session["eventId"];
            Session["timeBeginCheck"] = Session["timeBeginCheck"];
            Session["timeEndCheck"] = Session["timeEndCheck"];
            Session["beginDate"] = Session["beginDate"];
            Session["endDate"] = Session["endDate"];
            Session["hasSession"] = Session["hasSession"];
            Session["location"] = Session["location"];
            Utilities.editId = -1;
            Utilities.pageName = "btnManageEvents";
            Response.Redirect("/Organisations/Events/EventAlerts.aspx");
        }

        protected void btnEventPricing_OnClick(object sender, EventArgs e)
        {
            Session["eventName"] = Session["eventName"];
            Session["eventId"] = Session["eventId"];
            Session["timeBeginCheck"] = Session["timeBeginCheck"];
            Session["timeEndCheck"] = Session["timeEndCheck"];
            Session["beginDate"] = Session["beginDate"];
            Session["endDate"] = Session["endDate"];
            Session["hasSession"] = Session["hasSession"];
            Session["location"] = Session["location"];
            Utilities.editId = -1;
            Utilities.pageName = "btnManageEvents";
            Response.Redirect("/Organisations/Events/EventPricing.aspx");
        }

        protected void btnViewAttendance_OnClick(object sender, EventArgs e)
        {

        }

        protected void btnFeedback_OnClick(object sender, EventArgs e)
        {

        }

        protected void btnMonitirinterest_OnClick(object sender, EventArgs e)
        {

        }
    }
}