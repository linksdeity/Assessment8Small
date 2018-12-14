using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Assessment8.Models
{
    public class DishModel
    {
        
        public string PersonName { get; set; }

        [Required]
        [RegularExpression(@"[0-9]{10}", ErrorMessage = "10 digit phone number, numbers only")]
        [StringLength(100, ErrorMessage = "50 character max")]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(@"[a-zA-Z\s]*", ErrorMessage = "Dish Name should contain letters and spaces only please")]
        public string DishName { get; set; }

        [Required]
        [RegularExpression(@"[a-zA-Z\s]*", ErrorMessage = "Dish Description should contain only letters and spaces only please")]
        [StringLength(100, ErrorMessage = "100 character max")]
        public string DishDescription { get; set; }


        public string Email { get; set; }

        [Required]
        public string FoodOption { get; set; }
    }
}