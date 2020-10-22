using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace AureoleCore.Utils
{
    public static class Net
    {
        private static string Host = "http://192.168.1.69:5000/api";

        public static T Get<T>(string method, Dictionary<string, object> _params = null)
            where T : class
        {
            string _params_string = "";

            if (_params != null)
            {
                bool first = true;
                foreach (var pair in _params)
                {
                    if (!first)
                    {
                        _params_string += "&";
                    }

                    _params_string += $"{pair.Key}={pair.Value}";
                }
            }

            string url = $"{Host}/{method}?{_params_string}";

            string result = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                result = wc.DownloadString(url);
            }

            return JsonConvert.DeserializeObject<T>(result);
        }

        public static T Post<T>(string method, object _obj)
            where T : class
        {
            string url = $"{Host}/{method}";

            string result = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                result = wc.UploadString(url, JsonConvert.SerializeObject(_obj));
            }

            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}