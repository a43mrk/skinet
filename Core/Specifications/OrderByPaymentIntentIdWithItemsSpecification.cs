using System;
using System.Linq.Expressions;
using Core.Entities.OrderAggregate;

// 270-3 Specification for PaymentIntent
namespace Core.Specifications
{
    // 276-2 renamed
    public class OrderByPaymentIntentIdSpecification : BaseSpecification<Order>
    {
        public OrderByPaymentIntentIdSpecification(string paymentIntentId)
            : base(o => o.PaymentIntentId == paymentIntentId)
        {
        }
    }
}