using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;

using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using System;
using MessageProcessor.Shared;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace HelloWorld
{
    using System.IO;

    public class Function
    {
        private readonly IStockChecker _stockChecker;

        public Function()
        {
            this._stockChecker = new StockChecker();
        }

        public async Task FunctionHandler(Stream _)
        {
            foreach (var message in sqsEvent.Records)
            {
                var plcaeOrderCommand = JsonSerializer.Deserialize<PlaceOrderCommand>(message.Body);

                Console.WriteLine($"Processing order for {plcaeOrderCommand.CustomerName}");

                foreach (var product in plcaeOrderCommand.Items)
                {
                    Console.WriteLine($"Checking stock for {product.ProductCode}");

                    var stockCheckResutlt = await this._stockChecker.CheckStockAsync(product.ProductCode, product.Quantity);

                    Console.WriteLine($"Stock Check Result: {stockCheckResutlt}");
                }
            }
        }
    }
}
