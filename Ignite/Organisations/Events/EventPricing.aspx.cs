using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ignite.DAL;

namespace Ignite.Organisations.Events
{
    public partial class EventPricing : System.Web.UI.Page
    {
        private DataProvider dataProvider = DataProvider.GetInstance();
        private string userToken = Utilities.userToken;
        private int orgId = Utilities.companyId;
        private int eventId;
        private string eventName;


        protected void Page_Load(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageEvents";

            

            if (Session["eventName"] == null && Session["eventId"] == null)
            {
                Response.Redirect("/Organisations/Events/ManageEvents.aspx");
            }
            else
            {
                lblEventName.Text = "'" + Session["eventName"].ToString() + "'";
                Utilities.pageName = "btnManageEvents";
                eventId = int.Parse(Session["eventId"].ToString());
                eventName = Session["eventName"].ToString();
                btnEventPricing.ForeColor = Color.Black;
                btnEventPricing.BackColor = Color.White;

            }

            if (bool.Parse(Session["hasSession"].ToString()))
                btnEventSessions.Visible = true;

            removeInjectedScript();

            if (!IsPostBack)
            {
                DisplayTicketsInGrid(); //Populate datagridview
                LoadCurrencyCB();
            }


        }

        private void LoadCurrencyCB()
        {
            var source = dataProvider.GetActiveCurrencyByOrgId(userToken, Utilities.companyId);
            cbCurrency.DataSource = source.Currency;
            cbCurrency.DataBind();
            cbCurrency.DataTextField = "Name";
            cbCurrency.DataValueField = "Id";
            cbCurrency.DataBind();
        }


        private void removeInjectedScript()
        {
            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper");
            if (jsHelper != null)
            {
                jsHelper.Text = "";
            }
        }

        private void DisplayTicketsInGrid()
        {
            var org = dataProvider.GetAllEventTicketPricesByEvent(userToken, eventId);
            dgvTicket.DataSource = org.EventTicket; //bind result to datagridview
            dgvTicket.DataBind();
        }


        protected void dgvTicket_OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            e.Cancel = true;

            if (!(cbCurrency.Items.Count > 0))
            {
                SetToast("error", "No active currencies.");
                return;
            }

            Utilities.editId = int.Parse(dgvTicket.DataKeys[e.NewEditIndex].Value.ToString());
            //UserDataSet ticketDS = dataProvider.GetCurrencyById(Utilities.userToken, Utilities.editId);

            
            txtName.Text = dgvTicket.Rows[e.NewEditIndex].Cells[1].Text;
            try
            {
                cbCurrency.Items.FindByText(dgvTicket.Rows[e.NewEditIndex].Cells[3].Text).Selected = true;
            }
            catch (NullReferenceException ex)
            {
                LoadCurrencyCB();
                
            }
            
