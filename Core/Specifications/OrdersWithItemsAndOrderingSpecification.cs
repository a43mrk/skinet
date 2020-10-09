using Core.Entities.OrderAggregate;
using System.Linq.Expressions;

// 221-1 Specification for Order with Items ordered.
namespace Core.Specifications
{
    public class OrdersWithItemsAndOrderingSpecification : BaseSpecification<Order>
    {
        public OrdersWithItemsAndOrderingSpecification( string email ) : base( o => o.BuyerEmail == email )
        {
            AddInclude( i => i.OrderItems);
            AddInclude( i => i.DeliveryMethod );
            AddOrderByDescending( i => i.OrderDate );
        }

        public OrdersWithItemsAndOrderingSpecification(int id, string email) : base( i => i.Id == id && i.BuyerEmail == email)
        {
            AddInclude( i => i.OrderItems);
            AddInclude( i => i.DeliveryMethod );
        }
    }
}