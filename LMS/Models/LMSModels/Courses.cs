using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Courses
    {
        public Courses()
        {
            Classes = new HashSet<Classes>();
        }

        public string Name { get; set; }
        public int CourseNumber { get; set; }
        public string CatalogId { get; set; }
        public string SubjectAbbreviation { get; set; }

        public virtual Departments SubjectAbbreviationNavigation { get; set; }
        public virtual ICollection<Classes> Classes { get; set; }
    }
}
