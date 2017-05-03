using ChilliSource.Core.Extensions;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ChilliSource.Cloud.Web
{
    public static class NameValueCollectionExtensions
    {
        /// <summary>
        /// Parse a query string into a named value collection for easy manipulation
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns>Named value collection containing querystring parameters</returns>
        public static NameValueCollection ParseQueryString(this string queryString)
        {
            var parsed = QueryHelpers.ParseQuery(queryString);

            var nvc = new NameValueCollection();
            foreach (var item in parsed)
            {
                foreach(var value in item.Value)
                {
                    if(!String.IsNullOrEmpty(value))
                        nvc.Add(item.Key, value);
                }
            }
            return nvc;
        }

        /// <summary>
        /// Merge into a named value collection an objects properties/values
        /// </summary>
        /// <param name="nvc"></param>
        /// <param name="parameters">Object containing key/value pairs to add. Each value must be able to be represented as a string</param>
        /// <returns>NameValueCollection with objects properties/values merged in</returns>
        public static NameValueCollection AddQuery(this NameValueCollection nvc, object parameters)
        {
            var data = parameters.ToDictionary();

            foreach(var item in data)
            {
                if (item.Value != null)
                {
                    var s = item.Value.ToString();
                    if (!String.IsNullOrEmpty(s))
                        nvc.Add(item.Key, s);
                }
            }
            return nvc;
        }

        /// <summary>
        /// Output a name valued collection as a querystring.
        /// </summary>
        /// <param name="nvc"></param>
        /// <param name="parameters">Optionally merge in an object properties/values to the resulting querystring</param>
        /// <returns>A querystring containing key value pairs from the named value collection</returns>
        public static string ToQueryString(this NameValueCollection nvc, object parameters = null)
        {
            if (parameters != null)
            {
                nvc = nvc.AddQuery(parameters);
            }

            var qb = new QueryBuilder();
            foreach(var key in nvc.AllKeys)
            {
                qb.Add(key, nvc[key].Split(','));
            }
            return qb.ToString();
        }

    }
}
