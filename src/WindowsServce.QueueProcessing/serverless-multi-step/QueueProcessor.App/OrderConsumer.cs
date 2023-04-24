using System;
using System.Threading.Tasks;
using MassTransit;
using QueueProcessor.Shared;

namespace QueueProcessor.App
{
    internal class OrderConsumer : IConsumer<PlaceOrderCommand>
    {
        private readonly IStockChecker _stockChecker;

        public OrderConsumer()
        {
            this._stockChecker = new StockChecker();
        }

        public async Task Consume(ConsumeContext<PlaceOrderCommand> context)
        {
            Console.WriteLine($"Processing order for {context.Message.CustomerName}");

            foreach (var product in context.Message.Items)
            {
                Console.WriteLine($"Checking stock for {product.ProductCode}");

                var stockCheckResutlt = await this._stockChecker.CheckStockAsync(product.ProductCode, product.Quantity);

                Console.WriteLine($"Stock Check Result: {stockCheckResutlt}");
            }

            return;
        }
    }
}
