using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_Day_6.Models
{
    public class LoginModel
    {
        [Required]
        [RegularExpression(@"[a-zA-Z0-9_]*@[A-Za-z]+.[a-zA-Z]{2,4}")]
        public string Email { set; get; }
        [Required]
        public string Password { set; get; }
    }
}