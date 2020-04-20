using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : CommonController
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Catalog()
        {
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


        public IActionResult ClassListings(string subject, string num)
        {
            System.Diagnostics.Debug.WriteLine(subject + num);
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            return View();
        }


        /*******Begin code to modify********/

        /// <summary>
        /// Returns a JSON array of the classes the given student is enrolled in.
        /// Each object in the array should have the following fields:
        /// "subject" - The subject abbreviation of the class (such as "CS")
        /// "number" - The course number (such as 5530)
        /// "name" - The course name
        /// "season" - The season part of the semester
        /// "year" - The year part of the semester
        /// "grade" - The grade earned in the class, or "--" if one hasn't been assigned
        /// </summary>
        /// <param name="uid">The uid of the student</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetMyClasses(string uid)
        {
            var query = from s in db.Students
                        where s.UId == uid
                        join en in db.EnrollmentGrade on s.UId equals en.UId into join1

                        from j1 in join1
                        join klasse in db.Classes on j1.ClassId equals klasse.ClassId into join2

                        from j2 in join2
                        join c in db.Courses on j2.CourseId equals c.CourseId into join3

                        from j3 in join3
                        select new
                        {
                            subject = j3.DeptAbbreviation,
                            number = j3.CourseNumber,
                            name = j3.CourseName,
                            season = j2.Season,
                            year = j2.Year,
                            grade = j1.Grade ?? "--"
                        };

            return Json(query.ToArray());
        }

        /// <summary>
        /// Returns a JSON array of all the assignments in the given class that the given student is enrolled in.
        /// Each object in the array should have the following fields:
        /// "aname" - The assignment name
        /// "cname" - The category name that the assignment belongs to
        /// "due" - The due Date/Time
        /// "score" - The score earned by the student, or null if the student has not submitted to this assignment.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="uid"></param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentsInClass(string subject, int num, string season, int year, string uid)
        {
            var query = from c in db.Courses
                        where c.DeptAbbreviation == subject && c.CourseNumber == num
                        join klasse in db.Classes on c.CourseId equals klasse.CourseId into join1

                        from j1 in join1
                        where j1.Season == season && j1.Year == year
                        join ac in db.AssignmentCategories on j1.ClassId equals ac.ClassId into join2

                        from j2 in join2
                        join assign in db.Assignments on j2.CategoryId equals assign.CategoryId into join3

                        from j3 in join3
                        select new
                        {
                            aname = j3.AsgmtName,
                            cname = j2.CategoryName,
                            due = j3.DueDate,
                            score = (from s in db.Submission
                                     where s.AssignmentId == j3.AssignmentId && s.UId == uid
                                     select s.Score).DefaultIfEmpty()
                                     
                        };
            var quer = query.ToList();
            return Json(query.ToArray());
        }

        /// <summary>
        /// Adds a submission to the given assignment for the given student
        /// The submission should use the current time as its DateTime
        /// You can get the current time with DateTime.Now
        /// The score of the submission should start as 0 until a Professor grades it
        /// If a Student submits to an assignment again, it should replace the submission contents
        /// and the submission time (the score should remain the same).
        /// Does *not* automatically reject late submissions.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The new assignment name</param>
        /// <param name="uid">The student submitting the assignment</param>
        /// <param name="contents">The text contents of the student's submission</param>
        /// <returns>A JSON object containing {success = true/false}.</returns>
        public IActionResult SubmitAssignmentText(string subject, int num, string season, int year,
          string category, string asgname, string uid, string contents)
        {
            var query = from c in db.Courses
                        where c.DeptAbbreviation == subject && c.CourseNumber == num
                        join klasse in db.Classes on c.CourseId equals klasse.CourseId into join1

                        from j1 in join1
                        where j1.Year == year && j1.Season == season
                        join asgn in db.AssignmentCategories on j1.ClassId equals asgn.ClassId into join2

                        from j2 in join2
                        where j2.CategoryName == category
                        join assign in db.Assignments on j2.CategoryId equals assign.CategoryId into join3

                        from j3 in join3
                        where j3.AsgmtName == asgname
                        select new
                        {
                            assignID = j3.AssignmentId,
                        };

            if (query.ToList().Count == 0)
            {
                return Json(new { success = false });
            }

            var subquery = from sub in db.Submission
                           where sub.UId == uid && sub.AssignmentId == query.First().assignID
                           select sub;
            if (subquery.ToList().Count == 0)
            {
                Submission sub = new Submission()
                {
                    AssignmentId = query.First().assignID,
                    UId = uid,
                    SubTime = DateTime.Now,
                    Score = 0,
                    Content = contents
                };
                db.Submission.Add(sub);

                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    return Json(new { success = false });
                }

            }
            else
            {
                Submission sub = subquery.First();
                sub.Content = contents;
                sub.SubTime = DateTime.Now;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    return Json(new { success = false });
                }
            }


            return Json(new { success = true });
        }

        /// <summary>
        /// Enrolls a student in a class.
        /// </summary>
        /// <param name="subject">The department subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester</param>
        /// <param name="year">The year part of the semester</param>
        /// <param name="uid">The uid of the student</param>
        /// <returns>A JSON object containing {success = {true/false},
        /// false if the student is already enrolled in the Class.</returns>
        public IActionResult Enroll(string subject, int num, string season, int year, string uid)
        {
            var query = from c in db.Courses
                        where c.DeptAbbreviation == subject && c.CourseNumber == num
                        join klasse in db.Classes on c.CourseId equals klasse.CourseId into join1

                        from j1 in join1
                        where j1.Year == year && j1.Season == season
                        select j1;

            EnrollmentGrade grade = new EnrollmentGrade()
            {
                ClassId = query.First().ClassId,
                UId = uid,
                Grade = "--"
            };

            try
            {
                db.EnrollmentGrade.Add(grade);
                db.SaveChanges();
            }
            catch(Exception e)
            {
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }



        /// <summary>
        /// Calculates a student's GPA
        /// A student's GPA is determined by the grade-point representation of the average grade in all their classes.
        /// Assume all classes are 4 credit hours.
        /// If a student does not have a grade in a class ("--"), that class is not counted in the average.
        /// If a student does not have any grades, they have a GPA of 0.0.
        /// Otherwise, the point-value of a letter grade is determined by the table on this page:
        /// https://advising.utah.edu/academic-standards/gpa-calculator-new.php
        /// </summary>
        /// <param name="uid">The uid of the student</param>
        /// <returns>A JSON object containing a single field called "gpa" with the number value</returns>
        public IActionResult GetGPA(string uid)
        {
            var query = from sub in db.EnrollmentGrade
                        where sub.UId == uid
                        select sub;
            if(query.ToList().Count == 0)
            {
                return Json(new { gpa = 0.0 });
            }
            double numCredits = 0.0;
            double gradePoints = 0.0;
            foreach(var klasse in query.ToList())
            {
                string grade = klasse.Grade;
                if (grade.Equals("--")){
                    continue;
                }
                else if (grade.Equals("A")){
                    gradePoints += 4* 4.0;
                }
                else if (grade.Equals("A-"))
                {
                    gradePoints += 4 * 3.7;
                }
                else if (grade.Equals("B+"))
                {
                    gradePoints += 4 * 3.3;
                }
                else if (grade.Equals("B"))
                {
                    gradePoints += 4 * 3.0;
                }
                else if (grade.Equals("B-"))
                {
                    gradePoints += 4 * 2.7;
                }
                else if (grade.Equals("C+"))
                { 
                    gradePoints += 4 * 2.3;
                }
                else if (grade.Equals("C"))
                {
                    gradePoints += 4 * 2.0;
                }
                else if (grade.Equals("C-"))
                {
                    gradePoints += 4 * 1.7;
                }
                else if (grade.Equals("D+")){
                    gradePoints += 4 * 1.3;
                }
                else if (grade.Equals("D")){
                    gradePoints += 4 * 1.0;
                }
                else if (grade.Equals("D-")){
                    gradePoints += 4 * 0.7;
                }
                else if (grade.Equals("E")){
                    gradePoints += 0.0;
                }
                numCredits += 4;
            }
            return Json(new {gpa = gradePoints/numCredits });
        }

        /*******End code to modify********/

    }
}