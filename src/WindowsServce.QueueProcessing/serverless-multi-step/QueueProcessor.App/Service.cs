using MassTransit;
using QueueProcessor.Shared;

namespace QueueProcessor.App
{
    public class Service : IService
    {
        IBusControl MessageBus;

        public void Start()
        {
            MessageBus = Bus.Factory.CreateUsingRabbitMq(opt =>
            {
                opt.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                opt.Message<PlaceOrderCommand>(x => x.SetEntityName("placeOrder"));

                opt.ReceiveEndpoint("place-order-queue", e =>
                {
                    e.ConfigureConsumeTopology = false;

                    e.Consumer<OrderConsumer>();

                    e.Bind("placeOrder", s =>
                    {
                    });
                });
            });

            MessageBus.Start();
        }

        public void Stop()
        {
            MessageBus.Stop();
        }
    }

    public interface IService
    {
        void Start();

        void Stop();
    }
}
