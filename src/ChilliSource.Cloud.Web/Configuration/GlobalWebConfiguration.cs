using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChilliSource.Cloud.Web
{
    public class GlobalWebConfiguration
    {
        private static readonly GlobalWebConfiguration _instance = new GlobalWebConfiguration();
        public static GlobalWebConfiguration Instance { get { return _instance; } }

        string _baseUrl = null;
        public string BaseUrl
        {
            get
            {
                if (String.IsNullOrEmpty(_baseUrl))
                    throw new ApplicationException("Base Url not set.");

                return _baseUrl;
            }
            set { _baseUrl = value; }
        }
    }
}
