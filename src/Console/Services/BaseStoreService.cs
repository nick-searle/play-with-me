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
        private int consecutiveFails = 0;
        private DateTime? lastError = null;

        public BaseStoreService(string storeName, string methodName, string key)
        {
            this.storeName = storeName;
            this.methodName = methodName;
            this.key = key;
        }

        private int GetWaitTime()
        {
            switch (consecutiveFails)
            {
                case 1:
                    return 1;
                case 2:
                    return 2;
                case 3:
                    return 8;
                default:
                    return 24;
            }
        }

        public List<SearchItem> GetItemStatuses(string mode)
        {
            var items = new List<SearchItem>();
            try
            {                
                if (consecutiveFails > 0)
                {
                    var timeSinceLastError = DateTime.Now - lastError;
                    var waitTime = GetWaitTime();

                    if (timeSinceLastError.Value.TotalHours < waitTime)
                    {
                        log.Error($"{storeName} Fails Count {consecutiveFails} Waiting: {waitTime}");
                        log.Error("");
                        return items;
                    }
                }

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
                
                items = JsonConvert.DeserializeObject<List<SearchItem>>(response);
                log.Info($"{storeName} Response: {JsonConvert.SerializeObject(items)}");
                log.Info("");

                return items;
            }
            catch (Exception ex)
            {
                consecutiveFails++;
                lastError = DateTime.Now;
                log.Error(ex);
                return items;
            }
        }
    }
}