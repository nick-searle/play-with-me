using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlayWithMe.Common.Helpers;
using PlayWithMe.Common.Models;
using PlayWithMe.Common.Services;
using PlayWithMe.ConsoleApp.Services;

namespace PlayWithMe.ConsoleApp
{
    class Program
    {
        // -l = live, -e = email, -t = test-features, -d = local-dev
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string mode = "";
        private static readonly HttpHelper httpHelper = new HttpHelper();

        static void Main(string[] args)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
 
            if (args.Any())
            {
                mode = string.Join(' ', args.Select(a => a.ToLower()));
            }

            if (mode.Contains("-t"))
            {
                AddTargetCart(null);
                return;
            }

            log.Info($"Mode: '{mode}'");
            log.Info("");

            var targetService = new TargetService();
            var neweggService = new NeweggService();
            var gamestopService = new GamestopService();
            
            var count = 0;
            var random = new Random(1);            
            do
            {
                log.Info($"Running at {DateTime.Now}");
                log.Info("");

                var items = targetService.GetItemStatuses(mode);
                
                if (count % 3 == 0)
                {
                    items.AddRange(neweggService.GetItemStatuses(mode));
                }
                else
                {
                    log.Info("Newegg set to run every 3rd run");
                    log.Info("");
                }

                items.AddRange(gamestopService.GetItemStatuses(mode));

                items = items.Where(i => i.Instock).ToList();
                
                if (items.Any())
                {
                    if (mode.Contains("-e"))
                    {
                        var emailService = new EmailService();
                        emailService.EmailAvailability(items);
                    }
                    log.Info("AVAILABLE!!!");
                    return;
                }

                var wait = 10000 + random.Next(8000);
                log.Info($"No stock waiting {wait}");
                log.Info("");

                count++;
                Thread.Sleep(wait);
            } while(true);
        }

