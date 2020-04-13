using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LMS.Models.LMSModels;
using Microsoft.EntityFrameworkCore;

namespace LMS.Controllers
{
    public class CommonController : Controller
    {

        /*******Begin code to modify********/

        // TODO: Uncomment and change 'X' after you have scaffoled


        protected Team69LMSContext db;

        public CommonController()
        {
            db = new Team69LMSContext();
        }


        /*
         * WARNING: This is the quick and easy way to make the controller
         *          use a different LibraryContext - good enough for our purposes.
         *          The "right" way is through Dependency Injection via the constructor 
         *          (look this up if interested).
        */

        // TODO: Uncomment and change 'X' after you have scaffoled

        public void UseLMSContext(Team69LMSContext ctx)
        {
            db = ctx;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        /// <summary>
        /// Retreive a JSON array of all departments from the database.
        /// Each object in the array should have a field called "name" and "subject",
        /// where "name" is the department name and "subject" is the subject abbreviation.
        /// </summary>
        /// <returns>The JSON array</returns>
        public IActionResult GetDepartments()
        {
            // TODO: Do not return this hard-coded array.

            var query = from d in db.Departments
                        select new
                        {
                            name = d.DeptName,
                            subject = d.DeptAbbreviation

                        };
            return Json(query.ToArray());
        }



        /// <summary>
        /// Returns a JSON array representing the course catalog.
        /// Each object in the array should have the following fields:
        /// "subject": The subject abbreviation, (e.g. "CS")
        /// "dname": The department name, as in "Computer Science"
        /// "courses": An array of JSON objects representing the courses in the department.
        ///            Each field in this inner-array should have the following fields:
        ///            "number": The course number (e.g. 5530)
        ///            "cname": The course name (e.g. "Database Systems")
        /// </summary>
        /// <returns>The JSON array</returns>
        public IActionResult GetCatalog()
        {
            var query = from d in db.Departments
                        select d;
            List<JsonResult> obj = new List<JsonResult>();
            foreach(var dept in query.ToList())
            {
                var courseQuery = from c in db.Courses
                                  where c.DeptAbbreviation == dept.DeptAbbreviation
                                  select new
                                  {
                                      subject = dept.DeptAbbreviation,
                                      dname = dept.DeptName,
                                      courses = c
                                  };
                obj.Add(Json(courseQuery.ToArray()));
            }
            //this may not work
            return Json(obj.ToArray());
        }

        /// <summary>
        /// Returns a JSON array of all class offerings of a specific course.
        /// Each object in the array should have the following fields:
        /// "season": the season part of the semester, such as "Fall"
        /// "year": the year part of the semester
        /// "location": the location of the class
        /// "start": the start time in format "hh:mm:ss"
        /// "end": the end time in format "hh:mm:ss"
        /// "fname": the first name of the professor
        /// "lname": the last name of the professor
        /// </summary>
        /// <param name="subject">The subject abbreviation, as in "CS"</param>
        /// <param name="number">The course number, as in 5530</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetClassOfferings(string subject, int number)
        {

            var query = from c in db.Courses
                        where c.DeptAbbreviation == subject && c.CourseNumber == number
                        join cl in db.Classes
                        on c.CourseId equals cl.CourseId into classes

                        from j1 in classes
                        join p in db.Professors
                        on j1.Taught equals p.UId
                        select new
                        {
                            season = j1.Season,
                            year = j1.Year,
                            location = j1.Location,
                            start = j1.StartTime,
                            end = j1.EndTime,
                            fname = p.FirstName,
                            lname = p.LastName
                        };

            return Json(query.ToArray());
        }

        /// <summary>
        /// This method does NOT return JSON. It returns plain text (containing html).
        /// Use "return Content(...)" to return plain text.
        /// Returns the contents of an assignment.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment in the category</param>
        /// <returns>The assignment contents</returns>
        public IActionResult GetAssignmentContents(string subject, int num, string season, int year, string category, string asgname)
        {
            var query = from c in db.Courses
                        where c.CourseName == subject && c.CourseNumber == num
                        join cl in db.Classes on c.CourseId equals cl.CourseId into classes

                        from j1 in classes
                        where j1.Season == season && j1.Year == year
                        join ac in db.AssignmentCategories on j1.ClassId equals ac.ClassId into AssingCat

                        from j2 in AssingCat
                        where j2.CategoryName == category
                        join ass in db.Assignments on j2.CategoryId equals ass.CategoryId into Assign

                        from j3 in Assign
                        where j3.AsgmtName == asgname
                        select j3.Contents;

            return Content(query.ToString());
        }


        /// <summary>
        /// This method does NOT return JSON. It returns plain text (containing html).
        /// Use "return Content(...)" to return plain text.
        /// Returns the contents of an assignment submission.
        /// Returns the empty string ("") if there is no submission.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment in the category</param>
        /// <param name="uid">The uid of the student who submitted it</param>
        /// <returns>The submission text</returns>
        public IActionResult GetSubmissionText(string subject, int num, string season, int year, string category, string asgname, string uid)
        {
            var query = from c in db.Courses
                        where c.CourseName == subject && c.CourseNumber == num
                        join cl in db.Classes on c.CourseId equals cl.CourseId into classes

                        from j1 in classes
                        where j1.Season == season && j1.Year == year
                        join ac in db.AssignmentCategories on j1.ClassId equals ac.ClassId into AssingCat

                        from j2 in AssingCat
                        where j2.CategoryName == category
                        join ass in db.Assignments on j2.CategoryId equals ass.CategoryId into Assign

                        from j3 in Assign
                        where j3.AsgmtName == asgname
                        join sub in db.Submission on j3.AssignmentId equals sub.AssignmentId into submission

                        from j4 in submission
                        where j4.UId == uid
                        select j4.Content;

            return Content(query.ToString());
        }


        /// <summary>
        /// Gets information about a user as a single JSON object.
        /// The object should have the following fields:
        /// "fname": the user's first name
        /// "lname": the user's last name
        /// "uid": the user's uid
        /// "department": (professors and students only) the name (such as "Computer Science") of the department for the user. 
        ///               If the user is a Professor, this is the department they work in.
        ///               If the user is a Student, this is the department they major in.    
        ///               If the user is an Administrator, this field is not present in the returned JSON
        /// </summary>
        /// <param name="uid">The ID of the user</param>
        /// <returns>
        /// The user JSON object 
        /// or an object containing {success: false} if the user doesn't exist
        /// </returns>
        public IActionResult GetUser(string uid)
        {
            var studQuery = from s in db.Students
                            where s.UId == uid
                            select s;

            if(studQuery.ToList().Count == 0)
            {
                //not a student
                var profQuery = from p in db.Professors
                                where p.UId == uid
                                select p;

                if(profQuery.ToList().Count == 0)
                {
                    //not a professor
                    var adminQuery = from a in db.Administrators
                                    where a.UId == uid
                                    select a;

                    if (adminQuery.ToList().Count == 0)
                    {
                        //not an administrator so user does not exist
                        return Json(new { success = false });
                    }
                    else
                    {
                        //is an administrator
                        var deptQuery = from a in db.Administrators
                                        where a.UId == uid
                                        select new
                                        {
                                            fname = a.FirstName,
                                            lname = a.LastName,
                                            uid = uid,
                                        };

                        return Json(deptQuery);
                    }
                }
                else
                {
                    //is a professor
                    var deptQuery = from p in db.Professors
                                    where p.UId == uid
                                    join d in db.Departments on p.WorksIn equals d.DeptAbbreviation
                                    select new
                                    {
                                        fname = p.FirstName,
                                        lname = p.LastName,
                                        uid = uid,
                                        department = d.DeptName
                                    };

                    return Json(deptQuery);
                }
            }
            else
            {
                //is a student
                var deptQuery = from s in db.Students
                                where s.UId == uid
                                join d in db.Departments on s.Major equals d.DeptAbbreviation
                                select new
                                {
                                    fname = s.FirstName,
                                    lname = s.LastName,
                                    uid = uid,
                                    department = d.DeptName
                                };
                return Json(deptQuery);
            }
            
        }

        /*******End code to modify********/

    }
}