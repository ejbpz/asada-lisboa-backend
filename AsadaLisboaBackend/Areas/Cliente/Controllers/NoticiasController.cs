using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.ServiceContracts.News;

namespace AsadaLisboaBackend.Areas.Cliente.Controllers
{
    [ApiController]
    [Area("Cliente")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class NoticiasController : ControllerBase
    {
        private readonly INewsGetterService _newsGetterService;

        public NoticiasController(INewsGetterService newsGetterService)
        {
            _newsGetterService = newsGetterService;
        }

        [HttpGet("")]
        public async Task<ActionResult<PageResponseDTO<NewResponseDTO>>> GetNews([FromQuery] SearchSortRequestDTO searchSortRequestDTO)
        {
            return Ok(await _newsGetterService.GetNews(searchSortRequestDTO));
        }

        [HttpGet("{slug}")]
        public async Task<ActionResult<NewResponseDTO>> GetNew([FromRoute] string slug)
        {
            return Ok(await _newsGetterService.GetNewBySlug(slug));
        }
    }
}
