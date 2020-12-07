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
    public static class CheckGamestopStatus
    {
        private static readonly HttpHelper httpHelper = new HttpHelper();
        
        [FunctionName("CheckGamestopStatus")]
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
                    
                    var raw = httpHelper.GetStringResponse($"https://www.gamestop.com/on/demandware.store/Sites-gamestop-us-Site/default/Product-Variation?dwvar_11108140_condition=New&pid=11108140&quantity=1&redesignFlag=true&rt=productDetailsRedesign",
                    null,
                    new Dictionary<string, string>
                    {
                        { "referer", "https://www.gamestop.com/video-games/playstation-5/consoles/products/playstation-5/11108140.html?condition=New" },
                        { "sec-fetch-dest", "empty" },
                        { "sec-fetch-mode", "cors" },
                        { "sec-fetch-site", "same-origin" },
                    });
                    ps5 = JsonConvert.DeserializeObject<JObject>(raw);
                }
                else
                {
                    ps5 = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(Path.Combine(context.FunctionAppDirectory, "requests/gamestop/gamestop-ps5.json")));
                }
                
                var item = new SearchItem
                {
                    Url = "https://www.gamestop.com" + (ps5["product"]["selectedProductUrl"]?.ToString() ?? ""),
                    Instock = (bool)ps5["product"]["available"],
                    Title = ps5["product"]["productName"]?.ToString()
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
