using ChilliSource.Core.Extensions;
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
       
        public static Uri Parse(string url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                return new Uri(url);
                        
            return new Uri(GlobalWebConfiguration.Instance.BaseUrl + VirtualPathUtility.ToAbsolute(url));
        }
    }
}
