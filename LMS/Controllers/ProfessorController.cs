using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS.Controllers
{
    [Authorize(Roles = "Professor")]
    public class ProfessorController : CommonController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Students(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult Class(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult Categories(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult CatAssignments(string subject, string num, string season, string year, string cat)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            return View();
        }

        public IActionResult Assignment(string subject, string num, string season, string year, string cat, string aname)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            return View();
        }

        public IActionResult Submissions(string subject, string num, string season, string year, string cat, string aname)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            return View();
        }

        public IActionResult Grade(string subject, string num, string season, string year, string cat, string aname, string uid)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            ViewData["uid"] = uid;
            return View();
        }

        /*******Begin code to modify********/


        /// <summary>
        /// Returns a JSON array of all the students in a class.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "dob" - date of birth
        /// "grade" - the student's grade in this class
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetStudentsInClass(string subject, int num, string season, int year)
        {
            var query = from c in db.Courses
                        where c.DeptAbbreviation == subject && c.CourseNumber == num
                        join klasse in db.Classes on c.CourseId equals klasse.CourseId into join1

                        from j1 in join1
                        where j1.Season == season && j1.Year == year
                        join e in db.EnrollmentGrade on j1.ClassId equals e.ClassId into join2

                        from j2 in join2
                        join s in db.Students on j2.UId equals s.UId into stud

                        from st in stud
                        select new
                        {
                            fname = st.FirstName,
                            lname = st.LastName,
                            uid = st.UId,
                            dob = st.Dob,
                            grade = st.EnrollmentGrade
                        };

            return Json(query.ToArray());
        }



        /// <summary>
        /// Returns a JSON array with all the assignments in an assignment category for a class.
        /// If the "category" parameter is null, return all assignments in the class.
        /// Each object in the array should have the following fields:
        /// "aname" - The assignment name
        /// "cname" - The assignment category name.
        /// "due" - The due DateTime
        /// "submissions" - The number of submissions to the assignment
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class, 
        /// or null to return assignments from all categories</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentsInCategory(string subject, int num, string season, int year, string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                var nullquery = from c in db.Courses
                                where c.DeptAbbreviation == subject && c.CourseNumber == num
                                join klasse in db.Classes on c.CourseId equals klasse.CourseId into join1

                                from j1 in join1
                                where j1.Season == season && j1.Year == year
                                join ac in db.AssignmentCategories on j1.ClassId equals ac.ClassId into join2

                                from j2 in join2
                                join assign in db.Assignments on j2.CategoryId equals assign.CategoryId into foo

                                from thing in foo
                                join sub in db.Submission on thing.AssignmentId equals sub.AssignmentId into bar
                                select new
                                {
                                    aname = thing.AsgmtName,
                                    cname = j2.CategoryName,
                                    due = thing.DueDate,
                                    submissions = bar.Count()
                                };

                return Json(nullquery.ToArray());
            }

            var query = from c in db.Courses
                        where c.DeptAbbreviation == subject && c.CourseNumber == num
                        join klasse in db.Classes on c.CourseId equals klasse.CourseId into join1

                        from j1 in join1
                        where j1.Season == season && j1.Year == year
                        join ac in db.AssignmentCategories on j1.ClassId equals ac.ClassId into join2

                        from j2 in join2
                        where j2.CategoryName == category
                        join assign in db.Assignments on j2.CategoryId equals assign.CategoryId into foo

                        from thing in foo
                        join sub in db.Submission on thing.AssignmentId equals sub.AssignmentId into bar
                        select new
                        {
                            aname = thing.AsgmtName,
                            cname = j2.CategoryName,
                            due = thing.DueDate,
                            submissions = bar.Count()
                        };
            return Json(query.ToArray());
        }


        /// <summary>
        /// Returns a JSON array of the assignment categories for a certain class.
        /// Each object in the array should have the folling fields:
        /// "name" - The category name
        /// "weight" - The category weight
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentCategories(string subject, int num, string season, int year)
        {
            var query = from c in db.Courses
                            where c.DeptAbbreviation == subject && c.CourseNumber == num
                            join klasse in db.Classes on c.CourseId equals klasse.CourseId into join1

                            from j1 in join1
                            where j1.Season == season && j1.Year == year
                            join ac in db.AssignmentCategories on j1.ClassId equals ac.ClassId into join2

                            from j2 in join2
                            select new
                            {
                                name = j2.CategoryName,
                                weight = j2.Weight
                            };
            return Json(query.ToArray());
        }

        /// <summary>
        /// Creates a new assignment category for the specified class.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The new category name</param>
        /// <param name="catweight">The new category weight</param>
        /// <returns>A JSON object containing {success = true/false},
        ///	false if an assignment category with the same name already exists in the same class.</returns>
        public IActionResult CreateAssignmentCategory(string subject, int num, string season, int year, string category, int catweight)
        {
            var query = from c in db.Courses
                        where c.DeptAbbreviation == subject && c.CourseNumber == num
                        join klasse in db.Classes on c.CourseId equals klasse.CourseId into join1

                        from j1 in join1
                        where j1.Season == season && j1.Year == year
                        join ac in db.AssignmentCategories on j1.ClassId equals ac.ClassId into join2

                        from j2 in join2
                        where j2.CategoryName == category
                        select j2;


            if(query.ToList().Count != 0)
            {
                return Json(new { success = false });
            }

            var classQuery = from c in db.Courses
                             where c.DeptAbbreviation == subject && c.CourseNumber == num
                             join klasse in db.Classes on c.CourseId equals klasse.CourseId into join1

                             from j1 in join1
                             where j1.Season == season && j1.Year == year
                             select new
                             {
                                 classID = j1.ClassId,
                             };

            AssignmentCategories cat = new AssignmentCategories()
            {
                CategoryName = category,
                Weight = (uint)catweight,
                ClassId = classQuery.First().classID
            };

            db.AssignmentCategories.Add(cat);

            try
            {
                db.SaveChanges();
            }
            catch(Exception e)
            {

                return Json(new { success = false });
            }
            return Json(new { success = true });
        }

        /// <summary>
        /// Creates a new assignment for the given class and category.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The new assignment name</param>
        /// <param name="asgpoints">The max point value for the new assignment</param>
        /// <param name="asgdue">The due DateTime for the new assignment</param>
        /// <param name="asgcontents">The contents of the new assignment</param>
        /// <returns>A JSON object containing success = true/false,
        /// false if an assignment with the same name already exists in the same assignment category.</returns>
        public IActionResult CreateAssignment(string subject, int num, string season, int year, string category, string asgname, int asgpoints, DateTime asgdue, string asgcontents)
        {
            var query = from c in db.Courses
                        where c.DeptAbbreviation == subject && c.CourseNumber == num
                        join klasse in db.Classes on c.CourseId equals klasse.CourseId into join1

                        from j1 in join1
                        where j1.Season == season && j1.Year == year
                        join ac in db.AssignmentCategories on j1.ClassId equals ac.ClassId into join2

                        from j2 in join2
                        where j2.CategoryName == category
                        join assign in db.Assignments on j2.CategoryId equals assign.CategoryId into join3

                        from j3 in join3
                        where j3.AsgmtName == asgname
                        select j3;

            if (query.ToList().Count != 0)
            {
                return Json(new { success = false });
            }

            var AssignmentQuery = from c in db.Courses
                                  where c.DeptAbbreviation == subject && c.CourseNumber == num
                                  join klasse in db.Classes on c.CourseId equals klasse.CourseId into join1

                                  from j1 in join1
                                  where j1.Season == season && j1.Year == year
                                  join ac in db.AssignmentCategories on j1.ClassId equals ac.ClassId into join2

                                  from j2 in join2

                                  select new
                                  {
                                      CategoryID = j2.CategoryId,
                                  };

            Assignments Asgmt = new Assignments()
            {
                AsgmtName = asgname,
                MaxPointValue = (uint)asgpoints,
                CategoryId = AssignmentQuery.First().CategoryID,
                Contents = asgcontents,
                DueDate = asgdue
            };

            db.Assignments.Add(Asgmt);

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }


        /// <summary>
        /// Gets a JSON array of all the submissions to a certain assignment.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "time" - DateTime of the submission
        /// "score" - The score given to the submission
        /// 
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetSubmissionsToAssignment(string subject, int num, string season, int year, string category, string asgname)
        {
            var query = from c in db.Courses
                        where c.DeptAbbreviation == subject && c.CourseNumber == num
                        join klasse in db.Classes on c.CourseId equals klasse.CourseId into join1

                        from j1 in join1
                        where j1.Season == season && j1.Year == year
                        join ascat in db.AssignmentCategories on j1.ClassId equals ascat.ClassId into join2

                        from j2 in join2
                        where j2.CategoryName == category
                        join asgn in db.Assignments on j2.CategoryId equals asgn.CategoryId into join3

                        from j3 in join3
                        where j3.AsgmtName == asgname
                        join sub in db.Submission on j3.AssignmentId equals sub.AssignmentId into join4

                        from j4 in join4
                        join stud in db.Students on j4.UId equals stud.UId into join5

                        from j5 in join5
                        select new
                        {
                            fname = j5.FirstName,
                            lanme = j5.LastName,
                            uid = j5.UId,
                            time = j4.SubTime,
                            score = j4.Score
                        };
            return Json(query.ToArray());
        }


        /// <summary>
        /// Set the score of an assignment submission
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <param name="uid">The uid of the student who's submission is being graded</param>
        /// <param name="score">The new score for the submission</param>
        /// <returns>A JSON object containing success = true/false</returns>
        public IActionResult GradeSubmission(string subject, int num, string season, int year, string category, string asgname, string uid, int score)
        {
            var query = from c in db.Courses
                        where c.DeptAbbreviation == subject && c.CourseNumber == num
                        join klasse in db.Classes on c.CourseId equals klasse.CourseId into join1

                        from j1 in join1
                        where j1.Season == season && j1.Year == year
                        join ac in db.AssignmentCategories on j1.ClassId equals ac.ClassId into join2

                        from j2 in join2
                        where j2.CategoryName == category
                        join assign in db.Assignments on j2.CategoryId equals assign.CategoryId into join3

                        from j3 in join3
                        where j3.AsgmtName == asgname
                        join sub in db.Submission on j3.AssignmentId equals sub.AssignmentId into join4

                        from j4 in join4
                        where j4.UId == uid
                        select j4;

            foreach(var sub in query.ToList())
            {
                sub.Score = (uint)score;
            }

            try
            {
                db.SaveChanges();
            }
            catch(Exception e)
            {
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }


        /// <summary>
        /// Returns a JSON array of the classes taught by the specified professor
        /// Each object in the array should have the following fields:
        /// "subject" - The subject abbreviation of the class (such as "CS")
        /// "number" - The course number (such as 5530)
        /// "name" - The course name
        /// "season" - The season part of the semester in which the class is taught
        /// "year" - The year part of the semester in which the class is taught
        /// </summary>
        /// <param name="uid">The professor's uid</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetMyClasses(string uid)
        {
            var query = from c in db.Classes
                        where c.Taught == uid
                        join course in db.Courses on c.CourseId equals course.CourseId into join1

                        from j1 in join1
                        select new
                        {
                            subject = j1.DeptAbbreviation,
                            number = j1.CourseNumber,
                            name = j1.CourseName,
                            season = c.Season,
                            year = c.Year
                        };
            return Json(query.ToArray());
        }


        /*******End code to modify********/

    }
}