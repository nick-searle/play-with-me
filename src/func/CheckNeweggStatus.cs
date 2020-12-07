using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using play_with_me.common.Helpers;
using play_with_me.common.Models;
using play_with_me.common.Services;
using System.Web.Http;

namespace func
{
    public static class CheckNeweggStatus
    {
        private static readonly HttpHelper httpHelper = new HttpHelper();
        
        [FunctionName("CheckNeweggStatus")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log,
            ExecutionContext context)
        {
            try
            {
                var mode = ((string)req.Query["mode"]) ?? string.Empty;

                JObject ps5;
                if (mode.Contains("-l"))
                {
                    ps5 = JsonConvert.DeserializeObject<JObject>(httpHelper.GetStringResponse($"https://www.newegg.com/product/api/ProductRealtime?ItemNumber=68-110-292&RecommendItem=&BestSellerItemList=&IsVATPrice=true"));
                }
                else
                {
                    ps5 = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(Path.Combine(context.FunctionAppDirectory, "requests/newegg/newegg-ps5-response.json")));
                }
                
                var item = new SearchItem
                {
                    Url = "https://www.newegg.com/p/N82E16868110292?Description=ps5&cm_re=ps5-_-68-110-292-_-Product&quicklink=true",
                    Instock = (bool)ps5["MainItem"]["Instock"],
                    Title = ps5["MainItem"]["Description"]["Title"]?.ToString()
                };
                Console.WriteLine($"Newegg Response: {JsonConvert.SerializeObject(item)}");
                Console.WriteLine();
                
                return new OkObjectResult(new[] { item });
            } 
            catch (Exception ex)
            {
                return new ExceptionResult(ex, true);
            }            
        }
    }
}
