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
            var funcUrl = $"https://bccg-ns-test-func.azurewebsites.net/api/checkneweggstatus?mode={mode}&code=uIsqGlUhAv7FVZhIHaJin6U4A050ak0l2ucHnkq6sCaajUCyBAR/jw==";

            if (mode.Contains("-d"))
            {
                funcUrl = $"http://localhost:7071/api/CheckGamestopStatus/?mode={mode}";
            }

            Console.WriteLine($"Newegg func: {funcUrl}");
            Console.WriteLine();

            var response = httpHelper.GetStringResponse(funcUrl);
            Console.WriteLine($"Newegg func response: {response}");
            Console.WriteLine();
            
            var items = JsonConvert.DeserializeObject<List<SearchItem>>(response);
            Console.WriteLine($"Newegg Response: {JsonConvert.SerializeObject(items)}");
            Console.WriteLine();
            
            return items;
        }
    }
}