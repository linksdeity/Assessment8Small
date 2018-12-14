using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Assessment8.Models
{
    public class RegisterModel
    {
        [Required]
        [RegularExpression(@"[A-Z][a-z]*", ErrorMessage = "First name must be title case and letters only please")]
        [StringLength(20, ErrorMessage = "Less than 20 characters in First Name please")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"[A-Z][a-z]*", ErrorMessage = "Last name must be title case and letters only please")]
        [StringLength(20, ErrorMessage = "Less than 20 characters in Last Name please")]
        public string LastName { get; set; }

        [Required]
        public DateTime AttendanceDate { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100, ErrorMessage = "100 character max")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"[a-zA-Z\s]*", ErrorMessage = "Guest should be 'none' or guest name with letters and spaces only")]
        [StringLength(50, ErrorMessage = "Guest name must be less than 50 characters")]
        public string Guest { get; set; }


        //-------------identity stuff

        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage ="Password must be between 6 and 50 characters")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The password does not match")]
        public string ConfirmPassword { get; set; }

    }
}