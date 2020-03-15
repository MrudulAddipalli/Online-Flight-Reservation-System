using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Web.Mvc;
using Online_Flight_Reservation_System.Models;
namespace Online_Flight_Reservation_System.Controllers
{
    public class RegisterController : Controller
    {
        
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Register register)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            conn.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Insert into Registration(Password,FirstName,LastName,DOB,Age,Gender,Address,PhoneNo,UserType,LoginStatus) values (@Password,@FirstName,@LastName,@DOB,@Age,@Gender,@Address,@PhoneNo,'U',0) SELECT SCOPE_IDENTITY()";
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue("@Password", register.Password);
            cmd.Parameters.AddWithValue("@FirstName", register.FirstName);
            cmd.Parameters.AddWithValue("@LastName", register.LastName);
            cmd.Parameters.AddWithValue("@DOB", register.DOB);
            cmd.Parameters.AddWithValue("@Age", register.Age);
            cmd.Parameters.AddWithValue("@Gender", register.Gender);
            cmd.Parameters.AddWithValue("@Address", register.Address);
            cmd.Parameters.AddWithValue("@PhoneNo", register.PhoneNo);

            int UserID = Convert.ToInt32(cmd.ExecuteScalar());

            if (UserID != 0)
            {
                conn.Close();
                Session["UserID"] = UserID+"";
                return RedirectToAction("UserListFlightDetails", "FlightUser");
            }

            return View();
        }
    }
}