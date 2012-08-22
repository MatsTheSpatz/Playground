using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.IdentityModel.Claims;

namespace RecipeWebRole.Utilities
{
    public static class ControllerExtensions
    {
        public static string GetUserName(this Controller controller) 
        {
            var identity = (IClaimsIdentity)controller.User.Identity;
            return identity.Label;
        }

        public static string GetUserNameIdentifier(this Controller controller)
        {
            var identity = (IClaimsIdentity)controller.User.Identity;
            Claim claim = identity.Claims.FirstOrDefault(c => c.ClaimType == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                throw new ArgumentException("Invalid Claim Identity. Need at least name identifier.");
            }

            return claim.Value ?? String.Empty;
        }     
    }
}
