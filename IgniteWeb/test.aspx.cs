using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IgniteWeb.DAL;

namespace IgniteWeb
{
    public partial class test : System.Web.UI.Page
    {
        private DataProvider dp = DataProvider.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_OnClick(object sender, EventArgs e)
        {
            lbl.Text = Utils.IsEqual(hash.Text, txtTest.Text).ToString();
        }
    }
}