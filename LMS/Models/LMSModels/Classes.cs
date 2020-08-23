using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Classes
    {
        public Classes()
        {
            AssignmentCategories = new HashSet<AssignmentCategories>();
            StudentEnrollment = new HashSet<StudentEnrollment>();
        }

        public string ClassId { get; set; }
        public string SemesterSeason { get; set; }
        public uint SemesterYear { get; set; }
        public string Location { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int CourseNumber { get; set; }
        public string ProfessorId { get; set; }

        public virtual Courses CourseNumberNavigation { get; set; }
        public virtual Professors Professor { get; set; }
        public virtual ICollection<AssignmentCategories> AssignmentCategories { get; set; }
        public virtual ICollection<StudentEnrollment> StudentEnrollment { get; set; }
    }
}
