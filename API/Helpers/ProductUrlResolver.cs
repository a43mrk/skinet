using API.Dtos;
using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Configuration;

// 45-1 add key value at appsettingsDevelopment.json
// 45-2 create an implementation for IValueResolver<take the source mapping from, where to map to, destination property to be>
namespace API.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _config;
        // 45-3 inject the configuration from Microsoft; not from automapper!
        public ProductUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            // 45-4 get value from key at configuration file json.
            if(!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _config["ApiUrl"] + source.PictureUrl;
            } else {
                return null;
            }
        }
    }
}