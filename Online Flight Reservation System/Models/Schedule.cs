using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace Online_Flight_Reservation_System.Models
{
    public class Schedule
    {
        //ScheduleID int Primary Key Identity(1,1),FlightID int,DepartureDateTime datetime, ArrivalDateTime datetime,RouteName varchar(50)

        public string ScheduleID { get; set; }

        

        public string FlightID { get; set; }

        [Display(Name = "Departure Date Time")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Departure Date Time Required!")]
        [RegularExpression(@"\d{4}/\d{2}/\d{2} \d{2}:\d{2}:\d{2}", ErrorMessage = "Departure Date Time Should be Format YYYY/MM/DD HH:MM:SS")]
        public string DepartureDateTime { get; set; }


        [Display(Name = "Arrival Date Time")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Arrival Date Time Required!")]
        [RegularExpression(@"\d{4}/\d{2}/\d{2} \d{2}:\d{2}:\d{2}", ErrorMessage = "Arrival Date Time Should be Format YYYY/MM/DD HH:MM:SS")]
        public string ArrivalDateTime { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Route Name Required!")]
        [MaxLength(50, ErrorMessage = "Route Name Should Not Be More Than 50 Characters Long")]
        public string RouteName { get; set; }
        
    }
}