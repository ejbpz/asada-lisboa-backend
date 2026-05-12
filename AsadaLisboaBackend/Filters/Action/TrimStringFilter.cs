using System.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AsadaLisboaBackend.Models.Filters.ActionFilter
{
    /// <summary>
    /// Action filter that trims string properties of action arguments.
    /// </summary>
    public class TrimStringFilter : IActionFilter
    {
        /// <summary>
        /// Called before the action executes, trims string properties of action arguments.
        /// </summary>
        /// <param name="context">The context for the action executing.</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var arg in context.ActionArguments.Values)
            {
                if (arg is not null)
                    TrimStrings(arg);
            }
        }

        /// <summary>
        /// Called after the action executes. This implementation does nothing.
        /// </summary>
        /// <param name="context">The context for the action executed.</param>
        public void OnActionExecuted(ActionExecutedContext context) { }

        /// <summary>
        /// Normalizes an object by trimming all string properties. If a string is null or whitespace after trimming, it sets it to null.
        /// </summary>
        /// <param name="obj">The object to normalize.</param>
        private static void TrimStrings(object obj)
        {
            var properties = obj.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p =>
                    p.CanRead &&
                    p.CanWrite &&
                    p.PropertyType == typeof(string));

            foreach (var prop in properties)
            {
                var value = prop.GetValue(obj) as string;

                if (value is null)
                    continue;

                var normalized = value.Trim();

                prop.SetValue(
                    obj,
                    string.IsNullOrWhiteSpace(normalized)
                        ? null
                        : normalized);
            }
        }
    }
}
