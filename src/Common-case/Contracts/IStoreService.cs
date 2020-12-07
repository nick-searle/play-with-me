using System.Collections.Generic;
using PlayWithMe.Common.Models;

namespace PlayWithMe.Common.Contracts
{
    public interface IStoreService
    {
         List<SearchItem> GetItemStatuses(string mode);
    }
}