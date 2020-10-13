using System.Threading.Tasks;
using Core.Entities;

// 258-1 Interface for payments
namespace Core.Interfaces
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);
    }
}