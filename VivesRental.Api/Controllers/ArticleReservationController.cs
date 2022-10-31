using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VivesRental.Services.Abstractions;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;

namespace VivesRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleReservationController : ControllerBase
    {
        private readonly IArticleReservationService _articleReservationService;

        public ArticleReservationController(IArticleReservationService articleReservationService)
        {
            _articleReservationService = articleReservationService;
        }

        [HttpGet]
        public async Task<IActionResult> Find([FromQuery]ArticleReservationFilter? filter = null)
        {
            var articles = await _articleReservationService.FindAsync(filter);
            return Ok(articles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var article = await _articleReservationService.GetAsync(id);
            return Ok(article);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ArticleReservationRequest article)
        {
            var createdArticle = await _articleReservationService.CreateAsync(article);
            return Ok(createdArticle);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove([FromRoute] Guid id)
        {
            var isDeleted = await _articleReservationService.RemoveAsync(id);
            return Ok(isDeleted);
        }
    }
}
