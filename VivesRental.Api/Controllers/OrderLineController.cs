﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VivesRental.Services.Abstractions;
using VivesRental.Services.Model.Filters;

namespace VivesRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderLineController : ControllerBase
    {
        private readonly IOrderLineService _orderLineService;

        public OrderLineController(IOrderLineService orderLineService)
        {
            _orderLineService = orderLineService;
        }

        [HttpGet]
        public async Task<IActionResult> Find([FromQuery]OrderLineFilter? filter = null)
        {
            var orders = await _orderLineService.FindAsync(filter);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var order = await _orderLineService.GetAsync(id);
            return Ok(order);
        }

        [HttpPost("{orderId}/rentedorderline")]
        public async Task<IActionResult> RentItem([FromRoute] Guid customerId, Guid articleId)
        {
            var rentedOrderLine = await _orderLineService.RentAsync(customerId,articleId);
            return Ok(rentedOrderLine);
        }

        [HttpPut("{orderId}/rentedorderlines")]
        public async Task<IActionResult> RentItems([FromRoute] Guid customerId, IList<Guid> articleIds)
        {
            var rentedOrderLines = await _orderLineService.RentAsync(customerId, articleIds);
            return Ok(rentedOrderLines);
        }

        [HttpDelete("{id}/returnedorderline")]
        public async Task<IActionResult> ReturnOrder([FromRoute] Guid Id, DateTime returnedAt)
        {
            var returnedOrderLine = await _orderLineService.ReturnAsync(Id, returnedAt);
            return Ok(returnedOrderLine);
        }
    }
}
