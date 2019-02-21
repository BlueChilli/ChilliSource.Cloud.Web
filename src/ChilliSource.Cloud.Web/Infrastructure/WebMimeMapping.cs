#if NET_4X
using ChilliSource.Cloud.Core;
using System.Web;

namespace ChilliSource.Cloud.Web
{
    public class WebMimeMapping : IMimeMapping
    {
        public string GetMimeType(string fileName)
        {
            return MimeMapping.GetMimeMapping(fileName);
        }
    }
}
#else
using ChilliSource.Cloud.Core;
using Microsoft.AspNetCore.StaticFiles;

namespace ChilliSource.Cloud.Web
{
    public class WebMimeMapping : IMimeMapping
    {
        public string GetMimeType(string fileName)
        {
            string contentType = null;
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);
            return contentType ?? "application/octet-stream";
        }
    }
}
#endif