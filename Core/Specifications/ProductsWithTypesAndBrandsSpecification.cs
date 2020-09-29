using Core.Entities;

// 39-1 Concret type of Specification for Products with Brands and Types.
namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        // 60-1 adding sort parameter into specification
        // 64 -4 replace the gazillions parameters by custom parameter class productParams.
        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productParams):
        base(x => 
                // 66-2 pass expression to base to get search functionality
                (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) && // 66-2. Search Functionality
                // 62-2. the where close is at baseSpecification, them we need to pass the ProductBrandId and
                // the productTypeId filters to the base to be evaluated.
                (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId ) &&
                (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId )
            )
        {
            // 39-2 Start including ProductType and ProductBrand
            AddInclude( x => x.ProductType );
            AddInclude( x => x.ProductBrand );

            // 60-2 adding sort by name
            AddOrderBy( x => x.Name );

            // 64-6 adding pagination
            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

            // 60-3 sort by price or name.
            if(!string.IsNullOrEmpty(productParams.Sort))
            {
                switch(productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default: AddOrderBy(n => n.Name);
                        break;
                }
            }
        }
        
        // 40-1 accepts id
        // pass expression to base
        public ProductsWithTypesAndBrandsSpecification(int id) : base( x => x.Id == id)
        {
            AddInclude( x => x.ProductType );
            AddInclude( x => x.ProductBrand );
        }
    }
}