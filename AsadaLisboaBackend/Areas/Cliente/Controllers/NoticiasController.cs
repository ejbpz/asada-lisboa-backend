using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.ServiceContracts.News;

namespace AsadaLisboaBackend.Areas.Cliente.Controllers
{
    /// <summary>
    /// Controller for getting news, public access.
    /// </summary>
    [ApiController]
    [Area("Cliente")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class NoticiasController : ControllerBase
    {
        private readonly INewsGetterService _newsGetterService;

        /// <summary>
        /// Constructor for NoticiasController.
        /// </summary>
        /// <param name="newsGetterService">Service for getting news.</param>
        public NoticiasController(INewsGetterService newsGetterService)
        {
            _newsGetterService = newsGetterService;
        }

        /// <summary>
        /// Get a paginated list of news based on the search and sort criteria.
        /// </summary>
        /// <param name="searchSortRequestDTO">An object containing search filters, sorting options, and pagination.</param>
        /// <returns>ActionResult containing PageResponseDTO of NewResponseDTO objects.</returns>
        [HttpGet("")]
        public async Task<ActionResult<PageResponseDTO<NewResponseDTO>>> GetNews([FromQuery] SearchSortRequestDTO searchSortRequestDTO)
        {
            searchSortRequestDTO.IsPublic = true;
            return Ok(await _newsGetterService.GetNews(searchSortRequestDTO));
        }

        /// <summary>
        /// Retrieves a news item identified by the specified slug.
        /// </summary>
        /// <param name="slug">The unique slug that identifies the news item to retrieve. Cannot be null or empty.</param>
        /// <returns>An ActionResult of NewResponse containing the new item.</returns>
        [HttpGet("{slug}")]
        public async Task<ActionResult<NewResponseDTO>> GetNew([FromRoute] string slug)
        {
            return Ok(await _newsGetterService.GetNewBySlug(slug));
        }

        /// <summary>
        /// Retrieves recommended news by the new slug.
        /// </summary>
        /// <param name="slug">The unique slug that identifies the news item to retrieve. Cannot be null or empty.</param>
        /// <returns>An list of news recommended by the categories of the main new.</returns>
        [HttpGet("recomendaciones/{slug}")]
        public async Task<ActionResult<List<NewMinimalResponseDTO>>> GetRecommendedNews([FromRoute] string slug)
        {
            return Ok(await _newsGetterService.GetRecommendedNews(slug));
        }
    }
}
