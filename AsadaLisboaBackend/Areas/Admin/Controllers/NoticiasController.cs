using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.ServiceContracts.News;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    public class NoticiasController : ControllerBase
    {
        private readonly INewsAdderService _newsAdderService;
        private readonly INewsGetterService _newsGetterService;
        private readonly INewsUpdaterService _newsUpdaterService;
        private readonly INewsDeleterService _newsDeleterService;

        public NoticiasController(INewsAdderService newsAdderService, INewsUpdaterService newsUpdaterService, INewsDeleterService newsDeleterService, INewsGetterService newsGetterService)
        {
            _newsAdderService = newsAdderService;
            _newsGetterService = newsGetterService;
            _newsUpdaterService = newsUpdaterService;
            _newsDeleterService = newsDeleterService;
        }

        [HttpGet("")]
        public async Task<ActionResult<PageResponseDTO<NewResponseDTO>>> GetNews([FromQuery] SearchSortRequestDTO searchSortRequestDTO)
        {
            return Ok(await _newsGetterService.GetNews(searchSortRequestDTO));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PageResponseDTO<NewResponseDTO>>> GetNew([FromRoute] Guid id)
        {
            return Ok(await _newsGetterService.GetNew(id));
        }

        [HttpPost("")]
        public async Task<ActionResult<NewResponseDTO>> CreateNew([FromForm] NewRequestDTO newRequestDTO)
        {
            return Created("~/api/admin/noticias", await _newsAdderService.CreateNew(newRequestDTO));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<NewResponseDTO>> UpdateNew([FromRoute] Guid id, [FromBody] NewRequestDTO newRequestDTO)
        {
            return Ok(await _newsUpdaterService.UpdateNew(id, newRequestDTO));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<NewResponseDTO>> DeleteNew([FromRoute] Guid id)
        {
            await _newsDeleterService.DeleteNew(id);
            return NoContent();
        }
    }
}
