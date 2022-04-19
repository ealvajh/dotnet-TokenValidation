using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TokenValidation.Models;

namespace TokenValidation.Controllers.api
{
    public class StudentController : ValidationController
    {

        [HttpPost]
        public IHttpActionResult List(SecurityModel model)
        {
            if (!VerifyModel(model))
                return BadRequest("Invalid data.");

            if (!VerifyToken(model.Token))
                return BadRequest("There is a security problem. Please log in again.");

            var students = new List<StudentModel>();

            try
            {
                using (var db = new SchoolEntities())
                {
                    students = db.Students.Where(s => s.StatusId == 1)
                        .Select(x => new StudentModel
                        {
                            FirstName = x.FirstName,
                            LastName = x.LastName
                        }).ToList();

                    if (students.Any())
                        return Ok(students);
                }
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest("Please, contact the administrador.");
            }
        }

        [HttpPost]
        public IHttpActionResult Add(StudentModel student)
        {
            if (!VerifyModel(student))
                return BadRequest("Invalid data.");

            if (!VerifyToken(student.Token))
                return BadRequest("There is a security problem. Please log in again.");

            try
            {
                using (var db = new SchoolEntities())
                {
                    db.Students.Add(new Student()
                    {
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        StandardId = 1
                    });

                    db.SaveChanges();
                }
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Please, contact the administrador.");
            }
        }

        [HttpPut]
        public IHttpActionResult Edit(StudentModel student)
        {
            if (!VerifyModel(student))
                return BadRequest("Invalid data.");

            if (!VerifyToken(student.Token))
                return BadRequest("There is a security problem. Please log in again.");

            try
            {
                using (var db = new SchoolEntities())
                {
                    var studentDb = db.Students.Find(student.StudentId);

                    studentDb.FirstName = student.FirstName;
                    studentDb.LastName = student.LastName;

                    db.Entry(studentDb).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Please, contact the administrador.");
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete(StudentModel student)
        {
            if (!VerifyModel(student))
                return BadRequest("Invalid data.");

            if (!VerifyToken(student.Token))
                return BadRequest("There is a security problem. Please log in again.");

            try
            {
                using (var db = new SchoolEntities())
                {
                    var studentDb = db.Students.Find(student.StudentId);

                    studentDb.StatusId = 0;

                    db.Entry(studentDb).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Please, contact the administrador.");
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Upload([FromUri] ProfileModel model)
        {
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            if (!VerifyToken(model.Token))
                return BadRequest("There is a security problem. Please log in again.");

            if (!Request.Content.IsMimeMultipartContent())
                return BadRequest("There isn't a picture.");

            await Request.Content.ReadAsMultipartAsync(provider);

            FileInfo fileInforPicture = null;

            foreach (MultipartFileData fileData in provider.FileData)
            {
                if (fileData.Headers.ContentDisposition.Name.Replace("\\","").Replace("\"","").Equals("Picture"));
                    fileInforPicture = new FileInfo(fileData.LocalFileName);
            }

            if (fileInforPicture != null) 
            {
                using (FileStream fs = fileInforPicture.Open(FileMode.Open, FileAccess.Read))
                {
                    byte[] data = new byte[fileInforPicture.Length];
                    UTF8Encoding temp = new UTF8Encoding();
                    while (fs.Read(data, 0, data.Length) > 0) ;

                    try
                    {
                        using (var db = new SchoolEntities())
                        {
                            var student = db.Students.Find(model.StudentId);

                            student.Picture = data;
                            db.Entry(student).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            return Ok("Your picture was uploaded!");
                        }
                    }
                    catch (Exception e)
                    {

                        return BadRequest("There is a problem.");
                    }

                }
            }
            return Ok();
        }
    }
}
