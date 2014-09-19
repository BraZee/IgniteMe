using System;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Ignite.DAL;

namespace Ignite.Organisations.Events
{
    public partial class EventAlerts : System.Web.UI.Page
    {
        DataProvider dataProvider = DataProvider.GetInstance();
        private string userToken = Utilities.userToken;
        private int eventId;
        private string eventNameParam;
        private bool isEdit;
        private bool initEmail;
        private bool initSMS;

        protected void Page_Load(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageEvents";

            if (Session["eventName"] == null || Session["eventId"] == null)
            {
                Response.Redirect("/Organisations/Events/ManageEvents.aspx");
            }
            else
            {
                btnScheduleAlerts.ForeColor = Color.Black;
                btnScheduleAlerts.BackColor = Color.White;
                eventId = int.Parse(Session["eventId"].ToString());
                eventNameParam = Session["eventName"].ToString();
            }

            if (bool.Parse(Session["hasSession"].ToString()))
                btnEventSessions.Visible = true;

            if(!IsPostBack)
            {
                initFields();
            }
            removeInjectedScript();
        }

        private void initFields()
        {
            int msgType = 0;
            string isUser = string.Empty;

            if (messagetypes.SelectedValue == "1")
            {
                msgType = 1;
            }
            else if (messagetypes.SelectedValue == "2")
            {
                msgType = 2;
            }
            else if (messagetypes.SelectedValue == "3")
            {
                msgType = 3;
            }

            if (recipients.SelectedValue == "True")
            {
                isUser = "True";
            }
            else if (recipients.SelectedValue == "False")
            {
                isUser = "False";
            }

            if (msgType != 0 && isUser != string.Empty)
            {
                var SMS = dataProvider.GetEventAlertSMS(userToken, eventId, msgType, bool.Parse(isUser));
                var Email = dataProvider.GetEventAlertEmail(userToken, eventId, msgType, bool.Parse(isUser));
                DateTime _smsSendDate = DateTime.Now;
                DateTime _emailSendDate = DateTime.Now;

                if (SMS.SMS.Count > 0 && Email.Email.Count < 1)
                {
                    _smsSendDate = SMS.SMS[0].SendDate;
                    txtSMS.Text = SMS.SMS[0].SMS;
                    smsId.Value = SMS.SMS[0].Id.ToString();
                    txtBody.Text = string.Empty;
                    txtSubject.Text = string.Empty;
                    Utilities.editId = 2;
                }
                else if (Email.Email.Count > 0 && SMS.SMS.Count < 1)
                {
                    _emailSendDate = Email.Email[0].SendDate;
                    txtSubject.Text = Email.Email[0].Subject;
                    txtBody.Text = Email.Email[0].Body;
                    emailId.Value = Email.Email[0].Id.ToString();
                    txtSMS.Text = string.Empty;
                    Utilities.editId = 3;
                }
                else if (SMS.SMS.Count > 0 && Email.Email.Count > 0)
                {
                    _emailSendDate = Email.Email[0].SendDate;
                    _smsSendDate = SMS.SMS[0].SendDate;
                    txtSubject.Text = Email.Email[0].Subject;
                    txtBody.Text = Email.Email[0].Body;
                    txtSMS.Text = SMS.SMS[0].SMS;
                    smsId.Value = SMS.SMS[0].Id.ToString();
                    emailId.Value = Email.Email[0].Id.ToString();
                    Utilities.editId = 4;
                }
                else
                {
                    txtBody.Text = string.Empty;
                    txtSMS.Text = string.Empty;
                    txtSubject.Text = string.Empty;
                    txtDaysBeforeAfterEmail.Text = string.Empty;
                    txtDaysBeforeAfterEmail.Text = string.Empty;
                    Utilities.editId = -1;
                    initSMS = true;
                    initEmail = true;
                    return;
                }

                
                
                if (msgType == 1)
                {
                    TimeSpan smsDays = DateTime.Parse(Session["beginDate"].ToString()).Subtract(_smsSendDate);
                    TimeSpan emailDays = DateTime.Parse(Session["beginDate"].ToString()).Subtract(_emailSendDate);
                    txtDaysBeforeAfterSMS.Text = smsDays.Days.ToString();
                    txtDaysBeforeAfterEmail.Text = emailDays.Days.ToString();
                   
                }
                else if (msgType == 2)
                {
                    txtDaysBeforeAfterEmail.Enabled = false;
                    txtDaysBeforeAfterSMS.Enabled = false;
                }
                else if (msgType == 3)
                {
                    TimeSpan smsDays = _smsSendDate.Subtract(DateTime.Parse(Session["endDate"].ToString()));
                    TimeSpan emailDays = _emailSendDate.Subtract(DateTime.Parse(Session["endDate"].ToString()));
                    txtDaysBeforeAfterEmail.Text = emailDays.Days.ToString();
                    txtDaysBeforeAfterSMS.Text = smsDays.Days.ToString();
                   
                }

                //Utilities.editId = 10;
            }
            else
            {
                txtBody.Text = string.Empty;
                txtSMS.Text = string.Empty;
                txtSubject.Text = string.Empty;
                txtDaysBeforeAfterEmail.Text = string.Empty;
                txtDaysBeforeAfterSMS.Text = string.Empty;
                Utilities.editId = -1;
            }


        }

