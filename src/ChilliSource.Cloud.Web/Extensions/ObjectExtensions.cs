using ChilliSource.Core.Extensions;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;

namespace ChilliSource.Cloud.Web
{
    /// <summary>
    /// Extension methods for System.Object.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts object to System.Web.Routing.RouteValueDictionary.
        /// </summary>
        /// <param name="value">Object to convert.</param>
        /// <returns>A System.Web.Routing.RouteValueDictionary.</returns>
        public static RouteValueDictionary ToRouteValues(this object value)
        {
            if (value == null) return new RouteValueDictionary();
            if (value is RouteValueDictionary) return new RouteValueDictionary(value as RouteValueDictionary);

            return new RouteValueDictionary(value.ToDynamic() as IDictionary<string, object>);
        }
    }
}
