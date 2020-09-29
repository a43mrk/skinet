using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using System.Collections.Generic;
using Core.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;
using API.Errors;
using Microsoft.AspNetCore.Http;
using API.Helpers;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        // 23-2 Inject Repository at Controller
        public ProductsController(
            IGenericRepository<Product> productsRepo,
            IGenericRepository<ProductBrand> productBrandRepo,
            IGenericRepository<ProductType> productTypeRepo,
            // 43-4 Inject IMapper
            IMapper mapper
            )
        {
            _productBrandRepo = productBrandRepo;
            _productsRepo = productsRepo;
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
        }

        // 55-1 document at Swagger w/ ProducesResponseType and possible returned StatusCode.
        // 55-2 pass the custom error type as the first parameter to be documented at swagger.
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
                [FromQuery] ProductSpecParams productParams
            )
        {
            // 39-1 create specification for product
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var countSpec = new ProductWithFiltersForCountSpecification(productParams);
            var totalItems = await _productsRepo.CountAsync(countSpec);

            // 39-3 pass specification to repo
            var products = await _productsRepo.ListAsync(spec);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            if(products == null ) return NotFound( new ApiResponse(404));

            return Ok(
                new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data)
                );
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id){
            // return await _context.Products.FindAsync(id);
            // return await _repo.GetProductByIdAsync(id);

            // 40-2 create the specification
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            // return await _productsRepo.GetByIdAsync(id);

            // 40-3 pass the specification with id.
            var product = await _productsRepo.GetEntityWithSpec(spec);
            // return new ProductToReturnDto
            // {
            //     Id = product.Id,
            //     Name = product.Name,
            //     Description = product.Description,
            //     PictureUrl = product.PictureUrl,
            //     Price = product.Price,
            //     ProductBrand = product.ProductBrand.Name,
            //     ProductType = product.ProductType.Name
            // };

            // 43-5 return the automapped object
            // _.Map<from,to>(instance);
            return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        // 29-3 make accessible productBrands and productTypes at controllers
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            // IReadOnlyList needs to be wrapped inside Ok().
            // return Ok(await _repo.GetProductBrandsAsync());
            return Ok(await _productBrandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            // return Ok(await _repo.GetProductTypesAsync());
            return Ok(await _productTypeRepo.ListAllAsync());
        }
    }
}