        protected void appendPlaceholder(object sender, EventArgs e)
        {
            var control = (HtmlControl) sender;

            switch (control.ID)
            {
                case "recipientName":
                    PlaceholderInsert("[Recipient Name]");
                    break;

                case "eventName":
                    PlaceholderInsert("[Event Name]");
                    break;

                case "location":
                    PlaceholderInsert("[Location]");
                    break;

                case "beginTime":
                    PlaceholderInsert("[Begin Time]");
                    break;

                case "beginDate":
                    PlaceholderInsert("[Begin Date]");
                    break;

                case "siteURL":
                    PlaceholderInsert("[Site URL]");
                    break;

                case "price":
                    PlaceholderInsert("[Price]");
                    break;

                case "endDate":
                    PlaceholderInsert("[End Date]");
                    break;

                case "endTime":
                    PlaceholderInsert("[End Time]");
                    break;

                default:
                    return;

            }
        }

        private void PlaceholderInsert(string text)
        {
            if (selectedBox.Value == "body")
            {
                if (CaretPos.Value != "")
                {
                    string newText = txtBody.Text.Insert(int.Parse(CaretPos.Value), text);
                    txtBody.Text = newText;
                }
                else
                {
                    txtBody.Text = txtBody.Text.Insert(0, text);
                }
                txtBody.Focus();
            }
            else if (selectedBox.Value == "sms")
            {
                    if (CaretPos.Value != "")
                {
                    string newText = txtSMS.Text.Insert(int.Parse(CaretPos.Value), text);
                    txtSMS.Text = newText;
                }
                else
                {
                    txtSMS.Text = txtSMS.Text.Insert(0, text);
                }

                    if (Session["placeholderLength"] == null)
                        Session["placeholderLength"] = "0";

                    int temp = int.Parse(Session["placeholderLength"].ToString());
                    Session["placeholderLength"] = temp+text.Length;

                txtSMS.Focus();
            }
            else if (selectedBox.Value == "subject")
            {
                if (CaretPos.Value != "")
                {
                    string newText = txtSubject.Text.Insert(int.Parse(CaretPos.Value), text);
                    txtSubject.Text = newText;
                }
                else
                {
                    txtSubject.Text = txtSubject.Text.Insert(0, text);
                }
                txtSubject.Focus();
            }

           
            
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

        
        protected void btnSaveAlertEmail_OnClick(object sender, EventArgs e)
        {
            DateTime _sendDate;
            int _msgType;

            if (txtDaysBeforeAfterEmail.Text == "")
                txtDaysBeforeAfterEmail.Text = "0";

            if (emailId.Value == "")
                emailId.Value = "0";

            if (smsId.Value == "")
                smsId.Value = "0";

            if (messagetypes.SelectedValue == "1")
            {

                TimeSpan sub = new TimeSpan(int.Parse(txtDaysBeforeAfterEmail.Text),0,0,0);
                _sendDate = DateTime.Parse(Session["beginDate"].ToString()).Subtract(sub);
                _msgType = 1;
            }
            else if (messagetypes.SelectedValue == "2")
            {
                _sendDate = DateTime.Parse(Session["beginDate"].ToString());
                _msgType = 2;
            }
            else if (messagetypes.SelectedValue == "3")
            {
                TimeSpan add = new TimeSpan(int.Parse(txtDaysBeforeAfterEmail.Text), 0, 0, 0);
                _sendDate = DateTime.Parse(Session["endDate"].ToString()).Add(add);
                _msgType = 3;
            }
            else
            {
                SetToast("error","Select a message type!");
                return;
            }

            bool _isUser;

            if (recipients.SelectedValue == "True")
            {
                _isUser = true;
            }
            else if (recipients.SelectedValue == "False")
            {
                _isUser = false;
            }
            else
            {
                SetToast("error","Select a recipient!");
                return;
            }


            var emDS = new EmailDataSet();
            emDS.Email.AddEmailRow(int.Parse(emailId.Value), eventId, _sendDate, txtSubject.Text, txtBody.Text, _isUser,
                false, _msgType, eventNameParam);
            
           
            if (Utilities.editId == 3 || Utilities.editId == 4)
            {
                int result = dataProvider.UpdateEventAlertEmail(userToken, emDS);
                
                if (result != -1)
                {
                    SetToast("success", "Alert Email updated.");

                }
                else
                {
                    SetToast("errror", "An error occured, this type of email exists already.");
                }
            }
            else
            {
                int result = dataProvider.AddEventAlertEmail(userToken,emDS);
                
                if (result != -1)
                {
                    SetToast("success", "Alert Email added.");

                }
                else
                {
                    SetToast("errror", "An error occured, this type of email exists already.");
                }
            }

            initFields();
        }

        protected void SetToast(string type, string errorMessage)
        {
            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>toastr." + type + "('" + errorMessage + "', '');</script>";  //if not null inject js to show modal
            }
        }

