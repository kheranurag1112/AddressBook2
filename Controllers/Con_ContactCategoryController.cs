using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AddressBook2.Models;

namespace AddressBook2.Controllers
{
    public class Con_ContactCategoryController : Controller
    {
        private IConfiguration Configuration;
        public Con_ContactCategoryController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        public IActionResult Index()
        {
            String str = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection Conn = new SqlConnection(str);
            Conn.Open();
            SqlCommand cmd = Conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Con_ContactCategory_SelectAll";
            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            Conn.Close();

            return View("Con_ContactCategoryList", dt);
        }
        public IActionResult Delete(int ContactCategoryID)
        {
            string str = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection objConnection = new SqlConnection(str);
            objConnection.Open();

            //SqlCommand objCommand = new SqlCommand();
            SqlCommand objCommand = objConnection.CreateCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "PR_Con_ContactCategory_Delete";
            objCommand.Parameters.AddWithValue("@ContactCategoryID", ContactCategoryID);
            objCommand.ExecuteNonQuery();


            return RedirectToAction("Index");
        }

        public IActionResult Add(int? ContactCategoryID)
        {
            


            if (ContactCategoryID != null)
            {
                String str = this.Configuration.GetConnectionString("MyConnectionString");
                SqlConnection Conn = new SqlConnection(str);
                Conn.Open();
                SqlCommand cmd = Conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Con_ContactCategory_SelectByPK";
                cmd.Parameters.Add("@ContactCategoryID", SqlDbType.Int).Value = ContactCategoryID;
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                Con_ContactCategoryModel modelCon_ContactCategory = new Con_ContactCategoryModel();
                foreach (DataRow dr in dt.Rows)
                {

                    modelCon_ContactCategory.ContactCategoryID = Convert.ToInt32(dr["ContactCategoryID"]);
                    modelCon_ContactCategory.ContactCategoryName = dr["ContactCategoryName"].ToString();
                    modelCon_ContactCategory.CreatDate = Convert.ToDateTime(dr["CreatDate"]);
                    modelCon_ContactCategory.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                }
                return View("Con_ContactCategoryAddEdit", modelCon_ContactCategory);



            }
            return View("Con_ContactCategoryAddEdit");
        }
        [HttpPost]
        public IActionResult Save(Con_ContactCategoryModel modelCon_ContactCategory)
        {
            String str = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection Conn = new SqlConnection(str);
            Conn.Open();
            SqlCommand cmd = Conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (modelCon_ContactCategory.ContactCategoryID == null)
            {
                cmd.CommandText = "PR_Con_ContactCategory_Insert";

            }
            else
            {
                cmd.CommandText = "PR_Con_ContactCategory_Update";
                cmd.Parameters.Add("@ContactCategoryID", SqlDbType.Int).Value = modelCon_ContactCategory.ContactCategoryID;

            }
            cmd.Parameters.Add("@ContactCategoryName", SqlDbType.NVarChar).Value = modelCon_ContactCategory.ContactCategoryName;
            cmd.Parameters.Add("@CreatDate", SqlDbType.Date).Value = modelCon_ContactCategory.CreatDate;
            cmd.Parameters.Add("@ModifiedDate", SqlDbType.Date).Value = modelCon_ContactCategory.ModifiedDate;

            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelCon_ContactCategory.ContactCategoryID == null)
                    TempData["ContactCategoryInsertMsg"] = "Record Insertrd Succeesfully !";
                else
                    TempData["ContactCategoryInsertMsg"] = "Record Updated Succeesfully !";

            }
            Conn.Close();
            return RedirectToAction("Add");
        }

    }
}
