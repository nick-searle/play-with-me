using System.Collections.Generic;
using play_with_me.common.Models;

namespace common.Contracts
{
    public interface IStoreService
    {
         List<SearchItem> GetItemStatuses(string mode);
    }
}