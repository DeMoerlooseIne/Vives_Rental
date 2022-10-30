using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VivesRental.Services.Abstractions;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Results;

namespace VivesRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Find([FromQuery]OrderFilter? filter = null)
        {
            var orders = await _orderService.FindAsync(filter);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var order = await _orderService.GetAsync(id);
            return Ok(order);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromRoute] Guid customerId)
        {
            var createdOrder = await _orderService.CreateAsync(customerId);
            return Ok(createdOrder);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> ReturnOrder([FromRoute] Guid id, DateTime returnedAt)
        {
            var isReturned = await _orderService.ReturnAsync(id, returnedAt);
            return Ok(isReturned);
        }
    }
}
