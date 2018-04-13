using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using Xunit;

namespace ChilliSource.Cloud.Web.Tests
{
    public class UriExtensionTests
    {
        [Fact]
        public void ParseQuery_ShouldQueryStringParameters_InANamedValueCollection()
        {
            var uri = new Uri("https://www.mysite.com/something/1?A=1&A=2&B=up&C=down");

            var result = uri.ParseQuery();

            Assert.Equal(result["A"], "1,2");
            Assert.Equal(result["B"], "up");
            Assert.Equal(result["C"], "down");

            var uri2 = new Uri("https://www.mysite.com/something/1");

            var result2 = uri2.ParseQuery();

            Assert.False(result2.HasKeys());
        }

        [Fact]
        public void ParseQuery_ShouldReturnCorrect_Uri()
        {
            // Fake out env for VirtualPathUtility.ToAbsolute(..)
            string path = AppDomain.CurrentDomain.BaseDirectory;
            const string virtualDir = "/";
            AppDomain.CurrentDomain.SetData(".appDomain", "*");
            AppDomain.CurrentDomain.SetData(".appPath", path);
            AppDomain.CurrentDomain.SetData(".appVPath", virtualDir);
            AppDomain.CurrentDomain.SetData(".hostingVirtualPath", virtualDir);
            AppDomain.CurrentDomain.SetData(".hostingInstallDir", HttpRuntime.AspInstallDirectory);
            TextWriter tw = new StringWriter();
            HttpWorkerRequest wr = new SimpleWorkerRequest("default.aspx", "", tw);
            HttpContext.Current = new HttpContext(wr);

            GlobalWebConfiguration.Instance.BaseUrl = "https://localhost/Tests";
            var url = "/Admin/User/Users";
            var result = UriExtensions.Parse(url).AbsoluteUri;
            Assert.Equal("https://localhost/Admin/User/Users", result);

            GlobalWebConfiguration.Instance.BaseUrl = "http://www.mysite.com";
            result = UriExtensions.Parse(url).AbsoluteUri;
            Assert.Equal("http://www.mysite.com/Admin/User/Users", result);

            url = "~/Admin/User/Users";
            result = UriExtensions.Parse(url).AbsoluteUri;
            Assert.Equal("http://www.mysite.com/Admin/User/Users", result);

            GlobalWebConfiguration.Instance.BaseUrl = "https://localhost/Tests";
            result = UriExtensions.Parse(url).AbsoluteUri;
            Assert.Equal("https://localhost/Tests/Admin/User/Users", result);

            GlobalWebConfiguration.Instance.BaseUrl = "https://localhost/Tests/";
            url = "/tests/Admin/User/Users";
            result = UriExtensions.Parse(url).AbsoluteUri;
            Assert.Equal("https://localhost/tests/Admin/User/Users", result);
        }

        [Fact]
        public void AddQuery_ShouldAddQuery_AndReturnNewUri()
        {
            var uri = new Uri("https://www.mysite.com/something/1?A=1&A=2&B=up&C=down");

            var result = uri.AddQuery(new { A = 3, D = "straight" });

            Assert.Equal("?A=1&A=2&A=3&B=up&C=down&D=straight", result.Query);

            var uri2 = new Uri("https://www.mysite.com/something/1");

            var result2 = uri2.AddQuery(new { A = 3, D = "straight", C = "" });

            Assert.Equal("?A=3&D=straight", result2.Query);

        }
    }
}
