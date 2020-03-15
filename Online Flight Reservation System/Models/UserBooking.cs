using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace Online_Flight_Reservation_System.Models
{
    public class UserBooking
    {
        [Display(Name = "Booking Id")]
        public string BID { get; set; }


        [Display(Name = "Trip Price")]
        public string TripPrice { get; set; }



        [Display(Name = "First Class Seats Booked")]
        public string FCSeats { get; set; }


        [Display(Name = "Business Class Seats Booked")]
        public string BCSeats { get; set; }


        [Display(Name = "Economy Class Seats Booked")]
        public string ECSeats { get; set; }


        public string DOJ { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }


        [Display(Name = "Departure Date Time")]
        public string DepartureDateTime { get; set; }



        [Display(Name = "Arrival Date Time")]
        public string ArrivalDateTime { get; set; }



        [Display(Name = "Route")]
        public string RouteName { get; set; }
        
        public string UserID { get; set; }
        public string ScheduleID { get; set; }



        [Display(Name = "Flight Name")]
        public string FlightName { get; set; }



        [Display(Name = "Flight Type")]
        public string FlightType { get; set; }
        public string AvailFCSeats { get; set; }
        public string AvailBCSeats { get; set; }
        public string AvailECSeats { get; set; }
        public string FCPrice { get; set; }
        public string BCPrice { get; set; }
        public string ECPrice { get; set; }
        public string BookedFCSeats { get; set; }
        public string BookedBCSeats { get; set; }
        public string BookedECSeats { get; set; }
        
        public string BookStatus { get; set; }
        

    }
}