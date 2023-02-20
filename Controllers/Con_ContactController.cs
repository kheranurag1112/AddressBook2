using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AddressBook2.Models;

namespace AddressBook2.Controllers
{
    public class Con_ContactController : Controller
    {
        private IConfiguration Configuration;
        public Con_ContactController (IConfiguration _configuration)
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
            cmd.CommandText = "PR_Con_Contact_SelectAll";
            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);


            return View("Con_ContactList", dt);
        }
        public IActionResult Delete(int ContactID)
        {
            string str = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection objConnection = new SqlConnection(str);
            objConnection.Open();

            //SqlCommand objCommand = new SqlCommand();
            SqlCommand objCommand = objConnection.CreateCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "PR_Con_Contact_DeleteByPK";
            objCommand.Parameters.AddWithValue("@ContactID", ContactID);
            objCommand.ExecuteNonQuery();


            return RedirectToAction("Index");
        }
        

        public IActionResult Add(int? ContactID)
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
            cmd2.CommandText = "PR_Loc_State_SelectForDropDown2";
            DataTable dt2 = new DataTable();
            SqlDataReader sdr2 = cmd2.ExecuteReader();
            dt2.Load(sdr2);
            Conn2.Close();*/

            List<Loc_StateDropDownModel> list1 = new List<Loc_StateDropDownModel>();
            /*foreach (DataRow dr in dt2.Rows)
            {
                Loc_StateDropDownModel vlst = new Loc_StateDropDownModel();
                vlst.StateID = Convert.ToInt32(dr["StateID"]);
                vlst.StateName = Convert.ToString(dr["StateName"]);
                list1.Add(vlst);
            }*/
            ViewBag.StateList = list1;



            /*String str3 = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection Conn3 = new SqlConnection(str3);
            Conn3.Open();
            SqlCommand cmd3 = Conn3.CreateCommand();
            cmd3.CommandType = CommandType.StoredProcedure;
            cmd3.CommandText = "PR_Loc_City_SelectForDropDown";
            DataTable dt3 = new DataTable();
            SqlDataReader sdr3 = cmd3.ExecuteReader();
            dt3.Load(sdr3);
            Conn3.Close();
*/
            List< Loc_CityDropDownModel> list2 = new List< Loc_CityDropDownModel>();
            /*foreach (DataRow dr in dt3.Rows)
            {
                 Loc_CityDropDownModel vlst = new Loc_CityDropDownModel();
                vlst.CityID = Convert.ToInt32(dr["CityID"]);
                vlst.CityName = dr["CityName"].ToString();
                list2.Add(vlst);
            }*/
            ViewBag.CityList = list2;





            if (ContactID != null)
            {
                String str = this.Configuration.GetConnectionString("MyConnectionString");
                SqlConnection Conn = new SqlConnection(str);
                Conn.Open();
                SqlCommand cmd = Conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Con_Contact_SelectByPK";
                cmd.Parameters.Add("@ContactID", SqlDbType.Int).Value = ContactID;
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                Con_ContactModel modelCon_Contact = new Con_ContactModel();
                foreach (DataRow dr in dt.Rows)
                {

                    modelCon_Contact.ContactID = Convert.ToInt32(dr["ContactID"]);
                    modelCon_Contact.ContactName = dr["ContactName"].ToString();
                    modelCon_Contact.ContactNumber = dr["ContactNumber"].ToString();
                    modelCon_Contact.ContactEmail = dr["ContactEmail"].ToString();
                    modelCon_Contact.ContactLinkdin = dr["ContactLinkdin"].ToString();
                    modelCon_Contact.ContactTweeter = dr["ContactTweeter"].ToString();
                    modelCon_Contact.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelCon_Contact.StateID = Convert.ToInt32(dr["StateID"]);
                    modelCon_Contact.CityID = Convert.ToInt32(dr["CityID"]);
                    modelCon_Contact.CreateDate = Convert.ToDateTime(dr["CreateDate"]);
                    modelCon_Contact.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                }
                return View("Con_ContactAddEdit", modelCon_Contact);



            }
            return View("Con_ContactAddEdit");
        }
        [HttpPost]
        public IActionResult Save(Con_ContactModel modelCon_Contact)
        {
            String str = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection Conn = new SqlConnection(str);
            Conn.Open();
            SqlCommand cmd = Conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (modelCon_Contact.ContactID == null)
            {
                cmd.CommandText = "PR_Con_Contact_Insert";
                cmd.Parameters.Add("@CreateDate", SqlDbType.Date).Value = modelCon_Contact.CreateDate;

            }
            else
            {
                cmd.CommandText = "PR_Con_Contact_UpdateByPK";
                cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelCon_Contact.ContactID;

            }
            cmd.Parameters.Add("@ContactName", SqlDbType.NVarChar).Value = modelCon_Contact.ContactName;
            cmd.Parameters.Add("@ContactNumber", SqlDbType.NVarChar).Value = modelCon_Contact.ContactNumber;
            cmd.Parameters.Add("@ContactEmail", SqlDbType.NVarChar).Value = modelCon_Contact.ContactEmail;
            cmd.Parameters.Add("@ContactLinkdin", SqlDbType.NVarChar).Value = modelCon_Contact.ContactLinkdin;
            cmd.Parameters.Add("@ContactTweeter", SqlDbType.NVarChar).Value = modelCon_Contact.ContactTweeter;
            cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelCon_Contact.CountryID;
            cmd.Parameters.Add("@StateID", SqlDbType.Int).Value = modelCon_Contact.StateID;
            cmd.Parameters.Add("@CityID", SqlDbType.Int).Value = modelCon_Contact.CityID;
           
            cmd.Parameters.Add("@ModifiedDate", SqlDbType.Date).Value = modelCon_Contact.ModifiedDate;

            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelCon_Contact.ContactID == null)
                    TempData["ContactInsertMsg"] = "Record Insertrd Succeesfully !";
                else
                    TempData["ContactInsertMsg"] = "Record Updated Succeesfully !";

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
            cmd2.CommandText = "PR_Loc_State_SelectForDropDown2";
            DataTable dt2 = new DataTable();
            SqlDataReader sdr2 = cmd2.ExecuteReader();
            dt2.Load(sdr2);
            Conn2.Close();
            List<Loc_StateDropDownModel> list1 = new List<Loc_StateDropDownModel>();
            foreach (DataRow dr in dt2.Rows)
            {
                Loc_StateDropDownModel vlst = new Loc_StateDropDownModel();
                vlst.StateID = Convert.ToInt32(dr["StateID"]);
                vlst.StateName = Convert.ToString(dr["StateName"]);
                list1.Add(vlst);
            }

            var VModel = list1;
            return Json(VModel);

        }

        public IActionResult DropDownByCity(int StateID)
        {
            String str3 = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection Conn3 = new SqlConnection(str3);
            Conn3.Open();
            SqlCommand cmd3 = Conn3.CreateCommand();
            cmd3.CommandType = CommandType.StoredProcedure;
            cmd3.CommandText = "PR_Loc_City_SelectForDropDown";
            DataTable dt3 = new DataTable();
            SqlDataReader sdr3 = cmd3.ExecuteReader();
            dt3.Load(sdr3);
            Conn3.Close();
            List<Loc_CityDropDownModel> list2 = new List<Loc_CityDropDownModel>();
            foreach (DataRow dr in dt3.Rows)
            {
                 Loc_CityDropDownModel vlst = new Loc_CityDropDownModel();
                vlst.CityID = Convert.ToInt32(dr["CityID"]);
                vlst.CityName = dr["CityName"].ToString();
                list2.Add(vlst);
            }

            var VModel = list2;
            return Json(VModel);

        }

    }
    }

