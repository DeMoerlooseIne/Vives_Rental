using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VivesRental.Services.Abstractions;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> Find([FromQuery]CustomerFilter? filter = null)
        {
            var customers = await _customerService.FindAsync(filter);
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var customer = await _customerService.GetAsync(id);
            return Ok(customer);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerRequest entity)
        {
            var createdCustomer = await _customerService.CreateAsync(entity);
            return Ok(createdCustomer);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] Guid customerId, [FromBody] CustomerRequest entity)
        {
            var updatedCustomer = await _customerService.EditAsync(customerId, entity);
            return Ok(updatedCustomer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove([FromRoute] Guid id)
        {
            var isDeleted = await _customerService.RemoveAsync(id);
            return Ok(isDeleted);
        }
    }
}

