using System;
using System.Collections.Generic;
using System.Linq;
using Admission_Office.Models;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Rotativa.MVC;
using System.Net.Mail;
using System.Net;
using iTextSharp.text.pdf;
using itextsharp.pdfa;
using iTextSharp.text.xml;
using System.IO;
using System.Text;
using iTextSharp.tool.xml;
using iTextSharp.text.html.simpleparser;
using System.Data;
using System.Web.UI;
using iTextSharp.text;
using System.Drawing;
using System.Diagnostics;

namespace Admission_Office.Controllers
{
    public class StudentController : Controller
    {
        public static List<Student> EnteredStudents = new List<Student>();
        public static List<Student> SelectedStudents = new List<Student>();
        List<Student> SelectedStudents2 = new List<Student>();
        
        

        public static List<Dept_Students> Selected_Dept_Students = new List<Dept_Students>();
        

        List<Department> selecteddepts = new List<Department>();
        List<Department> dpl = new List<Department>();
        public Student tempst = new Student();
        public static List<string> UsedARNs = new List<string>();
        // GET: Student
        public ActionResult Index()
        {
                if (EnteredStudents.Count == 0)
                {
                    Session["slen"] = 0;
                }
                else if (EnteredStudents.Count > 0)
                {
                    Session["slen"] = EnteredStudents.Count;
                }
           


                return View(EnteredStudents);
            }

        public ActionResult Create()
        {
            //ViewData["entdps"] = DepartmentController.EnteredDepartments.ToList();
            Student s = new Student();
            s.dps = DepartmentController.EnteredDepartments.ToList();

            return View(s);
        }


        [HttpPost]
        public ActionResult othertests(Student student)
        {
            tempst = student;
            float fsc = ((float)tempst.Fsc_Marks / (float)1100.00) * (float)70;
            float ect = ((float)tempst.ECAT_Marks / (float)400.00) * (float)30;
            float agr = fsc + ect;
            tempst.aggregate = agr;
            tempst.ARN = get_arnum();


            string stdept = tempst.department;
            Session["1stpref"] = student.department;
            foreach (Department d in DepartmentController.EnteredDepartments)
            {
                if (d.Name == stdept)
                {
                    tempst.Other_Tests = new List<Test>();
                    
                    d.seat_counter++;
                    tempst.preferences = new List<Department>();
                    tempst.preferences.Add(d);
                    EnteredStudents.Add(tempst);

                    return RedirectToAction("Create","Preference");
                }

            }

            return RedirectToAction("Create");
        }

        [HttpPost]
        public ActionResult othertestsF(Test t)
        {
            ViewBag.enterstderror = null;
            Test tt = new Test();
            tt = t;
            EnteredStudents[EnteredStudents.Count - 1].Other_Tests.Add(tt);

            return View();
        }

        public ActionResult othertestsF()
        {

            return View();
        }

