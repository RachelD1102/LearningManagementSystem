using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class StudentEnrollment
    {
        public string UId { get; set; }
        public string ClassId { get; set; }
        public string Grade { get; set; }

        public virtual Classes Class { get; set; }
        public virtual Students U { get; set; }
    }
}
