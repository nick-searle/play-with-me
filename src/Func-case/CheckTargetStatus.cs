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
using PlayWithMe.Common.Helpers;
using PlayWithMe.Common.Models;
using PlayWithMe.Common.Services;
using System.Web.Http;

namespace PlayWithMe.Func
{
    public static class CheckTargetStatus
    {
        private static readonly HttpHelper httpHelper = new HttpHelper();
        
        [FunctionName("CheckTargetStatus")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log,
            ExecutionContext context)
        {
            try
            {
                var mode = ((string)req.Query["mode"]) ?? string.Empty;

                var rawResponse = httpHelper.GetResponse("https://www.target.com");
                var responseHeaders = rawResponse.Headers;
                var key = httpHelper.GetCookie(responseHeaders, "visitorId");

                if (key == null)
                {
                    throw new ArgumentException("No key provided");
                }

                string searchResults;
                if (!mode.Contains("-l"))
                {
                    searchResults = File.ReadAllText(Path.Combine(context.FunctionAppDirectory, "requests/target/target-search-response.json"));
                }
                else
                {
                    var searchTerm = "sony playstation consoles";
                    searchResults = httpHelper.GetStringResponse($"https://redsky.target.com/v2/plp/search/?channel=web&count=96&keyword={searchTerm}&offset=0&pricing_store_id=3991&key={key}");
                }

                var items = new List<TargetSearchItem>();
                foreach (var item in JsonConvert.DeserializeObject<JObject>(searchResults)["search_response"]["items"]["Item"])
                {
                    var searchItem = new TargetSearchItem
                    {
                        Title = item["title"]?.ToString(),
                        Tcin = item["tcin"]?.ToString(),
                        Dpci = item["dpci"]?.ToString(),
                        Upc = item["upc"]?.ToString(),
                        Url = "https://www.target.com" + (item["url"]?.ToString() ?? ""),
                        Status = item["availability_status"]?.ToString()
                    };
                    
                    if (searchItem.Status != "IN_STOCK")
                    {
                        searchItem.Status = item["loyalty_availability_status"]?.ToString();
                    }

                    searchItem.Instock = searchItem.Status == "IN_STOCK";

                    items.Add(searchItem);
                }

                items = items.Where(i => i.Title.Contains("playstation", StringComparison.InvariantCultureIgnoreCase)
                    && i.Title.Contains("5")
                    && i.Title.Contains("console", StringComparison.InvariantCultureIgnoreCase)).ToList();

                var responseMessage = JsonConvert.SerializeObject(items);

                return new OkObjectResult(responseMessage);
            } 
            catch (Exception ex)
            {
                return new ExceptionResult(ex, true);
            }            
        }
    }
}
