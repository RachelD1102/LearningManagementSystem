using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class AssignmentCategories
    {
        public AssignmentCategories()
        {
            Assignments = new HashSet<Assignments>();
        }

        public string AssignmentCategoryId { get; set; }
        public string Name { get; set; }
        public int? GradingWeight { get; set; }
        public string ClassId { get; set; }

        public virtual Classes Class { get; set; }
        public virtual ICollection<Assignments> Assignments { get; set; }
    }
}
