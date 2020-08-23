using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Submission
    {
        public DateTime SubmitTime { get; set; }
        public int? Score { get; set; }
        public string Contents { get; set; }
        public string UId { get; set; }
        public string AssignmentId { get; set; }

        public virtual Assignments Assignment { get; set; }
        public virtual Students U { get; set; }
    }
}
