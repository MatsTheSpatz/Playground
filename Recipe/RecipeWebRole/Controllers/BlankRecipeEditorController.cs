using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Microsoft.IdentityModel.Claims;
using RecipeWebRole.DataAccess;
using RecipeWebRole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using RecipeWebRole.Utilities;

namespace RecipeWebRole.Controllers
{
    [ActionFilters.Authorize]
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
            Recipe recipe;
            if (recipeId > 0)
            {
                // Existing recipe is being edited.
                // Validate that only the initial author of the recipe can edit it.
                recipe = _recipeRepo.GetRecipe(recipeId);
                
                string currentAuthorId =  this.GetUserNameIdentifier();
                if (recipe.AuthorId != currentAuthorId)
                {
                    return View("WrongAuthor", recipe);
                }
            }
            else
            {
                recipe = new TextRecipe
                             {
                                 Author = this.GetUserName(),
                                 AuthorId = this.GetUserNameIdentifier(),
                                 CreationDate = DateTime.Now
                             };
                _recipeRepo.SetRecipe(recipe);
            }

            return View(recipe);
        }


        //
        // GET: /BlankRecipeEditor/RecipeEditor

        [HttpGet]
        public ActionResult RecipeEditor()
        {
            Thread.Sleep(1000);
            return PartialView();
        }
        

        //
        // POST: /BlankRecipeEditor/Save

        [HttpPost]
        public JsonResult Save(TextRecipe recipe)
        {
            recipe.Author = this.GetUserName();
            recipe.AuthorId = this.GetUserNameIdentifier();

            // The recipe must belong to the current user.
            string oldAuthorId = _recipeRepo.GetRecipe(recipe.Id).AuthorId;
            if (oldAuthorId != recipe.AuthorId)
            {
                throw new InvalidOperationException("Invalid author. The author must not change.");
            }

            _recipeRepo.SetRecipe(recipe);

            Thread.Sleep(1000);
            //  throw new ArgumentException("Bad things happended");
            return Json(recipe);
        }        
    }
}
