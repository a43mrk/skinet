using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Core.Entities;
using System;
using Core.Entities.OrderAggregate;

// 28-1. Seed Data to Database with some data that comes from json files.
namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory){
            try {
                if(!context.ProductBrands.Any())
                {
                    var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");

                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    foreach(var item in brands){
                        context.ProductBrands.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

                if(!context.ProductTypes.Any())
                {
                    var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    foreach(var item in types)
                    {
                        context.ProductTypes.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                if(!context.Products.Any())
                {
                    var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");

                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    products.ForEach( item => context.Products.Add(item));

                    // foreach(var item in products)
                    // {
                    //     context.Products.Add(item);
                    // }
                    await context.SaveChangesAsync();
                }

                // 210. data to be seeded for DeliveryMethods
                // -> add migration next.
                // 211.
                // dotnet ef migrations add OrderEntityAdded -p Infrastructure -s API -c StoreContext
                if(!context.DeliveryMethods.Any())
                {
                    var dmData = File.ReadAllText("../Infrastructure/Data/SeedData/delivery.json");

                    var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);


                    foreach(var item in methods)
                    {
                    methods.ForEach( item => context.DeliveryMethods.Add(item));
                    }
                    await context.SaveChangesAsync();
                }
            } 
             catch(Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}