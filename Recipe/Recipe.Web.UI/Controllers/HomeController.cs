using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;
using System.Threading;

namespace Recipe.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public JsonResult Test()
        {
            var a = Request;  // analyse for all properties of the call.
            var p = new Person() { FirstName = "Mats", Name = "Bader" };
            return Json(p, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetRecipe()
        {
            dynamic obj = new ExpandoObject();
            obj.Value = 10;

            Thread.Sleep(1000);
            return Json(obj);
        }

    }


    public class Person
    {
        public string Name { get; set; }

        public string FirstName { get; set; } 

    }
}
