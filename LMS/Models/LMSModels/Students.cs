using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Students
    {
        public Students()
        {
            StudentEnrollment = new HashSet<StudentEnrollment>();
            Submission = new HashSet<Submission>();
        }

        public string UId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string SubjectAbbreviation { get; set; }

        public virtual Departments SubjectAbbreviationNavigation { get; set; }
        public virtual ICollection<StudentEnrollment> StudentEnrollment { get; set; }
        public virtual ICollection<Submission> Submission { get; set; }
    }
}
