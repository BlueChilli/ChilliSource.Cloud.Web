using ChilliSource.Cloud.Core;
using ChilliSource.Core.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

#if NET_4X
using System.Web;
#else
using Microsoft.AspNetCore.Http;
#endif

namespace ChilliSource.Cloud.Web
{
    public static class StorageCommandWebExtensions
    {
#if NET_4X
        public static StorageCommand SetHttpPostedFileSource(this StorageCommand command, HttpPostedFileBase file)
#else
        public static StorageCommand SetHttpPostedFileSource(this StorageCommand command, IFormFile file)
#endif        
        {
            var source = StorageCommand.CreateSourceProvider(async () =>
            {
                command.Extension = command.Extension.DefaultTo(Path.GetExtension(file.FileName));
                command.ContentType = command.ContentType.DefaultTo(file.ContentType);
                return await GetFileStreamAsync(file).IgnoreContext();
            }, true);

            return command.SetSourceProvider(source);
        }

#if NET_4X
        private static async Task<MemoryStream> GetFileStreamAsync(HttpPostedFileBase file)
         {
            int? fileLength = null;
            try
            {
                fileLength = file.ContentLength;
            }
            catch {/* noop */ }

            var memStream = fileLength == null ? new MemoryStream() : new MemoryStream(fileLength.Value);

            var bufferSize = Math.Min(32 * 1024, fileLength ?? 32 * 1024);
            await file.InputStream.CopyToAsync(memStream, bufferSize);
            memStream.Position = 0;
            return memStream;
        }
#else
        private static async Task<MemoryStream> GetFileStreamAsync(IFormFile file)
        {
            long? fileLength = null;
            try
            {
                fileLength = file.Length;
            }
            catch {/* noop */ }

            var memStream = fileLength == null ? new MemoryStream() : new MemoryStream((int)fileLength.Value);
            await file.CopyToAsync(memStream);
            memStream.Position = 0;
            return memStream;
        }
#endif
    }
}
