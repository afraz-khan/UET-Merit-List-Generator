using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Admission_Office.Models
{
    public class Preferences
    {
        [Required]
        public List<Department> Prefs = new List<Department>();
        public List<Department> dps = new List<Department>();
    }
}