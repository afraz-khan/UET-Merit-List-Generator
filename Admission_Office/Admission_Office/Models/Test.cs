using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admission_Office.Models
{
    public class Test 
    {
       
        [Display(Name ="Name of Test")]
        public string Name { get; set; }

        [Display(Name ="Obtained Marks")]
        public int Marks { get; set; }
    }
}