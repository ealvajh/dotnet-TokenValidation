using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TokenValidation.Models;

namespace TokenValidation.Controllers.api
{
    public class UserController : ApiController
    {
        /*
                public IHttpActionResult GetUsers()
                {
                    IList<LoginModel> users = null;

                    using(var db = new SchoolEntities())
                    {
                           users = db.Logins.Select(x =>  new LoginModel()
                            {
                                UserId = x.UserId,
                                Email = x.Email,
                                Password = x.Password,
                                Token = x.Token
                            }).ToList();
                    }

                    if(users.Count == 0)
                        return NotFound();

                    return Ok(users);
                }
        */
        public IHttpActionResult GetLogin(string email)
        {
            try
            {
                Login existingUser = null;

                using (var db = new SchoolEntities())
                {
                    existingUser = db.Logins.Where(s => s.Email == email).FirstOrDefault();

                    if (existingUser != null)
                    {
                        existingUser.Token = Guid.NewGuid().ToString();
                        db.SaveChanges();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                return Ok(existingUser.Token);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
