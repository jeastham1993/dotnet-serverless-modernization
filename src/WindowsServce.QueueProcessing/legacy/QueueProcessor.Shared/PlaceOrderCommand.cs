using System.Collections.Generic;

namespace MessageProcessor.Shared
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

    public class OrderItem
    {
        public string ProductCode { get; set; }

        public int Quantity { get; set; }
    }
}
