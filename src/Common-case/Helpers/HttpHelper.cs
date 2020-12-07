using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace PlayWithMe.Common.Helpers
{
    public class HttpHelper
    {
        private readonly static HttpClient client = new HttpClient(new HttpClientHandler { UseCookies = false });

        public string GetStringResponse(string url)
        {
            var rawResponse = client.GetAsync(url).Result;
            
            return rawResponse.Content.ReadAsStringAsync().Result;
        }

        public string GetStringResponse(string url, string cookie, Dictionary<string, string> headers)
        {
            if (headers == null)
            {
                headers = new Dictionary<string, string>();
            }

            var rawResponse = GetResponse(url, cookie, headers);
            
            return rawResponse.Content.ReadAsStringAsync().Result;
        }

        public HttpResponseMessage GetResponse(string url)
        {
            return client.GetAsync(url).Result;
        }

        public HttpResponseMessage GetResponse(string url, string cookie, Dictionary<string, string> headers)
        {
            var message = new HttpRequestMessage(HttpMethod.Get, url);
            
            foreach (var header in headers)
            {
                message.Headers.Add(header.Key, header.Value);
            }

            message.Headers.Add("cookie", cookie);
            return client.SendAsync(message).Result;
        }

        public HttpResponseMessage Post(string url, string postBody)
        {
            var data = new StringContent(postBody, Encoding.UTF8, "application/json");
            return client.PostAsync(url, data).Result;
        }

        public HttpResponseMessage Post(string url, string postBody, string cookie)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, url);
            message.Headers.Add("cookie", cookie);
            return client.SendAsync(message).Result;
        }

        public HttpResponseMessage Post(string url, string postBody, string cookie, Dictionary<string, string> headers)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, url);
            
            foreach (var header in headers)
            {
                message.Headers.Add(header.Key, header.Value);
            }

            message.Headers.Add("cookie", cookie);
            return client.SendAsync(message).Result;
        }

        public string GetHeader(HttpContentHeaders headers, string name)
        {
            IEnumerable<string> values;
            if (headers.TryGetValues(name, out values))
            {
                return values.First();
            }

            return null;
        }

        public List<string> GetHeader(HttpResponseHeaders headers, string name)
        {
            IEnumerable<string> values;
            if (headers.TryGetValues(name, out values))
            {
                return values.ToList();
            }

            return null;
        }

        public string GetCookie(HttpResponseHeaders headers, string name)
        {
            var rawCookies = GetHeader(headers, "Set-Cookie");

            if (rawCookies == null || !rawCookies.Any())
            {
                return null;
            }

            var cookies = rawCookies.Select(c => c.Split(';').First().Split('=')).ToDictionary(c => c[0], c => c[1]);

            string value;
            if (cookies.TryGetValue(name, out value))
            {
                return value;
            }

            return null;
        }
    }
}