        public ActionResult MeritIndexView()
        {

            if (EnteredStudents.Count == 0)
            {
                ViewBag.enterstderror = "Pl, first enter atleast one student...";
                return View("Index");

            }
            else
            {


                foreach (Student s in EnteredStudents.ToList())
                {
                    foreach (Department d in DepartmentController.EnteredDepartments.ToList())
                    {
                        Department n = new Department();
                        if (d.Name == s.department)
                        {
                            n = d;
                            if (!selecteddepts.Exists(n1 => n1.Name == n.Name))
                            {
                                Dept_Students dp = new Dept_Students();
                                dp.dept = n;
                                selecteddepts.Add(n);
                                Selected_Dept_Students.Add(dp);
                            }
                        }
                    }
                }



                foreach (Dept_Students dp in Selected_Dept_Students.ToList())
                {
                    dp.SelectedStudents = new List<Student>();
                    foreach (Student s in EnteredStudents.ToList())
                    {
                        if (s.department == dp.dept.Name)
                        {
                            Student ss = new Student();
                            ss = s;
                            dp.SelectedStudents.Add(ss);
                        }
                    }
                }


                foreach (Dept_Students dp in Selected_Dept_Students)
                {
                    dp.SelectedStudents.Sort();
                    dp.SelectedStudents.Reverse();
                }
                int i = 0;
                foreach (Dept_Students dp in Selected_Dept_Students)
                {
                    if (dp.dept.seat_counter < dp.dept.seats)
                    {
                        for (int f = 0; f < dp.dept.seat_counter; f++)
                        {
                            SelectedStudents.Add(dp.SelectedStudents[f]);
                        }

                        dp.dept.Aggregate = dp.SelectedStudents[dp.dept.seat_counter-1].aggregate;
                    }
                    else if (dp.dept.seat_counter >= dp.dept.seats)
                    {
                        for (int f = 0; f < dp.dept.seats; f++)
                        {
                            SelectedStudents.Add(dp.SelectedStudents[f]);
                        }
                        dp.dept.Aggregate = dp.SelectedStudents[dp.dept.seats-1].aggregate;
                    }

                }

            


                foreach (Student s in SelectedStudents)
                {
                    try
                    {
                        SendEmail(s);
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Sorry, Email is wrong or Internet Problem";
                    }

                }

                foreach(Dept_Students sp in Selected_Dept_Students)
                {
                    foreach(Department d in DepartmentController.EnteredDepartments)
                    {
                        if (d.Name == sp.dept.Name)
                        {
                            d.Aggregate = sp.dept.Aggregate;
                        }
                    }
                }
                return View(SelectedStudents);
            }
        }

        public  ActionResult MeritList2()
        {
            if (EnteredStudents.Count == 0)
            {
                ViewBag.enterstderror = "Pl, first enter atleast one student...";
                return View("Index");

            }
            else
            {

                foreach (Department d in DepartmentController.EnteredDepartments.ToList())
                {
                    Dept_Students dp = new Dept_Students();
                    dp.SelectedStudents = new List<Student>();
                    dp.dept = d;
                    Selected_Dept_Students.Add(dp);
                }

                char exist = 'N';
                char entered = 'N';
                foreach (Student s in EnteredStudents.ToList())
                {
                    //foreach(Department ddd in s.preferences.ToList())
                    //{
                    //    ddd.seat_counter = 0;
                    //}

                    foreach (Department d in s.preferences)
                    {
                        
                        foreach (Dept_Students dps in Selected_Dept_Students.ToList())
                        {
                            if (dps.dept == d)
                            {
                                if (entered == 'N')
                                {

                                    if (!dps.SelectedStudents.Exists(n => n.CNIC == s.CNIC))
                                    {
                                        exist = 'N';
                                    }
                                    else
                                    {
                                        exist = 'Y';
                                        break;
                                    }

                                }
                            }
                        }

                        if (exist == 'Y')
                        {
                            break;
                        }
                        else
                        {
                            foreach (Dept_Students dps2 in Selected_Dept_Students.ToList())
                            {
                                if (d == dps2.dept)
                                {
                                    if (dps2.dept.seat_counter < dps2.dept.seats)
                                    {
                                        
                                        s.department = dps2.dept.Name; 
                                        SelectedStudents.Add(s);
                                        dps2.dept.seat_counter++;
                                        dps2.SelectedStudents.Add(s);
                                        dps2.SelectedStudents.Sort();
                                        
                                        entered = 'Y';
                                        exist = 'Y';
                                    }
                                    else if (dps2.dept.seat_counter >= dps2.dept.seats)
                                    {
                                        if (s.aggregate > dps2.SelectedStudents[dps2.SelectedStudents.Count - 1].aggregate)
                                        {
                                            Student ss = dps2.SelectedStudents[dps2.SelectedStudents.Count - 1];
                                            s.department = dps2.dept.Name;
                                            dps2.SelectedStudents.Remove(ss);
                                            dps2.dept.seat_counter++;
                                            dps2.SelectedStudents.Add(s);
                                            dps2.SelectedStudents.Sort();
                                            SelectedStudents.Add(s);
                                            
                                            entered = 'Y';
                                            exist = 'Y';
                                        }
                                    }
                                }
                            }
                        }

                    }



                    entered = 'N';
                }

                foreach (Department d in DepartmentController.EnteredDepartments.ToList())
                {
                    foreach (Dept_Students dp in Selected_Dept_Students.ToList())
                    {
                        if (d == dp.dept)
                        {
                           if (dp.SelectedStudents.Count != 0)
                            {
                                d.seat_counter = dp.dept.seat_counter;
                                d.Aggregate = dp.SelectedStudents[dp.SelectedStudents.Count - 1].aggregate;
                            }
                        }
                    }
                }


                //foreach (Student s in SelectedStudents)
                //{
                //    try
                //    {
                //        SendEmail(s);
                //    }
                //    catch (Exception)
                //    {
                //        ViewBag.Error = "Sorry, Email is wrong or Internet Problem";
                //    }

                //}


                return View(SelectedStudents.ToList());
            }
        }

