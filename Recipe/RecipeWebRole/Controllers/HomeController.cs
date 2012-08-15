using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Microsoft.IdentityModel.Claims;
using RecipeWebRole.ActionFilters;
using RecipeWebRole.DataAccess;
using RecipeWebRole.Models;
using RecipeWebRole.Utilities;

namespace RecipeWebRole.Controllers
{
    [ActionFilters.Authorize]
    public class HomeController : Controller
    {
        private static readonly IRecipeRepository _repo = new InMemoryRecipeRepository();//new FakeInMemoryRecipeRepository();
        //private static readonly LocalFileSystemRecipeRepository _repo = LocalFileSystemRecipeRepository.Instance;

        //
        // GET: /Home/Index

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            IList<int> ids = _repo.GetRecipeIds();
            IEnumerable<Tuple<int, string>> data = (from id in ids
                                                    let name = _repo.GetRecipe(id).Name
                                                    select new Tuple<int, string>(id, name)).ToList();
            return View(data);
        }


        //
        // GET: /Home/Index

        [HttpGet]
        [AllowAnonymous]
        public ActionResult SearchRecipe()
        {
            return View();
        }


        //
        // GET: /Home/CreateRecipe

        [HttpGet]
        [AllowAnonymous]
        public ActionResult CreateRecipe()
        {
            return View();
        }


        //
        // GET: /Home/UserProfile

        [HttpGet]
        [AllowAnonymous]
        public ActionResult UserProfile()
        {
            return View();
        }






        public ActionResult RecipeEditor(int recipeId)
        {
            Recipe recipe = null;
            if (recipeId > 0)
            {
                recipe = _repo.GetRecipe(recipeId);
            }

            // ViewBag is a dynamic object
            ViewBag.Message = "Edit Recipe " + recipeId;
            ViewBag.Recipe = recipe;

            return View();
        }

        [HttpPost]
        public JsonResult SetRecipe(Recipe recipe)
        {
            _repo.SetRecipe(recipe);

            Thread.Sleep(1000);
            //  throw new ArgumentException("Bad things happended");
            return Json(recipe);
        }
    }
}
