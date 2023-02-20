using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AddressBook2.Models;

namespace AddreshBook.Controllers
{
    public class Loc_CityController : Controller
    {
        private IConfiguration Configuration;
        public Loc_CityController(IConfiguration _configuration)
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
            cmd.CommandText = "PR_Loc_City_SelectAll";
            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);


            return View("Loc_CityList", dt);
        }
        public IActionResult Delete(int CityID)
        {
            string str = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection objConnection = new SqlConnection(str);
            objConnection.Open();

            //SqlCommand objCommand = new SqlCommand();
            SqlCommand objCommand = objConnection.CreateCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "PR_LOC_City_DeleteByPK";
            objCommand.Parameters.AddWithValue("@CityID", CityID);
            objCommand.ExecuteNonQuery();


            return RedirectToAction("Index");
        }



        public IActionResult Add(int? CityID)
        {


            String str1 = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection Conn1 = new SqlConnection(str1);
            Conn1.Open();
            SqlCommand cmd1 = Conn1.CreateCommand();
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.CommandText = "PR_Loc_Contry_SelectForDropDown";
            DataTable dt1 = new DataTable();
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            dt1.Load(sdr1);
            Conn1.Close();

            List<Loc_CountryDropDownModel> list = new List<Loc_CountryDropDownModel>();
            foreach (DataRow dr in dt1.Rows)
            {
                Loc_CountryDropDownModel vlst = new Loc_CountryDropDownModel();
                vlst.CountryID = Convert.ToInt32(dr["CountryID"]);
                vlst.CountryName = dr["CountryName"].ToString();
                list.Add(vlst);
            }
            ViewBag.CountryList = list;

            /*String str2 = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection Conn2 = new SqlConnection(str2);
            Conn2.Open();
            SqlCommand cmd2 = Conn2.CreateCommand();
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.CommandText = "PR_Loc_State_SelectForDropDown";
            DataTable dt2 = new DataTable();
            SqlDataReader sdr2 = cmd2.ExecuteReader();
            dt2.Load(sdr2);
            Conn2.Close();*/

            List<Loc_StateDropDownModel> list1 = new List<Loc_StateDropDownModel>();
            /*foreach (DataRow dr in dt2.Rows)
            {
                Loc_StateDropDownModel vlst = new Loc_StateDropDownModel();
                vlst.StateID = Convert.ToInt32(dr["StateID"]);
                vlst.StateName = dr["StateName"].ToString();
                list1.Add(vlst);
            }*/
            ViewBag.StateList = list1;



            if (CityID != null)
            {
                String str = this.Configuration.GetConnectionString("MyConnectionString");
                SqlConnection Conn = new SqlConnection(str);
                Conn.Open();
                SqlCommand cmd = Conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Loc_City_SelectByPK";
                cmd.Parameters.Add("@CityID", SqlDbType.Int).Value = CityID;
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                Loc_CityModel modelLoc_City = new Loc_CityModel();
                foreach (DataRow dr in dt.Rows)
                {

                    modelLoc_City.CityID = Convert.ToInt32(dr["CityID"]);
                    modelLoc_City.CityName = dr["CityName"].ToString();
                    modelLoc_City.CityCode = dr["CityCode"].ToString();
                    modelLoc_City.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelLoc_City.StateID = Convert.ToInt32(dr["StateID"]);
                    modelLoc_City.CreateDate = Convert.ToDateTime(dr["CreateDate"]);
                    modelLoc_City.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                }
                return View("Loc_CityAddEdit", modelLoc_City);



            }
            return View("Loc_CityAddEdit");
        }
        [HttpPost]
        public IActionResult Save(Loc_CityModel modelLoc_City)
        {
            String str = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection Conn = new SqlConnection(str);
            Conn.Open();
            SqlCommand cmd = Conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (modelLoc_City.CityID == null)
            {
                cmd.CommandText = "PR_Loc_City_Insert";

            }
            else
            {
                cmd.CommandText = "PR_Loc_City_UpdateByPK";
                cmd.Parameters.Add("@CityID", SqlDbType.Int).Value = modelLoc_City.CityID;

            }
            cmd.Parameters.Add("@CityName", SqlDbType.NVarChar).Value = modelLoc_City.CityName;
            cmd.Parameters.Add("@CityCode", SqlDbType.NVarChar).Value = modelLoc_City.CityCode;
            cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelLoc_City.CountryID;
            cmd.Parameters.Add("@StateID", SqlDbType.Int).Value = modelLoc_City.StateID;
            cmd.Parameters.Add("@CreateDate", SqlDbType.Date).Value = modelLoc_City.CreateDate;
            cmd.Parameters.Add("@ModifiedDate", SqlDbType.Date).Value = modelLoc_City.ModifiedDate;

            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelLoc_City.CityID == null)
                    TempData["CityInsertMsg"] = "Record Insertrd Succeesfully !";
                else
                    TempData["CityInsertMsg"] = "Record Updated Succeesfully !";

            }
            Conn.Close();
            return RedirectToAction("Add");
        }

        public IActionResult DropDownByState(int CountryID)
        {
            String str2 = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection Conn2 = new SqlConnection(str2);
            Conn2.Open();
            SqlCommand cmd2 = Conn2.CreateCommand();
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.CommandText = "PR_Loc_State_SelectForDropDown";
            cmd2.Parameters.AddWithValue("@CountryID",CountryID);
            DataTable dt2 = new DataTable();
            SqlDataReader sdr2 = cmd2.ExecuteReader();
            dt2.Load(sdr2);
            Conn2.Close();

            List<Loc_StateDropDownModel> list1 = new List<Loc_StateDropDownModel>();
            foreach (DataRow dr in dt2.Rows)
            {
                Loc_StateDropDownModel vlst = new Loc_StateDropDownModel();
                vlst.StateID = Convert.ToInt32(dr["StateID"]);
                vlst.StateName = dr["StateName"].ToString();
                list1.Add(vlst);
            }

            var VModel = list1;
            return Json(VModel);
        }

    }
}

