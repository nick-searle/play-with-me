using System;
using System.Collections.Generic;
using PlayWithMe.Common.Contracts;
using Newtonsoft.Json;
using PlayWithMe.Common.Helpers;
using PlayWithMe.Common.Models;
using log4net;
using System.Reflection;

namespace PlayWithMe.ConsoleApp.Services
{
    public class BaseStoreService : IStoreService
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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
            try
            {
                var funcUrl = $"https://bccg-ns-test-func.azurewebsites.net/api/{methodName}?mode={mode}&code={key}";

                if (mode.Contains("-d"))
                {
                    funcUrl = $"http://localhost:7071/api/{methodName}/?mode={mode}";
                }

                log.Info($"{storeName} func: {funcUrl}");
                log.Info("");

                var response = httpHelper.GetStringResponse(funcUrl);
                log.Info($"{storeName} func response: {response}");
                log.Info("");
                
                var items = JsonConvert.DeserializeObject<List<SearchItem>>(response);
                log.Info($"{storeName} Response: {JsonConvert.SerializeObject(items)}");
                log.Info("");

                return items;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return new List<SearchItem>();
            }
        }
    }
}