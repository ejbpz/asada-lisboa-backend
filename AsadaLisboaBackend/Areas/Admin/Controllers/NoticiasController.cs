using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.Enums;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Status;
using AsadaLisboaBackend.Models.DTOs.Category;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.ServiceContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.Categories;

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
        private readonly IStatusesUpdaterService _statusesUpdaterService;
        private readonly ICategoriesGetterService _categoriesGetterService;

        public NoticiasController(INewsAdderService newsAdderService, INewsUpdaterService newsUpdaterService, INewsDeleterService newsDeleterService, INewsGetterService newsGetterService, IStatusesUpdaterService statusesUpdaterService, ICategoriesGetterService categoriesGetterService)
        {
            _newsAdderService = newsAdderService;
            _newsGetterService = newsGetterService;
            _newsUpdaterService = newsUpdaterService;
            _newsDeleterService = newsDeleterService;
            _statusesUpdaterService = statusesUpdaterService;
            _categoriesGetterService = categoriesGetterService;
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

        [HttpGet("categorias")]
        public async Task<ActionResult<List<CategoryResponseDTO>>> GetCategories()
        {
            return Ok(await _categoriesGetterService.GetCategories(ObjectTypeEnum.New));
        }
        
        [HttpGet("categorias/buscar")]
        public async Task<ActionResult<List<CategoryResponseDTO>>> SearchCategories([FromQuery] string search)
        {
            return Ok(await _categoriesGetterService.SearchCategories(ObjectTypeEnum.New, search));
        }

        [HttpPost("")]
        public async Task<ActionResult<NewResponseDTO>> CreateNew([FromForm] NewRequestDTO newRequestDTO)
        {
            return Created("~/api/admin/noticias", await _newsAdderService.CreateNew(newRequestDTO));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<NewResponseDTO>> UpdateNew([FromRoute] Guid id, [FromForm] NewRequestDTO newRequestDTO)
        {
            return Ok(await _newsUpdaterService.UpdateNew(id, newRequestDTO));
        }

        [HttpPatch("cambiar-estado/{id}")]
        public async Task<IActionResult> ChangeNewStatus([FromRoute] Guid id, [FromBody] StatusChangeRequestDTO statusChangeRequestDTO)
        {
            await _statusesUpdaterService.ChangeStatus(id, statusChangeRequestDTO.StatusId, ObjectTypeEnum.New);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<NewResponseDTO>> DeleteNew([FromRoute] Guid id)
        {
            await _newsDeleterService.DeleteNew(id);
            return NoContent();
        }
    }
}
