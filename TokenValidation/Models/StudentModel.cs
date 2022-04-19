using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TokenValidation.Models
{
    public class StudentModel : SecurityModel
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}