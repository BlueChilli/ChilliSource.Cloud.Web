﻿#if NET_4X
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
        IContentTypeProvider _contentTypeProvider;

        public WebMimeMapping()
            :this(new FileExtensionContentTypeProvider())
        { }

        public WebMimeMapping(IContentTypeProvider contentTypeProvider)
        {
            _contentTypeProvider = contentTypeProvider;
        }

        public string GetMimeType(string fileName)
        {
            string contentType;
            if (!_contentTypeProvider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        public string GetExtension(string mimeType)
        {
            if (mimeType == "image/jpeg") return ".jpg";

            if (_contentTypeProvider is FileExtensionContentTypeProvider)
            {
                var provider = _contentTypeProvider as FileExtensionContentTypeProvider;

                foreach(var item in provider.Mappings)
                {
                    if (item.Value == mimeType) return item.Key;
                }
            }
            return ".bin";
        }
    }
}
#endif