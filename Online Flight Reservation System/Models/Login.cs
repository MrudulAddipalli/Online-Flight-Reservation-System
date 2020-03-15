using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Online_Flight_Reservation_System.Models
{
    public class Login
    {
     
        //[Display(Name = "Enter UserID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "UserID Required!")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only Numbers Are Allowed In UserId")]
        [MaxLength(9, ErrorMessage = "UserId Should Not Be More Than 9 Characters Long")]
        public string UserId { get; set; }



        //[Display(Name = "Enter Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password Required!")]
        [DataType(DataType.Password)]
        [MaxLength(9, ErrorMessage = "Password Should Not Be More Than 9 Characters Long")]
        public string Password { get; set; }
    }
}