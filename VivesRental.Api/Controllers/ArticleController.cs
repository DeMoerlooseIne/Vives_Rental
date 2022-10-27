using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VivesRental.Enums;
using VivesRental.Model;
using VivesRental.Services.Abstractions;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;

namespace VivesRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> Find(ArticleFilter? filter = null)
        {
            var articles = await _articleService.FindAsync(filter);
            return Ok(articles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var article = await _articleService.GetAsync(id);
            return Ok(article);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ArticleRequest article)
        {
            var createdArticle = await _articleService.CreateAsync(article);
            return Ok(createdArticle);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] Guid articleId, [FromBody] ArticleStatus status)
        {
            var updatedArticle = await _articleService.UpdateStatusAsync(articleId, status);
            return Ok(updatedArticle);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove([FromRoute] Guid id)
        {
            var isDeleted = await _articleService.RemoveAsync(id);
            return Ok(isDeleted);
        }
    }
}
