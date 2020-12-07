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
            var funcUrl = $"https://bccg-ns-test-func.azurewebsites.net/api/checktargetstatus?mode={mode}&code=woxKa6bScWocvrGV6zZIjoOoHdVI3V5yxWz1bhekISzzFuafL5GkKg==";

            if (mode.Contains("-d"))
            {
                funcUrl = $"http://localhost:7071/api/CheckTargetStatus/?mode={mode}";
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