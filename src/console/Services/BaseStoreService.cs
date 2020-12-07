using System;
using System.Collections.Generic;
using common.Contracts;
using Newtonsoft.Json;
using play_with_me.common.Helpers;
using play_with_me.common.Models;

namespace console.Services
{
    public class BaseStoreService : IStoreService
    {
        private static readonly HttpHelper httpHelper = new HttpHelper();
        private readonly string storeName;
        private readonly string methodName;
        private readonly string key;

        public BaseStoreService(string storeName, string methodName, string key)
        {
            this.storeName = storeName;
            this.methodName = methodName;
            this.key = key;
        }
        
        public List<SearchItem> GetItemStatuses(string mode)
        {
            var funcUrl = $"https://bccg-ns-test-func.azurewebsites.net/api/{methodName}?mode={mode}&code={key}";

            if (mode.Contains("-d"))
            {
                funcUrl = $"http://localhost:7071/api/{methodName}/?mode={mode}";
            }

            Console.WriteLine($"{storeName} func: {funcUrl}");
            Console.WriteLine();

            var response = httpHelper.GetStringResponse(funcUrl);
            Console.WriteLine($"{storeName} func response: {response}");
            Console.WriteLine();
            
            var items = JsonConvert.DeserializeObject<List<SearchItem>>(response);
            Console.WriteLine($"{storeName} Response: {JsonConvert.SerializeObject(items)}");
            Console.WriteLine();

            return items;
        }
    }
}