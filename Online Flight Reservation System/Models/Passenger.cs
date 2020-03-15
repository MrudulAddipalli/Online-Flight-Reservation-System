using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace Online_Flight_Reservation_System.Models
{
    public class Passenger
    {
        public string Name{ get; set; }
        public string Age{ get; set; }
        public string Gender{ get; set; }


        [Display(Name = "Flight Class")]
        public string Class{ get; set; }


        [Display(Name = "Seat Number")]
        public string SeatNo { get; set; }
    }
}