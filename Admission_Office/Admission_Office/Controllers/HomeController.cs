using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Admission_Office.Models;
using System.Web.Mvc;

namespace Admission_Office.Controllers
{
    public class HomeController : Controller
    {
        List<char> Categories = new List<char>();


        public ActionResult Index()
        {

            Categories.Add('A');
            Categories.Add('I');
            Categories.Add('L');
            Categories.Add('O');
            
            List<Department> dps = new List<Department>();
            dps = DepartmentController.EnteredDepartments.ToList();
            if (dps.Count == 0)
            {
               Session["len"] = 0;
            }
            else if (dps.Count > 0)
            {
                Session["len"] = dps.Count;
            }


           


            return View(dps);
        }

        public ActionResult About()
        {

            Department d = new Department();
            Department d1 = new Department();
            Department d2 = new Department();

            d.Name = "Computer Science";
            d.seats = 1;
            d.seat_counter = 0;

            d1.Name = "Electrical Engg";
            d1.seats = 1;
            d1.seat_counter = 0;

            d2.Name = "Machanical Engg";
            d2.seats = 1;
            d2.seat_counter = 0;

            DepartmentController.EnteredDepartments.Add(d);

            DepartmentController.EnteredDepartments.Add(d1);
            DepartmentController.EnteredDepartments.Add(d2);

            //Student s = new Student();
            //s.Name = "AfrazKhan";
            //s.Father_Name = "Imtiaz Khan";
            //s.Category = "B";
            //s.CNIC = "3540487606013";
            //s.Phone = "03104504335";
            //s.ECAT_Marks = 236;
            //s.Matric_Marks = 969;
            //s.Fsc_Marks = 923;

            List<Department> dd1 = new List<Department>();
            List<Department> dd2 = new List<Department>();

            dd1.Add(d);
            dd1.Add(d1);
            dd1.Add(d2);

            dd2.Add(d2);
            dd2.Add(d1);
            dd2.Add(d);

            //s.preferences = dd2;

            //s.dps = dd1;
            //s.ARN = "93743782";
            //s.aggregate = 76.2f;
            //s.DOB = "2/3/2312";
            //s.Email = "afrazmaxxx4183@gmail.com";
            List<Test> ts = new List<Test>();
            ts.Add(new Test
            {
                Name = "lcat",
                Marks = 236
            });
            //s.Other_Tests = ts;
            // StudentController.EnteredStudents.Add(s);

            // StudentController.EnteredStudents = new List<Student>();

            StudentController.EnteredStudents.Add(new Student
            {
                Name = "AfrazKhan",
                Father_Name = "Imtiaz Khan",
                Category = "B",
                CNIC = "3540487606013",
                Phone = "03104504335",
                ECAT_Marks = 236,
                Matric_Marks = 969,
                Fsc_Marks = 923,
                dps = dd2,
                preferences = dd1,
                ARN = "93743782",
                aggregate = 76.2f,
                DOB = "2/3/2312",
                Other_Tests = ts,
                Email = "afrazmaxxx4183@gmail.com",
            });

            StudentController.EnteredStudents.Add(new Student
            {
                Name = "AfrazKhan2",
                Father_Name = "Imtiaz Khan2",
                Category = "C",
                CNIC = "3540487606012",
                Phone = "03104504334",
                ECAT_Marks = 236,
                Matric_Marks = 969,
                Fsc_Marks = 923,
                dps = dd1,
                preferences = dd2,
                ARN = "93743782",
                aggregate = 89.2f,
                DOB = "2/3/2312",
                Other_Tests = ts,
                Email = "afrazmaxxx4183@gmail.com",
            });

            StudentController.EnteredStudents.Add(new Student
            {
                Name = "AfrazKhan3",
                Father_Name = "Imtiaz Khan2",
                Category = "C",
                CNIC = "3540487606011",
                Phone = "03104504334",
                ECAT_Marks = 236,
                Matric_Marks = 969,
                Fsc_Marks = 923,
                dps = dd1,
                preferences = dd2,
                ARN = "93743783",
                aggregate = 69.2f,
                DOB = "2/3/2312",
                Other_Tests = ts,
                Email = "afrazmaxxx4183@gmail.com",
            });

            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

      
    }
}