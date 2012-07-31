using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;
using System.Threading;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace Recipe.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        private static readonly Repository _repository = Repository.Instance;
        
        public ActionResult Index()
        {
            ViewBag.Message = "Recipe List";
            var ids = _repository.GetRecipeIds();
            return View(ids);
        }

        public ActionResult RecipeEditor(int recipeId)
        {
            Recipe recipe = null;
            if (recipeId > 0)
            {
                recipe = _repository.GetRecipe(recipeId);
            }

            // ViewBag is a dynamic object
            ViewBag.Message = "Edit Recipe " + recipeId;
            ViewBag.Recipe = recipe;

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SetRecipe(Recipe recipe)
        {            
            _repository.SetRecipe(recipe);

            Thread.Sleep(1000);
            //  throw new ArgumentException("Bad things happended");
            return Json(recipe);
        }
    }
}
