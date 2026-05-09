using AsadaLisboaBackend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace AsadaLisboaBackend.Conventions
{
    /// <summary>
    /// Convention for adding Authorize Filters to Action Controllers.
    /// </summary>
    public class AuthorizationConvention : IApplicationModelConvention
    {
        /// <summary>
        /// Apply the AuthorizeFilter into actions.
        /// </summary>
        /// <param name="application">Model to configure controllers.</param>
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var areaAttribute = controller.Attributes
                    .OfType<AreaAttribute>()
                    .FirstOrDefault();

                if (areaAttribute?.RouteValue != "Admin")
                    continue;

                foreach (var action in controller.Actions)
                {
                    // if there's already an AuthorizeAttribute, skip this action
                    var hasAuthorize =
                        action.Attributes.OfType<AuthorizeAttribute>().Any() ||
                        controller.Attributes.OfType<AuthorizeAttribute>().Any();

                    if (hasAuthorize)
                        continue;

                    var methods = action.Selectors
                        .SelectMany(s => s.ActionConstraints ?? [])
                        .OfType<HttpMethodActionConstraint>()
                        .SelectMany(c => c.HttpMethods)
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();

                    if (!methods.Any())
                        continue;

                    string? policy = null;

                    if (methods.Contains("DELETE", StringComparer.OrdinalIgnoreCase))
                    {
                        policy = Constants.ROLE_ADMINISTRADOR;
                    }
                    else if (
                        methods.Contains("POST", StringComparer.OrdinalIgnoreCase) ||
                        methods.Contains("PUT", StringComparer.OrdinalIgnoreCase) ||
                        methods.Contains("PATCH", StringComparer.OrdinalIgnoreCase))
                    {
                        policy = Constants.ROLE_ESCRITOR;
                    }
                    else if (methods.Contains("GET", StringComparer.OrdinalIgnoreCase))
                    {
                        policy = Constants.ROLE_LECTOR;
                    }

                    if (policy is not null)
                    {
                        action.Filters.Add(new AuthorizeFilter(policy));
                    }
                }
            }
        }
    }
}
