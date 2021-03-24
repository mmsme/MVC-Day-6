using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_Day_6.Models
{
    public class RegisterModel
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { set; get; }
        [Range(18, 40)]
        public int Age { set; get; }

        [Required, RegularExpression(@"[a-zA-Z0-9_]*@[A-Za-z]+.[a-zA-Z]{2,4}")]
        public string Email { set; get; }
        [Required]
        public string Password { set; get; }
        [Compare("Password")]
        public string CPassword { set; get; }
    }
}