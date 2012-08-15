using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.IdentityModel.Claims;
using RecipeWebRole.ActionFilters;
using RecipeWebRole.DataAccess;
using RecipeWebRole.Models;
using RecipeWebRole.Utilities;

namespace RecipeWebRole.Controllers
{
    [ActionFilters.Authorize]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login
        // Without view -> use WSFederationAuthenticationModule to redirect to Azure ACS
        
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // User is not authenticated.
                // WSFederationAuthenticationModule is configured to kick in
                // when in EndRequest a request indicates "unauthorized access".
                // Therefore, the following call will start the login process,
                // and after completion, will come back to this address.
                return new HttpUnauthorizedResult();
            }

            // The user successfully authenticated.

            string nameIdentifier = GetUserNameIdentifier();
            if (!UserRepository.Instance.IsExistingUser(nameIdentifier))
            {
                // New user. Redirect to page for recording user name.
                var queryString = new QueryString { { "returnUrl", returnUrl } };
                return new RedirectResult("/Account/UserData" + queryString);
            }

            // user exists.
            // Redirect back to the page the user was querying.
            return new RedirectResult(returnUrl);
        }

        //
        // GET: /Account/RequiresLogin

        [HttpGet]
        [AllowAnonymous]
        public ActionResult RequiresLogin(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        //
        // GET: /Account/Register

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }



        //
        // GET: /Account/UserData

        [HttpGet]
        public ActionResult UserData(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/UserData

        [HttpPost]
        public ActionResult UserData(string username, string returnUrl)
        {
            //string username = Request.Form["username"];
            
            string userIdentifier = GetUserNameIdentifier();

            // TODO: validate that username is valid!
            UserRepository.Instance.AddUser(new User { Id = userIdentifier, Name = username });
            
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            // If we got this far, something failed
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            throw new NotImplementedException();

            return RedirectToAction("Index", "Home");
        }


        private string GetUserNameIdentifier()
        {
            var identity = (IClaimsIdentity)User.Identity;
            Claim claim = identity.Claims.FirstOrDefault(c => c.ClaimType == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                throw new ArgumentException("Invalid Claim Identity. Need at least name identifier.");
            }

            return claim.Value ?? String.Empty;
        }     
    }
}
