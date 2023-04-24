using System;

namespace QueueProcessor.Shared;

public class OrderNotInStockException : Exception
{
    public OrderNotInStockException(string message) : base(message)
    {
    }
}