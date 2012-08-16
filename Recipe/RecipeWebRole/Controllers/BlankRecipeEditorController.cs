using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using RecipeWebRole.DataAccess;
using RecipeWebRole.Models;

namespace RecipeWebRole.Controllers
{
    public class BlankRecipeEditorController : Controller
    {
        private readonly IRecipeRepository _recipeRepo;

        public BlankRecipeEditorController(IRecipeRepository recipeRepo)
        {
            _recipeRepo = recipeRepo;
        }


        //
        // GET: /BlankRecipeEditor/Open

        public ActionResult Open(int recipeId)
        {
            Recipe recipe = null;
            if (recipeId > 0)
            {
                recipe = _recipeRepo.GetRecipe(recipeId);
            }

            ViewBag.Message = "Edit Recipe " + recipeId;
            ViewBag.Recipe = recipe;

            return View();
        }


        //
        // POST: /BlankRecipeEditor/Save

        [HttpPost]
        public JsonResult Save(Recipe recipe)
        {
            _recipeRepo.SetRecipe(recipe);

            Thread.Sleep(1000);
            //  throw new ArgumentException("Bad things happended");
            return Json(recipe);
        }

    }
}
