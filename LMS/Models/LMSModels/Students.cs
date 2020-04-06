using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Students
    {
        public Students()
        {
            EnrollmentGrade = new HashSet<EnrollmentGrade>();
            Submission = new HashSet<Submission>();
        }

        public string UId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public string Major { get; set; }

        public virtual Departments MajorNavigation { get; set; }
        public virtual ICollection<EnrollmentGrade> EnrollmentGrade { get; set; }
        public virtual ICollection<Submission> Submission { get; set; }
    }
}
