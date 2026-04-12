using AsadaLisboaBackend.Utils;
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
                if (!controller.RouteValues.TryGetValue("area", out var area) || (area != "Admin"))
                    continue;

                foreach (var controllerActions in controller.Actions)
                {
                    var httpMethods = controllerActions.Selectors
                        .SelectMany(selector => selector.ActionConstraints ?? [])
                        .OfType<HttpMethodActionConstraint>()
                        .SelectMany(constraint => constraint.HttpMethods)
                        .ToList();

                    if (!httpMethods.Any())
                        continue;

                    if (httpMethods.Contains("GET", StringComparer.OrdinalIgnoreCase))
                        controllerActions.Filters.Add(new AuthorizeFilter(Constants.ROLE_LECTOR));

                    else if (httpMethods.Contains("POST", StringComparer.OrdinalIgnoreCase) || httpMethods.Contains("PUT", StringComparer.OrdinalIgnoreCase) || httpMethods.Contains("PATCH",  StringComparer.OrdinalIgnoreCase))
                        controllerActions.Filters.Add(new AuthorizeFilter(Constants.ROLE_EDITOR));

                    else if (httpMethods.Contains("DELETE", StringComparer.OrdinalIgnoreCase))
                        controllerActions.Filters.Add(new AuthorizeFilter(Constants.ROLE_ADMINISTRADOR));
                }
            }
        }
    }
}
