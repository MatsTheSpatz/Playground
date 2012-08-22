﻿using System;
using System.Collections.Generic;
using System.Configuration;
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
    [ActionFilters.Authorize]
    public class HomeController : Controller
    {
        private readonly IRecipeRepository _recipeRepo;
        private readonly IUserRepository _userRepo;

        public HomeController(IRecipeRepository recipeRepo, IUserRepository userRepo)
        {
            _recipeRepo = recipeRepo;
            _userRepo = userRepo;
        }


        //
        // GET: /Home/Index

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {

            IList<int> ids = _recipeRepo.GetRecipeIds();
            IEnumerable<Tuple<int, string>> data = from id in ids
                                                   let name = _recipeRepo.GetRecipe(id).Name
                                                   select new Tuple<int, string>(id, name);
            return View(data);
        }


        //
        // GET: /Home/Get

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Get(int recipeId)
        {
            Recipe recipe = _recipeRepo.GetRecipe(recipeId);
            if (recipe == null)
            {
                throw new ArgumentException("Invalid recipe-Id " + recipeId);
            }

            return View(recipe as TextRecipe);
        }


        //
        // GET: /Home/SearchRecipe

        [HttpGet]
        [AllowAnonymous]
        public ActionResult SearchRecipe()
        {
            return View();
        }


        //
        // GET: /Home/CreateRecipe

        [HttpGet]
        public ActionResult CreateRecipe()
        {
            return View();
        }


        //
        // GET: /Home/UserProfile

        [HttpGet]
        public ActionResult UserProfile()
        {
            string userId = this.GetUserNameIdentifier();
            User user;
            if (!_userRepo.TryGetUser(userId, out user))
            {
                throw new InvalidOperationException("User Profile does not exist for this user.");
            }

            return View(user);
        }


        //
        // GET: /Home/MyRecipes

        [HttpGet]
        public ActionResult MyRecipes()
        {
            string userId = this.GetUserNameIdentifier();

            IList<int> ids = _recipeRepo.GetRecipeIds();
            IEnumerable<Tuple<int, string>> data = from id in ids
                                                   let recipe = _recipeRepo.GetRecipe(id)
                                                   where recipe.AuthorId == userId
                                                   select new Tuple<int, string>(recipe.Id, recipe.Name);

            return View("Index", data);
        }
    }
}
