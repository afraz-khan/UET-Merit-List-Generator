using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admission_Office.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}


/*
 @model Admission_Office.Models.Test

@{
    ViewBag.Title = "othertests";
}

<h2>othertests</h2>

@using (Html.BeginForm()) 
{
    @Html.AntiForgeryToken()
    <div class="container">
        
        <div class="panel-group">
            <div class="panel panel-default" style="width:200px;">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a data-toggle="collapse" href="#collapse1">New Test</a>
                    </h4>
                </div>
                <div id="collapse1" class="panel-collapse collapse">
                    <ul class="list-group">
                        <li class="list-group-item">
                            <label>Name</label>
                            <input type="text" name="testname" placeholder="Name of Test" style="border-radius:3px"/>
                        </li>
                        <li class="list-group-item">
                            <label>Marks obtained</label>
                            <input type="text" id="marks" name="marks" min="0" step="1" data-bind="value:marks" placeholder="Marks obtained here" style="border-radius:3px" />
                        </li>
                    </ul>
                    <div class="panel-footer">Footer</div>
                </div>
            </div>
        </div>
    </div>

}

<div>
    @Html.ActionLink("Back to List", "Index")
</div>
     */
