using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ignite.DAL;

namespace Ignite.Organisations.Events
{
    public partial class EventSessions : System.Web.UI.Page
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
            else if (!bool.Parse(Session["hasSession"].ToString()))
            {
                Utilities.pageName = "btnManageEvents";
                Response.Redirect("/Organisations/Events/EventCategories.aspx");
            }
            else
            {
                lblEventName.Text = "'"+Session["eventName"].ToString()+"'";
                Utilities.pageName = "btnManageEvents";
                eventId = int.Parse(Session["eventId"].ToString());
                eventName = Session["eventName"].ToString();
                timeBeginCheck.Text = Session["timeBeginCheck"].ToString();
                timeEndCheck.Text = Session["timeEndCheck"].ToString();
                btnEventSessions.ForeColor = Color.Black;
                btnEventSessions.BackColor = Color.White;
            }

            removeInjectedScript();

            if (!IsPostBack)
            {
                DisplaySessionsInGrid();
            }
        }

        private void DisplaySessionsInGrid()
        {
            var source = dataProvider.GetEventSessionsByEventId(userToken, eventId);
            dgvSessions.DataSource = source.EventSession;
            dgvSessions.DataBind();
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

        protected void btnSaveSession_OnClick(object sender, EventArgs e)
        {
            EventSessionDataSet eventSessionDS = new EventSessionDataSet();
            TimeSpan _beginTime = DateTime.Parse(timeBegin.Text).TimeOfDay;
            TimeSpan _endTime = DateTime.Parse(timeEnd.Text).TimeOfDay;

            eventSessionDS.EventSession.AddEventSessionRow(Utilities.editId, txtName.Text, _beginTime, _endTime, eventId, eventName, chkActive.Checked,txtDescription.Text);

            if (Utilities.editId != -1)
            {
                    int result = dataProvider.UpdateEventSession(Utilities.userToken, eventSessionDS);

                    if (result != -1)
                    {
                        SetToast("success", "The Session has been succesfully updated");
                        Utilities.editId = -1;
                        btnCloseModal_OnClick(sender, e);

                    }
                    else
                    {
                        SetToast("error", "Session exists already.");
                    }
               
            }
            else
            {
                if (!Utilities.CheckGridForText(dgvSessions, txtName.Text))
                {
                    int result = dataProvider.AddEventSession(Utilities.userToken, eventSessionDS,eventId);

                    if (result != -1)
                    {
                        Utilities.editId = -1;
                        SetToast("success", "The Session was created successfully.");
                        btnCloseModal_OnClick(sender, e);
                    }
                    else
                    {
                        SetToast("error", "Session exists already.");
                    }

                    
                }
                else
                {
                    SetToast("info", "A session with that name already exists!");
                }

                DisplaySessionsInGrid();
            }

            DisplaySessionsInGrid();
        }

        protected void btnCloseModal_OnClick(object sender, EventArgs e)
        {
            txtName.Text = string.Empty;
            timeBegin.Text = string.Empty;
            timeEnd.Text = string.Empty;
            chkActive.Checked = true;
            chkActive.Enabled = false;
            Utilities.editId = -1;
        }

        protected void dgvSessions_OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            Utilities.editId = int.Parse(dgvSessions.DataKeys[e.NewEditIndex].Value.ToString());
            
            e.Cancel = true;
            txtName.Text = dgvSessions.Rows[e.NewEditIndex].Cells[1].Text;
            txtDescription.Text = dgvSessions.Rows[e.NewEditIndex].Cells[2].Text;
            string _beginTime = DateTime.Parse(dgvSessions.Rows[e.NewEditIndex].Cells[3].Text).ToString("HH:mm:ss");
            timeBegin.Text = _beginTime;
            string _endTime = DateTime.Parse(dgvSessions.Rows[e.NewEditIndex].Cells[4].Text).ToString("HH:mm:ss");
            timeEnd.Text = _endTime;
            chkActive.Enabled = true;
            chkActive.Checked = bool.Parse(dgvSessions.Rows[e.NewEditIndex].Cells[5].Text);

            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>$('#myModal').modal('show')</script>"; //if not null inject js to show modal
            }
        }

        protected void dgvSessions_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            if (dgvSessions.Rows[e.RowIndex].Cells[4].Text != "False")
            {
                Utilities.editId = int.Parse(dgvSessions.DataKeys[e.RowIndex].Value.ToString());
                EventSessionDataSet eventSessionDS = new EventSessionDataSet();
                TimeSpan _beginTime = DateTime.Parse(dgvSessions.Rows[e.RowIndex].Cells[3].Text).TimeOfDay;
                TimeSpan _endTime = DateTime.Parse(dgvSessions.Rows[e.RowIndex].Cells[4].Text).TimeOfDay;
                string _name = dgvSessions.Rows[e.RowIndex].Cells[1].Text;
                string _desc = dgvSessions.Rows[e.RowIndex].Cells[2].Text;
                eventSessionDS.EventSession.AddEventSessionRow(Utilities.editId,_name,_beginTime,_endTime,eventId,eventName,false,_desc);
                
                dataProvider.UpdateEventSession(userToken, eventSessionDS);
                SetToast("success", "Event deactivated succesfully");
            }
            else
            {
                SetToast("info", "Event already deactivated");
            }
            DisplaySessionsInGrid();
        }

        protected void btnBacktoEvents_OnClick(object sender, EventArgs e)
        {
            Session["eventId"] = Session["eventId"];
            Session["eventName"] = Session["eventName"];
            Session["timeBeginCheck"] = Session["timeBeginCheck"];
            Session["timeEndCheck"] = Session["timeEndCheck"];
            Utilities.pageName = "btnManageEvents";
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