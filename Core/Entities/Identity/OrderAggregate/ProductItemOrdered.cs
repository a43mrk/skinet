// 207
// Snapshop of the ordered item.
namespace Core.Entities.OrderAggregate
{
    public class ProductItemOrdered
    {
        // this constructor is need, or EF will complying
        public ProductItemOrdered()
        {
        }

        public ProductItemOrdered(int productItemId, string productName, string pictureUrl)
        {
            ProductItemId = productItemId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }

        public int ProductItemId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
    }
}