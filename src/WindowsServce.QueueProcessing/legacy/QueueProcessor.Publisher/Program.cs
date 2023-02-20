using MassTransit;
using MessageProcessor.Shared;
using System;

namespace MessageProcessor.Publisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var messageBus = Bus.Factory.CreateUsingRabbitMq(opt =>
            {
                opt.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                opt.Message<PlaceOrderCommand>(x => x.SetEntityName("placeOrder"));

                opt.Publish<PlaceOrderCommand>(x =>
                {
                    x.Durable = true;
                    x.AutoDelete = false;
                    x.ExchangeType = "fanout";
                });
            });

            messageBus.Start();

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

                messageBus.Publish<PlaceOrderCommand>(placeOrderCommand).Wait();

                Console.WriteLine("Published");
            }
        }
    }
}
