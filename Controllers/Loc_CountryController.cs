using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AddressBook2.Models;
using AddressBook2.DAL;

namespace AddressBook2.Controllers
{
    public class Loc_CountryController : Controller
    {
        private IConfiguration Configuration;
        public Loc_CountryController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        public IActionResult Index()
        {
            string str = this.Configuration.GetConnectionString("MyConnectionString");
            LOC_DALBase dalloc = new LOC_DALBase();
            DataTable dt = dalloc.PR_Loc_Contry_SelectAll(str);
            return View("Loc_CountryList", dt);
            /*String str = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection Conn = new SqlConnection(str);
            Conn.Open();
            SqlCommand cmd = Conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Loc_Contry_SelectAll";
            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            Conn.Close();

            return View("Loc_CountryList", dt);*/
        }
        public IActionResult Delete(int CountryID)
        {
            string str = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection objConnection = new SqlConnection(str);
            objConnection.Open();

            //SqlCommand objCommand = new SqlCommand();
            SqlCommand objCommand = objConnection.CreateCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "PR_Loc_Contry_DeleteByPK";
            objCommand.Parameters.AddWithValue("@CountryID", CountryID);
            objCommand.ExecuteNonQuery();


            return RedirectToAction("Index");
        }
        public IActionResult Add(int? CountryID)
        {
            if (CountryID != null)
            {
                String str = this.Configuration.GetConnectionString("MyConnectionString");
                SqlConnection Conn = new SqlConnection(str);
                Conn.Open();
                SqlCommand cmd = Conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Loc_Contry_SelectByPK";
                cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = CountryID;
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                Loc_CountryModel modelLoc_Country = new Loc_CountryModel();
                foreach (DataRow dr in dt.Rows)
                {

                    modelLoc_Country.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelLoc_Country.CountryName = dr["CountryName"].ToString();
                    modelLoc_Country.CountryCode = dr["CountryCode"].ToString();
                    modelLoc_Country.CreateDate = Convert.ToDateTime(dr["CreateDate"]);
                    modelLoc_Country.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                }
                return View("Loc_CountryAddEdit", modelLoc_Country);



            }
            return View("Loc_CountryAddEdit");
        }
        [HttpPost]
        public IActionResult Save(Loc_CountryModel modelLoc_Country)
        {
            String str = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection Conn = new SqlConnection(str);
            Conn.Open();
            SqlCommand cmd = Conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (modelLoc_Country.CountryID == null)
            {
                cmd.CommandText = "PR_Loc_Contry_Insert";

            }
            else
            {
                cmd.CommandText = "PR_Loc_Contry_UpdateByPK";
                cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelLoc_Country.CountryID;

            }
            cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar).Value = modelLoc_Country.CountryName;
            cmd.Parameters.Add("@CountryCode", SqlDbType.NVarChar).Value = modelLoc_Country.CountryCode;
            cmd.Parameters.Add("@CreateDate", SqlDbType.Date).Value = modelLoc_Country.CreateDate;
            cmd.Parameters.Add("@ModifiedDate", SqlDbType.Date).Value = modelLoc_Country.ModifiedDate;

            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelLoc_Country.CountryID == null)
                    TempData["CountryInsertMsg"] = "Record Insertrd Succeesfully !";
                else
                    TempData["CountryInsertMsg"] = "Record Updated Succeesfully !";

            }
            Conn.Close();
            return View("Loc_CountryAddEdit");
        }

    }
}