        private static void AddTargetCart(List<TargetSearchItem> items)
        {            
            // TEST WITH SWITCH IN STOCK
            items = new List<TargetSearchItem>
            {
                new TargetSearchItem
                {
                    Tcin = "80369290",
                    Title = "TEST SWITCH"
                }
            };

            //TODO: ADD TO CART
            // Get cookie from login to target

            var result = httpHelper.Post("https://carts.target.com/web_checkouts/v1/cart_items?field_groups=CART%2CCART_ITEMS%2CSUMMARY&key=feaf228eb2777fd3eee0fd5192ae7107d6224b39", 
                GetTargetCartPayload(items[0]), 
                "TealeafAkaSid=VPBe5qCJHsNzriKI_Dg5JbODJ1V7610z; visitorId=01761698BC5A02018F6E6E1A6C525BD2; sapphire=1; adaptiveSessionId=A7970807957; fiatsCookie=DSI_966|DSN_Huntersville|DSZ_28078; cd_user_id=1761698c1e0efd-0064d93395f788-18356153-1fa400-1761698c1e1f1c; criteo={%22criteo%22:%22lgFx_S70yBz_pSx5tXtJmwONvYhq03cc%22}; tlThirdPartyIds=%7B%22pt%22%3A%22v2%3Ad403b1a2c8667c27e0827f63212ec47fd1f9c0c814027fbca545fb71a80fd107%7Cdf72d8640925d7332d8f454997d4843a8feb6d6199db497c105cb42fd1181c04%22%2C%22adHubTS%22%3A%22Sun%20Nov%2029%202020%2019%3A41%3A19%20GMT-0500%20(Eastern%20Standard%20Time)%22%7D; ci_pixmgr=other; _gcl_au=1.1.108521206.1606696879; mystate=1606696914838; ffpersistent={%22channelStack%22:[{%22channel%22:%22%22%2C%22timestamp%22:%222020-11-30T00:41:55.498Z%22}]}; 3YCzT93n=AB3hmRZ2AQAAaKwRSPW5srmzLczhcxLU4dO6770Ww54rLibeywFqQORJZjny|1|1|2f489f3cc30ba4db34579be5ace390421ef31635; login-session=TDAKC1RZofq48APErd0UxyvkYJ8IrIcBKJeOBf9Kh76QEtlgDXmI-K1BoMmVespu-whnO1uXpvD9dGSI6as8j2alphWxgHDVtdnRNxS9hSlt5u8MTkWfnMRZp_cyCWA3KsqquVq0Puqire5HP8yHvn8EvE4NyBpKT2dYKYwWlQ9wviTkeaL96TjAbLRisuFR-RYTbA2UePOv25uh5yFs-4sideXCrI7H0vMLtZV9a0OApq3YMq5GURRRw5VgU3gZHWo8rm0Zir4xXmZ0KJ5NSYRi2tBGHPAGFBKgTv23wpdABl923a1wsUr9kz90R5LIWvLQRZAth7yFaiRaLUF69vm8gBel8C95PQDF4CWNPWUTzoLYp18DIoaczea65OeAicPdbNpLcV8MN7BO1gVdSZmKkkKmh3Q81Xw2BzryQ9PGoS2XX_KD9S4AmqfDl3tDdZf-pZo9Wo3jSggJEltYk6LZQwt6AFAdEv7CAp-85MkhEPMiLNFeuJlIn_3UZkOfoLk70XhRg32HwHhgAZ03W_xHem-3BI7JQeS1kvRVuk2QVOqH5aPB0k9gqfupWQC8eFeH8JTTGyW8Wk5n61YlS7ab1sCU96n3eKrVgELbEz4FiNDmy291gJAIYiEj6KZpkaPxtq4JthqI9JzdM36txlNaX0UlvZHJAMIg_vdVxDjf0Cz7nAcKKJRXTrSu1WgUbhG-byqN235rszuN4JFYVlxL7QhxQjxSXXxxlZFpA6OlYY4OdegT1G5BXMP2m1SnGxXdRoh0UZUhxl_JnDKzXpCxIdb86lF1EhAL1lZcKhWuWr8ghWNLT8d27Rk9ryCHI5va9SPvPg-a8Z-ZT1LMftOx3k3LJDDeT5Ym-kZTQoguhD84SYupoXxWLCRcTpq18tBJp1prciIKNrnRKU-6Z0waWrjAFSwQmS0f-vi1suYY3vQ76gGXBs6_TaLxCC8tfEZmpmONR7wLYR28iTiczCLc9x3qOlZeMFENd46y-GJ9CPNKQIP68jDk5OOnLh_k8Gd33WGup_WoIWM28pYzZjXyEcUASWjfh6UK5y5B1ZuWWxIdjspclJSrvUDQSw3wOsKvcsLIe3tTSMZVU-1Nw_GO_ciPWCBdFlUenxrOiiOX3z0w6JWrOJM2HtgPV1oquf83NlNnOp0TPbFhb-BjQ_njHRsbm_JRf110PxwmnJqUXENxlBrlWVvxicFef6Ff9jYy88-8cEjT2Pk34Ax8PHGyWVhPZA8e1iZ_Yeu0z0UWEoZV5OlwP99mtv1wcfSJseJYT5t3q-zV3FBm0j9vDIqeXcu-TP6Rc0i3aLj9PM25lysjTTL3c4Y_B5sElzSS23kdGWqoc1U3EDA2SB7QWg; accessToken=eyJraWQiOiJlYXMyIiwiYWxnIjoiUlMyNTYifQ.eyJzdWIiOiIyMDAyMjA4NzgzNiIsImlzcyI6Ik1JNiIsImV4cCI6MTYwNjcxMTM1MywiaWF0IjoxNjA2Njk2OTUzLCJqdGkiOiJUR1QuODMyMjRiOGQzYmRhNDcxMzlkN2QzOTRmYjA4MzE2NmMtbSIsInNreSI6ImVhczIiLCJzdXQiOiJSIiwiZGlkIjoiNmU3OWI4MTI4ZmFiZjRkN2FlMDU4ODcwNTRiMzA1YTU4Nzc0NzZkYzYzODE1NGE3ZTQ4MTY4MTg3ZTM3YmM0ZSIsImVpZCI6ImJ5ZWJ5ZXBjQGdtYWlsLmNvbSIsImdzcyI6MC41LCJzY28iOiJlY29tLm1lZCxvcGVuaWQiLCJjbGkiOiJlY29tLXdlYi0xLjAuMCIsImFzbCI6Ik0ifQ.Xt1bK3JZWigo7BWjReKvShHFbKJAeFyPuRWtBD4pBnfhEfKqmF2Bi2qMShDytCIHTt2-JIRctrfZPnkAUAktxIm1L-qPVPBk57LVveyOgp2ryIy7xaFKfsg81-s5ePgSKzwRJw8FUWKcTMM14T3P3Zxh0a-BZWTC-Z9VwLWVWs3EXRAPwMExze21lxvT7i9f99Li6VzUDixByXRrUOXqISclY5sbSGYeHoy9UGr5fGnzwseVfvSMWTqk0UzH4U90tStvue7L9cx1rUzXLPuPH-cAR033Ng1cmUOL_Oq_NKnWKP51eC5Di197FSfr8CfXJo85FcPf8rPY6vO765-imw; idToken=eyJhbGciOiJub25lIn0.eyJzdWIiOiIyMDAyMjA4NzgzNiIsImlzcyI6Ik1JNiIsImV4cCI6MTYwNjcxMTM1MywiaWF0IjoxNjA2Njk2OTUzLCJhc3MiOiJNIiwic3V0IjoiUiIsImNsaSI6ImVjb20td2ViLTEuMC4wIiwicHJvIjp7ImZuIjoiTmljayIsImVtIjoiYnllYnllcGNAZ21haWwuY29tIiwicGgiOmZhbHNlLCJsZWQiOm51bGwsImx0eSI6dHJ1ZX19.; refreshToken=TGT.83224b8d3bda47139d7d394fb083166c-m; guestType=R|1606696953000; mid=20022087836; UserLocation=28031|35.466392|-80.879734|NC|US; __gads=ID=6c8668b5ab39d3f0:T=1606696953:S=ALNI_MaE0ovXZ2UFkV8HI-T5VsRux7ZLSQ; crl8.fpcuid=32c8cb9d-8976-4686-bd21-1a0ff84dd8f8; _uetsid=c2eb1ba032a411ebb84ca984855b91ac; _uetvid=c2eb40f032a411ebb14cbd459596d780; ffsession={%22sessionHash%22:%22cd82208e299641606696877703%22%2C%22sessionHit%22:235%2C%22prevPageType%22:%22product%20details%22%2C%22prevPageName%22:%22video%20games:%20product%20detail%22%2C%22prevPageUrl%22:%22https://www.target.com/p/nintendo-switch-lite-yellow/-/A-77419249#lnk=sametab%22%2C%22marketingChannel%22:%22%22%2C%22lnkClickContent%22:null%2C%22lnkClickList%22:null%2C%22lnkClickNav%22:null%2C%22lnkPromotions%22:null%2C%22lnkClickRecommendation%22:null%2C%22prevSearchTerm%22:%22console%22}; targetMobileCookie=hasRC:false~loyaltyId:tly.9b860ef07f3146d0a022410c67a33d69~cartQty:1~guestLogonId:byebyepc@gmail.com~guestDisplayName:Nick~guestHasVerifiedPhone:false",
                new Dictionary<string, string>
                {
                    { "origin", "https://www.target.com" },
                    { "referer", "https://www.target.com/p/fifa-21-playstation-4-5/-/A-80369290" },
                    { "sec-fetch-dest", "empty" },
                    { "sec-fetch-mode", "cors" },
                    { "sec-fetch-site", "same-site" },
                    { "x-application-name", "web" }
                });
        }

        private static string GetTargetCartPayload(TargetSearchItem item)
        {
            var body = @"{{
                    ""cart_type"": ""REGULAR"",
                    ""channel_id"": ""91"",
                    ""shopping_context"": ""DIGITAL"",
                    ""cart_item"": {{
                        ""tcin"": ""{0}"",
                        ""quantity"": 1,
                        ""item_channel_id"": ""10""
                    }},
                    ""fulfillment"": {{
                        ""fulfillment_test_mode"": ""grocery_opu_team_member_test""
                    }}
                }}";            
            return string.Format(body, item.Tcin);
        }
    }
}
