using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ignite.DAL;



namespace Ignite.Admin
{
    public partial class OrganisationCategories : System.Web.UI.Page
    {
        private DataProvider dataProvider = DataProvider.GetInstance();
        private string userToken = Utilities.userToken;
        private int userId = Utilities.userId;
        private int orgId = Utilities.companyId;
//        private int userGroupId_edit;
//        private string userGroup_edit = Utilities.userGroup_edit;
        Dictionary<int, string> categories = new Dictionary<int, string>();
        List<string> categoriesList = new List<string>();
        OrganisationDataSet catDS;
       

        protected void Page_Load(object sender, EventArgs e)
        {
            Utilities.pageName = "btnManageOrganisations";

            removeInjectedScript();
            if (Session["orgParams"] == null)
            {
                Response.Redirect("/LandingPage.aspx");
            }
            else
            {
                catDS = (OrganisationDataSet)Session["orgParams"];
            }
            
            if (!IsPostBack)
            {
                LoadCategoriesCB();
            }
            DisplayCategoriesInGrid(true);
           
        }

        private void LoadCategoriesCB()
        {
            var source = dataProvider.GetActiveCategoriesByType(userToken,bool.Parse(Session["catType"].ToString()));
            cbCategories.DataSource = source;
            cbCategories.DataBind();
            cbCategories.DataTextField = "Name";
            cbCategories.DataValueField = "Id";
            cbCategories.DataBind();
        }

        private void DisplayCategoriesInGrid(bool init)
        {
            if(init)
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

            categories = dataProvider.GetCategoriesByOrgId(userToken, catDS.Organisation[0].Id);
            //Dictionary<int,string> categories = new Dictionary<int, string>();

           
        }

        protected void btnAdd_OnClick(object sender, EventArgs e)
        {
           Session["orgParams"] = Session["orgParams"];
            //Utilities.pageName = "btnManageOrganisations";

            if (!Utilities.CheckGridForText(dgvCategories, cbCategories.SelectedItem.ToString()))
            {
                categories.Add(int.Parse(cbCategories.SelectedValue), cbCategories.SelectedItem.ToString());

                foreach (int category in categories.Keys)
                {
                    categoriesList.Add(category.ToString());
                }

                int result = dataProvider.SetCategoriesForOrganisation(catDS.Organisation[0].Id, categoriesList);

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

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            //CategoryDataSet catDS = new CategoryDataSet();

            foreach (int category in categories.Keys)
            {
                categoriesList.Add(category.ToString());
            }

            //catDS.Category.AddCategoryRow(_id, _name, _desc, _active);

//            if (Utilities.editId != -1)
//            {
                int result = dataProvider.UpdateOwnerOrganiasations(userToken, catDS, categoriesList);

                if (result != -1)
                {
                    Utilities.editId = -1;
                    Session.RemoveAll();
                    Session["success"] = "Organisation updated succesfully.";
                    Utilities.pageName = "btnManageOrganisations";
                    Utilities.editId = -1;
                    Response.Redirect("/Admin/ManageOrganisations.aspx");
                }
                else
                {
                    Utilities.editId = -1;
                    Session.RemoveAll();
                    Session["error"] = "An Organisation with that name exists already.";
                    Utilities.pageName = "btnManageOrganisations";
                    Utilities.editId = -1;
                    Response.Redirect("/Admin/ManageOrganisations.aspx");
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
        }

        protected void dgvCategories_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            List<string> categoriesList = new List<string>();

            categories.Remove(int.Parse(dgvCategories.DataKeys[e.RowIndex].Value.ToString()));

            foreach (int category in categories.Keys)
            {
                categoriesList.Add(category.ToString());
            }

            int result = dataProvider.SetCategoriesForOrganisation(catDS.Organisation[0].Id, categoriesList);

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
    }
}