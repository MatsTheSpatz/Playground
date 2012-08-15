using System;
using System.Collections.Generic;
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
    public class HomeController : Controller
    {
        private static readonly IRecipeRepository _repo = new BlobStorageRecipeRepository();
        //private static readonly LocalFileSystemRecipeRepository _repo = LocalFileSystemRecipeRepository.Instance;

        public ActionResult Index()
        {
            bool isInRole = User.IsInRole("Family");
            System.Diagnostics.Debug.WriteLine(isInRole);

            ViewBag.Message = "Recipe List";
            IEnumerable<int> data = new List<int>() {1, 2, 3};
            //IEnumerable<int> data = _repo.GetRecipeIds();

            return View(data);
        }

        [ActionFilters.Authorize]
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
