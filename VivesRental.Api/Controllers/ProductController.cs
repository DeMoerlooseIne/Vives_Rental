using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VivesRental.Services.Abstractions;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;

namespace VivesRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Find(ProductFilter? filter = null)
        {
            var articles = await _productService.FindAsync(filter);
            return Ok(articles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var article = await _productService.GetAsync(id);
            return Ok(article);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductRequest entity)
        {
            var createdArticle = await _productService.CreateAsync(entity);
            return Ok(createdArticle);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] Guid id, [FromBody] ProductRequest entity)
        {
            var updatedArticle = await _productService.EditAsync(id, entity);
            return Ok(updatedArticle);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove([FromRoute] Guid id)
        {
            var isDeleted = await _productService.RemoveAsync(id);
            return Ok(isDeleted);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> GenerateArticlesAsync([FromRoute] Guid id)
        {
            var isDeleted = await _productService.RemoveAsync(id);
            return Ok(isDeleted);
        }
    }
}
