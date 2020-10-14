using System;
using System.Collections.Generic;

// 208
namespace Core.Entities.OrderAggregate
{
    public class Order : BaseEntity
    {
        // 211. paramerless constructor for EF
        public Order()
        {
        }

        // 270-1 add paymentIntentId parameter
        public Order(
            IReadOnlyList<OrderItem> orderItems,
            string buyerEmail,
            Address shipToAddress,
            DeliveryMethod deliveryMethod,
            decimal subtotal,
            string paymentIntentId
            )
        {
            BuyerEmail = buyerEmail;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }

        // used to retrieve a list of orders for a particular user
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public Address ShipToAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string PaymentIntentId { get; set; }

        public decimal GetTotal()
        {
            return Subtotal + DeliveryMethod.Price;
        }
    }
}