        public ActionResult MeritIndex()
        {
            if (EnteredStudents.Count == 0)
            {
                ViewBag.enterstderror = "Pl, first enter atleast one student...";

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        
        int generate()
        {
            Random r = new Random();
            int d = r.Next(10000000, 99999999);
            return d;
        }

        string get_arnum()
        {
            int ran = generate();
            string rAn = Convert.ToString(ran);
            while (UsedARNs.Contains(rAn))
            {
                ran = generate();
                rAn = Convert.ToString(ran);
            }

            return (rAn);
        }


        public ActionResult Delete(string id)
        {
            //List<Student> ss = new List<Student>();
            //ss = EnteredStudents;
            foreach (Student s in EnteredStudents.ToList())
            {
                if (s.ARN == id)
                {
                    EnteredStudents.Remove(s);
                }
            }

            return RedirectToAction("Index");
        }

        public void SendEmail(Student toAdress)
        {

            MailAddress fromAddress = new MailAddress("afrazmaxxx4183@gmail.com", "Afraz Khan");
            MailAddress toAddress = new MailAddress(toAdress.Email, "Reciever");

            string fromPass = "2015N4183";
            string subject = "UET Admission Confirmation";
            string body = "Congrats, U have got admission in " + toAdress.department + " Department, UET Lahore in Category: "+toAdress.Category+", securing "+toAdress.aggregate+" Aggregate. Thanks for believing in us.";

            SmtpClient smp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPass)

            };

            MailMessage msg = new MailMessage(fromAddress.Address, toAddress.Address)
            {
                Body = body,
                Subject = subject

            };

            smp.Send(msg);
        }
        
        PdfPTable pdft;
        
        public ActionResult meritlist()
        {
            TableStyle tbst = new TableStyle();
            tbst.CellSpacing = 20;
            tbst.BorderColor = Color.Black;
            tbst.BorderStyle = BorderStyle.Dashed;
            tbst.GridLines = GridLines.Both;
            
            Document doc = new Document(PageSize.A4,10f,10f,10f,0f);
            doc.SetPageSize(PageSize.A4);
            doc.SetMargins(20f, 20f, 20f, 20f);
            iTextSharp.text.Font font_style = FontFactory.GetFont("Tahoma",8f,1);
            pdft = new PdfPTable(6);
            
            pdft.WidthPercentage = 100;
            pdft.HorizontalAlignment = Element.ALIGN_LEFT;
            pdft.SetWidths(new float[] { 20f, 20f, 20f, 30f, 30f,20f });
            MemoryStream mst = new MemoryStream();
            PdfWriter.GetInstance(doc, new FileStream("G:/Visual Studio Pros/Admission_Office/Admission_Office/Merit_List.pdf", FileMode.Create));
            this.addheader();
            doc.Open();



          
            this.addbody();
           // pdft.HeaderRows = 2;
            doc.Add(pdft);
            doc.Close();
            
            return View("SuccessPdf");
            
        }

