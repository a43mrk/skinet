using AutoMapper;
using Core.Entities;
using API.Dtos;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;

// 43-1 install nuget AutoMapper.Extensions.Microsoft.DependencyInjection
// >dotnet restore

// 43-2 add Mapping Profile
namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // create a map between Product and ProductToReturnDto.
            // 44-1 resolve specific types for non primitive properties at automapping.
            CreateMap<Product, ProductToReturnDto>()
                // 44-2 map manually productBrand as ProductBrand.Name
                .ForMember( d => d.ProductBrand, o => o.MapFrom( s => s.ProductBrand.Name ))
                // 44-3 map manually productType as ProductType.Name
                .ForMember( d => d.ProductType, o => o.MapFrom( s => s.ProductType.Name ))
                // 45-5 add the custom value resolver(IvalueResolver implementation)
                .ForMember( d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());

            // 176-2 Define how to AutoMap for AddressDto
            // 214- Beware to not mix with the Address of aggregates.
            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();

            // 182-2 map customer basket and basket item Dto's
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();

            // 214-2 Maping for AddressDto to Address of aggregates
            CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();

            //224-3 register the Dto to be automapped
            CreateMap<Order, OrderToReturnDto>()
                // 215-1 fix missing automap properties
                .ForMember( to => to.DeliveryMethod, from => from.MapFrom( entity => entity.DeliveryMethod.ShortName))
                .ForMember( to => to.ShippingPrice, from => from.MapFrom( entity => entity.DeliveryMethod.Price));
            CreateMap<OrderItem, OrderItemDto>()
                // 215-2 fix missing automap properties
                .ForMember( t => t.ProductId, f => f.MapFrom( e => e.ItemOrdered.ProductItemId))
                .ForMember( t => t.ProductName, f => f.MapFrom( e => e.ItemOrdered.ProductName))
                .ForMember( t => t.PictureUrl, f => f.MapFrom( e => e.ItemOrdered.PictureUrl))
                // 226-2 resolve full url for the picture
                .ForMember( t => t.PictureUrl, f => f.MapFrom<OrderItemUrlResolver>());
        }
        
    }
}