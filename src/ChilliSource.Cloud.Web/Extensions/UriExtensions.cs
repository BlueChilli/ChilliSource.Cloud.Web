using ChilliSource.Core.Extensions;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Specialized;
using System.Web;

namespace ChilliSource.Cloud.Web
{
    public static class UriExtensions
    {
        /// <summary>
        /// Parse the query string portion of an Uri into a NameValueCollection
        /// </summary>
        /// <param name="uri">this</param>
        /// <returns>NameValueCollection containing query parameters</returns>
        public static NameValueCollection ParseQuery(this Uri uri)
        {
            return uri.Query.ParseQueryString();
        }

        /// <summary>
        /// Add an object to the query string of an Uri
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="parameters">Object containing key/value pairs to add. Each value must be able to be represented as a string</param>
        /// <returns>Uri with objects properties/values merged in</returns>
        public static Uri AddQuery(this Uri uri, object parameters)
        {
            var newQueryString = uri.ParseQuery().AddQuery(parameters).ToQueryString();

            return new Uri(uri.Base() + newQueryString);
        }

        /// <summary>
        /// Add an object to the query string of an Uri. Main differnce to AddQuery is that of the name of the parameter is 'id' , it will be added to the route parameters instead of the query string.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="parameters">Object containing key/value pairs to add. Each value must be able to be represented as a string</param>
        /// <returns>Uri with objects properties/values merged in</returns>
        public static Uri AddRouteQuery(this Uri uri, object parameters)
        {
            var data = uri.ParseQuery().AddQuery(parameters);

            if (data["id"] is not null)
            {
                uri = new Uri(uri.GetLeftPart(UriPartial.Path) + "/" + data["id"]);
                data.Remove("id");
            }

            var newQueryString = data.ToQueryString();

            return new Uri(uri.Base() + newQueryString);
        }

    }
}
