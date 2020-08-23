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

        public string Name { get; set; }
        public int? MaxPointValue { get; set; }
        public string Contents { get; set; }
        public DateTime? DueDate { get; set; }
        public string AssignmentCategoryId { get; set; }
        public string AssignmentId { get; set; }

        public virtual AssignmentCategories AssignmentCategory { get; set; }
        public virtual ICollection<Submission> Submission { get; set; }
    }
}