        protected void recipients_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            initFields();
        }

        protected void btnSaveAlertSMS_OnClick(object sender, EventArgs e)
        {
            if (Session["placeholderLength"] == null)
                Session["placeholderLength"] = "0";

            if ((txtSMS.Text.Length - int.Parse(Session["placeholderLength"].ToString())) > 160)
            {
                smsOverflow.Visible = true;
            return;
            }
            DateTime _sendDate;
            int _msgType;

            if (txtDaysBeforeAfterSMS.Text == "")
                txtDaysBeforeAfterSMS.Text = "0";

            if (emailId.Value == "")
                emailId.Value = "0";

            if (smsId.Value == "")
                smsId.Value = "0";

            if (messagetypes.SelectedValue == "1")
            {

                TimeSpan sub = new TimeSpan(int.Parse(txtDaysBeforeAfterSMS.Text), 0, 0, 0);
                _sendDate = DateTime.Parse(Session["beginDate"].ToString()).Subtract(sub);
                _msgType = 1;
            }
            else if (messagetypes.SelectedValue == "2")
            {
                _sendDate = DateTime.Parse(Session["beginDate"].ToString());
                _msgType = 2;
            }
            else if (messagetypes.SelectedValue == "3")
            {
                TimeSpan add = new TimeSpan(int.Parse(txtDaysBeforeAfterSMS.Text), 0, 0, 0);
                _sendDate = DateTime.Parse(Session["endDate"].ToString()).Add(add);
                _msgType = 3;
            }
            else
            {
                SetToast("error", "Select a message type!");
                return;
            }

            bool _isUser;

            if (recipients.SelectedValue == "True")
            {
                _isUser = true;
            }
            else if (recipients.SelectedValue == "False")
            {
                _isUser = false;
            }
            else
            {
                SetToast("error", "Select a recipient!");
                return;
            }

            var orgDs = new OrganisationDataSet();
            orgDs = dataProvider.GetOwnerOrganisationByID(userToken, Utilities.companyId);
            var smsDS = new SMSDataSet();
            smsDS.SMS.AddSMSRow(int.Parse(smsId.Value), eventId, txtSMS.Text, orgDs.Organisation[0].SMSName, _sendDate, _isUser, false, _msgType, eventNameParam);


            if (Utilities.editId == 2 || Utilities.editId == 4)
            {
                int result = dataProvider.UpdateEventAlertSMS(userToken, smsDS);

                if (result != -1)
                {
                    SetToast("success", "Event Alert SMS updated");
                }
                else
                {
                    SetToast("error", "Event Alert SMS type exists already.");
                }
            }
            else
            {
                int result = dataProvider.AddEventAlertSMS(userToken, smsDS);

                if (result != -1)
                {
                    SetToast("success", "Event Alert SMS added");
                }
                else
                {
                    SetToast("error", "Event Alert SMS type exists already.");
                }
                
            }
            smsOverflow.Visible = false;
            initFields();
        }

