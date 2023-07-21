using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using AWS.Lambda.Powertools.Logging;
using QueueProcessor.Shared;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace PriceOrder
{
    public class Function
    {
        private readonly IPricingEngine _pricingEngine;

        public Function()
        {
            this._pricingEngine = new PricingEngine();
        }

        public async Task<OrderState> FunctionHandler(OrderState state, ILambdaContext context)
        {
            if (state.Order.Items.Any(p => p.InStock == false))
            {
                throw new OrderNotInStockException("Order contains products that are not in stock.");
            }
            
            var price = await this._pricingEngine.PriceOrder(state.Order.Items);
            
            Logger.LogInformation($"Order priced at {price}");

            state.Order.Price = price;

            return state;
        }
    }
}
