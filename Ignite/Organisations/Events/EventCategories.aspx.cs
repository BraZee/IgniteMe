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
    public partial class EventCategories : System.Web.UI.Page
    {
        private DataProvider dataProvider = DataProvider.GetInstance();
        private string userToken = Utilities.userToken;
        private int userId = Utilities.userId;
        private int orgId = Utilities.companyId;
        //        private int userGroupId_edit;
        //        private string userGroup_edit = Utilities.userGroup_edit;
        Dictionary<int, string> categories = new Dictionary<int, string>();
        List<string> categoriesList = new List<string>();
        private int _eventId;


        protected void Page_Load(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageEvents";

            

            removeInjectedScript();
            if (Session["eventName"] == null || Session["eventId"] ==null)
            {
                Response.Redirect("/Organisations/Events/ManageEvents.aspx");
            }
            else
            {
                _eventId = int.Parse(Session["eventId"].ToString());
                eventName.Text = "to '" + Session["eventName"] + "'";
                btnEventCategories.ForeColor = Color.Black;
                btnEventCategories.BackColor = Color.White;
            }

            if (bool.Parse(Session["hasSession"].ToString()))
                btnEventSessions.Visible = true;

            if (!IsPostBack)
            {
                LoadCategoriesCB();
            }
            DisplayCategoriesInGrid(true);

        }

        private void LoadCategoriesCB()
        {
            var source = dataProvider.GetActiveCategoriesByType(userToken, false);
            cbCategories.DataSource = source;
            cbCategories.DataBind();
            cbCategories.DataTextField = "Name";
            cbCategories.DataValueField = "Id";
            cbCategories.DataBind();
        }

        private void DisplayCategoriesInGrid(bool init)
        {
            if (init)
                GetOperations();
            dgvCategories.DataSource = categories;
            dgvCategories.DataBind();
        }

        private void GetOperations()
        {
            /*var categoriesDS = dataProvider.GetCategoriesByOrgId(userToken, catDS.Organisation[0].Id);
            //Dictionary<int,string> categories = new Dictionary<int, string>();

            foreach (var category in categoriesDS.Category)
            {
                if (!categories.ContainsKey(category.CategoryId))
                {
                    categories.Add(category.CategoryId, category.Category);
                }
            }*/

            categories = dataProvider.GetCategoriesByEventId(userToken, _eventId);
            //Dictionary<int,string> categories = new Dictionary<int, string>();


        }

        protected void btnAdd_OnClick(object sender, EventArgs e)
        {
            Session["eventParams"] = Session["eventParams"];
            //Utilities.pageName = "btnManageOrganisations";

            if (!Utilities.CheckGridForText(dgvCategories, cbCategories.SelectedItem.ToString()))
            {
                categories.Add(int.Parse(cbCategories.SelectedValue), cbCategories.SelectedItem.ToString());

                foreach (int category in categories.Keys)
                {
                    categoriesList.Add(category.ToString());
                }

                int result = dataProvider.SetCategoriesForEvent(_eventId, categoriesList);

                if (result != -1)
                {
                    SetToast("success", "Category added.");
                }
                else
                {
                    SetToast("error", "An error occured, try again.");
                }


            }
            else
            {
                SetToast("info", "Category already added.");
            }

            DisplayCategoriesInGrid(false);
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

            foreach (int category in categories.Keys)
            {
                categoriesList.Add(category.ToString());
            }

            //catDS.Category.AddCategoryRow(_id, _name, _desc, _active);

            //            if (Utilities.editId != -1)
            //            {
            //int result = dataProvider.UpdateEvent(userToken, catDS);

            if (result != -1)
            {
                Session["eventId"] = Session["eventId"];
                Session["eventName"] = Session["eventName"];
                //TimeSpan sub = new TimeSpan(0, 0, 0, 1);
                Session["timeBeginCheck"] = Session["timeBeginCheck"];
                Session["timeEndCheck"] = Session["timeEndCheck"];
                Session["hasSession"] = Session["hasSession"];
                Session["eventParams"] = Session["eventParams"];
                Utilities.pageName = "btnManageEvents";
                //Utilities.editId = -1;
                Response.Redirect("/Organisations/EventInterests.aspx");
            }
            else
            {
                Utilities.editId = -1;
                Session.RemoveAll();
                Session["error"] = "Event exists already.";
                Utilities.pageName = "btnManageOrganisations";
                Utilities.editId = -1;
                Response.Redirect("/Organisations/ManageEvents.aspx");
            }
            //}
            // else
            // {
            //                int result = dataProvider.AddOwnerOrganiasation(userToken, catDS);
            //
            //
            //                if (result != -1)
            //                {
            //                    Utilities.editId = -1;
            //                    Session.RemoveAll();
            //                    Session["success"] = "Organisation added succesfully.";
            //                    Utilities.pageName = "btnManageOrganisations";
            //                    Response.Redirect("/Admin/ManageOrganisations.aspx");
            //                }
            //                else
            //                {
            //                    SetToast("error", "An error has occured, try again.");
            //                }

            // }
        }*/

        protected void dgvCategories_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            List<string> categoriesList = new List<string>();

            categories.Remove(int.Parse(dgvCategories.DataKeys[e.RowIndex].Value.ToString()));

            foreach (int category in categories.Keys)
            {
                categoriesList.Add(category.ToString());
            }

            int result = dataProvider.SetCategoriesForEvent(_eventId, categoriesList);

            if (result != -1)
            {
                SetToast("success", "Category removed.");
            }
            else
            {
                SetToast("error", "An error occured, try again.");
            }

            DisplayCategoriesInGrid(false);
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