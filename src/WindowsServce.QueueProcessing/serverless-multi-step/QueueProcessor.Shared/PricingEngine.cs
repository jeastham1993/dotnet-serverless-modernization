using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AWS.Lambda.Powertools.Logging;

namespace QueueProcessor.Shared;

public interface IPricingEngine
{
    Task<decimal> PriceOrder(List<OrderItem> items);
}

public class PricingEngine : IPricingEngine
{
    public async Task<decimal> PriceOrder(List<OrderItem> items)
    {
        Logger.LogInformation($"Pricing order for {items.Count} items(s)");
        
        await Task.Delay(TimeSpan.FromSeconds(30));
        
        Logger.LogInformation("Price generated");

        return RandomNumberGenerator.GetInt32(1000);
    }
}