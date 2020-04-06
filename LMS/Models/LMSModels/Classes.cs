using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Classes
    {
        public Classes()
        {
            AssignmentCategories = new HashSet<AssignmentCategories>();
            EnrollmentGrade = new HashSet<EnrollmentGrade>();
        }

        public uint ClassId { get; set; }
        public uint CourseId { get; set; }
        public uint Year { get; set; }
        public string Season { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Taught { get; set; }

        public virtual Courses Course { get; set; }
        public virtual Professors TaughtNavigation { get; set; }
        public virtual ICollection<AssignmentCategories> AssignmentCategories { get; set; }
        public virtual ICollection<EnrollmentGrade> EnrollmentGrade { get; set; }
    }
}
