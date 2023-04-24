using System.Collections.Generic;

namespace QueueProcessor.Shared
{
    public class PlaceOrderCommand
    {
        public PlaceOrderCommand()
        {
            this.Items = new List<OrderItem>();
        }

        public string CustomerName { get; set; }

        public List<OrderItem> Items { get; set; }
    }
}
