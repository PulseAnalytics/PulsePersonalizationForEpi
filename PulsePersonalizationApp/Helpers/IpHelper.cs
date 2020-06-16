using EPiServer;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace PulsePersonalizationApp.Helpers
{
    public class IpHelper
    {
        public static string GetIPAddress(HttpRequestBase request)
        {
            var requestIp = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrWhiteSpace(requestIp))
            {
                requestIp = request.ServerVariables["REMOTE_ADDR"];
            }
            if (requestIp.Contains(":"))
            {
                //Port number is included, disregard it
                requestIp = requestIp.Substring(0, requestIp.IndexOf(':'));
            }
            if (!requestIp.Contains(".") || requestIp == "127.0.0.1")
            {
                requestIp = GetLocalRequestIp();
            }
            return requestIp;
        }

        private static string GetLocalRequestIp()
        {
            var requestIp = CacheManager.Get("local_ip") as string;
            if (!string.IsNullOrWhiteSpace(requestIp))
            {
                return requestIp;
            }
            var lookupRequest = WebRequest.Create("http://ipinfo.io/ip/");
            var webResponse = lookupRequest.GetResponse();
            using (var responseStream = webResponse.GetResponseStream())
            {
                var streamReader = new StreamReader(responseStream, Encoding.UTF8);
                requestIp = streamReader.ReadToEnd().Trim();
            }
            webResponse.Close();
            CacheManager.Insert("local_ip", requestIp);
            return requestIp;
        }
    }
}