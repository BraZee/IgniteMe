using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ignite.DAL;

namespace Ignite.Organisations
{
    public partial class ManageCurrency : System.Web.UI.Page
    {
        private DataProvider dataProvider = DataProvider.GetInstance();
        private string userToken = Utilities.userToken;
        private int orgId = Utilities.companyId;


        protected void Page_Load(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageCurrencies";

            removeInjectedScript();

            if (!IsPostBack)
            {
                DisplayCurrenciesInGrid(); //Populate datagridview
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

        private void DisplayCurrenciesInGrid()
        {
            var org = dataProvider.GetCurrencyByOrgId(Utilities.userToken, orgId);
            dgvCurrency.DataSource = org.Currency; //bind result to datagridview
            dgvCurrency.DataBind();
        }


        protected void dgvCurrency_OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            Utilities.editId = int.Parse(dgvCurrency.DataKeys[e.NewEditIndex].Value.ToString());
            //UserDataSet currencyDS = dataProvider.GetCurrencyById(Utilities.userToken, Utilities.editId);
            
            e.Cancel = true;
            txtName.Text = dgvCurrency.Rows[e.NewEditIndex].Cells[1].Text;
            txtSymbol.Text = HttpUtility.HtmlDecode(dgvCurrency.Rows[e.NewEditIndex].Cells[2].Text);
            txtISO.Text = dgvCurrency.Rows[e.NewEditIndex].Cells[3].Text;
            txtUnit.Text = dgvCurrency.Rows[e.NewEditIndex].Cells[4].Text;
            txtSubUnit.Text = dgvCurrency.Rows[e.NewEditIndex].Cells[5].Text;
            txtUnitSingle.Text = dgvCurrency.Rows[e.NewEditIndex].Cells[6].Text;
            txtSubUnitSngle.Text = dgvCurrency.Rows[e.NewEditIndex].Cells[7].Text;
            chkBaseCurrency.Checked = bool.Parse(dgvCurrency.Rows[e.NewEditIndex].Cells[8].Text);
            chkActive.Checked = bool.Parse(dgvCurrency.Rows[e.NewEditIndex].Cells[9].Text);
            chkActive.Enabled = true;

            if(chkBaseCurrency.Checked)
            chkBaseCurrency.Enabled = false;
            else
            chkBaseCurrency.Enabled = true;

            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>$('#myModal').modal('show')</script>"; //if not null inject js to show modal
            }
        }

        

        protected void btnCloseModal_OnClick(object sender, EventArgs e)
        {
            txtName.Text = string.Empty;
            txtSymbol.Text = string.Empty;
            txtISO.Text = string.Empty;
            txtUnit.Text = string.Empty;
            txtSubUnit.Text = string.Empty;
            txtUnitSingle.Text = string.Empty;
            txtSubUnitSngle.Text = string.Empty;
            chkBaseCurrency.Checked = false;
            chkActive.Checked = true;
            chkActive.Enabled = false;
            Utilities.editId = -1;

        }

        protected void btnSaveUser_OnClick(object sender, EventArgs e)
        {
            var currencyDS = new CurrencyDataSet();
            string _symbol = HttpUtility.HtmlDecode(txtSymbol.Text);
            currencyDS.Currency.AddCurrencyRow(txtName.Text, _symbol, txtISO.Text, txtUnit.Text, txtSubUnit.Text
                , txtUnitSingle.Text, txtSubUnitSngle.Text, chkBaseCurrency.Checked, chkActive.Checked, Utilities.editId);

            if (Utilities.editId != -1)
            {
                int result = dataProvider.UpdateCurrency(Utilities.userToken, currencyDS, orgId);

                if (result != -1)
                {
                    SetToast("success", "Currency has been succesfully updated");
                    Utilities.editId = -1;
                    btnCloseModal_OnClick(sender, e);
                }
                else
                {
                    SetToast("error", "Currency already exists");
                }
            }
            else
            {
                if (!Utilities.CheckGridForText(dgvCurrency, txtName.Text))
                {
                    int result = dataProvider.AddCurrency(Utilities.userToken, currencyDS, orgId);

                    if (result != -1)
                    {
                        Utilities.editId = -1;
                        SetToast("success", "Currency was created successfully.");
                    }
                    else
                    {
                        SetToast("error", "An error has occured, try again.");
                    }
                }
                else
                {
                    SetToast("info", "Currency already exists");
                }

                DisplayCurrenciesInGrid();
            }

            DisplayCurrenciesInGrid();
        }

        protected void SetToast(string type, string errorMessage)
        {
            var jsHelper = (Label)Master.Master.FindControl("LblJsHelper"); //get label in master page

            if (jsHelper != null)
            {
                jsHelper.Text = "<script>toastr." + type + "('" + errorMessage + "', '');</script>";  //if not null inject js to show modal
            }
        }

        protected void dgvCurrency_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (dgvCurrency.Rows[e.RowIndex].Cells[9].Text != "False")
            {
                Utilities.editId = int.Parse(dgvCurrency.DataKeys[e.RowIndex].Value.ToString());

                string _txtName = dgvCurrency.Rows[e.RowIndex].Cells[1].Text;
                string _txtSymbol = HttpUtility.HtmlDecode(dgvCurrency.Rows[e.RowIndex].Cells[2].Text);
                string _txtISO = dgvCurrency.Rows[e.RowIndex].Cells[3].Text;
                string _txtUnit = dgvCurrency.Rows[e.RowIndex].Cells[4].Text;
                string _txtSubUnit = dgvCurrency.Rows[e.RowIndex].Cells[5].Text;
                string _txtUnitSingle = dgvCurrency.Rows[e.RowIndex].Cells[6].Text;
                string _txtSubUnitSngle = dgvCurrency.Rows[e.RowIndex].Cells[7].Text;
                bool _chkBaseCurrency = bool.Parse(dgvCurrency.Rows[e.RowIndex].Cells[8].Text);
                
                CurrencyDataSet curr = new CurrencyDataSet();
                curr.Currency.AddCurrencyRow(_txtName, _txtSymbol, _txtISO, _txtUnit, _txtSubUnit, _txtUnitSingle,
                    _txtSubUnitSngle,
                    _chkBaseCurrency, false, Utilities.editId);
                
                dataProvider.UpdateCurrency(userToken,curr,orgId);
                SetToast("success", "Currency deactivated succesfully");
            }
            else
            {
                SetToast("info", "Currency already deactivated");
            }
            DisplayCurrenciesInGrid();
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
        }
    }
}