using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admission_Office.Models
{
    public class Student : IComparable<Student>
    {
        [Display(Name ="ARN Number")]
        public string ARN { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string Name { get; set; }
        [Display(Name = "Father Name")]
        [Required]
        public string Father_Name { get; set; }
        [Display(Name = "CNIC Number")]
        [Required]
        public string CNIC { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [MaxLength(length:13)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Birth")]
        public string DOB { get; set; }

        [Display(Name = "Matric Marks")]
        [Required]
        public int Matric_Marks { get; set; }
        [Display(Name = "Intermediate Marks")]
        [Required]
        public int Fsc_Marks { get; set; }
        [Required]
        [Display(Name = "Ecat Marks")]
        public int ECAT_Marks { get; set; }
        [Display(Name = "Other Entry Test Results")]
        public List<Test> Other_Tests { get; set; }

        [Display(Name = "First Preference")]
        [Required]
        public string department { get; set; }  


        public List<Department> dps { get; set; }

        [Required]
        [Display(Name ="Aggregate")]
        public float aggregate { get; set; }

        public int CompareTo(Student other)
        {
            return this.aggregate.CompareTo(other.aggregate);
        }

        [Display(Name = "Preferences")]
        public List<Department> preferences { get; set; }
        [Required]
        [Display(Name ="Choose a Category")]
        public string Category { get; set; }
    }

  
}