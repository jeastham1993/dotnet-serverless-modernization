using System;
using System.Threading.Tasks;

namespace QueueProcessor.Shared
{
    public interface IStockChecker
    {
        Task<bool> CheckStockAsync(string productCode, int requiredQuantity);

    }

    public class StockChecker : IStockChecker
    {
        public Task<bool> CheckStockAsync(string productCode, int requiredQuantity)
        {
            return Task.FromResult(productCode.Equals("INSTOCKPROD", StringComparison.OrdinalIgnoreCase));
        }
    }
}
