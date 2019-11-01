using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace buysharps.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter shop name.")]
        [Display(Name = "Shop name", Description = "Just the shop name part of shopname.myshopify.com")]
        public string ShopName { get; set; }
    }
}