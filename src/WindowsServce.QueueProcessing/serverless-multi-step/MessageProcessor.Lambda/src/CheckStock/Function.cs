using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using AWS.Lambda.Powertools.Logging;
using QueueProcessor.Shared;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace CheckStock
{

    public class Function
    {
        private readonly IStockChecker _stockChecker;

        public Function()
        {
            this._stockChecker = new StockChecker();
        }

        public async Task<OrderState> FunctionHandler(PlaceOrderCommand command, ILambdaContext context)
        {
            Logger.LogInformation($"Processing order for {command.CustomerName}");

            foreach (var product in command.Items)
            {
                Logger.LogInformation($"Checking stock for {product.ProductCode}");

                var stockCheckResult = await this._stockChecker.CheckStockAsync(product.ProductCode, product.Quantity);

                product.InStock = stockCheckResult;

                Logger.LogInformation($"Stock Check Result: {stockCheckResult}");
            }
            
            if (command.Items.Any(p => p.InStock == false))
            {
                throw new OrderNotInStockException("Order contains products that are not in stock.");
            }

            return new OrderState()
            {
                Order = new Order()
                {
                    Items = command.Items,
                    CustomerName = command.CustomerName
                }
            };
        }
    }
}
