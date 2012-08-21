using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using RecipeWebRole.DataAccess;
using RecipeWebRole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
namespace RecipeWebRole.Controllers
{
    public class BlankRecipeEditorController : Controller
    {
        private readonly IRecipeRepository _recipeRepo;

        public BlankRecipeEditorController(IRecipeRepository recipeRepo)
        {
            _recipeRepo = recipeRepo;
        }

         public ActionResult RecipeEditor()
         {
             Thread.Sleep(1000);
             return PartialView();
         }
        

        //
        // GET: /BlankRecipeEditor/Open

        public ActionResult Open(int recipeId)
        {
            Recipe recipe;
            if (recipeId <= 0)
            {
                recipe = new TextRecipe {Author = User.Identity.Name, CreationDate = DateTime.Now};
                _recipeRepo.SetRecipe(recipe);
            }
            else
            {
                recipe = _recipeRepo.GetRecipe(recipeId);
            }

            //recipe.IsVegetarian = true;
            //recipe.DishCategories = new [] { DishCategory.Starter, DishCategory.Soup, };
            //recipe.Seasons = new[] { Season.Winter };
            //recipe.SkillLevel = SkillLevel.Average;
            ViewBag.Recipe = recipe;

            return View();
        }



        //
        // POST: /BlankRecipeEditor/Save

        [HttpPost]
        public JsonResult Save(TextRecipe recipe)
        {
            _recipeRepo.SetRecipe(recipe);

            Thread.Sleep(1000);
            //  throw new ArgumentException("Bad things happended");
            return Json(recipe);
        }

    }
}
