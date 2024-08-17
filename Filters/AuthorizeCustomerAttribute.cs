using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using MvcMCBA.Models;

namespace MvcMCBA.Filters;

public class AuthorizeCustomerAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Skip authorization check if the [AllowAnonymous] attribute is present.
        if (context.ActionDescriptor.EndpointMetadata.Any(x => x is AllowAnonymousAttribute))
            return;

        var customerID = context.HttpContext.Session.GetInt32(nameof(Customer.CustomerID));
        if (!customerID.HasValue)
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}
