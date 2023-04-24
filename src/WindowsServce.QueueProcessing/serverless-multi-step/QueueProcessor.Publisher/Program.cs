using System;
using System.Text.Json;
using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime.Internal.Transform;
using Amazon.SQS;
using QueueProcessor.Shared;

namespace QueueProcessor.Publisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var credsChain = new CredentialProfileStoreChain();
            AWSCredentials creds = null;
            var profileCredentials = credsChain.TryGetAWSCredentials("dev", out creds);
            
            var client = new AmazonSQSClient(creds, RegionEndpoint.EUWest1);

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
