using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using play_with_me.common.Models;
using play_with_me.common.Helpers;

namespace console.Services
{
    public class GamestopService
    {
        private static readonly HttpHelper httpHelper = new HttpHelper();

        public List<SearchItem> CheckGamestop(string mode)
        {
            var funcUrl = $"https://bccg-ns-test-func.azurewebsites.net/api/checkgamestopstatus?mode={mode}&code=5t2DeTTf4aslgSY2TUy3QDOfEeGO9muNvP4nPH8tZRRb6A8OTHEBPA==";

            if (mode.Contains("-d"))
            {
                funcUrl = $"http://localhost:7071/api/CheckGamestopStatus/?mode={mode}";
            }

            Console.WriteLine($"Gamestop func: {funcUrl}");
            Console.WriteLine();

            var response = httpHelper.GetStringResponse(funcUrl);
            Console.WriteLine($"Gamestop func response: {response}");
            Console.WriteLine();
            
            var items = JsonConvert.DeserializeObject<List<SearchItem>>(response);
            Console.WriteLine($"Gamestop Response: {JsonConvert.SerializeObject(items)}");
            Console.WriteLine();
            
            return items;
        }
    }
}