            txtPrice.Text = dgvTicket.Rows[e.NewEditIndex].Cells[5].Text;
            txtMaxSession.Text = dgvTicket.Rows[e.NewEditIndex].Cells[6].Text;
            txtMaxEntries.Text = dgvTicket.Rows[e.NewEditIndex].Cells[7].Text;
            txtNumDays.Text = dgvTicket.Rows[e.NewEditIndex].Cells[8].Text;
            chkDuplicates.Checked = bool.Parse(dgvTicket.Rows[e.NewEditIndex].Cells[9].Text);
            chkActive.Checked = bool.Parse(dgvTicket.Rows[e.NewEditIndex].Cells[10].Text);
            chkActive.Enabled = true;
            txtDescription.Text = dgvTicket.Rows[e.NewEditIndex].Cells[2].Text;


            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>$('#myModal').modal('show')</script>"; //if not null inject js to show modal
            }
        }



        protected void btnCloseModal_OnClick(object sender, EventArgs e)
        {
            txtName.Text = string.Empty;
            //cbCurrency.Items.FindByText(dgvTicket.Rows[e.NewEditIndex].Cells[2].Text).Selected = true;
            txtPrice.Text = string.Empty;
            txtMaxSession.Text = string.Empty;
            txtMaxEntries.Text = string.Empty;
            txtNumDays.Text = string.Empty;
            chkDuplicates.Checked = false;
            chkActive.Checked = true;
            chkActive.Enabled = false;
            txtDescription.Text = string.Empty;
            Utilities.editId = -1;

        }

        protected void btnSaveTicket_OnClick(object sender, EventArgs e)
        {
            var ticketDS = new DAL.EventTicketDataSet();

            ticketDS.EventTicket.AddEventTicketRow(Utilities.editId, txtName.Text, decimal.Parse(txtPrice.Text),
                int.Parse(cbCurrency.SelectedValue), eventId, eventName, int.Parse(txtMaxEntries.Text),
                int.Parse(txtNumDays.Text), chkDuplicates.Checked, chkActive.Checked, cbCurrency.SelectedItem.ToString(),
                int.Parse(txtMaxSession.Text),txtDescription.Text);

            if (Utilities.editId != -1)
            {
                int result = dataProvider.UpdateEventTicketPrice(userToken, ticketDS);

                if (result != -1)
                {
                    SetToast("success", "Ticket has been succesfully updated");
                    Utilities.editId = -1;
                    btnCloseModal_OnClick(sender, e);
                }
                else
                {
                    SetToast("error", "Ticket already exists");
                }
            }
            else
            {
                if (!Utilities.CheckGridForText(dgvTicket, txtName.Text))
                {
                    int result = dataProvider.AddEventTicketPrice(Utilities.userToken, ticketDS);

                    if (result != -1)
                    {
                        Utilities.editId = -1;
                        SetToast("success", "Ticket was created successfully.");
                    }
                    else
                    {
                        SetToast("error", "An error has occured, try again.");
                    }
                }
                else
                {
                    SetToast("info", "Ticket already exists");
                }

                DisplayTicketsInGrid();
            }

            DisplayTicketsInGrid();
        }

        protected void SetToast(string type, string errorMessage)
        {
            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>toastr." + type + "('" + errorMessage + "', '');</script>";  //if not null inject js to show modal
            }
        }

        protected void dgvTicket_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (dgvTicket.Rows[e.RowIndex].Cells[9].Text != "False")
            {
                Utilities.editId = int.Parse(dgvTicket.DataKeys[e.RowIndex].Value.ToString());

                string _txtName = dgvTicket.Rows[e.RowIndex].Cells[1].Text;
                int _currencyId = int.Parse(dgvTicket.Rows[e.RowIndex].Cells[4].Text);
                string _currency = dgvTicket.Rows[e.RowIndex].Cells[3].Text;
                string _txtPrice = dgvTicket.Rows[e.RowIndex].Cells[5].Text;
                string _txtMaxSession = dgvTicket.Rows[e.RowIndex].Cells[6].Text;
                string _txtMaxEntries = dgvTicket.Rows[e.RowIndex].Cells[7].Text;
                string _txtNumDays = dgvTicket.Rows[e.RowIndex].Cells[8].Text;
                bool _chkDuplicates = bool.Parse(dgvTicket.Rows[e.RowIndex].Cells[9].Text);
                string _txtDescription = dgvTicket.Rows[e.RowIndex].Cells[2].Text;

                EventTicketDataSet ticket = new EventTicketDataSet();
                ticket.EventTicket.AddEventTicketRow(Utilities.editId, _txtName, decimal.Parse(_txtPrice), _currencyId,
                    eventId, eventName,
                    int.Parse(_txtMaxEntries), int.Parse(_txtNumDays), _chkDuplicates,false, _currency,
                    int.Parse(_txtMaxSession),_txtDescription);

                dataProvider.UpdateEventTicketPrice(userToken, ticket);
                SetToast("success", "Currency deactivated succesfully");
            }
            else
            {
                SetToast("info", "Currency already deactivated");
            }
            DisplayTicketsInGrid();
        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            if (!(cbCurrency.Items.Count > 0))
            {
                SetToast("error","No active currencies.");
                return;
            }

            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>$('#myModal').modal('show')</script>"; //if not null inject js to show modal
            }
            Utilities.editId = -1;
            btnCloseModal_OnClick(sender, e);
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