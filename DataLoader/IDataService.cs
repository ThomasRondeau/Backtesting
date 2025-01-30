using Newtonsoft.Json.Linq;

namespace DataLoader
{
    public interface IDataService
    {
        Task<JObject> FetchForexDataAsync(string fromSymbol, string toSymbol, DateTime startDate, DateTime endDate);
    }
}