        protected void messagetypes_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (messagetypes.SelectedValue == "2")
            {
                txtDaysBeforeAfterEmail.Enabled = false;
                txtDaysBeforeAfterEmail.Text = string.Empty;
                txtDaysBeforeAfterSMS.Enabled = false;
                txtDaysBeforeAfterSMS.Text = string.Empty;
            }
            else
            {
                txtDaysBeforeAfterEmail.Enabled = true;
                txtDaysBeforeAfterSMS.Enabled = true;
            }
            initFields();
        }

        private void removeInjectedScript()
        {
            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper");
            if (jsHelper != null)
            {
                jsHelper.Text = "";
            }
        }

        protected void btnSMSPreview_OnClick(object sender, EventArgs e)
        {
            TimeSpan add = new TimeSpan(0,0,0,1);
            TimeSpan timeBegin = TimeSpan.Parse(Session["timeBeginCheck"].ToString()).Add(add);
            string message = txtSMS.Text;

            message = message.Replace("[Recipient Name]", "John");
            message = message.Replace("[Event Name]", Session["eventName"].ToString());
            message = message.Replace("[Recipient Name]", "John");
            message = message.Replace("[Location]", Session["location"].ToString());
            message = message.Replace("[Begin Time]", DateTime.Parse(timeBegin.ToString()).ToString("h:mm tt"));
            message = message.Replace("[Begin Time]", DateTime.Parse(Session["timeEndCheck"].ToString()).ToString("h:mm tt"));
            message = message.Replace("[Begin Date]", DateTime.Parse(Session["beginDate"].ToString()).ToLongDateString());
            message = message.Replace("[Site URL]", "www.igniteme.com");
            message = message.Replace("[End Date]", DateTime.Parse(Session["endDate"].ToString()).ToLongDateString());
            
            msgPreview.Text = message;

            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>$('#myModal').modal('show')</script>"; //if not null inject js to show modal
            }
        }

        protected void btnEmailPreview_OnClick(object sender, EventArgs e)
        {
            TimeSpan add = new TimeSpan(0, 0, 0, 1);
            TimeSpan timeBegin = TimeSpan.Parse(Session["timeBeginCheck"].ToString()).Add(add);
            string message = txtBody.Text;
            string subject = txtSubject.Text;
            subjHeading.Visible = true;
            bodyHeading.Visible = true;


            message = message.Replace("[Recipient Name]", "John");
            message = message.Replace("[Event Name]", Session["eventName"].ToString());
            message = message.Replace("[Recipient Name]", "John");
            message = message.Replace("[Location]", Session["location"].ToString());
            message = message.Replace("[Begin Time]", DateTime.Parse(timeBegin.ToString()).ToString("h:mm tt"));
            message = message.Replace("[Begin Time]", DateTime.Parse(Session["timeEndCheck"].ToString()).ToString("h:mm tt"));
            message = message.Replace("[Begin Date]", DateTime.Parse(Session["beginDate"].ToString()).ToLongDateString());
            message = message.Replace("[Site URL]", "www.igniteme.com");
            message = message.Replace("[End Date]", DateTime.Parse(Session["endDate"].ToString()).ToLongDateString());

            subject = subject.Replace("[Recipient Name]", "John");
            subject = subject.Replace("[Event Name]", Session["eventName"].ToString());
            subject = subject.Replace("[Recipient Name]", "John");
            subject = subject.Replace("[Location]", Session["location"].ToString());
            subject = subject.Replace("[Begin Time]", DateTime.Parse(timeBegin.ToString()).ToString("h:mm tt"));
            subject = subject.Replace("[Begin Time]", DateTime.Parse(Session["timeEndCheck"].ToString()).ToString("h:mm tt"));
            subject = subject.Replace("[Begin Date]", DateTime.Parse(Session["beginDate"].ToString()).ToLongDateString());
            subject = subject.Replace("[Site URL]", "www.igniteme.com");
            subject = subject.Replace("[End Date]", DateTime.Parse(Session["endDate"].ToString()).ToLongDateString());

            msgPreview.Text = message;
            msgSubject.Text = subject;

            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>$('#myModal').modal('show')</script>"; //if not null inject js to show modal
            }
        }
    }
}