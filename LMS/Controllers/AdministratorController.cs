using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : CommonController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Department(string subject)
        {
            ViewData["subject"] = subject;
            return View();
        }

        public IActionResult Course(string subject, string num)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            return View();
        }

        /*******Begin code to modify********/

        /// <summary>
        /// Returns a JSON array of all the courses in the given department.
        /// Each object in the array should have the following fields:
        /// "number" - The course number (as in 5530)
        /// "name" - The course name (as in "Database Systems")
        /// </summary>
        /// <param name="subject">The department subject abbreviation (as in "CS")</param>
        /// <returns>The JSON result</returns>
        public IActionResult GetCourses(string subject)
        {
            var query = from c in db.Courses
                        where c.DeptAbbreviation == subject
                        select new
                        {
                            number = c.CourseNumber,
                            name = c.CourseName
                        };
            return Json(query.ToArray());
        }

        /// <summary>
        /// Returns a JSON array of all the professors working in a given department.
        /// Each object in the array should have the following fields:
        /// "lname" - The professor's last name
        /// "fname" - The professor's first name
        /// "uid" - The professor's uid
        /// </summary>
        /// <param name="subject">The department subject abbreviation</param>
        /// <returns>The JSON result</returns>
        public IActionResult GetProfessors(string subject)
        {
            var query = from p in db.Professors
                        where p.WorksIn == subject
                        select new
                        {
                            lname = p.LastName,
                            fname = p.FirstName,
                            uid = p.UId
                        };
            return Json(query.ToArray());
        }

        /// <summary>
        /// Creates a course.
        /// A course is uniquely identified by its number + the subject to which it belongs
        /// </summary>
        /// <param name="subject">The subject abbreviation for the department in which the course will be added</param>
        /// <param name="number">The course number</param>
        /// <param name="name">The course name</param>
        /// <returns>A JSON object containing {success = true/false},
        /// false if the Course already exists.</returns>
        public IActionResult CreateCourse(string subject, int number, string name)
        {
            // get the course and its course number
            var query = from c in db.Courses
                        where c.DeptAbbreviation == subject && c.CourseNumber == number
                        select c;

            // CHECK 1: THE COURSE ALREADY EXISTS
            if (query.ToList().Count != 0)
            {
                return Json(new { success = false });
            }

            Courses course = new Courses()
            {
                DeptAbbreviation = subject,
                CourseNumber = (uint)number,
                CourseName = name
            };
            try
            {
                db.Courses.Add(course);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }

        /// <summary>
        /// Creates a class offering of a given course.
        /// </summary>
        /// <param name="subject">The department subject abbreviation</param>
        /// <param name="number">The course number</param>
        /// <param name="season">The season part of the semester</param>
        /// <param name="year">The year part of the semester</param>
        /// <param name="start">The start time</param>
        /// <param name="end">The end time</param>
        /// <param name="location">The location</param>
        /// <param name="instructor">The uid of the professor</param>
        /// <returns>A JSON object containing {success = true/false}. 
        /// false if another class occupies the same location during any time 
        /// within the start-end range in the same semester, or if there is already
        /// a Class offering of the same Course in the same Semester.</returns>
        public IActionResult CreateClass(string subject, int number, string season, int year, DateTime start, DateTime end, string location, string instructor)
        {
            var query = from c in db.Courses
                        where c.DeptAbbreviation == subject && c.CourseNumber == number
                        join c1 in db.Classes
                        on c.CourseId equals c1.CourseId into join1
                        from j1 in join1
                        where j1.Season == season && j1.Year == year
                        select j1;

            // CHECK 1: CHECK IF THE CLASS ALREADY EXISTS
            if (query.ToList().Count != 0)
                return Json(new { success = false });

            // CHECK 2: CHECK IF ANOTHER CLASS OCCUPIES THE SAME LOCATION WITHIN THE START END RANGE
            var query2 = from classe in db.Classes
                         where classe.Location == location && classe.Season == season
                         select classe;

            foreach (Classes c in query2)
            {
                // CHECK 2: CHECK IF A CLASS STARTS DURING ANOTHER CLASS IN THE SAME LOCATION
                if (c.StartTime < start && start < end)
                    return Json(new { success = false });

                // CHECK 3: CHECK IF A CLASS ENDS DURING ANOTHER CLASS IN THE SAME LOCATIN
                if (c.StartTime < end && end < c.EndTime)
                    return Json(new { success = false });
            }

            // Get the course ID for the class
            var query3 = from c in db.Courses
                         where c.DeptAbbreviation == subject && c.CourseNumber == number
                         select c.CourseId;

            // Create and add new classes to the database
            Classes klasse = new Classes()
            {
                CourseId = (uint)query3.First(),
                Year = (uint)year,
                Season = season,
                Location = location,
                StartTime = start,
                EndTime = end,
                Taught = instructor
            };

            try
            {
                db.Classes.Add(klasse);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });

            /*******End code to modify********/

        }
    }
}