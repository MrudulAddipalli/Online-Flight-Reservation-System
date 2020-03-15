using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Web.Mvc;
using Online_Flight_Reservation_System.Models;
namespace Online_Flight_Reservation_System.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            if (Session["UserID"] != null)
            {
                Session["UserID"] = null;
                Session.Clear();
            }
            return View();
        }


        public ActionResult SessionCheck()
        {
            if (Session["UserID"] != null)
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                conn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select UserType from Registration where UserID=@UserID";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@UserId", Session["UserID"]);


                SqlDataReader sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {

                    sdr.Read();
                    string UserType = sdr["UserType"].ToString();
                    conn.Close();

                    if (UserType.Equals("A"))
                    {
                        return RedirectToAction("ListFlightDetails", "FlightAdmin");
                    }
                    else if (UserType.Equals("U"))
                    {
                        return RedirectToAction("UserListFlightDetails", "FlightUser");
                    }
                }
            }

            return RedirectToAction("Login", "Login");
        }


        [HttpPost]
        public ActionResult Login(Login login)
        {
            //ViewBag.ErrorMessage = "User ID = " + login.UserId + " And Password = " + login.Password;

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            conn.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select UserId,UserType from Registration where UserID=@UserID And Password=@Password";
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue("@UserId", login.UserId);
            cmd.Parameters.AddWithValue("@Password", login.Password);
        
            
            SqlDataReader sdr = cmd.ExecuteReader();

            if (sdr.HasRows)
            {

                sdr.Read();
                string UserID = sdr["UserId"].ToString();
                string UserType = sdr["UserType"].ToString();


                Session["UserID"] = UserID;

                conn.Close();

                if (UserType.Equals("A"))
                {
                    return RedirectToAction("ListFlightDetails", "FlightAdmin");
                }
                else if (UserType.Equals("U"))
                {
                    return RedirectToAction("UserListFlightDetails", "FlightUser");
                }


            }
            else
            {
                ViewBag.ErrorMessage = "Invalid User ID or Password";
            }
            

            return View();
        }
    }
}