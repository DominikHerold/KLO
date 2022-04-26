using System.Collections.Generic;
using System.Net;

namespace Klo.Core
{
    public interface IWebClientWrapper
    {
        string DownloadString(string address, IDictionary<string, string> optionalHeaders);
    }

    public class WebClientWrapper : IWebClientWrapper
    {
        public string DownloadString(string address, IDictionary<string, string> optionalHeaders)
        {
            using (var webClient = new WebClient())
            {
                foreach (var header in optionalHeaders)
                    webClient.Headers.Set(header.Key, header.Value);

                return webClient.DownloadString(address);
            }
        }
    }
}
