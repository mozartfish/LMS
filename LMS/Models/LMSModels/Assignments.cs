using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Assignments
    {
        public Assignments()
        {
            Submission = new HashSet<Submission>();
        }

        public uint AssignmentId { get; set; }
        public uint CategoryId { get; set; }
        public string AsgmtName { get; set; }
        public uint MaxPointValue { get; set; }
        public string Contents { get; set; }
        public DateTime DueDate { get; set; }

        public virtual AssignmentCategories Category { get; set; }
        public virtual ICollection<Submission> Submission { get; set; }
    }
}
