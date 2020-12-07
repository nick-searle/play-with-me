using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using play_with_me.common.Helpers;
using play_with_me.common.Models;


namespace console.Services
{
    public class TargetService
    {
        private static readonly HttpHelper httpHelper = new HttpHelper();

        public List<SearchItem> CheckTarget(string mode)
        {
            var rawResponse = httpHelper.GetResponse("https://www.target.com");
            var responseHeaders = rawResponse.Headers;
            var visitorId = httpHelper.GetCookie(responseHeaders, "visitorId");
            Console.WriteLine($"Key: {visitorId}");
            Console.WriteLine();

            var funcUrl = $"https://bccg-ns-test-func.azurewebsites.net/api/checktargetstatus?mode={mode}&key={visitorId}&code=woxKa6bScWocvrGV6zZIjoOoHdVI3V5yxWz1bhekISzzFuafL5GkKg==";

            if (mode.Contains("-d"))
            {
                funcUrl = $"http://localhost:7071/api/CheckTargetStatus/?mode={mode}&key={visitorId}";
            }

            Console.WriteLine($"Target func: {funcUrl}");
            Console.WriteLine();

            var response = httpHelper.GetStringResponse(funcUrl);
            Console.WriteLine($"Target func response: {response}");
            Console.WriteLine();
            
            var items = JsonConvert.DeserializeObject<List<SearchItem>>(response);
            Console.WriteLine($"Target Response: {JsonConvert.SerializeObject(items)}");
            Console.WriteLine();

            return items;
        }
    }
}