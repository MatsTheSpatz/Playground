using System.Web;
using System.Web.Mvc;
using RecipeWebRole.Utilities;

namespace RecipeWebRole.ActionFilters
{
    public class AuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        /// <summary>
        /// Processes HTTP requests that fail authorization.
        /// Redirect unauthorized requests to Account/Login.
        /// </summary>
        /// <param name="filterContext">
        /// Encapsulates the information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute"/>. 
        /// The <paramref name="filterContext"/> object contains the controller, HTTP context, request context, action result, 
        /// and route data.
        /// </param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            HttpRequestBase request = filterContext.RequestContext.HttpContext.Request;

            string queryString = string.Empty;
            if (request.Url != null)
            {
                queryString = new QueryString {{"returnUrl", request.Url.PathAndQuery}}.ToString();
            };
            
            filterContext.Result = new RedirectResult("/Account/Login" + queryString);
        }
    }
}