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
    public partial class EventSocialMediaPage : System.Web.UI.Page
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
                btnEventSocialMediaPage.ForeColor = Color.Black;
                btnEventSocialMediaPage.BackColor = Color.White;
            }

            if (bool.Parse(Session["hasSession"].ToString()))
                btnEventSessions.Visible = true;

            removeInjectedScript();

            if (!IsPostBack)
            {
                DisplaySocialMediaInGrid();
                LoadTypesCB();
            }
        }

        private void LoadTypesCB()
        {
            var source = dataProvider.GetAllSocialMediaTypes(userToken);
            cbType.DataSource = source;
            cbType.DataBind();
            cbType.DataTextField = "Value";
            cbType.DataValueField = "Key";
            cbType.DataBind();
        }

        private void DisplaySocialMediaInGrid()
        {
            var source = dataProvider.GetSocialMediaPageByEventId(userToken, eventId);
            dgvSocialMedia.DataSource = source.SocialMedia;
            dgvSocialMedia.DataBind();
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
            SocialMediaDataSet eventSocialMediaDS = new SocialMediaDataSet();
            
            eventSocialMediaDS.SocialMedia.AddSocialMediaRow(int.Parse(cbType.SelectedValue), txtURL.Text,chkActive.Checked," ",Utilities.editId);

            if (Utilities.editId != -1)
            {
                int result = dataProvider.UpdateEventSocialMedia(Utilities.userToken, eventSocialMediaDS,eventId);

                    if (result != -1)
                    {
                        SetToast("success", "Social media page has been succesfully updated");
                        Utilities.editId = -1;
                        btnCloseModal_OnClick(sender, e);
                    }
                    else
                    {
                        SetToast("error", "Social Media Page exists already.");
                    }
               
            }
            else
            {
                if (!Utilities.CheckGridForText(dgvSocialMedia, txtURL.Text))
                {
                    int result = dataProvider.AddEventSocialMediaPage(Utilities.userToken, eventSocialMediaDS, eventId);

                    if (result != -1)
                    {
                        Utilities.editId = -1;
                        SetToast("success", "Social media page was created successfully.");
                        btnCloseModal_OnClick(sender, e);
                    }
                    else
                    {
                        SetToast("error", "An error has occured, try again.");
                    }


                }
                else
                {
                    SetToast("info", "A page with that URL already exists!");
                }

                DisplaySocialMediaInGrid();
            }

            DisplaySocialMediaInGrid();
        }

        protected void btnCloseModal_OnClick(object sender, EventArgs e)
        {
            txtURL.Text = string.Empty;
            chkActive.Checked = true;
            chkActive.Enabled = false;
            Utilities.editId = -1;
        }

        protected void dgvSocialMedia_OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            Utilities.editId = int.Parse(dgvSocialMedia.DataKeys[e.NewEditIndex].Value.ToString());

            e.Cancel = true;
            txtURL.Text = dgvSocialMedia.Rows[e.NewEditIndex].Cells[2].Text;
            cbType.Items.FindByText(dgvSocialMedia.Rows[e.NewEditIndex].Cells[1].Text).Selected = true;
            chkActive.Enabled = true;
            chkActive.Checked = bool.Parse(dgvSocialMedia.Rows[e.NewEditIndex].Cells[3].Text);

            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>$('#myModal').modal('show')</script>"; //if not null inject js to show modal
            }
        }

        protected void dgvSocialMedia_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            
            if (dgvSocialMedia.Rows[e.RowIndex].Cells[3].Text != "False")
            {
                Utilities.editId = int.Parse(dgvSocialMedia.DataKeys[e.RowIndex].Value.ToString());
                SocialMediaDataSet eventSessionDS = new SocialMediaDataSet();
                string _url = dgvSocialMedia.Rows[e.RowIndex].Cells[2].Text;
               
                eventSessionDS.SocialMedia.AddSocialMediaRow(int.Parse(cbType.SelectedValue), _url, false," ",Utilities.editId);

                dataProvider.UpdateEventSocialMedia(userToken, eventSessionDS,eventId);
                SetToast("success", "Event deactivated succesfully");
            }
            else
            {
                SetToast("info", "Event already deactivated");
            }
            DisplaySocialMediaInGrid();
        }

        protected void btnBacktoEvents_OnClick(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageEvents";
            Utilities.editId = -1;
            Response.Redirect("/Organisations/ManageEvents.aspx");
        }

        protected void btnAddEventCoordinator_OnClick(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageEvents";
            Session["eventName"] = Session["eventName"];
            Session["eventId"] = Session["eventId"];
            Utilities.editId = -1;
            Response.Redirect("/Organisations/EventCoordinator.aspx");
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