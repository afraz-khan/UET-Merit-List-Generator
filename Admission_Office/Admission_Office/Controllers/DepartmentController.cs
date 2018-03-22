using Admission_Office.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admission_Office.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: Department
        public static List<Department> EnteredDepartments = new List<Department>();

        public ActionResult MakeDepartment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MakeDepartment(Department d)
        {
            Department dd = new Department();
            dd.Aggregate = 0.0;
            dd.Name = d.Name;
            dd.seats = d.seats;
            EnteredDepartments.Add(dd);

            return RedirectToAction("Index","Home");
        }

        public ActionResult Deletedept(string id)
        {
            foreach (Department d in DepartmentController.EnteredDepartments.ToList())
            {
                if (d.Name == id)
                {
                    DepartmentController.EnteredDepartments.Remove(d);
                }
            }

            return RedirectToAction("Index", "Home");
        }




    }
}