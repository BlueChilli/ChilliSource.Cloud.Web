using Microsoft.AspNetCore.Http;
using System;

namespace ChilliSource.Cloud.Web
{
    ///https://stackoverflow.com/questions/38437005/how-to-get-current-url-in-view-in-asp-net-core-1-0
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// before >> https://localhost:44304/information/about?param1=a&param2=b
        /// Request.GetUri(addQuery: false);
        /// after >> https://localhost:44304/information/about
        /// </summary>
        /// <param name="request">Request</param>
        /// <param name="addPath">Include path</param>
        /// <param name="addQuery">Include query string</param>
        public static Uri GetUri(this HttpRequest request, bool addPath = true, bool addQuery = true)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Port = request.Host.Port.GetValueOrDefault(80),
                Path = addPath ? request.Path.ToString() : default(string),
                Query = addQuery ? request.QueryString.ToString() : default(string)
            };
            return uriBuilder.Uri;
        }

    }
}
