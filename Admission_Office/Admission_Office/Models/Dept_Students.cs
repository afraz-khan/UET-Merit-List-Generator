using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admission_Office.Models
{
    public class Dept_Students
    {
        public List<Student> SelectedStudents { get; set; }
        public Department dept { get; set; }
    }
}