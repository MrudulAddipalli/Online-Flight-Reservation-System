using Online_Flight_Reservation_System.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Online_Flight_Reservation_System.Controllers
{
    public class FlightAdminController : Controller
    {
        // GET: FlightAdmin
        public ActionResult ListFlightDetails()
        {
            List<Flight> list = new List<Flight>();

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
                cmd.CommandText = "select * from Flight Left Join ReservationSeats on Flight.FlightID=ReservationSeats.FlightID where ReservationSeats.ScheduleID IS NULL Order By Flight.FlightID DESC ";
                cmd.Connection = conn;

                conn.Open();
                
                SqlDataReader sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        Flight flight = new Flight();

                        flight.FlightID = sdr["FlightID"].ToString();
                        flight.FlightName = sdr["FlightName"].ToString();
                        flight.Source = sdr["Source"].ToString();
                        flight.Destination = sdr["Destination"].ToString();
                        flight.EstimatedTime = sdr["EstimatedTime"].ToString();
                        flight.SeatCapacity = sdr["SeatCapacity"].ToString();
                        flight.FlightType = sdr["FlightType"].ToString();
                        flight.FirstSeats = sdr["FirstClassSeats"].ToString();
                        flight.BusinessSeats = sdr["BusinessSeats"].ToString();
                        flight.EconomySeats = sdr["EconomySeats"].ToString();
                        flight.FCPrice = sdr["FCPrice"].ToString();
                        flight.BCPrice = sdr["BCPrice"].ToString();
                        flight.ECPrice = sdr["ECPrice"].ToString();
                        list.Add(flight);

                    }
                }
                conn.Close();
            }

            return View(list);
        }

        public ActionResult AddFlight()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult AddFlight(Flight flight)
        {
            if (!ModelState.IsValid)
            {
                return View(flight);
            }

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            int seat = 0, seatFirst = 0;
            if (flight.FirstSeats != null){ seat += Convert.ToInt32(flight.FirstSeats.ToString()); seatFirst = seat; }
            if (flight.BusinessSeats != null) { seat += Convert.ToInt32(flight.BusinessSeats.ToString()); }
            if (flight.EconomySeats != null) { seat += Convert.ToInt32(flight.EconomySeats.ToString()); }

            string seats = seat.ToString();

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            conn.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Insert into Flight(FlightName,Source,Destination,EstimatedTime,SeatCapacity,FlightType) values (@FlightName,@Source,@Destination,@EstimatedTime,@SeatCapacity,@FlightType) SELECT SCOPE_IDENTITY()";
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue("@FlightName", flight.FlightName);
            cmd.Parameters.AddWithValue("@Source", flight.Source);
            cmd.Parameters.AddWithValue("@Destination", flight.Destination);
            cmd.Parameters.AddWithValue("@EstimatedTime", flight.EstimatedTime);
            cmd.Parameters.AddWithValue("@SeatCapacity", seats);
            cmd.Parameters.AddWithValue("@FlightType", flight.FlightType);


            int FlightId = Convert.ToInt32(cmd.ExecuteScalar());

            if (FlightId != 0)
            {
                cmd.CommandText = "Insert into ReservationSeats (FlightID,FirstClassSeats,BusinessSeats,EconomySeats,RemainingFirstClassSeats,RemainingBusinessSeats,RemainingEconomySeats,FCPrice,BCPrice,ECPrice) values (@FlightID,@FirstClassSeats,@BusinessSeats,@EconomySeats,@FirstClassSeats,@BusinessSeats,@EconomySeats,@FCPrice,@BCPrice,@ECPrice)";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@FlightID", FlightId);
                cmd.Parameters.AddWithValue("@FirstClassSeats", seatFirst);
                cmd.Parameters.AddWithValue("@BusinessSeats", flight.BusinessSeats);
                cmd.Parameters.AddWithValue("@EconomySeats", flight.EconomySeats);
                cmd.Parameters.AddWithValue("@FCPrice", flight.FCPrice);
                cmd.Parameters.AddWithValue("@BCPrice", flight.BCPrice);
                cmd.Parameters.AddWithValue("@ECPrice", flight.ECPrice);

                int rows = cmd.ExecuteNonQuery();

                conn.Close();

                if (rows == 1)
                {
                    return RedirectToAction("ListFlightDetails", "FlightAdmin");
                }
                else
                {
                    return View(flight);
                }
            }

            return View(flight);
        }


        public ActionResult EditFlight(int id)
        {
            Flight flight = new Flight();

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            else
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from Flight Inner Join ReservationSeats on Flight.FlightID=ReservationSeats.FlightID where Flight.FlightID=@FlightID ";
                cmd.Parameters.AddWithValue("@FlightID", id);
                cmd.Connection = conn;

                conn.Open();

                SqlDataReader sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    sdr.Read();
                    flight.FlightID = sdr["FlightID"].ToString();
                    flight.FlightName = sdr["FlightName"].ToString();
                    flight.Source = sdr["Source"].ToString();
                    flight.Destination = sdr["Destination"].ToString();
                    flight.EstimatedTime = sdr["EstimatedTime"].ToString();
                    flight.SeatCapacity = sdr["SeatCapacity"].ToString();
                    flight.FlightType = sdr["FlightType"].ToString();
                    flight.FirstSeats = sdr["FirstClassSeats"].ToString();
                    flight.BusinessSeats = sdr["BusinessSeats"].ToString();
                    flight.EconomySeats = sdr["EconomySeats"].ToString();
                    flight.FCPrice = sdr["FCPrice"].ToString();
                    flight.BCPrice = sdr["BCPrice"].ToString();
                    flight.ECPrice = sdr["ECPrice"].ToString();

                }
                conn.Close();
            }

            return View(flight);
        }


        [HttpPost]
        public ActionResult EditFlight(Flight flight)
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }


            int seat = 0, seatFirst = 0;
            if (flight.FirstSeats != null)
            {
                if (int.TryParse(flight.FirstSeats, out seatFirst))
                {
                    seat += seatFirst;
                }
            }
            seat += Convert.ToInt32(flight.BusinessSeats.ToString()) + Convert.ToInt32(flight.EconomySeats.ToString());

            string seats = seat.ToString();

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            conn.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Update Flight SET FlightName=@FlightName , Source=@Source , Destination=@Destination , EstimatedTime=@EstimatedTime , SeatCapacity=@SeatCapacity , FlightType=@FlightType where FlightID=@FlightID";
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue("@FlightID", flight.FlightID);
            cmd.Parameters.AddWithValue("@FlightName", flight.FlightName);
            cmd.Parameters.AddWithValue("@Source", flight.Source);
            cmd.Parameters.AddWithValue("@Destination", flight.Destination);
            cmd.Parameters.AddWithValue("@EstimatedTime", flight.EstimatedTime);
            cmd.Parameters.AddWithValue("@SeatCapacity", seats);
            cmd.Parameters.AddWithValue("@FlightType", flight.FlightType);


            int rows = cmd.ExecuteNonQuery();

            if (rows!=0)
            {
                //FlightID int,FirstClassSeats int,BusinessSeats int,EconomySeats int,RemainingFirstClassSeats int,RemainingBusinessSeats int,RemainingEconomySeats int);

                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandText = "Update ReservationSeats SET FirstClassSeats=@FirstClassSeats , BusinessSeats=@BusinessSeats , EconomySeats=@EconomySeats , RemainingFirstClassSeats=@FirstClassSeats , RemainingBusinessSeats=@BusinessSeats , RemainingEconomySeats=@EconomySeats , FCPrice=@FCPrice , BCPrice=@BCPrice , ECPrice=@ECPrice   where FlightID= @FlightID";
                cmd1.Connection = conn;
                cmd1.Parameters.AddWithValue("@FlightID", flight.FlightID);
                cmd1.Parameters.AddWithValue("@FirstClassSeats", seatFirst);
                cmd1.Parameters.AddWithValue("@BusinessSeats", flight.BusinessSeats);
                cmd1.Parameters.AddWithValue("@EconomySeats", flight.EconomySeats);

                cmd1.Parameters.AddWithValue("@FCPrice", flight.FCPrice);
                cmd1.Parameters.AddWithValue("@BCPrice", flight.BCPrice);
                cmd1.Parameters.AddWithValue("@ECPrice", flight.ECPrice);

                int row = cmd1.ExecuteNonQuery();

                conn.Close();

                if (row == 1)
                {
                    return RedirectToAction("ListFlightDetails", "FlightAdmin");
                }
            }

            return View(flight);
        }

        

        public ActionResult DeleteFlight(int id)
        {
            Flight flight = new Flight();

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            else
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from Flight Inner Join ReservationSeats on Flight.FlightID=ReservationSeats.FlightID where Flight.FlightID=@FlightID ";
                cmd.Parameters.AddWithValue("FlightID", id);
                cmd.Connection = conn;

                conn.Open();

                SqlDataReader sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    sdr.Read();
                    flight.FlightID = sdr["FlightID"].ToString();
                    flight.FlightName = sdr["FlightName"].ToString();
                    flight.Source = sdr["Source"].ToString();
                    flight.Destination = sdr["Destination"].ToString();
                    flight.EstimatedTime = sdr["EstimatedTime"].ToString();
                    flight.SeatCapacity = sdr["SeatCapacity"].ToString();
                    flight.FlightType = sdr["FlightType"].ToString();
                    flight.FirstSeats = sdr["FirstClassSeats"].ToString();
                    flight.BusinessSeats = sdr["BusinessSeats"].ToString();
                    flight.EconomySeats = sdr["EconomySeats"].ToString();

                    flight.FCPrice = sdr["FCPrice"].ToString();
                    flight.BCPrice = sdr["BCPrice"].ToString();
                    flight.ECPrice = sdr["ECPrice"].ToString();
                }

                conn.Close();
            }

            return View(flight);
        }



        [HttpPost]
        public ActionResult DeleteFlight(int id,Flight flight)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            else
            {
                SqlConnection conn1 = new SqlConnection();
                conn1.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandText = "Delete from Flight where FlightID=@FlightID ";
                cmd1.Parameters.AddWithValue("FlightID", id);
                cmd1.Connection = conn1;
                conn1.Open();
                int row1 = cmd1.ExecuteNonQuery();


                SqlConnection conn2 = new SqlConnection();
                conn2.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = "Delete from  ReservationSeats where FlightID=@FlightID ";
                cmd2.Parameters.AddWithValue("FlightID", id);
                cmd2.Connection = conn2;
                conn2.Open();
                int row2 = cmd2.ExecuteNonQuery();


                SqlConnection conn3 = new SqlConnection();
                conn3.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                SqlCommand cmd3 = new SqlCommand();
                cmd3.CommandText = "Delete from  ScheduleTable where FlightID=@FlightID ";
                cmd3.Parameters.AddWithValue("FlightID", id);
                cmd3.Connection = conn3;
                conn3.Open();
                int row3 = cmd3.ExecuteNonQuery();

                if (row3 == 1)
                {
                    return RedirectToAction("ListFlightDetails", "FlightAdmin");
                }
                
            }

            return View();
        }



























        public ActionResult SchdeuleFlight(int id)
        {

            List<Schedule> list = new List<Schedule>();

            ViewBag.FlightID = ""+ id;
            //session

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            else
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from ScheduleTable where FlightID=@FlightID";
                cmd.Parameters.AddWithValue("@FlightID", id);
                cmd.Connection = conn;

                conn.Open();

                SqlDataReader sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        Schedule schedule = new Schedule();
                        schedule.ScheduleID = sdr["ScheduleID"].ToString();
                        schedule.FlightID = sdr["FlightID"].ToString();
                        schedule.DepartureDateTime=  sdr["DepartureDateTime"].ToString();
                        schedule.ArrivalDateTime = sdr["ArrivalDateTime"].ToString();
                        schedule.RouteName = sdr["RouteName"].ToString();

                        list.Add(schedule);

                    }
                }

                conn.Close();
            }

            return View(list);

        }




        public ActionResult AddSchedule(int id)
        {

            //string id = "";
            //int flightId = 0;

            //if (Request.QueryString["Fid"] == null)
            //{
            //    Response.Write("No Id Found");
            //}

            //id = Request.QueryString["Fid"].ToString();

            ViewBag.FlightID = "" + id;

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            //if (Request.QueryString["Fid"] == null)
            //{
            //    Response.Write("No Id Found");
            //}

            return View();

        }

        [HttpPost]
        public ActionResult AddSchedule(int id,Schedule schedule)//here id is flight id
        {
            //string flightID = "";
            //int flightId = 0;

            //if (Request.QueryString["Fid"] == null)
            //{
            //    Response.Write("No Id Found");
            //}

            //flightID = Request.QueryString["Fid"].ToString();
            //flightId = Convert.ToInt32(flightID);

            string flightID = id+"";


            if (!ModelState.IsValid)
            {
                return View(schedule);
            }

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Insert into ScheduleTable(FlightID,DepartureDateTime,ArrivalDateTime,RouteName) values (@FlightID,@DepartureDateTime,@ArrivalDateTime,@RouteName) SELECT SCOPE_IDENTITY() ";
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue("@FlightID", flightID);
            cmd.Parameters.AddWithValue("@DepartureDateTime", Convert.ToDateTime(schedule.DepartureDateTime).ToString(@"yyyy/MM/dd HH:mm:ss") );
            cmd.Parameters.AddWithValue("@ArrivalDateTime", Convert.ToDateTime(schedule.ArrivalDateTime).ToString(@"yyyy/MM/dd HH:mm:ss") );
            cmd.Parameters.AddWithValue("@RouteName", schedule.RouteName);

            string ScheduleID = Convert.ToInt32(cmd.ExecuteScalar()) + "";



            SqlConnection conn3 = new SqlConnection();
            conn3.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            SqlCommand cmd3 = new SqlCommand();
            cmd3.CommandText = "select * from ReservationSeats where FlightID=@FlightID ";
            cmd3.Parameters.AddWithValue("@FlightID", flightID);
            cmd3.Connection = conn3;
            conn3.Open();
            SqlDataReader sdr2 = cmd3.ExecuteReader();

            string FirstClassSeats="", BusinessSeats="", EconomySeats="", RemainingFirstClassSeats = "", RemainingBusinessSeats = "", RemainingEconomySeats = "", FCPrice = "", BCPrice = "", ECPrice = "";
            if (sdr2.HasRows)
            {
                sdr2.Read();
                FirstClassSeats += sdr2["FirstClassSeats"].ToString();
                BusinessSeats += sdr2["BusinessSeats"].ToString();
                EconomySeats += sdr2["EconomySeats"].ToString();
                RemainingFirstClassSeats += sdr2["RemainingFirstClassSeats"].ToString();
                RemainingBusinessSeats += sdr2["RemainingBusinessSeats"].ToString();
                RemainingEconomySeats += sdr2["RemainingEconomySeats"].ToString();
                FCPrice += sdr2["FCPrice"].ToString();
                BCPrice += sdr2["BCPrice"].ToString();
                ECPrice += sdr2["ECPrice"].ToString(); ;
            }

            SqlConnection connx = new SqlConnection();
            connx.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            connx.Open();
            SqlCommand cmdx = new SqlCommand();
            cmdx.CommandText = "Insert into ReservationSeats (FlightID,ScheduleID,FirstClassSeats,BusinessSeats,EconomySeats,RemainingFirstClassSeats,RemainingBusinessSeats,RemainingEconomySeats,FCPrice,BCPrice,ECPrice) values (@FlightID,@ScheduleID,@FirstClassSeats,@BusinessSeats,@EconomySeats,@FirstClassSeats,@BusinessSeats,@EconomySeats,@FCPrice,@BCPrice,@ECPrice)";
            cmdx.Connection = connx;
            cmdx.Parameters.AddWithValue("@FlightID", flightID);
            cmdx.Parameters.AddWithValue("@ScheduleID", ScheduleID);
            cmdx.Parameters.AddWithValue("@FirstClassSeats", FirstClassSeats);
            cmdx.Parameters.AddWithValue("@BusinessSeats", BusinessSeats);
            cmdx.Parameters.AddWithValue("@EconomySeats", EconomySeats);
            cmdx.Parameters.AddWithValue("@FCPrice", FCPrice);
            cmdx.Parameters.AddWithValue("@BCPrice", BCPrice);
            cmdx.Parameters.AddWithValue("@ECPrice", ECPrice);

            int rows = cmdx.ExecuteNonQuery();
            if(rows!=0)
            {
                return RedirectToAction("SchdeuleFlight", "FlightAdmin", new { id = flightID });
            }


            return View(schedule);
        }



        public ActionResult EditSchedule(int id)
        {
            
            Schedule schedule = new Schedule();

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            else
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from ScheduleTable where ScheduleID=@ScheduleID ";
                cmd.Parameters.AddWithValue("ScheduleID", id);
                cmd.Connection = conn;

                conn.Open();

                SqlDataReader sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    sdr.Read();
                    schedule.ScheduleID = sdr["ScheduleID"].ToString();
                    schedule.FlightID = sdr["FlightID"].ToString();

                    ViewBag.FlightID = "" + sdr["FlightID"].ToString();


                    schedule.DepartureDateTime = sdr["DepartureDateTime"].ToString();
                    schedule.ArrivalDateTime = sdr["ArrivalDateTime"].ToString();
                    schedule.RouteName = sdr["RouteName"].ToString();
                }
                conn.Close();
            }
            return View(schedule);
        }

        [HttpPost]
        public ActionResult EditSchedule(Schedule schedule)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            conn.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Update ScheduleTable SET DepartureDateTime=@DepartureDateTime , ArrivalDateTime=@ArrivalDateTime , RouteName=@RouteName  where ScheduleID=@ScheduleID";
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue("@ScheduleID", schedule.ScheduleID);
            cmd.Parameters.AddWithValue("@DepartureDateTime", schedule.DepartureDateTime);
            cmd.Parameters.AddWithValue("@ArrivalDateTime", schedule.ArrivalDateTime);
            cmd.Parameters.AddWithValue("@RouteName", schedule.RouteName);

            int rows = cmd.ExecuteNonQuery();

            if (rows != 0)
            {
                return RedirectToAction("SchdeuleFlight", "FlightAdmin", new { id = schedule.FlightID });
            }

            return View(schedule);
        }



        public ActionResult DeleteSchedule(int id)
        {

            Schedule schedule = new Schedule();

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            else
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from ScheduleTable where ScheduleID=@ScheduleID ";
                cmd.Parameters.AddWithValue("ScheduleID", id);
                cmd.Connection = conn;

                conn.Open();

                SqlDataReader sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    sdr.Read();
                    schedule.ScheduleID = sdr["ScheduleID"].ToString();
                    schedule.FlightID = sdr["FlightID"].ToString();

                    //
                    ViewBag.FlightID = "" + sdr["FlightID"].ToString();


                    schedule.DepartureDateTime = sdr["DepartureDateTime"].ToString();
                    schedule.ArrivalDateTime = sdr["ArrivalDateTime"].ToString();
                    schedule.RouteName = sdr["RouteName"].ToString();
                }
                conn.Close();
            }
            return View(schedule);
        }


        [HttpPost]
        public ActionResult DeleteSchedule(int id,Schedule schedule)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select * from ScheduleTable where ScheduleID=@ScheduleID ";
            cmd.Parameters.AddWithValue("ScheduleID", id);
            cmd.Connection = conn;

            conn.Open();

            SqlDataReader sdr = cmd.ExecuteReader();

           

            if (sdr.HasRows)
            {

                sdr.Read();

                string flightId = sdr["FlightID"].ToString();
                conn.Close();

                SqlConnection conn2 = new SqlConnection();
                conn2.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                conn2.Open();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = "Delete from ScheduleTable where ScheduleID=@ScheduleID";
                cmd2.Connection = conn2;
                cmd2.Parameters.AddWithValue("@ScheduleID", id);

                int rows = cmd2.ExecuteNonQuery();

                if (rows != 0)
                {
                    conn2.Close();

                    SqlConnection conn22 = new SqlConnection();
                    conn22.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                    SqlCommand cmd22 = new SqlCommand();
                    cmd22.CommandText = "Delete from  ReservationSeats where ScheduleID=@ScheduleID";
                    cmd22.Parameters.AddWithValue("@ScheduleID", id);
                    cmd22.Connection = conn22;
                    conn22.Open();
                    int row22 = cmd22.ExecuteNonQuery();
                    if (row22 != 0)
                    {
                        return RedirectToAction("SchdeuleFlight", "FlightAdmin", new { id = flightId });
                    }
                }

            }
            
            return View(schedule);
        }





        public ActionResult ViewBooking(int id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            List<string> ScheduleIDList = new List<string>();

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select ScheduleID from ScheduleTable where FlightID=@FlightID";
            cmd.Parameters.AddWithValue("@FlightID", id);
            cmd.Connection = conn;
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    ScheduleIDList.Add(sdr["ScheduleID"].ToString());
                }
            }

            string[] ScheduleIDArray = ScheduleIDList.ToArray();

            List<UserBooking> list = new List<UserBooking>();

            for(int i=0;i<ScheduleIDArray.Length-1;i++)
            { 
                SqlConnection conn2 = new SqlConnection();
                conn2.ConnectionString = "Server=.;Initial Catalog=OFRS;User Id=sa;Password=wipro@123";
                conn2.Open();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = "select * from UserBooking where ScheduleID=@ScheduleID And BookStatus='1' ";
                cmd2.Parameters.AddWithValue("@ScheduleID", ScheduleIDArray[i]);
                cmd2.Connection = conn2;
                SqlDataReader sdr2 = cmd2.ExecuteReader();

                if (sdr2.HasRows)
                {
                    while (sdr2.Read())
                    {
                        UserBooking user = new UserBooking();
                        user.BID = sdr2["BID"].ToString();
                        user.FCSeats = sdr2["FCSeats"].ToString();
                        user.BCSeats = sdr2["BCSeats"].ToString();
                        user.ECSeats = sdr2["ECSeats"].ToString();
                        user.TripPrice = sdr2["TripPrice"].ToString();
                        list.Add(user);
                    }
                }

            }

            return View(list);
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

            if (list.Count == 0)
            {
                int BID = id;
                //add passenger
                return RedirectToAction("AddPassenger", "FlightUser", new { id = BID });
            }


            return View(list);
        }



    }

}