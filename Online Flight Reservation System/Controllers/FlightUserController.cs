using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.SqlClient;

using Online_Flight_Reservation_System.Models;

namespace Online_Flight_Reservation_System.Controllers
{
    public class FlightUserController : Controller
    {
        // GET: FlightUser
        
        public ActionResult Profile()
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }


            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select * from Registration where UserID=@UserID";
            cmd.Parameters.AddWithValue("@UserID",Session["UserID"]);
            cmd.Connection = conn;

            conn.Open();

            SqlDataReader sdr = cmd.ExecuteReader();

            Register register = new Register();

            if (sdr.HasRows)
            {
                sdr.Read();
                register.FirstName = sdr["FirstName"].ToString();
                register.LastName = sdr["LastName"].ToString();
                register.DOB = sdr["DOB"].ToString();
                register.Age = sdr["Age"].ToString();
                register.Gender = sdr["Gender"].ToString();
                register.Address = sdr["Address"].ToString();
                register.PhoneNo = sdr["PhoneNo"].ToString();
            }
            conn.Close();
        
            return View(register);
        }


        [HttpPost]
        public ActionResult Profile(Register register)
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            //FirstName,LastName,DOB,Age,Gender,Address,PhoneNo

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Update Registration set FirstName=@FirstName,LastName=@LastName,Age=@Age,Gender=@Gender,Address=@Address,PhoneNo=@PhoneNo  where UserID=@UserID ";
            cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);
            cmd.Parameters.AddWithValue("@FirstName", register.FirstName);
            cmd.Parameters.AddWithValue("@LastName", register.LastName);
            cmd.Parameters.AddWithValue("@Age", register.Age);
            cmd.Parameters.AddWithValue("@Gender", register.Gender);
            cmd.Parameters.AddWithValue("@Address", register.Address);
            cmd.Parameters.AddWithValue("@PhoneNo", register.PhoneNo);
            cmd.Connection = conn;

            conn.Open();

            int rows = cmd.ExecuteNonQuery();
            
            if (rows != 0)
            {
                ViewBag.UpdateMessage = 1;
            }
            conn.Close();
            
            return View();
        }

        public ActionResult UserListFlightDetails()
        {
            List<UserBooking> list = new List<UserBooking>();

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            else
            {

                ViewBag.UserID = "" + Session["UserID"];


                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from ScheduleTable Left JOIN Flight on ScheduleTable.FlightID = Flight.FlightID where ScheduleTable.DepartureDateTime >= @DepartureDateTime";
                cmd.Parameters.AddWithValue("@DepartureDateTime", DateTime.Now.Date.ToString(@"yyyy/MM/dd") + " 00:00:00");
                cmd.Connection = conn;

                conn.Open();

                SqlDataReader sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        UserBooking userBooking = new UserBooking();

                        userBooking.FlightName = sdr["FlightName"].ToString();
                        userBooking.Source = sdr["Source"].ToString();
                        userBooking.Destination = sdr["Destination"].ToString();
                        userBooking.DepartureDateTime = sdr["DepartureDateTime"].ToString();
                        userBooking.ArrivalDateTime = sdr["ArrivalDateTime"].ToString();
                        userBooking.RouteName = sdr["RouteName"].ToString();
                        userBooking.FlightType = sdr["FlightType"].ToString();
                        userBooking.ScheduleID = sdr["ScheduleID"].ToString();

                        list.Add(userBooking);

                    }
                }
                conn.Close();
            }

            return View(list);
        }


        [HttpPost]
        public ActionResult UserListFlightDetails(string Source, string Destination, string DOJ)
        {
            List<UserBooking> list = new List<UserBooking>();

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            else
            {

                ViewBag.UserID1 = "" + Session["UserID"];


                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";

                SqlCommand cmd = new SqlCommand();

                DateTime dates;
                string date = "";

                if(DOJ!="")
                {
                    if (Convert.ToDateTime(DOJ).Date <= DateTime.Now.Date)
                    {
                        //dates = DateTime.Now;
                        date += DateTime.Now.Date.ToString(@"yyyy/MM/dd") + " 00:00:00";
                    }
                    else
                    {
                        date += Convert.ToDateTime(DOJ).Date.ToString(@"yyyy/MM/dd") + " 00:00:00";
                    }
                }
                else
                {
                    date += DateTime.Now.Date.ToString(@"yyyy/MM/dd") + " 00:00:00";
                }
                //try
                //{
                //    //dates = Convert.ToDateTime(DOJ).Date.ToString(@"yyyy/MM/dd 00:00:00")
                //    //date += dates.ToString(@"yyyy/MM/dd") + " 00:00:00";

                //    date += Convert.ToDateTime(DOJ).Date.ToString(@"yyyy/MM/dd") + " 00:00:00";

                //    if ( Convert.ToDateTime(DOJ).Date <= DateTime.Now.Date )
                //    {
                //        //dates = DateTime.Now;
                //        date += DateTime.Now.Date.ToString(@"yyyy/MM/dd") + " 00:00:00";
                //    }
                //}
                //catch (Exception e)
                //{
                //    date += DateTime.Now.Date.ToString(@"yyyy/MM/dd") + " 00:00:00";
                //}
                
                cmd.CommandText = "select * from ScheduleTable Left JOIN Flight on ScheduleTable.FlightID = Flight.FlightID where (Source LIKE @Source And Destination LIKE  @Destination) And (DepartureDateTime >= @DepartureDateTime) ";
                cmd.Parameters.AddWithValue("@Source","%" +  Source + "%");
                cmd.Parameters.AddWithValue("@Destination", "%" + Destination + "%");
                cmd.Parameters.AddWithValue("@DepartureDateTime", date );
                cmd.Connection = conn;

                conn.Open();

                SqlDataReader sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        UserBooking userBooking = new UserBooking();

                        userBooking.FlightName = sdr["FlightName"].ToString();
                        userBooking.Source = sdr["Source"].ToString();
                        userBooking.Destination = sdr["Destination"].ToString();
                        userBooking.DepartureDateTime = sdr["DepartureDateTime"].ToString();
                        userBooking.ArrivalDateTime = sdr["ArrivalDateTime"].ToString();
                        userBooking.RouteName = sdr["RouteName"].ToString();
                        userBooking.FlightType = sdr["FlightType"].ToString();
                        userBooking.ScheduleID = sdr["ScheduleID"].ToString();

                        list.Add(userBooking);

                    }
                }
                conn.Close();
            }
            return View(list);
        }


        public ActionResult CheckAvailability(int id)
        {

            List<UserBooking> list = new List<UserBooking>();

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            else
            {

                SqlConnection conn1 = new SqlConnection();
                conn1.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandText = "select FlightID from ScheduleTable where ScheduleID=@ScheduleID";
                cmd1.Parameters.AddWithValue("@ScheduleID", id);
                cmd1.Connection = conn1;

                conn1.Open();

                string FlightID = cmd1.ExecuteScalar().ToString();

                conn1.Close();

                if (FlightID != null)
                {
                    UserBooking userBooking = new UserBooking();


                    SqlConnection conn2 = new SqlConnection();
                    conn2.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.CommandText = "select * from Flight where FlightID=@FlightID ";
                    //cmd.CommandText = "select * from ReservationSeats where FlightID=@FlightID ";
                    //cmd.CommandText = "select * from ScheduleTable where ScheduleID=@ScheduleID ";
                    cmd2.Parameters.AddWithValue("@FlightID", FlightID);
                    //cmd.Parameters.AddWithValue("@ScheduleID", id);
                    cmd2.Connection = conn2;

                    conn2.Open();

                    SqlDataReader sdr = cmd2.ExecuteReader();

                    if (sdr.HasRows)
                    {
                        sdr.Read();
                        userBooking.FlightName = sdr["FlightName"].ToString();
                        userBooking.Source = sdr["Source"].ToString();
                        userBooking.Destination = sdr["Destination"].ToString();
                        userBooking.FlightType = sdr["FlightType"].ToString();

                        //conn2.Close();
                    }

                    SqlConnection conn3 = new SqlConnection();
                    conn3.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                    SqlCommand cmd3 = new SqlCommand();
                    cmd3.CommandText = "select * from ReservationSeats where FlightID=@FlightID and ScheduleID=@ScheduleID";
                    cmd3.Parameters.AddWithValue("@FlightID", FlightID);
                    cmd3.Parameters.AddWithValue("@ScheduleID", id);
                    cmd3.Connection = conn3;

                    conn3.Open();

                    SqlDataReader sdr2 = cmd3.ExecuteReader();

                    if (sdr2.HasRows)
                    {
                        sdr2.Read();

                        if (sdr2["RemainingFirstClassSeats"].ToString() == "0")
                        {
                            userBooking.AvailFCSeats = "Seats Not Availble";
                        }
                        else
                        {
                            userBooking.AvailFCSeats = sdr2["RemainingFirstClassSeats"].ToString();
                            ViewBag.BookedFCSeats = sdr2["RemainingFirstClassSeats"].ToString();
                        }

                        if (sdr2["RemainingBusinessSeats"].ToString() == "0")
                        {
                            userBooking.AvailBCSeats = "Seats Not Availble";
                        }
                        else
                        {
                            userBooking.AvailBCSeats = sdr2["RemainingBusinessSeats"].ToString();
                            ViewBag.BookedBCSeats = sdr2["RemainingBusinessSeats"].ToString();
                        }

                        if (sdr2["RemainingEconomySeats"].ToString() == "0")
                        {
                            userBooking.AvailECSeats = "Seats Not Availble";
                        }
                        else
                        {
                            userBooking.AvailECSeats = sdr2["RemainingEconomySeats"].ToString();
                            ViewBag.BookedECSeats = sdr2["RemainingEconomySeats"].ToString();
                        }

                        //userBooking.AvailFCSeats = sdr2["RemainingFirstClassSeats"].ToString();
                        //userBooking.AvailBCSeats = sdr2["RemainingBusinessSeats"].ToString();
                        //userBooking.AvailECSeats = sdr2["RemainingEconomySeats"].ToString();

                        userBooking.FCPrice = sdr2["FCPrice"].ToString();
                        userBooking.BCPrice = sdr2["BCPrice"].ToString();
                        userBooking.ECPrice = sdr2["ECPrice"].ToString();

                        ViewBag.FCPrice = sdr2["FCPrice"].ToString();
                        ViewBag.BCPrice = sdr2["BCPrice"].ToString();
                        ViewBag.ECPrice = sdr2["ECPrice"].ToString();


                        //conn3.Close();
                    }


                    SqlConnection conn4 = new SqlConnection();
                    conn4.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                    SqlCommand cmd4 = new SqlCommand();
                    cmd4.CommandText = "select * from ScheduleTable where ScheduleID=@ScheduleID";
                    cmd4.Parameters.AddWithValue("@ScheduleID", id);
                    cmd4.Connection = conn4;

                    conn4.Open();

                    SqlDataReader sdr3 = cmd4.ExecuteReader();

                    if (sdr3.HasRows)
                    {
                        sdr3.Read();

                        ///////////////////////
                        ViewBag.ScheduleID = sdr3["ScheduleID"].ToString();


                        userBooking.DepartureDateTime = sdr3["DepartureDateTime"].ToString();
                        userBooking.ArrivalDateTime = sdr3["ArrivalDateTime"].ToString();
                        userBooking.RouteName = sdr3["RouteName"].ToString();

                        //conn4.Close();
                    }

                    list.Add(userBooking);

                }//if

            }//else

            return View(list);
        }//method


        //[HttpPost]
        //public ActionResult CheckAvailability(string ScheduleID, string TotalPriceTF, string BookedFCSeats, string BookedBCSeats, string BookedECSeats)
        //{

        //    if ( BookedFCSeats=="0" && BookedBCSeats=="0" && BookedECSeats=="0" ) { ViewBag.ErrorMessage = "Please Select Seats"; return View(); }
        //    List<UserBooking> list = new List<UserBooking>();
        //    UserBooking userBooking = new UserBooking();
        //    userBooking.FlightName = "ScheduleID = " + ScheduleID + " Total Price = " + TotalPriceTF + " BookedFCSeats = " + BookedFCSeats + " BookedBCSeats = " + BookedBCSeats + " BookedECSeats " + BookedECSeats;
        //    list.Add(userBooking);
        //    return View(list);
        //}//method


        [HttpPost]
        public ActionResult CheckAvailability(string ScheduleID, string TotalPriceTF, string BookedFCSeats, string BookedBCSeats, string BookedECSeats)
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }


            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            conn.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Insert into UserBooking (UserID,ScheduleID,FCSeats,BCSeats,ECSeats,TripPrice,BookStatus) values (@UserID,@ScheduleID,@FCSeats,@BCSeats,@ECSeats,@TripPrice,'0') SELECT SCOPE_IDENTITY()";
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);
            cmd.Parameters.AddWithValue("@ScheduleID", ScheduleID);
            cmd.Parameters.AddWithValue("@FCSeats", BookedFCSeats);
            cmd.Parameters.AddWithValue("@BCSeats", BookedBCSeats);
            cmd.Parameters.AddWithValue("@ECSeats", BookedECSeats);
            cmd.Parameters.AddWithValue("@TripPrice", TotalPriceTF);


            int ReservationID = Convert.ToInt32(cmd.ExecuteScalar());

            if (ReservationID != 0)
            {


                //go to add passenger details
                return RedirectToAction("AddPassenger", "FlightUser", new { id = ReservationID });

                                        //// succes insertion check credit card exists or not
                                        //// if exists go to payment page else go to Add credit card details

                                        //SqlConnection conn1 = new SqlConnection();
                                        //conn1.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                                        //SqlCommand cmd1 = new SqlCommand();
                                        //cmd1.CommandText = "select *  from CreditCard  where UserID =@UserID ";
                                        //cmd1.Parameters.AddWithValue("@UserID ", Session["UserID"]);
                                        //cmd1.Connection = conn1;
                                        //conn1.Open();

                                        //SqlDataReader sdr2 = cmd1.ExecuteReader();

                                        //if (sdr2.HasRows)//if yes then credit card details are available
                                        //{
                                        //    //go to payment
                                        //    //return RedirectToAction("Payment", "FlightUser",new { id = ReservationID });


                                        //    //go tot add passenger details
                                        //    return RedirectToAction("AddPassenger", "FlightUser", new { id = creditcard.BID });
                                        //}
                                        //else
                                        //{
                                        //    //go to Add Credit Card
                                        //    return  RedirectToAction("CheckCreditCard", "FlightUser");

                                        //}//end of else - if (CCID != null)

            }//end of if (rows != 0)

            return View();

        }//method










        public ActionResult AddPassenger(int id)//booking id
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                            SqlConnection conn = new SqlConnection();
                            conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                            conn.Open();


                            SqlCommand cmd = new SqlCommand();
                            cmd.CommandText = "select FCSeats,BCSeats,ECSeats from  UserBooking where BID=@BID";
                            cmd.Parameters.AddWithValue("@BID", id);
                            cmd.Connection = conn;
                            SqlDataReader sdr = cmd.ExecuteReader();
                            if (sdr.HasRows)
                            {
                                sdr.Read();

                                ViewBag.BookingID = id;
                                ViewBag.FCSeats = sdr["FCSeats"].ToString();
                                ViewBag.BCSeats = sdr["BCSeats"].ToString();
                                ViewBag.ECSeats = sdr["ECSeats"].ToString();
                            }
                            else
                            {
                                // go to revervation history 
                                return RedirectToAction("ReservationHistory", "FlightUser");
                            }


            }

            return View();

        }//method




        [HttpPost]
        public string AddPassenger(string BookingID, string[] Flightclass, string[] Name, String[] Age , string[] Gender)//booking id
        {

            string result = "";

            if (Session["UserID"] == null)
            {
                result += "login";
                return result;
            }

            int F = 0, B = 0, E = 0;
            
            string[] Available_FCSeats_Array = Logic(BookingID, "1");
            string[] Available_BCSeats_Array = Logic(BookingID, "2");
            string[] Available_ECSeats_Array = Logic(BookingID, "3");

            int ScheduleID = 0;

            SqlConnection connx = new SqlConnection();
            connx.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            connx.Open();
            SqlCommand cmdx = new SqlCommand();
            cmdx.CommandText = "select ScheduleID from  UserBooking where BID=@BID";
            cmdx.Parameters.AddWithValue("@BID", BookingID);
            cmdx.Connection = connx;
            SqlDataReader sdrx = cmdx.ExecuteReader();
            if (sdrx.HasRows)
            {
                sdrx.Read();
                ScheduleID = Convert.ToInt32(sdrx["ScheduleID"].ToString());
            }


            for (int i=0;i<Age.Length;i++)
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                conn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Insert into Passengers (BID,ScheduleID,Name,Age,Gender,Class,SeatNo) values (@BID,@ScheduleID,@Name,@Age,@Gender,@Class,@SeatNo)";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@ScheduleID", ScheduleID);
                cmd.Parameters.AddWithValue("@BID", BookingID );
                cmd.Parameters.AddWithValue("@Class", Flightclass[i] );
                cmd.Parameters.AddWithValue("@Name", Name[i] );
                cmd.Parameters.AddWithValue("@Age", Age[i] );
                cmd.Parameters.AddWithValue("@Gender", Gender[i] );

                //if(Flightclass[i]== "First Class") { Random FC_Random = new Random(); cmd.Parameters.AddWithValue("@SeatNo", "F" + FC_Random.Next(1, 49) + FC_Random.Next(1, 49)); }
                //if(Flightclass[i]== "Business Class") { Random FC_Random = new Random(); cmd.Parameters.AddWithValue("@SeatNo", "B" + FC_Random.Next(1, 49) + FC_Random.Next(1, 49)); }
                //if(Flightclass[i]== "Economy Class") { Random FC_Random = new Random(); cmd.Parameters.AddWithValue("@SeatNo", "E" + FC_Random.Next(1, 49) + FC_Random.Next(1, 49)); }


                if (Flightclass[i] == "First Class") { cmd.Parameters.AddWithValue("@SeatNo", Available_FCSeats_Array[F++]); }
                if (Flightclass[i] == "Business Class") { cmd.Parameters.AddWithValue("@SeatNo", Available_BCSeats_Array[B++]); }
                if (Flightclass[i] == "Economy Class") { cmd.Parameters.AddWithValue("@SeatNo", Available_ECSeats_Array[E++]); }

                
                int row = cmd.ExecuteNonQuery();

                //Available_FCSeats_Array = Logic(BookingID, "1");
                //Available_BCSeats_Array = Logic(BookingID, "2");
                //Available_ECSeats_Array = Logic(BookingID, "3");
            

        }


            // succes insertion check credit card exists or not
            // if exists go to payment page else go to Add credit card details

            SqlConnection conn1 = new SqlConnection();
            conn1.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            SqlCommand cmd1 = new SqlCommand();
            cmd1.CommandText = "select *  from CreditCard  where UserID = @UserID ";
            cmd1.Parameters.AddWithValue("@UserID ", Session["UserID"]);
            cmd1.Connection = conn1;
            conn1.Open();

            SqlDataReader sdr2 = cmd1.ExecuteReader();

            if (sdr2.HasRows)//if yes then credit card details are available
            {
                //go to payment
                //return RedirectToAction("Payment", "FlightUser", new { id = ReservationID });
                result += BookingID ;
            }
            else
            {
                //go to Add Credit Card
                //return RedirectToAction("CheckCreditCard", "FlightUser");
                result += "xxx";
            }

            return result;
        }




        public String[] Logic(string id, string type)
        {
            int FlightID = 0, ScheduleID = 0, FirstClassSeats = 0, BusinessSeats = 0, EconomySeats = 0;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            conn.Open();


            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select ScheduleID from  UserBooking where BID=@BID";
            cmd.Parameters.AddWithValue("@BID", id);
            cmd.Connection = conn;
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                sdr.Read();
                ScheduleID = Convert.ToInt32(sdr["ScheduleID"].ToString());
            }


            SqlConnection conn2 = new SqlConnection();
            conn2.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            conn2.Open();

            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "select FlightID from ScheduleTable where ScheduleID=@ScheduleID";
            cmd2.Parameters.AddWithValue("@ScheduleID", ScheduleID);
            cmd2.Connection = conn2;
            SqlDataReader sdr2 = cmd2.ExecuteReader();
            if (sdr2.HasRows)
            {
                sdr2.Read();
                FlightID = Convert.ToInt32(sdr2["FlightID"].ToString());
            }

            //select FirstClassSeats ,BusinessSeats ,EconomySeats from ReservationSeats where FlightID=1;

            SqlConnection conn3 = new SqlConnection();
            conn3.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            conn3.Open();

            SqlCommand cmd3 = new SqlCommand();
            cmd3.CommandText = "select FirstClassSeats ,BusinessSeats ,EconomySeats from ReservationSeats where FlightID=@FlightID and ScheduleID=@ScheduleID";
            cmd3.Parameters.AddWithValue("@FlightID", FlightID);
            cmd3.Parameters.AddWithValue("@ScheduleID", ScheduleID);
            cmd3.Connection = conn3;
            SqlDataReader sdr3 = cmd3.ExecuteReader();
            if (sdr3.HasRows)
            {
                sdr3.Read();
                FirstClassSeats += Convert.ToInt32(sdr3["FirstClassSeats"].ToString());
                BusinessSeats += Convert.ToInt32(sdr3["BusinessSeats"].ToString());
                EconomySeats += Convert.ToInt32(sdr3["EconomySeats"].ToString());
            }
            

            //logic

            string[] All_FCSeatsArray = new string[FirstClassSeats];
            string[] All_BCSeatsArray = new string[BusinessSeats];
            string[] All_ECSeatsArray = new string[EconomySeats];

            int i;
            for (i = 0; i < FirstClassSeats ; i++)
            {
                All_FCSeatsArray[i] += "F" + (i+1) + "";
            }

            for (i = 0; i < BusinessSeats; i++)
            {
                All_BCSeatsArray[i] += "B" + (i+1) + "";
            }

            for (i = 0; i < EconomySeats; i++)
            {
                All_ECSeatsArray[i] += "E" + (i+1) + "";
            }
            
            List<String> Allocated_FCSeats = new List<String>();
            List<String> Allocated_BCSeats = new List<String>();
            List<String> Allocated_ECSeats = new List<String>();
            
            //to find allocatred seats to remove from the all array to assign seats to newly booked passengers

            //Allocated_FCSeats
            SqlConnection conn4 = new SqlConnection();
            conn4.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            conn4.Open();
            SqlCommand cmd4 = new SqlCommand();
            cmd4.CommandText = "select SeatNo,PID from Passengers where Class='First Class' and ScheduleID=@ScheduleID ";
            cmd4.Parameters.AddWithValue("@ScheduleID", ScheduleID);
            cmd4.Connection = conn4;

            SqlDataReader sdr4 = cmd4.ExecuteReader();
            if (sdr4.HasRows)
            {
                while (sdr4.Read())
                {
                    Allocated_FCSeats.Add(sdr4["SeatNo"].ToString());
                }
            }



            //Allocated_BCSeats
            SqlConnection conn5 = new SqlConnection();
            conn5.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            conn5.Open();
            SqlCommand cmd5 = new SqlCommand();
            cmd5.CommandText = "select SeatNo from Passengers where Class='Business Class' and ScheduleID=@ScheduleID ";
            cmd5.Parameters.AddWithValue("@ScheduleID", ScheduleID);
            cmd5.Connection = conn5;

            SqlDataReader sdr5 = cmd5.ExecuteReader();
            if (sdr5.HasRows)
            {
                while (sdr5.Read())
                {
                    Allocated_BCSeats.Add(sdr5["SeatNo"].ToString());
                }
            }


            //Allocated_ECSeats
            SqlConnection conn6 = new SqlConnection();
            conn6.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            conn6.Open();
            SqlCommand cmd6 = new SqlCommand();
            cmd6.CommandText = "select SeatNo from Passengers where Class='Economy Class' and ScheduleID=@ScheduleID ";
            cmd6.Parameters.AddWithValue("@ScheduleID", ScheduleID);
            cmd6.Connection = conn6;

            SqlDataReader sdr6 = cmd6.ExecuteReader();
            if (sdr6.HasRows)
            {
                while (sdr6.Read())
                {
                    Allocated_ECSeats.Add(sdr6["SeatNo"].ToString());
                }
            }



            string[] Allocated_FCSeats_Array = Allocated_FCSeats.ToArray();
            string[] Allocated_BCSeats_Array = Allocated_BCSeats.ToArray();
            string[] Allocated_ECSeats_Array = Allocated_ECSeats.ToArray();



            //remove allocated seats from All_array 

            string[] Available_FCSeats_Array = All_FCSeatsArray.Except(Allocated_FCSeats_Array).ToArray();
            string[] Available_BCSeats_Array = All_BCSeatsArray.Except(Allocated_BCSeats_Array).ToArray();
            string[] Available_ECSeats_Array = All_ECSeatsArray.Except(Allocated_ECSeats_Array).ToArray();


            if (type == "1") { return Available_FCSeats_Array; }
            else if (type == "2") { return Available_BCSeats_Array; }
            else { return Available_ECSeats_Array; }
            
        }



        public ActionResult CheckCreditCard()
        {


            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }


            SqlConnection conn1 = new SqlConnection();
            conn1.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            SqlCommand cmd1 = new SqlCommand();
            cmd1.CommandText = "select *  from CreditCard  where UserID =@UserID ";
            cmd1.Parameters.AddWithValue("@UserID ", Session["UserID"]);
            cmd1.Connection = conn1;
            conn1.Open();

            SqlDataReader sdr2 = cmd1.ExecuteReader();

            if (sdr2.HasRows)//if yes then credit card details are available
            {
                //go to edit
                return RedirectToAction("EditCreditCard", "FlightUser");
            }
            else
            {
                //go to add
                return RedirectToAction("AddCreditCard", "FlightUser");
            }

            return View();
            
        }


        public ActionResult AddCreditCard()
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            SqlConnection conn1 = new SqlConnection();
            conn1.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            SqlCommand cmd1 = new SqlCommand();
            cmd1.CommandText = "select *  from CreditCard  where UserID =@UserID ";
            cmd1.Parameters.AddWithValue("@UserID ", Session["UserID"]);
            cmd1.Connection = conn1;
            conn1.Open();

            SqlDataReader sdr2 = cmd1.ExecuteReader();

            if (sdr2.HasRows)//if yes then credit card details are available
            {
                //go to edit
                return RedirectToAction("EditCreditCard", "FlightUser");
            }
            else
            {
                //go to add
                return View();
            }
        }

        [HttpPost]
        public ActionResult AddCreditCard(CreditCard creditcard)
        {
            if (!ModelState.IsValid)
            {
                return View(creditcard);
            }


            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }


            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "insert into CreditCard (UserID,CCNumber,ValidFrom,ValidTo,Balance) values (@UserID,@CCNumber,@ValidFrom,@ValidTo,@Balance) ";
            cmd.Parameters.AddWithValue("UserID", Session["UserID"] );
            cmd.Parameters.AddWithValue("CCNumber", creditcard.CCNumber);
            cmd.Parameters.AddWithValue("ValidFrom", creditcard.ValidFrom);
            cmd.Parameters.AddWithValue("ValidTo", creditcard.ValidTo);
            cmd.Parameters.AddWithValue("Balance", creditcard.Balance);
            cmd.Connection = conn;

            conn.Open();

            int rows = cmd.ExecuteNonQuery();

            if (rows!=0)
            {
                // go to revervation history

                return RedirectToAction("ReservationHistory", "FlightUser");
            }
            
            return View();

        }//end of method


        public ActionResult EditCreditCard()
        {
            CreditCard creditcard = new CreditCard();

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            else
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from CreditCard where UserID=@UserID ";
                cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);
                cmd.Connection = conn;

                conn.Open();

                SqlDataReader sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    sdr.Read();
                    creditcard.CCNumber = sdr["CCNumber"].ToString();
                    creditcard.ValidFrom = sdr["ValidFrom"].ToString();
                    creditcard.ValidTo = sdr["ValidTo"].ToString();
                    creditcard.Balance = sdr["Balance"].ToString();
                }
                else
                {
                    return RedirectToAction("AddCreditCard", "FlightUser");
                }
                conn.Close();
            }

            return View(creditcard);
        }

        [HttpPost]
        public ActionResult EditCreditCard(CreditCard creditcard)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            else
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Update CreditCard set CCNumber=@CCNumber, ValidFrom=@ValidFrom , ValidTo=@ValidTo , Balance=@Balance  where UserID=@UserID ";
                cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);
                cmd.Parameters.AddWithValue("@CCNumber", creditcard.CCNumber);
                cmd.Parameters.AddWithValue("@ValidFrom", creditcard.ValidFrom);
                cmd.Parameters.AddWithValue("@ValidTo", creditcard.ValidTo);
                cmd.Parameters.AddWithValue("@Balance", creditcard.Balance);
                cmd.Connection = conn;

                conn.Open();

                int rows = cmd.ExecuteNonQuery();

                if (rows != 0)
                {
                    ViewBag.UpdateMessage = 1;
                }
                conn.Close();
            }

            return View();
        }//method


        public ActionResult Payment(int id)
        {
            
            CreditCard creditcard = new CreditCard();

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            else
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select BID,Balance,TripPrice,BookStatus from CreditCard Left Join UserBooking on CreditCard.UserID = UserBooking.UserID where BID=@BID";
                cmd.Parameters.AddWithValue("@BID", id);
                cmd.Connection = conn;
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    sdr.Read();

                    creditcard.BID = sdr["BID"].ToString();
                    creditcard.TripPrice = sdr["TripPrice"].ToString();
                    creditcard.Balance = sdr["Balance"].ToString();

                    if (sdr["BookStatus"].ToString() == "1")
                    {
                        // go to revervation history 
                        return RedirectToAction("ReservationHistory", "FlightUser");
                    }

                    ViewBag.TripPrice = sdr["TripPrice"].ToString();
                    ViewBag.Balance = sdr["Balance"].ToString();
                }
                conn.Close();
            }

            return View(creditcard);
        }//method


        [HttpPost]
        public ActionResult Payment(CreditCard creditcard)
        {
            string ScheduleID;
            string FlightID;
            string FCSeats;
            string BCSeats;
            string ECSeats;

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                conn.Open();


                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select ScheduleID,FCSeats,BCSeats,ECSeats from CreditCard Left Join UserBooking on CreditCard.UserID = UserBooking.UserID where UserBooking.BID=@BID";
                cmd.Parameters.AddWithValue("@BID", creditcard.BID);
                cmd.Connection = conn;
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    sdr.Read();
                    ScheduleID = sdr["ScheduleID"].ToString();
                    FCSeats = sdr["FCSeats"].ToString();
                    BCSeats = sdr["BCSeats"].ToString();
                    ECSeats = sdr["ECSeats"].ToString();



                    SqlConnection conn2 = new SqlConnection();
                    conn2.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                    conn2.Open();
                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.CommandText = "select FlightID from ScheduleTable where ScheduleID=@ScheduleID;";
                    cmd2.Parameters.AddWithValue("@ScheduleID", ScheduleID);
                    cmd2.Connection = conn2;
                    SqlDataReader sdr2 = cmd2.ExecuteReader();
                    if (sdr2.HasRows)
                    {
                        sdr2.Read();
                        FlightID = sdr2["FlightID"].ToString();



                        //
                        SqlConnection conn3 = new SqlConnection();
                        conn3.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                        conn3.Open();
                        SqlCommand cmd3 = new SqlCommand();
                        cmd3.CommandText = "update ReservationSeats set RemainingFirstClassSeats=RemainingFirstClassSeats-@FCSeats , RemainingBusinessSeats=RemainingBusinessSeats-@BCSeats , RemainingEconomySeats=RemainingEconomySeats-@ECSeats where FlightID=@FlightID and ScheduleID=@ScheduleID";
                        cmd3.Parameters.AddWithValue("@FlightID", FlightID);
                        cmd3.Parameters.AddWithValue("@ScheduleID", ScheduleID);
                        cmd3.Parameters.AddWithValue("@FCSeats", FCSeats);
                        cmd3.Parameters.AddWithValue("@BCSeats", BCSeats);
                        cmd3.Parameters.AddWithValue("@ECSeats", ECSeats);
                        cmd3.Connection = conn3;
                        int rows3 = cmd3.ExecuteNonQuery();
                        if (rows3 != 0)
                        {

                            //update UserBooking set BookStatus=1 where BID=1;
                            SqlConnection conn4 = new SqlConnection();
                            conn4.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                            conn4.Open();
                            SqlCommand cmd4 = new SqlCommand();
                            cmd4.CommandText = "update UserBooking set BookStatus='1' where BID=@BID ";
                            cmd4.Parameters.AddWithValue("@BID", creditcard.BID);
                            cmd4.Connection = conn4;
                            int rows4 = cmd4.ExecuteNonQuery();
                            if (rows4 != 0)
                            {
                                //update CreditCard set Balance=Balance-10000 where UserID=1;
                                SqlConnection conn5 = new SqlConnection();
                                conn5.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                                conn5.Open();
                                SqlCommand cmd5 = new SqlCommand();
                                cmd5.CommandText = "update CreditCard set Balance=Balance-@TripPrice where UserID=@UserID";
                                cmd5.Parameters.AddWithValue("@TripPrice", creditcard.TripPrice);
                                cmd5.Parameters.AddWithValue("@UserID", Session["UserID"]);
                                cmd5.Connection = conn5;
                                int rows5 = cmd5.ExecuteNonQuery();
                                if (rows5 != 0)
                                {


                                    //go to pay slip
                                    return RedirectToAction("PaySlip", "FlightUser", new { id = creditcard.BID });
                                }





                            }//end of if line 661


                        } //end of if line 652

                    }//end of if line 637

                }//end of if line 623

            }// end of first else 
                
            return View(creditcard);

        }//method




        public ActionResult PaySlip(int id) // BookingID
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }


            string ScheduleID = "";

            //select FlightName,Source,Destination,FlightType,DepartureDateTime,ArrivalDateTime,RouteName from ScheduleTable iNNER jOIN Flight On ScheduleTable.FlightID=Flight.FlightID where ScheduleID=1;

            //select TripPrice from UserBooking where BID=1;

            //select name,Age,Gender,Class,SeatNo from Passengers where BID=1;

            


            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select ScheduleID,TripPrice from  UserBooking where BID=@BID";
            cmd.Parameters.AddWithValue("@BID", id);
            cmd.Connection = conn;
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                sdr.Read();

                ScheduleID += sdr["ScheduleID"].ToString();
                ViewBag.ScheduleID = sdr["ScheduleID"].ToString();
                ViewBag.TripPrice = sdr["TripPrice"].ToString();
            }


            SqlConnection conn2 = new SqlConnection();
            conn2.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            conn2.Open();

            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "select FlightName,Source,Destination,FlightType,DepartureDateTime,ArrivalDateTime,RouteName from ScheduleTable iNNER jOIN Flight On ScheduleTable.FlightID=Flight.FlightID where ScheduleID=@ScheduleID";
            cmd2.Parameters.AddWithValue("@ScheduleID", ScheduleID);
            cmd2.Connection = conn2;
            SqlDataReader sdr2 = cmd2.ExecuteReader();
            if (sdr2.HasRows)
            {
                sdr2.Read();

                ViewBag.FlightName = sdr2["FlightName"].ToString();
                ViewBag.Source = sdr2["Source"].ToString();
                ViewBag.Destination = sdr2["Destination"].ToString();
                ViewBag.DepartureDateTime = sdr2["DepartureDateTime"].ToString();
                ViewBag.ArrivalDateTime = sdr2["ArrivalDateTime"].ToString();
                ViewBag.RouteName = sdr2["RouteName"].ToString();
                ViewBag.FlightType = sdr2["FlightType"].ToString();
            }

            //select FirstClassSeats ,BusinessSeats ,EconomySeats from ReservationSeats where FlightID=1;

            SqlConnection conn3 = new SqlConnection();
            conn3.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            conn3.Open();

            SqlCommand cmd3 = new SqlCommand();
            cmd3.CommandText = "select name,Age,Gender,Class,SeatNo from Passengers where BID=@BID";
            cmd3.Parameters.AddWithValue("@BID", id);
            cmd3.Connection = conn3;
            SqlDataReader sdr3 = cmd3.ExecuteReader();

            List<Passenger> list = new List<Passenger>();


            if (sdr3.HasRows)
            {
                
                while(sdr3.Read())
                {
                    Passenger passenger = new Passenger();

                    passenger.Name = sdr3["name"].ToString();
                    passenger.Age = sdr3["Age"].ToString();
                    passenger.Gender = sdr3["Gender"].ToString();
                    passenger.Class = sdr3["Class"].ToString();
                    passenger.SeatNo = sdr3["SeatNo"].ToString();

                    list.Add(passenger);
                }
                
            }




            return View(list);
            
        }



        public ActionResult ReservationHistory()
        {


            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            int ScheduleID = 0;

            List<UserBooking> list = new List<UserBooking>();

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select BID,ScheduleID,TripPrice,BookStatus from  UserBooking where UserID=@UserID";
            cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);
            cmd.Connection = conn;
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                while(sdr.Read())
                {

                    UserBooking user = new UserBooking();

                    ScheduleID = Convert.ToInt32(sdr["ScheduleID"].ToString());
                    user.BID = sdr["BID"].ToString();
                    user.TripPrice = sdr["TripPrice"].ToString();
                    user.BookStatus = sdr["BookStatus"].ToString();


                    SqlConnection conn2 = new SqlConnection();
                    conn2.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                    conn2.Open();

                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.CommandText = "select FlightName,Source,Destination,FlightType,DepartureDateTime,ArrivalDateTime,RouteName from ScheduleTable iNNER jOIN Flight On ScheduleTable.FlightID=Flight.FlightID where ScheduleID=@ScheduleID";
                    cmd2.Parameters.AddWithValue("@ScheduleID", ScheduleID);
                    cmd2.Connection = conn2;
                    SqlDataReader sdr2 = cmd2.ExecuteReader();
                    if (sdr2.HasRows)
                    {
                        sdr2.Read();

                        user.FlightName = sdr2["FlightName"].ToString();
                        user.Source = sdr2["Source"].ToString();
                        user.Destination = sdr2["Destination"].ToString();
                        user.DepartureDateTime = sdr2["DepartureDateTime"].ToString();
                        user.ArrivalDateTime = sdr2["ArrivalDateTime"].ToString();
                        user.RouteName = sdr2["RouteName"].ToString();
                        user.FlightType = sdr2["FlightType"].ToString();
                    }

                    list.Add(user);

                }

                
            }
            

            return View(list);
        }


        //User View Passenger

        public string CancelTicket(string BookingID)
        {
            string TripPrice="", FCSeats = "", BCSeats = "", ECSeats = "", ScheduleID = "", FlightID = "" , BookingStatus="" ;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select ScheduleID,BookStatus,TripPrice,FCSeats,BCSeats,ECSeats from UserBooking where BID=@BID";
            cmd.Parameters.AddWithValue("BID", BookingID);
            cmd.Connection = conn;

            conn.Open();

            SqlDataReader sdr = cmd.ExecuteReader();
       

            if (sdr.HasRows)
            {
                sdr.Read();
                ScheduleID = sdr["ScheduleID"].ToString();
                TripPrice = sdr["TripPrice"].ToString();
                FCSeats = sdr["FCSeats"].ToString(); 
                BCSeats = sdr["BCSeats"].ToString();
                ECSeats = sdr["ECSeats"].ToString();

                if (sdr["BookStatus"].ToString() == "1")
                {
                    BookingStatus = sdr["BookStatus"].ToString();

                    SqlConnection conn2 = new SqlConnection();
                    conn2.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";

                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.CommandText = "select FlightID from ScheduleTable where ScheduleID=@ScheduleID";
                    cmd2.Parameters.AddWithValue("ScheduleID", ScheduleID);
                    cmd2.Connection = conn2;

                    conn2.Open();

                    SqlDataReader sdr2 = cmd2.ExecuteReader();

                    if (sdr2.HasRows)
                    {
                        sdr2.Read();
                        FlightID = sdr2["FlightID"].ToString();
                    }


                    SqlConnection conn3 = new SqlConnection();
                    conn3.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                    conn3.Open();
                    SqlCommand cmd3 = new SqlCommand();
                    cmd3.CommandText = "update ReservationSeats set RemainingFirstClassSeats=RemainingFirstClassSeats+@FCSeats , RemainingBusinessSeats=RemainingBusinessSeats+@BCSeats , RemainingEconomySeats=RemainingEconomySeats+@ECSeats where FlightID=@FlightID and ScheduleID=@ScheduleID";
                    cmd3.Parameters.AddWithValue("@FlightID", FlightID);
                    cmd3.Parameters.AddWithValue("@ScheduleID", ScheduleID);
                    cmd3.Parameters.AddWithValue("@FCSeats", FCSeats);
                    cmd3.Parameters.AddWithValue("@BCSeats", BCSeats);
                    cmd3.Parameters.AddWithValue("@ECSeats", ECSeats);
                    cmd3.Connection = conn3;
                    int rows3 = cmd3.ExecuteNonQuery();


                    SqlConnection conn4 = new SqlConnection();
                    conn4.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                    conn4.Open();
                    SqlCommand cmd4 = new SqlCommand();
                    cmd4.CommandText = "update CreditCard set Balance=Balance+@TripPrice where UserID=@UserID";
                    cmd4.Parameters.AddWithValue("UserID", Session["UserID"]);
                    cmd4.Parameters.AddWithValue("TripPrice", TripPrice);
                    cmd4.Connection = conn4;
                    int rows4 = cmd4.ExecuteNonQuery();
                }


                SqlConnection conn6 = new SqlConnection();
                conn6.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                SqlCommand cmd6 = new SqlCommand();
                cmd6.CommandText = "Delete from Passengers where BID=@BID ";
                cmd6.Parameters.AddWithValue("BID", BookingID);
                cmd6.Connection = conn6;
                conn6.Open();
                int row = cmd6.ExecuteNonQuery();


                SqlConnection conn66 = new SqlConnection();
                conn66.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                SqlCommand cmd66 = new SqlCommand();
                cmd66.CommandText = "Delete from UserBooking where BID=@BID";
                cmd66.Parameters.AddWithValue("BID", BookingID);
                cmd66.Connection = conn66;
                conn66.Open();
                int row2 = cmd66.ExecuteNonQuery();

            }

            if (BookingStatus == "1") { return "" + TripPrice; }
            else { return "ok"; }
        }


        public ActionResult ViewPassenger(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }


            string ScheduleID = "";

            //select FlightName,Source,Destination,FlightType,DepartureDateTime,ArrivalDateTime,RouteName from ScheduleTable iNNER jOIN Flight On ScheduleTable.FlightID=Flight.FlightID where ScheduleID=1;

            //select TripPrice from UserBooking where BID=1;

            //select name,Age,Gender,Class,SeatNo from Passengers where BID=1;




            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select ScheduleID,TripPrice from  UserBooking where BID=@BID";
            cmd.Parameters.AddWithValue("@BID", id);
            cmd.Connection = conn;
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                sdr.Read();

                ScheduleID += sdr["ScheduleID"].ToString();
                ViewBag.ScheduleID = sdr["ScheduleID"].ToString();
                ViewBag.TripPrice = sdr["TripPrice"].ToString();
            }


            SqlConnection conn2 = new SqlConnection();
            conn2.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            conn2.Open();

            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "select FlightName,Source,Destination,FlightType,DepartureDateTime,ArrivalDateTime,RouteName from ScheduleTable iNNER jOIN Flight On ScheduleTable.FlightID=Flight.FlightID where ScheduleID=@ScheduleID";
            cmd2.Parameters.AddWithValue("@ScheduleID", ScheduleID);
            cmd2.Connection = conn2;
            SqlDataReader sdr2 = cmd2.ExecuteReader();
            if (sdr2.HasRows)
            {
                sdr2.Read();

                ViewBag.FlightName = sdr2["FlightName"].ToString();
                ViewBag.Source = sdr2["Source"].ToString();
                ViewBag.Destination = sdr2["Destination"].ToString();
                ViewBag.DepartureDateTime = sdr2["DepartureDateTime"].ToString();
                ViewBag.ArrivalDateTime = sdr2["ArrivalDateTime"].ToString();
                ViewBag.RouteName = sdr2["RouteName"].ToString();
                ViewBag.FlightType = sdr2["FlightType"].ToString();
            }

            //select FirstClassSeats ,BusinessSeats ,EconomySeats from ReservationSeats where FlightID=1;

            SqlConnection conn3 = new SqlConnection();
            conn3.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            conn3.Open();

            SqlCommand cmd3 = new SqlCommand();
            cmd3.CommandText = "select name,Age,Gender,Class,SeatNo from Passengers where BID=@BID";
            cmd3.Parameters.AddWithValue("@BID", id);
            cmd3.Connection = conn3;
            SqlDataReader sdr3 = cmd3.ExecuteReader();

            List<Passenger> list = new List<Passenger>();


            if (sdr3.HasRows)
            {

                while (sdr3.Read())
                {
                    Passenger passenger = new Passenger();

                    passenger.Name = sdr3["name"].ToString();
                    passenger.Age = sdr3["Age"].ToString();
                    passenger.Gender = sdr3["Gender"].ToString();
                    passenger.Class = sdr3["Class"].ToString();
                    passenger.SeatNo = sdr3["SeatNo"].ToString();

                    list.Add(passenger);
                }

            }
           
            if(list.Count==0)
            {
                int BID = id;
                //add passenger
                return RedirectToAction("AddPassenger", "FlightUser", new { id = BID });
            }


            return View(list);
        }












    }
}