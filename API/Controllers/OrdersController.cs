using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;
using API.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using API.Extensions;
using API.Errors;
using System.Collections.Generic;

namespace API.Controllers
{
    // 214-1
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _mapper = mapper;
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            // this line was refactore as a extension
            // var email = HttpContext.User?.Claims?.FirstOrDefault( x => x.Type == ClaimTypes.Email) ?.Value;
            // equivalent to next line of code:
            var email = HttpContext.User.RetrieveEmailFromPrincipal();

            // be sure Address comes from Core.Entities.OrderAggregate.Address
            var address = _mapper.Map<AddressDto, Address>(orderDto.ShipToAddress);

            var order = await _orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, address);

            if (order == null ) return BadRequest(new ApiResponse(400, "Problem creating order"));

            return Ok(order);
        }

        // 224-4 use the dto otherthan the model itself
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrdersForUser()
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();
            var orders = await _orderService.GetOrdersForUserAsync(email);
            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        }

        // 224-5 use the dto otherthan the model itself
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();
            var order = await _orderService.GetOrderByIdAsync(id, email);
            if (order == null) return NotFound(new ApiResponse(404));
            return _mapper.Map<Order, OrderToReturnDto>( order );
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await _orderService.GetDeliveryMethodsAsync());
        }
    }
}