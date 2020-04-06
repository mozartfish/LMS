using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Submission
    {
        public string UId { get; set; }
        public uint AssignmentId { get; set; }
        public DateTime SubTime { get; set; }
        public uint Score { get; set; }
        public string Content { get; set; }

        public virtual Assignments Assignment { get; set; }
        public virtual Students U { get; set; }
    }
}
