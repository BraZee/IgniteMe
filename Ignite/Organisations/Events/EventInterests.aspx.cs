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
    public partial class EventInterests : System.Web.UI.Page
    {
        private DataProvider dataProvider = DataProvider.GetInstance();
        private string userToken = Utilities.userToken;
        private int _eventId;
        private int userGroupId_edit;
        private string userGroup_edit = Utilities.userGroup_edit;
        Dictionary<int, string> interests = new Dictionary<int, string>();
        List<string> interestsList = new List<string>();
       


        protected void Page_Load(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageEvents";
            

            removeInjectedScript();
            if (Session["eventName"] == null || Session["eventId"] == null)
            {
                Response.Redirect("/Organisations/Events/ManageEvents.aspx");
            }
            else
            {
                _eventId = int.Parse(Session["eventId"].ToString());
                userGroup.Text = "to '" + Session["eventName"] + "'";
                btnEventInterests.ForeColor = Color.Black;
                btnEventInterests.BackColor = Color.White;
            }

            if (bool.Parse(Session["hasSession"].ToString()))
                btnEventSessions.Visible = true;

            if (!IsPostBack)
            {
                LoadInterestsCB();
            }
            DisplayInterestsInGrid(true);

        }

        private void LoadInterestsCB()
        {
            var source = dataProvider.GetActiveInterests(userToken);
            cbInterests.DataSource = source;
            cbInterests.DataBind();
            cbInterests.DataTextField = "Name";
            cbInterests.DataValueField = "Id";
            cbInterests.DataBind();
        }

        private void DisplayInterestsInGrid(bool init)
        {
            if(init)
            GetInterests();

            dgvInterests.DataSource = interests;
            dgvInterests.DataBind();}

        private void GetInterests()
        {
            interests = dataProvider.GetInterestsByEventId(userToken,_eventId);
            //Dictionary<int,string> interests = new Dictionary<int, string>();

//            foreach (var category in interestsDS.Category)
//            {
//                if (!interests.ContainsKey(category.CategoryId))
//                {
//                    interests.Add(category.CategoryId, category.Category);
//                }
//            }
        }

        protected void btnAdd_OnClick(object sender, EventArgs e)
        {
            Session["orgParams"] = Session["orgParams"];


            if (!Utilities.CheckGridForText(dgvInterests, cbInterests.SelectedItem.ToString()))
            {
                interests.Add(int.Parse(cbInterests.SelectedValue), cbInterests.SelectedItem.ToString());

                foreach (int category in interests.Keys)
                {
                    interestsList.Add(category.ToString());
                }

                int result = dataProvider.SetInterestsForEvent(_eventId, interestsList);

                if (result != -1)
                {
                    SetToast("success", "Interest added.");
                }
                else
                {
                    SetToast("error", "An error occured, try again.");
                }

            }
            else
            {
                SetToast("info", "Interest already added.");
            }

            DisplayInterestsInGrid(false);
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

        /*protected void btnSave_OnClick(object sender, EventArgs e)
        {
            //CategoryDataSet catDS = new CategoryDataSet();

            foreach (int category in interests.Keys)
            {
                interestsList.Add(category.ToString());
            }

            //catDS.Category.AddCategoryRow(_id, _name, _desc, _active);

//            if (Utilities.editId != -1)
//            {
                int result = dataProvider.UpdateEvent(userToken, eventDS);

                if (result != -1)
                {
                    if (bool.Parse(Session["hasSession"].ToString()))
                    {
                        Session["eventId"] = Session["eventId"];
                        Session["eventName"] = Session["eventName"];
                        Session["timeBeginCheck"] = Session["timeBeginCheck"];
                        Session["timeEndCheck"] = Session["timeEndCheck"];
                        Utilities.pageName = "btnManageEvents";
                        Utilities.editId = -1;
                        Response.Redirect("/Organisations/EventSessions.aspx");
                    }
                    else
                    {
                        Session["eventId"] = Session["eventId"];
                        Session["eventName"] = Session["eventName"];
                        Session["timeBeginCheck"] = Session["timeBeginCheck"];
                        Session["timeEndCheck"] = Session["timeEndCheck"];
                        Utilities.pageName = "btnManageEvents";
                        Utilities.editId = -1;
                        Response.Redirect("/Organisations/EventSocialMediaPage.aspx");

                    }
                }
                else
                {
                    SetToast("error", "An Event with that name exists already.");
                }
           // }
//            else
//            {
                /*int result = dataProvider.AddEvent(userToken, eventDS, interestsList);

                if (result != -1)
                {
                    Utilities.editId = -1;
                    Session.RemoveAll();
                    Session["success"] = "Organisation added succesfully.";
                    Response.Redirect("/Admin/ManageOrganisations.aspx");
                }
                else
                {
                    SetToast("error", "An error has occured, try again.");
                }#1#

            //}
        }*/

        protected void dgvInterests_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            List<string> interestsList = new List<string>();

            interests.Remove(int.Parse(dgvInterests.DataKeys[e.RowIndex].Value.ToString()));

            foreach (int category in interests.Keys)
            {
                interestsList.Add(category.ToString());
            }

            int result = dataProvider.SetInterestsForEvent(_eventId, interestsList);

            if (result != -1)
            {
                SetToast("success", "Interest removed.");
            }
            else
            {
                SetToast("error", "An error occured, try again.");
            }

            DisplayInterestsInGrid(false);
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