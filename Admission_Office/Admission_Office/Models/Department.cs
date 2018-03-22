using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admission_Office.Models
{
    public class Department 
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1,500,ErrorMessage ="Out of bound" )]
        [Display(Name="Maximum Seats")]
        public int seats { get; set; }
        [Display(Name = "Closed Merit")]
        public double Aggregate { get; set; }


        public int seat_counter;
    }
}