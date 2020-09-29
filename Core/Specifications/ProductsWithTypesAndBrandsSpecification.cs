using Core.Entities;

// 39-1 Concret type of Specification for Products with Brands and Types.
namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productParams):
        base(x => 
                (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) && // 66-2. Search Functionality
                (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId ) &&
                (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId )
            )
        {
            // 39-2 Start including ProductType and ProductBrand
            AddInclude( x => x.ProductType );
            AddInclude( x => x.ProductBrand );

            //
            AddOrderBy( x => x.Name );
            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

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