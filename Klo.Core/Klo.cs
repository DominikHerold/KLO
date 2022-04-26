using System;
using System.Collections.Generic;

namespace Klo.Core
{
    public interface IKlo
    {
        bool? IsInUse();
    }

    public class Klo : IKlo
    {
        private readonly IWebClientWrapper _webClient;

        private readonly string _address;

        private readonly Dictionary<string, string> _optionalHeaders;
        
        public Klo(IWebClientWrapper webClient, string address)
        {
            _webClient = webClient;
            _address = address;
            _optionalHeaders = new Dictionary<string, string> { { "Content-Type", "application/xml" } };
        }
        
        public bool? IsInUse()
        {
            try
            {
                return _webClient.DownloadString(_address, _optionalHeaders).Contains("true");
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
