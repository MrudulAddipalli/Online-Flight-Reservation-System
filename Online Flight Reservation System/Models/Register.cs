using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Online_Flight_Reservation_System.Models
{
    public class Register
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "UserID Required!")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only Numbers Are Allowed In UserId")]
        [MaxLength(9, ErrorMessage = "UserId Should Not Be More Than 9 Characters Long")]
        public string UserId { get; set; }



        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "FirstName Required!")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only Letters Possible In FirstName")]
        [MaxLength(15, ErrorMessage = "FirstName Should Not Be More Than 15 Characters Long")]
        public string FirstName { get; set; }



        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "LastName Required!")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only Letters Possible In LastName")]
        [MaxLength(15, ErrorMessage = "LastName Should Not Be More Than 15 Characters Long")]
        public string LastName { get; set; }


        [Display(Name = "Date Of Birth")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date Of Birth Required!")]
        [DataType(DataType.Date, ErrorMessage = "Date Of Birth Should Be In Format YYYY-MM-DD")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [MaxLength(10, ErrorMessage = "Date Of Birth Should Be In Format YYYY-MM-DD")]
        public string DOB { get; set; }




        [Required(AllowEmptyStrings = false, ErrorMessage = "Age Required!")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only Numbers Are Allowed In Age")]
        [Range(12, 200, ErrorMessage = "Age Should Not Be Between 12 And 200")]
        //optional
        [MaxLength(3, ErrorMessage = "Age Should Not Be Between 12 And 200")]
        [DataType(DataType.PhoneNumber)]
        public string Age { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Gender Required!")]
        //[MaxLength(7, ErrorMessage = "Gender Should Be Male/Female/Other")]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only Letters Possible In Gender")]
        public string Gender { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Address Required!")]
        [MaxLength(30, ErrorMessage = "Address Should Be Of 30 Characters Only!")]
        public string Address { get; set; }



        [Display(Name = "Phone Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Phone Number Required!")]
        //[RegularExpression(@"^(\d{10})$", ErrorMessage = "Phone Number Should Be Of 10 Digits")]
        //[MaxLength(10, ErrorMessage = "PhoneNumber Should Be Of 10 Characters")]
        //[MinLength(10, ErrorMessage = "PhoneNumber Should Be Of 10 Characters")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Phone Number Should Be Of 10 Digits")]
        public string PhoneNo { get; set; }



        [Required(AllowEmptyStrings = false, ErrorMessage = "Password Required!")]
        [DataType(DataType.Password)]
        [MaxLength(9, ErrorMessage = "Password Should Not Be More Than 9 Characters Long")]
        public string Password { get; set; }



        [Compare("Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password Required!")]
        [DataType(DataType.Password)]
        [MaxLength(9, ErrorMessage = "Password Should Not Be More Than 9 Characters Long")]
        public string RepeatPassword { get; set; }

    }
}