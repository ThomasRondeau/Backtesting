using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DataLoader
{
    public interface IDataService
    {
        Task<JObject> FetchForexDataAsync(string fromSymbol, string toSymbol, DateTime startDate, DateTime endDate);
    }
}
