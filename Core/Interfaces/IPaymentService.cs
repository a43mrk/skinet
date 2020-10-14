using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;

// 258-1 Interface for payments
namespace Core.Interfaces
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);

        // 276-1 new methods to update orders
        Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId);
        Task<Order> UpdateOrderPaymentFailed(string paymentIntentId);
    }
}