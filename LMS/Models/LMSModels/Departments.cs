using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Departments
    {
        public Departments()
        {
            Courses = new HashSet<Courses>();
            Professors = new HashSet<Professors>();
            Students = new HashSet<Students>();
        }

        public string DeptName { get; set; }
        public string DeptAbbreviation { get; set; }

        public virtual ICollection<Courses> Courses { get; set; }
        public virtual ICollection<Professors> Professors { get; set; }
        public virtual ICollection<Students> Students { get; set; }
    }
}
