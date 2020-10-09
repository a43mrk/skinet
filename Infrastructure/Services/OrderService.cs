using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;

// 212 -2 implement interface
namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        // 213-1
        // 219-1 add unit of work to Order Service
        public OrderService(
            IBasketRepository basketRepo,
            IUnitOfWork unitOfWork
        )
        {
            _unitOfWork = unitOfWork;
            _basketRepo = basketRepo;
        }

    // 213-2
    public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
    {
        // get basket from the repo
        var basket = await _basketRepo.GetBasketAsync(basketId);
        // TODO: Handle non existing basket

        // get items from the product repo
        var items = new List<OrderItem>();
        foreach (var item in basket.Items)
        {
            // 219-2 update to unit of work as repository
            var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
            var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
            var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
            items.Add(orderItem);
        }

        // 219-3 update to unit of work as repository
        // get delivery method from repo
        var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

        // calc subtotal
        var subtotal = items.Sum(item => item.Price * item.Quantity);

        // create order
        var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal);
        // 219-4
        _unitOfWork.Repository<Order>().Add(order);

        // 219-5
        // save to db
        var result = await _unitOfWork.Complete();

        // 219-6
        if (result <= 0) return null;

        // 219-7
        //delete basket
        await _basketRepo.DeleteBasketAsync(basketId);

        // return order
        return order;

        // => use it at controller.
    }

    public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMehodsAsync()
    {
        throw new System.NotImplementedException();
    }

    public Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
    {
        throw new System.NotImplementedException();
    }

    public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        throw new System.NotImplementedException();
    }
}
}