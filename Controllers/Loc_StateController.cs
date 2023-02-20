using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AddressBook2.Models;

namespace AddressBook2.Controllers
{
    public class Loc_StateController : Controller
    {
        private IConfiguration Configuration;
        public Loc_StateController(IConfiguration _configuration)
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
            cmd.CommandText = "PR_Loc_State_SelectAll";
            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
          

            return View("Loc_StateList", dt);
        }
        public IActionResult Delete(int StateID)
        {
            string str = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection objConnection = new SqlConnection(str);
            objConnection.Open();

            //SqlCommand objCommand = new SqlCommand();
            SqlCommand objCommand = objConnection.CreateCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "PR_Loc_State_DeleteByPK";
            objCommand.Parameters.AddWithValue("@StateID", StateID);
            objCommand.ExecuteNonQuery();


            return RedirectToAction("Index");
        }
        public IActionResult Add(int? StateID)
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


            if (StateID != null)
            {
                String str = this.Configuration.GetConnectionString("MyConnectionString");
                SqlConnection Conn = new SqlConnection(str);
                Conn.Open();
                SqlCommand cmd = Conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Loc_State_SelectByPK";
                cmd.Parameters.Add("@StateID", SqlDbType.Int).Value = StateID;
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                Loc_StateModel modelLoc_State = new Loc_StateModel();

                foreach (DataRow dr in dt.Rows)
                {

                    modelLoc_State.StateID = Convert.ToInt32(dr["StateID"]);
                    modelLoc_State.StateName = dr["StateName"].ToString();
                    modelLoc_State.StateCode = dr["StateCode"].ToString();
                    modelLoc_State.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelLoc_State.CreateDate = Convert.ToDateTime(dr["CreateDate"]);
                    modelLoc_State.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                }
                return View("Loc_StateAddEdit", modelLoc_State);
            }

            
            return View("Loc_StateAddEdit");
        }
        [HttpPost]
        public IActionResult Save(Loc_StateModel modelLoc_State)
        {
            String str = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection Conn = new SqlConnection(str);
            Conn.Open();
            SqlCommand cmd = Conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (modelLoc_State.StateID == null)
            {
                cmd.CommandText = "PR_Loc_State_Insert";
            }
            else
            {
                cmd.CommandText = "PR_Loc_State_UpdateByPK";
                cmd.Parameters.Add("@StateID", SqlDbType.Int).Value = modelLoc_State.StateID;
            }
            
            cmd.Parameters.Add("@StateName", SqlDbType.NVarChar).Value = modelLoc_State.StateName;
            cmd.Parameters.Add("@StateCode", SqlDbType.NVarChar).Value = modelLoc_State.StateCode;
            cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelLoc_State.CountryID;
            cmd.Parameters.Add("@CreateDate", SqlDbType.Date).Value = modelLoc_State.CreateDate;
            cmd.Parameters.Add("@ModifiedDate", SqlDbType.Date).Value = modelLoc_State.ModifiedDate;

            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelLoc_State.StateID == null)
                    TempData["StateInsertMsg"] = "Record Insertrd Succeesfully !";
                else
                    TempData["StateInsertMsg"] = "Record Updated Succeesfully !";

            }
            Conn.Close();
            return RedirectToAction("Add");
        }
    }
}
