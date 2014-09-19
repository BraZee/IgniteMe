using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ignite.DAL;

namespace Ignite.Organisations.Events
{
    public partial class ManageEvents : System.Web.UI.Page
    {
        DataProvider dataProvider = DataProvider.GetInstance();
        private string userToken = Utilities.userToken;
        private int orgId = Utilities.companyId;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            Session.RemoveAll();
            Utilities.pageName = "btnManageEvents";
            currDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
           
            if (!IsPostBack)
            {
                dateBegin.Attributes.Add("min", DateTime.Now.ToString("yyyy-MM-dd"));
                dateBegin.Text = DateTime.Now.ToString("yyyy-MM-dd");
                dateEnd.Attributes.Add("min", DateTime.Now.ToString("yyyy-MM-dd"));
                dateEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");

                LoadLocationsCB();
                DisplayEventsInGrid();
            }

            removeInjectedScript();
            

        }

        private void LoadLocationsCB()
        {
            var source = dataProvider.GetActiveLocationsByOrgId(userToken, orgId);
            cbLocations.DataSource = source;
            cbLocations.DataBind();
            cbLocations.DataTextField = "Name";
            cbLocations.DataValueField = "Id";
            cbLocations.DataBind();
        }

        private void DisplayEventsInGrid()
        {
            var source = dataProvider.GetEventsByOrgId(userToken, orgId);
            dgvOrgEvents.DataSource = source;
            dgvOrgEvents.DataBind();
        }

        protected void btnCloseModal_OnClick(object sender, EventArgs e)
        {
            txtName.Text = string.Empty;
            dateBegin.Text = DateTime.Now.ToString("yyyy-MM-dd");
            dateEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");
            timeBegin.Text = string.Empty;
            timeEnd.Text = string.Empty;
            chkActive.Checked = true;
            chkActive.Enabled = false;
            Utilities.editId = -1;

        }

        protected void btnSaveEvent_OnClick(object sender, EventArgs e)
        {
            TimeSpan add = new TimeSpan(0, 23, 59, 59);
            DateTime _beginDate = DateTime.Parse(dateBegin.Text).Date;
            DateTime _endDate;
            if (chkRoutine.Checked)
            {
                _endDate = DateTime.Parse("9999-12-31").Add(add);
            }
            else
            {
                _endDate = DateTime.Parse(dateEnd.Text).Add(add);
            }
            TimeSpan _beginTime = DateTime.Parse(timeBegin.Text).TimeOfDay;
            TimeSpan _endTime = DateTime.Parse(timeEnd.Text).TimeOfDay;
            var eventDS = new EventDataSet();

            try
            {
                
                eventDS.Event.AddEventRow(Utilities.companyId, txtName.Text, _beginDate.ToString(), _endDate.ToString(),
                    _beginTime,
                    _endTime,
                    int.Parse(cbLocations.SelectedValue), chkActive.Checked, chkRoutine.Checked,
                    cbLocations.SelectedItem.ToString(), Utilities.editId, chkHasSessions.Checked,txtDesc.Text);
            }
            catch (Exception ex)
            {
                SetToast("Error","Check form data.");
                DisplayEventsInGrid();
                return;
            }

            if (Utilities.editId == -1)
            {
                int result = dataProvider.AddEvent(Utilities.userToken, eventDS);

                if (result != -1)
                {
                    Utilities.editId = -1;
                    SetToast("success","Event Created succesfully.");
                    btnCloseModal_OnClick(sender, e);
                }
                else
                {
                    SetToast("error", "Event exists already.");
                    return;
                }

            }
            else
            {
                int result = dataProvider.UpdateEvent(Utilities.userToken, eventDS);

                if (result != -1)
                {
                    SetToast("success","Event updated succesfully");
                    btnCloseModal_OnClick(sender,e);
                    Utilities.editId = -1;
                }
                else
                {
                    SetToast("info","Event exists already");
                    return;
                }
                DisplayEventsInGrid();
            }
            
            DisplayEventsInGrid();


//            var eventDS2 = new EventDataSet();
//            eventDS2.Event.AddEventRow(Utilities.companyId, txtName.Text, _beginDate, _endDate, _beginTime,
//                    _endTime,
//                    int.Parse(cbLocations.SelectedValue), chkActive.Checked, chkRoutine.Checked,
//                    cbLocations.SelectedItem.ToString(), Utilities.editId,chkHasSessions.Checked);
//
//            Session["eventId"] = Utilities.editId;
//            Session["eventName"] = txtName.Text;
//            TimeSpan sub = new TimeSpan(0,0,0,1);
//            Session["timeBeginCheck"] = _beginTime.Subtract(sub);
//            Session["timeEndCheck"] = _endTime;
//            Session["hasSession"] = chkHasSessions.Checked;
//            Session["eventParams"] = eventDS2;
//            Utilities.pageName = "btnManageEvents";
//            //Utilities.editId = -1;
//            Response.Redirect("/Organisations/EventCategories.aspx");
        }

