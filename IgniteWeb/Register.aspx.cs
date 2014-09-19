using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using IgniteWeb.DAL;

namespace IgniteWeb
{
    public partial class Register : System.Web.UI.Page
    {
        private DataProvider dataProvider = DataProvider.GetInstance();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            var login = (HtmlControl)Master.FindControl("login");

            if (login != null)
            {
                login.Visible = true;
            }

            if (!IsPostBack)
            {
                LoadSecurityQuestionsCB();
            }

        }

        private void LoadSecurityQuestionsCB()
        {
            var questions = dataProvider.GetAllSecurityQuestions();
            cbSecurityQuestions.DataSource = questions;
            cbSecurityQuestions.DataBind();
            cbSecurityQuestions.DataTextField = "SecurityQuestionName";
            cbSecurityQuestions.DataValueField = "SecurityQuestionId";
            cbSecurityQuestions.DataBind();
        }


        protected void btnRegister_OnClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var a = dataProvider.GetCustomerByLogon(txtUsername.Text);
                if (a.Customer.Count > 0)
                {
                    usernaemUnav.Visible = true;
                    return;
                }
                
                var customer = new CustomerDataSet();
                customer.Customer.AddCustomerRow(txtFName.Text, txtLName.Text, txtEmail.Text,txtPassword.Text, txtUsername.Text, 0, "", false);

                int result = dataProvider.AddCustomer(customer,int.Parse(cbSecurityQuestions.SelectedValue),txtAnswer.Text);

                if (result != -1)
                {
                    Message.Visible = true;
                    success.Visible = true;
                    registration.Visible = false;
                }
                else
                {
                    Message.Visible = false;
                    fail.Visible = true;
                    registration.Visible = false;
               }
            }
        }

      
    }
}