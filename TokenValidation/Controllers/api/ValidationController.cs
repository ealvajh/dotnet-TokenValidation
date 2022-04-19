using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TokenValidation.Models;

namespace TokenValidation.Controllers.api
{
    public class ValidationController : ApiController
    {
 
        public bool VerifyToken(string token)
        {

            try
            {
                if (string.IsNullOrEmpty(token))
                    return false;

                using (var db = new SchoolEntities())
                {
                   var user = db.Logins.Where(s => s.Token == token).FirstOrDefault();

                    if(user != null)
                        return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool VerifyModel(Object model)
        {

            if (model == null)
                return false;

            return true;
        }
    }
}