        public void addheader()
        {
           
            iTextSharp.text.Font f1 = FontFactory.GetFont("Arial", 13f, 1);
            f1.Color = BaseColor.BLUE;
            PdfPCell pdfcl = new PdfPCell(new Phrase("Merit List ", f1));
            pdfcl.Colspan = 1;
            f1.IsItalic();
            f1.IsBold();

           
            //f1.SetStyle(4);
            pdfcl.PaddingTop = 10f;
            pdfcl.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfcl.BackgroundColor = BaseColor.WHITE;
            pdfcl.VerticalAlignment = Element.ALIGN_LEFT;
            pdfcl.Border = 3;
            pdfcl.BorderColorBottom = BaseColor.BLACK;
            pdfcl.BorderColorTop = BaseColor.WHITE;
            pdfcl.BorderColorLeft = BaseColor.WHITE;
            pdfcl.BorderColorRight = BaseColor.WHITE;
            pdfcl.ExtraParagraphSpace = 0;
            pdft.AddCell(pdfcl);

            


            iTextSharp.text.Font f = FontFactory.GetFont("Arial", 15f, 1);
            
            f.IsBold();
            f.Color = BaseColor.DARK_GRAY;

           
            PdfPCell pdfcl1 = new PdfPCell(new Phrase("University of Engg & Technology, Lahore " , f));
            pdfcl1.FixedHeight = 30f;
            pdfcl1.PaddingTop = 30f;
            pdfcl1.Colspan = 3;
            pdfcl1.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfcl1.BackgroundColor = BaseColor.WHITE;
            pdfcl1.Border = 0;
            pdfcl1.ExtraParagraphSpace = 0;
            pdft.AddCell(pdfcl1);

            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance("G:/Visual Studio Pros/Admission_Office/Admission_Office/Photos/UET_Lahore_Logo.png");
            PdfPCell pdfcl7 = new PdfPCell(img);
            pdfcl7.Colspan = 2;
            pdfcl7.BorderColor = BaseColor.WHITE;
            pdfcl7.Border = 0;
            pdfcl7.PaddingBottom = 5f;
            
            pdft.AddCell(pdfcl7);
            
            iTextSharp.text.Font f2 = FontFactory.GetFont("calibri", 10f, 1);
            f2.SetColor(66, 134, 244);
            PdfPCell pdfcl2 = new PdfPCell(new Phrase("ARN number", f2));
            PdfPCell pdfcl3 = new PdfPCell(new Phrase("Name of Student", f2));
            PdfPCell pdfcl6 = new PdfPCell(new Phrase("Email", f2));
            PdfPCell pdfcl4 = new PdfPCell(new Phrase("Aggregate", f2));
            PdfPCell pdfcl5 = new PdfPCell(new Phrase("Dept of Admission", f2));
            PdfPCell pdfcl9 = new PdfPCell(new Phrase("Category", f2));

            pdfcl2.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfcl2.BorderWidth = 1f;
            pdfcl3.BorderWidth = 1f;
            pdfcl4.BorderWidth = 1f;
            pdfcl5.BorderWidth = 1f;
            pdfcl6.BorderWidth = 1f;
            pdfcl9.BorderWidth = 1f;

            pdfcl2.BorderColor = BaseColor.GRAY;
            pdfcl3.BorderColor = BaseColor.GRAY;
            pdfcl4.BorderColor = BaseColor.GRAY;
            pdfcl5.BorderColor = BaseColor.GRAY;
            pdfcl6.BorderColor = BaseColor.GRAY;
            pdfcl9.BorderColor = BaseColor.GRAY;

            pdfcl2.Left = 10f;
            pdfcl3.Left = 10f;
            pdfcl6.Left = 5f;

            pdfcl3.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfcl4.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfcl5.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfcl6.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdfcl9.BackgroundColor = BaseColor.LIGHT_GRAY;
            pdft.AddCell(pdfcl2);
            pdft.AddCell(pdfcl3);
            pdft.AddCell(pdfcl4);
            pdft.AddCell(pdfcl5);
            pdft.AddCell(pdfcl6);
            pdft.AddCell(pdfcl9);



        }

