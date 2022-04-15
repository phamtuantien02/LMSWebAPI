using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSWebAPI.Models
{
    public class Course
    {
        public int CourseID { get; set; }

        public string CourseCode { get; set; }

        public string CourseName { get; set; }

        public string CourseCredit { get; set; }
    }
}
