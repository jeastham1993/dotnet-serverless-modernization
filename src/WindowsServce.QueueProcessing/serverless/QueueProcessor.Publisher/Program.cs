using Amazon.SQS;
using MassTransit;
using MessageProcessor.Shared;
using System;
using System.Text.Json;

namespace MessageProcessor.Publisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new AmazonSQSClient();

            Console.WriteLine("Publish Queue URL?");
            var queueUrl = Console.ReadLine();

            Console.WriteLine("Publishing");

            while (true)
            {
                Console.WriteLine("Publish a message?");
                var input = Console.ReadLine();

                if (input.ToUpper().Trim() != "Y")
                {
                    break;
                }

                var placeOrderCommand = new PlaceOrderCommand() { CustomerName = "James Eastham" };
                placeOrderCommand.Items.Add(new OrderItem() {  ProductCode = "MYPROD123", Quantity= 1 });
                placeOrderCommand.Items.Add(new OrderItem() { ProductCode = "INSTOCKPROD", Quantity = 1 });

                client.SendMessageAsync(queueUrl, JsonSerializer.Serialize(placeOrderCommand)).Wait();

                Console.WriteLine("Published");
            }
        }
    }
}