        protected void SetToast(string type, string errorMessage)
        {
            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>toastr." + type + "('" + errorMessage + "', '');</script>";  //if not null inject js to show modal
            }
        }

        protected void dgvOrgEvents_OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            e.Cancel = true;
            LoadLocationsCB();
            Utilities.editId = int.Parse(dgvOrgEvents.Rows[e.NewEditIndex].Cells[0].Text);
            Session.RemoveAll();
            txtName.Text = dgvOrgEvents.Rows[e.NewEditIndex].Cells[1].Text;
            cbLocations.Items.FindByText(dgvOrgEvents.Rows[e.NewEditIndex].Cells[2].Text).Selected = true;
            string _beginDate = DateTime.Parse(dgvOrgEvents.Rows[e.NewEditIndex].Cells[3].Text).ToString("yyyy-MM-dd");
            dateBegin.Text = _beginDate;
            string _endDate = DateTime.Parse(dgvOrgEvents.Rows[e.NewEditIndex].Cells[4].Text).ToString("yyyy-MM-dd");
            dateEnd.Text = _endDate;
            chkRoutine.Checked = bool.Parse(dgvOrgEvents.Rows[e.NewEditIndex].Cells[5].Text);
            chkActive.Enabled = true;
            chkActive.Checked = bool.Parse(dgvOrgEvents.Rows[e.NewEditIndex].Cells[6].Text);
            string _timeBegin = DateTime.Parse(dgvOrgEvents.Rows[e.NewEditIndex].Cells[7].Text).ToString("HH:mm:ss");
            timeBegin.Text = _timeBegin;
            string _timeEnd = DateTime.Parse(dgvOrgEvents.Rows[e.NewEditIndex].Cells[8].Text).ToString("HH:mm:ss");
            timeEnd.Text = _timeEnd;
            chkHasSessions.Checked = bool.Parse(dgvOrgEvents.Rows[e.NewEditIndex].Cells[10].Text);
            txtDesc.Text = dgvOrgEvents.Rows[e.NewEditIndex].Cells[11].Text;
            
            if(chkHasSessions.Checked)
            chkHasSessions.Enabled = false;

            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>$('#myModal').modal('show')</script>";  //if not null inject js to show modal
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

        protected void dgvOrgEvents_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            /*if (dgvOrgEvents.Rows[e.RowIndex].Cells[6].Text != "False")
            {
                Utilities.editId = int.Parse(dgvOrgEvents.DataKeys[e.RowIndex].Value.ToString());
                dataProvider.DeactivateEvent(userToken, Utilities.editId);
                SetToast("success", "Event deactivated succesfully");
            }
            else
            {
                SetToast("info", "Event already deactivated");
            }
            DisplayEventsInGrid();*/
        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            if (cbLocations.Items.Count > 0)
            {
                var jsHelper = (Label) Master.Master.FindControl("LblJsHelper"); //get label in master page

                if (jsHelper != null)
                {
                    jsHelper.Text = "<script>$('#myModal').modal('show')</script>";
                    //if not null inject js to show modal
                }
                Utilities.editId = -1;
                btnCloseModal_OnClick(sender, e);
                chkActive.Checked = true;
            }
            else
            {
                SetToast("error","Please setup locations first.");
            }
        }

        protected void dgvOrgEvents_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = int.Parse(e.CommandArgument.ToString());
            GridViewRow row = (GridViewRow)dgvOrgEvents.Rows[rowIndex];

            if (row.Cells[6].Text == "False")
            {
                SetToast("error","No setup for deactivated events.");
                Session.RemoveAll();
                return;
            }
            if (e.CommandName != "Edit" && e.CommandName != "Delete")
            {
                
                Session["eventName"] = row.Cells[1].Text;
                //Utilities.userGroupId_edit = int.Parse(row.Cells[1].Text);
                DataKey rowDK = (DataKey)dgvOrgEvents.DataKeys[rowIndex];
                Session["eventId"] = int.Parse(rowDK.Value.ToString());
                TimeSpan _beginTime = DateTime.Parse(row.Cells[7].Text).TimeOfDay;
                TimeSpan _endTime = DateTime.Parse(row.Cells[8].Text).TimeOfDay;
                TimeSpan sub = new TimeSpan(0, 0, 0, 1);
                Session["timeBeginCheck"] = _beginTime.Subtract(sub);
                Session["timeEndCheck"] = _endTime;
                Session["hasSession"] = bool.Parse(row.Cells[10].Text);
                Session["beginDate"] = DateTime.Parse(row.Cells[3].Text).Date;
                Session["endDate"] = DateTime.Parse(row.Cells[4].Text).Date;
                Session["location"] = row.Cells[2].Text;
                Utilities.pageName = "btnManageEvents";
                Utilities.editId = -1;
                Response.Redirect("/Organisations/Events/EventCategories.aspx");
            }
        }
    }
}