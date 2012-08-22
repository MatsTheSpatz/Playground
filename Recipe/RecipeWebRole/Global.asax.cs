using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Web;
using RecipeWebRole.Controllers;
using RecipeWebRole.DataAccess;
using RecipeWebRole.Models;

namespace RecipeWebRole
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ControllerBuilder.Current.SetControllerFactory(new CustomControllerFactory());
        }

        /// <summary>
        /// Module Event Hanlder.
        /// Handles the SessionSecurityTokenReceived event of the SessionAuthenticationModule.
        /// See 'Programming WIF', p.74 for more details.
        /// </summary>
        void SessionAuthenticationModule_SessionSecurityTokenReceived(object sender, SessionSecurityTokenReceivedEventArgs e)
        {
            var identity = (IClaimsIdentity)e.SessionToken.ClaimsPrincipal.Identity;
            if (String.IsNullOrEmpty(identity.Label))
            {
                Claim nameIdentifierClaim = identity.Claims.FirstOrDefault(c => c.ClaimType == ClaimTypes.NameIdentifier);
                User user;
                if (UserRepository.Instance.TryGetUser(nameIdentifierClaim.Value, out user))
                {
                    identity.Label = user.Name;
                }
            }
        }

        /// <summary>
        /// Module Event Hanlder.
        /// Handles the SessionSecurityTokenCreated event of the WSFederationAuthenticationModule control.
        /// See 'Programming WIF', p.74 for more details.
        /// </summary>
        void WSFederationAuthenticationModule_SessionSecurityTokenCreated(object sender, SessionSecurityTokenCreatedEventArgs e)
        {
            var identity = (IClaimsIdentity)e.SessionToken.ClaimsPrincipal.Identity;

            var userClaim = new Claim(ClaimTypes.UserData, "local per-session userdata (injected once)", ClaimValueTypes.String, "(local)");
            identity.Claims.Add(userClaim);

            Claim nameIdentifierClaim = identity.Claims.FirstOrDefault(c => c.ClaimType == ClaimTypes.NameIdentifier);
            User user;
            if (UserRepository.Instance.TryGetUser(nameIdentifierClaim.Value, out user))
            {
                identity.Label = user.Name;
            }
        }

        /// <summary>
        /// Module Event Hanlder.
        /// Handles the RedirectingToIdentityProvider event of the WSFederationAuthenticationModule control.
        /// See 'Programming WIF', p.74 for more details.
        /// </summary>
        void WSFederationAuthenticationModule_RedirectingToIdentityProvider(object sender, RedirectingToIdentityProviderEventArgs e)
        {
        }

        void WSFederationAuthenticationModule_SignedIn(object sender, EventArgs e)
        {
        }

        void WSFederationAuthenticationModule_SignedOut(object sender, EventArgs e)
        {
        }
    }
}