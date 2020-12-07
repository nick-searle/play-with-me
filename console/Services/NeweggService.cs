using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using play_with_me.common.Models;
using play_with_me.common.Helpers;

namespace console.Services
{
    public class NeweggService : BaseStoreService
    {
        public NeweggService() : base("Newegg", "checkneweggstatus", "uIsqGlUhAv7FVZhIHaJin6U4A050ak0l2ucHnkq6sCaajUCyBAR/jw==") {}
    }
}