using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TokenValidation.Models
{
    public class LoginModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}