        public void addbody()
        {
            DateTime date = DateTime.Now;
            string Date = Convert.ToString(date);
            foreach (Student s in SelectedStudents)
            {
                iTextSharp.text.Font f = FontFactory.GetFont("Tahoma", 7f, 1);
                PdfPCell pdfcl = new PdfPCell(new Phrase(s.ARN, f));
                pdfcl.BorderColor = BaseColor.DARK_GRAY;
                pdfcl.ExtraParagraphSpace = 0;

                PdfPCell pdfcl1 = new PdfPCell(new Phrase(s.Name, f));
                pdfcl.BorderColor = BaseColor.DARK_GRAY;
                pdfcl.ExtraParagraphSpace = 0;

                PdfPCell pdfcl2 = new PdfPCell(new Phrase(s.aggregate.ToString(), f));
                pdfcl.BorderColor = BaseColor.DARK_GRAY;
                pdfcl.ExtraParagraphSpace = 0;

                PdfPCell pdfcl3 = new PdfPCell(new Phrase(s.department, f));
                pdfcl.BorderColor = BaseColor.DARK_GRAY;
                pdfcl.ExtraParagraphSpace = 0;

                PdfPCell pdfcl4 = new PdfPCell(new Phrase(s.Email, f));
                pdfcl.BorderColor = BaseColor.DARK_GRAY;
                pdfcl.ExtraParagraphSpace = 0;

                PdfPCell pdfcl5 = new PdfPCell(new Phrase(s.Category, f));
                pdfcl.BorderColor = BaseColor.DARK_GRAY;
                pdfcl.ExtraParagraphSpace = 0;

                pdfcl.BorderWidth = 1f;
                pdfcl1.BorderWidth = 1f;
                pdfcl2.BorderWidth = 1f;
                pdfcl3.BorderWidth = 1f;
                pdfcl4.BorderWidth = 1f;
                pdfcl5.BorderWidth = 1f;

                pdfcl.BorderColor = BaseColor.GRAY;
                pdfcl2.BorderColor = BaseColor.GRAY;
                pdfcl1.BorderColor = BaseColor.GRAY;
                pdfcl3.BorderColor = BaseColor.GRAY;
                pdfcl4.BorderColor = BaseColor.GRAY;
                pdfcl5.BorderColor = BaseColor.GRAY;


                


                pdft.AddCell(pdfcl);
                pdft.AddCell(pdfcl1);
                pdft.AddCell(pdfcl2);
                pdft.AddCell(pdfcl3);
                pdft.AddCell(pdfcl4);
                pdft.AddCell(pdfcl5);
            }

            iTextSharp.text.Font ff = FontFactory.GetFont("Arial", 7f);
            ff.IsBold();
            iTextSharp.text.Font fff = FontFactory.GetFont("Arial", 7f);
            fff.SetStyle(4);
            PdfPCell pdfcl11 = new PdfPCell(new Phrase ("@ all copy Rights reserved, www.uet.edu.pk"));
            pdfcl11.Colspan = 3;
            pdfcl11.Border = 0;
            PdfPCell pdfcl12 = new PdfPCell(new Phrase (Date));
            pdfcl12.Colspan = 3;
            pdfcl12.Border = 0;
            pdfcl11.PaddingTop = 50f;
            pdfcl12.PaddingTop = 50f;


            pdft.AddCell(pdfcl11);
            pdft.AddCell(pdfcl12);


        }
         
        public ActionResult deleteall()
        {
            foreach(Student s in SelectedStudents.ToList())
            {
                SelectedStudents.Remove(s);
            }
            foreach(Dept_Students sdp in Selected_Dept_Students.ToList())
            {
                Selected_Dept_Students.Remove(sdp);
            }

            return RedirectToAction("Index", "Student");
        }

        public ActionResult openlist()
        {
            Process.Start("G:/Visual Studio Pros/Admission_Office/Admission_Office/Merit_List.pdf");

            return RedirectToAction("meritlist");
        }
       
        public ActionResult ads_prefs()
        {
            Preferences prf = new Preferences();
            prf.Prefs = DepartmentController.EnteredDepartments.ToList();

            return View(prf);
        }

    }
}