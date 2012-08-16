using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Claims;
using RecipeWebRole.ActionFilters;
using RecipeWebRole.DataAccess;
using RecipeWebRole.Models;
using RecipeWebRole.Utilities;

namespace RecipeWebRole.Controllers
{
  //  [ActionFilters.Authorize]
    public class HomeController : Controller
    {
        private readonly IRecipeRepository _recipeRepo;

        public HomeController(IRecipeRepository recipeRepo)
        {
            _recipeRepo = recipeRepo;
        }

        //
        // GET: /Home/Index

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            IList<int> ids = _recipeRepo.GetRecipeIds();
            IEnumerable<Tuple<int, string>> data = (from id in ids
                                                    let name = _recipeRepo.GetRecipe(id).Name
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
    }
}
