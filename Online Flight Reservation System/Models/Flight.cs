using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Online_Flight_Reservation_System.Models
{
    public class Flight
    {
        //(FlightID int Primary Key identity(1,1),FlightName varchar(30),Source varchar(30),Destination varchar(30),
        //EstimatedTime varchar(10),SeatCapacity int,FlightType varchar(20));

        //FirstClassSeats int,BusinessSeats int,EconomySeats int

        public string FlightID { get; set; }



        [Display(Name = "Flight Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Flight Name Required!")]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only Letters Possible In Flight Name")]
        [MaxLength(30, ErrorMessage = "Flight Name Should Not Be More Than 30 Characters Long")]
        public string FlightName { get; set; }





        [Display(Name = "Flight Source")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Flight Source Required!")]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only Letters Possible In Flight Source")]
        [MaxLength(30, ErrorMessage = "Flight Source Should Not Be More Than 30 Characters Long")]
        public string Source { get; set; }




        [Display(Name = "Flight Destination")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Flight Destination Required!")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only Letters Possible In Flight Destination")]
        [MaxLength(30, ErrorMessage = "Flight Destination Should Not Be More Than 30 Characters Long")]
        public string Destination { get; set; }




        [Display(Name = "Flight Estimated Time")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Flight Estimated Time Required!")]
        [RegularExpression(@"\d{2}:\d{2}", ErrorMessage = "Departure Date Time Should be Format HH:MM")]
        [MaxLength(5, ErrorMessage = "Flight Estimated Time Be In Format HH:MM")]
        public string EstimatedTime { get; set; }



        //[Display(Name = "Flight Seat Capacity")]
        public string SeatCapacity { get; set; }




        [Display(Name = "Flight Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Flight Type Required!")]
        public string FlightType { get; set; }




        [Display(Name = "Total First Seats")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Total First Class Seats Required! (If Flight Is Domestic Then Enter 0)")]
        [Range(0, 500, ErrorMessage = "Total First Class Seats Should Not Be Between 0 And 500")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only Numbers Are Allowed In UserId")]
        public string FirstSeats { get; set; }




        [Display(Name = "Total Business Seats")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Total Business Class Seats Required!")]
        [Range(5, 500, ErrorMessage = "Total First Class Seats Should Not Be Between 5 And 500")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only Numbers Are Allowed In UserId")]
        public string BusinessSeats { get; set; }




        [Display(Name = "Total Economy Seats")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Total Economy Class Seats Required!")]
        [Range(5, 500, ErrorMessage = "Total Economy Class Seats Should Not Be Between 5 And 500")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only Numbers Are Allowed In UserId")]
        public string EconomySeats { get; set; }




        [Display(Name = "First Class Seat Price")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First Class Seat Price Required! (If Flight Is Domestic Then Enter 0)")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only Numbers Are Allowed In UserId")]
        public string FCPrice { get; set; }




        [Display(Name = "Business Class Seat Price")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Business Class Seat Price Required!)")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only Numbers Are Allowed In UserId")]
        public string BCPrice { get; set; }



        [Display(Name = "Economy Class Seat Price")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Economy Class Seat Price Required!")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only Numbers Are Allowed In UserId")]
        public string ECPrice { get; set; }


    }
    
}