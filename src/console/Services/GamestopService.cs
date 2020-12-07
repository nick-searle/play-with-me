using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using play_with_me.common.Models;
using play_with_me.common.Helpers;

namespace console.Services
{
    public class GamestopService : BaseStoreService
    {
        public GamestopService() : base("Gamestop", "checkgamestopstatus", "5t2DeTTf4aslgSY2TUy3QDOfEeGO9muNvP4nPH8tZRRb6A8OTHEBPA==") {}
    }
}