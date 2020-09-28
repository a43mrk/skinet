using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

// 22 -2 concrete class for Repository interface.
namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;

        // 23-1 inject StoreContext to Repository to get the Data.
        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            // return await _context.Products.FindAsync(id);
            return await _context.Products
                        .Include( p => p.ProductType) // 30-1 Eagerly load productType with Include.
                        .Include( p => p.ProductBrand) // 30-2 Eagerly load product Type with Include.
                        .FirstOrDefaultAsync( p => p.Id == id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await _context.Products
                .Include( p => p.ProductType ) // 30-3 Eagerly load product Type with Include.
                .Include( p => p.ProductBrand) // 30-4 Eagerly load product Type with Include.

                .ToListAsync();
        }
        // 29-1 - make ProductBrand accessible
        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        // 29-2 - make ProductType accessible
        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }
}