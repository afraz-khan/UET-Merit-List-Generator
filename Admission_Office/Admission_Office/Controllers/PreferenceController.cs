using Admission_Office.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admission_Office.Controllers
{
    public class PreferenceController : Controller
    {
        // GET: Preference
        public ActionResult Index()
        {
            return View();
        }
        int t = DepartmentController.EnteredDepartments.Count;
        public ActionResult Create()
        {
            Preferences f = new Preferences();
            f.dps = DepartmentController.EnteredDepartments.ToList();
         
            return View(f);
        }

        [HttpPost]
        public ActionResult Create(FormCollection  mpref)
        {
            
            for (int g = 0; g < t; g++)
            {
                string name = mpref["pref" + g.ToString()];
                foreach(Department d in DepartmentController.EnteredDepartments.ToList())
                {
                    int id = StudentController.EnteredStudents.Count - 1;
                    if (d.Name == name)
                    {
                        if (!StudentController.EnteredStudents[id].preferences.Exists(s => s.Name == d.Name))
                        {
                            StudentController.EnteredStudents[id].preferences.Add(d);
                        }
                    }
                }
            }

     
         

            return RedirectToAction("othertestsF", "Student");
        }
    }
}