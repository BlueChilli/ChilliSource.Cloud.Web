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
        {
            var source = StorageCommand.CreateSourceProvider(async (cancellationToken) =>
            {
                command.Extension = command.Extension.DefaultTo(Path.GetExtension(file.FileName));
                command.ContentType = command.ContentType.DefaultTo(file.ContentType);
                return await GetFileStreamAsync(file, cancellationToken).IgnoreContext();
            }, true);

            return command.SetSourceProvider(source);
        }

        private static async Task<MemoryStream> GetFileStreamAsync(HttpPostedFileBase file, CancellationToken cancellationToken)
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
        public static StorageCommand SetHttpPostedFileSource(this StorageCommand command, IFormFile file)
        {
            var source = StorageCommand.CreateSourceProvider(async (cancellationToken) =>
            {
                command.Extension = command.Extension.DefaultTo(Path.GetExtension(file.FileName));
                command.ContentType = command.ContentType.DefaultTo(file.ContentType);
                return await GetFileStreamAsync(file, cancellationToken).IgnoreContext();
            }, true);

            return command.SetSourceProvider(source);
        }

        private static async Task<MemoryStream> GetFileStreamAsync(IFormFile file, CancellationToken cancellationToken)
        {
            long? fileLength = null;
            try
            {
                fileLength = file.Length;
            }
            catch {/* noop */ }

            var memStream = fileLength == null ? new MemoryStream() : new MemoryStream((int)fileLength.Value);
            await file.CopyToAsync(memStream, cancellationToken);
            memStream.Position = 0;
            return memStream;
        }
#endif
    }
}
