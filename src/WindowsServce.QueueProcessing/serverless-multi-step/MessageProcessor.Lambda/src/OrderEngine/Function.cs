using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using AWS.Lambda.Powertools.Logging;
using QueueProcessor.Shared;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace OrderEngine
{

    public class Function
    {
        private readonly IStockChecker _stockChecker;
        private readonly IPricingEngine _pricingEngine;

        public Function()
        {
            this._stockChecker = new StockChecker();
            this._pricingEngine = new PricingEngine();
        }

        public async Task FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
        {
            foreach (var message in sqsEvent.Records)
            {
                var placeOrderCommand = JsonSerializer.Deserialize<PlaceOrderCommand>(message.Body);

                Logger.LogInformation($"Processing order for {placeOrderCommand.CustomerName}");

                foreach (var product in placeOrderCommand.Items)
                {
                    Logger.LogInformation($"Checking stock for {product.ProductCode}");

                    var stockCheckResult = await this._stockChecker.CheckStockAsync(product.ProductCode, product.Quantity);

                    Logger.LogInformation($"Stock Check Result: {stockCheckResult}");
                }

                var price = await this._pricingEngine.PriceOrder(placeOrderCommand.Items);
                
                Logger.LogInformation($"Order priced at {price}");
            }
        }
    }
}
