using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Online_Flight_Reservation_System.Models
{
    public class CreditCard
    {
        [Display(Name = "Flight Type")]
        public string CCID { get; set; }


        [Display(Name = "Flight Type")]
        public string UserID { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Credit Card Number Required!")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only Numbers Are Allowed In Credit Card Number")]
        [Display(Name = "Credit Card Number")]
        [MaxLength(20, ErrorMessage = "Credit Card Number Length Should Be Less Than 20")]
        public string CCNumber { get; set; }



        [Required(AllowEmptyStrings = false, ErrorMessage = "Valid From Date Required!")]
        [RegularExpression(@"\d{2}/\d{4}", ErrorMessage = "Valid From Date Should Be In Format MM/YYYY")]
        [Display(Name = "Valid From")]
        [MaxLength(7, ErrorMessage = "Valid From Date Should Be In Format MM/YYYY")]
        //[DataType(DataType.Date)]
        public string ValidFrom { get; set; }



        [Required(AllowEmptyStrings = false, ErrorMessage = "Valid To Date Required!")]
        [RegularExpression(@"\d{2}/\d{4}", ErrorMessage = "Valid To Date Should Be In Format MM/YYYY")]
        [Display(Name = "Valid To")]
        [MaxLength(7, ErrorMessage = "Valid To Date Should Be In Format MM/YYYY")]
        public string ValidTo { get; set; }



        [Required(AllowEmptyStrings = false, ErrorMessage = "Balance Amount Required!")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only Numbers Are Allowed In Balance Amount")]
        [Display(Name = "Balance Amount")]
        public string Balance { get; set; }



        public string TripPrice { get; set; }


        
        public string BID { get; set; }
    }
}