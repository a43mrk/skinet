using Core.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

// 22-1 Repository for products, productBrand adn productType.
namespace Core.Interfaces
{
    public interface IProductRepository
    {
       Task<Product> GetProductByIdAsync(int id); 

       Task<IReadOnlyList<Product>> GetProductsAsync();

        // 29-0 declare getters for ProductBrand and ProductType to be mandatory at repository
       Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();

       Task<IReadOnlyList<ProductType>> GetProductTypesAsync();
    }

}