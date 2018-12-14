using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assessment8.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        [StringLength(100, ErrorMessage = "100 character max")]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 50 characters")]
        public string Password { get; set; }
    }
}