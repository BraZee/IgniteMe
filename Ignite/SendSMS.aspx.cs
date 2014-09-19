using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Ignite.DAL;

namespace Ignite
{
    public partial class SendSMS : System.Web.UI.Page
    {
        private DataProvider dataProvider = DataProvider.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            Utilities.pageName = "btnPushSMS";

            if (!IsPostBack)
            {
                LoadRecipientsCB();
            }
        }

        private void LoadRecipientsCB()
        {
            Dictionary<int, string> recipientsCB = new Dictionary<int, string>();
            recipientsCB.Add(0, "All Organisations");
            recipientsCB.Add(1, "All Admin Users");
            recipientsCB.Add(2, "All Mobile Users");
            recipientsCB.Add(3, "Organisations by Category");
            recipientsCB.Add(4, "Mobile Users by Interest");

            cbRecipients.DataSource = recipientsCB;
            cbRecipients.DataBind();
            cbRecipients.DataTextField = "Value";
            cbRecipients.DataValueField = "Key";
            cbRecipients.DataBind();
        }

        protected void SetToast(string type, string errorMessage)
        {
            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>toastr." + type + "('" + errorMessage + "', '');</script>";  //if not null inject js to show modal
            }
        }

        protected void appendPlaceholder(object sender, EventArgs e)
        {
            var control = (HtmlControl)sender;

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
            if (CaretPos.Value != "")
            {
                string newText = txtMessage.Text.Insert(int.Parse(CaretPos.Value), text);
                txtMessage.Text = newText;
            }
            else
            {
                txtMessage.Text = txtMessage.Text.Insert(0, text);
            }

            if (Session["placeholderLength"] == null)
                Session["placeholderLength"] = "0";

            int temp = int.Parse(Session["placeholderLength"].ToString());
            Session["placeholderLength"] = temp + text.Length;

            txtMessage.Focus();
        }

        protected void cbRecipients_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            switch (int.Parse(cbRecipients.SelectedValue))
            {
                case 3:
                    cbFilter.Enabled = true;
                    LoadFilterCB(0);
                    break;

                case 4:
                    cbFilter.Enabled = true;
                    LoadFilterCB(1);
                    break;

                default:
                    return;

            }

        }

        private void LoadFilterCB(int p0)
        {
            if (p0 == 0)
            {
                var source = dataProvider.GetAllCategoriesByType(Utilities.userToken, true);
                cbFilter.DataSource = source;
                cbFilter.DataBind();
                cbFilter.DataTextField = "Name";
                cbFilter.DataValueField = "Id";
                cbFilter.DataBind();
            }
            else if (p0 == 1)
            {
                var source = dataProvider.GetAllInterests(Utilities.userToken);
                cbFilter.DataSource = source;
                cbFilter.DataBind();
                cbFilter.DataTextField = "Name";
                cbFilter.DataValueField = "Id";
                cbFilter.DataBind();
            }
        }

        protected void btnSend_OnClick(object sender, EventArgs e)
        {
            if (Session["placeholderLength"] == null)
                Session["placeholderLength"] = "0";

            if ((txtMessage.Text.Length - int.Parse(Session["placeholderLength"].ToString())) > 160)
            {
                smsOverflow.Text = txtMessage.Text.Length + "/160. SMS too long.";
                smsOverflow.Visible = true;
                return;
            }
            if (!(txtMessage.Text.Length > 0))
            {
                SetToast("error", "Fill out all fields!");
                return;
            }
        }
    }
}