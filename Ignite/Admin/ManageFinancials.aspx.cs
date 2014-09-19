using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Ignite.DAL;

namespace Ignite.Admin
{
    public partial class ManageFinancials : System.Web.UI.Page
    {
        private DataProvider dataProvider = DataProvider.GetInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageFinancials";
         
            if (!IsPostBack)
            {
                
                //DisplayUsersInGrid(); //Populate datagridview
            }
            removeInjectedScript();
        }

        private void removeInjectedScript()
        {
            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper");
            if (jsHelper != null)
            {
                jsHelper.Text = "";
            }
        } 

       /* private void DisplayUsersInGrid()
        {
            var wishes = dataProvider.getAllWishes(); //connect to db, get all wishes
            dgvAdminUsers.DataSource = wishes.wishes; //bind result to datagridview
            dgvAdminUsers.DataBind();
        }*/

       
        protected void dgvAdminUsers_OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            e.Cancel = true;
            result.Text = dgvAdminUsers.Rows[e.NewEditIndex].Cells[1].Text; //get text from selected row
            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>$('#myModal').modal('show')</script>";  //if not null inject js to show modal
            }
        }

        protected void btnsavefacility_Click(object sender, EventArgs e)
        {
            
        }
    }
}