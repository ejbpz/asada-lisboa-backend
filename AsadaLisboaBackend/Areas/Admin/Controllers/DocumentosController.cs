using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.Enums;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Status;
using AsadaLisboaBackend.Models.DTOs.Category;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.ServiceContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.Documents;
using AsadaLisboaBackend.ServiceContracts.Categories;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    public class DocumentosController : ControllerBase
    {
        private readonly IDocumentsAdderService _documentsAdderService;
        private readonly IDocumentsGetterService _documentsGetterService;
        private readonly IStatusesUpdaterService _statusesUpdaterService;
        private readonly IDocumentsUpdaterService _documentsUpdaterService;
        private readonly IDocumentsDeleterService _documentsDeleterService;
        private readonly ICategoriesGetterService _categoriesGetterService;

        public DocumentosController(IDocumentsAdderService documentsAdderService, IDocumentsGetterService documentsGetterService, IDocumentsUpdaterService documentsUpdaterService, IDocumentsDeleterService documentsDeleterService, IStatusesUpdaterService statusesUpdaterService, ICategoriesGetterService categoriesGetterService)
        {
            _documentsAdderService = documentsAdderService;
            _documentsGetterService = documentsGetterService;
            _statusesUpdaterService = statusesUpdaterService;
            _documentsUpdaterService = documentsUpdaterService;
            _documentsDeleterService = documentsDeleterService;
            _categoriesGetterService = categoriesGetterService;
        }

        [HttpGet("")]
        public async Task<ActionResult<PageResponseDTO<DocumentResponseDTO>>> GetDocument([FromQuery] SearchSortRequestDTO searchSortRequestDTO)
        {
            return Ok(await _documentsGetterService.GetDocuments(searchSortRequestDTO));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PageResponseDTO<DocumentResponseDTO>>> GetDocument([FromRoute] Guid id)
        {
            return Ok(await _documentsGetterService.GetDocument(id));
        }

        [HttpGet("categorias")]
        public async Task<ActionResult<List<CategoryResponseDTO>>> GetCategories()
        {
            return Ok(await _categoriesGetterService.GetCategories(ObjectTypeEnum.Document));
        }

        [HttpGet("categorias/buscar")]
        public async Task<ActionResult<List<CategoryResponseDTO>>> SearchCategories([FromQuery] string search)
        {
            return Ok(await _categoriesGetterService.SearchCategories(ObjectTypeEnum.Document, search));
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateDocument([FromForm] DocumentRequestDTO documentRequestDTO)
        {
            return Created("~/api/admin/documentos", await _documentsAdderService.CreateDocument(documentRequestDTO));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> EditDocument([FromRoute] Guid id, [FromForm] DocumentUpdateRequestDTO documentUpdateRequestDTO)
        {
            return Ok(await _documentsUpdaterService.UpdateDocument(id, documentUpdateRequestDTO));
        }

        [HttpPatch("cambiar-estado/{id}")]
        public async Task<IActionResult> ChangeDocumentStatus([FromRoute] Guid id, [FromBody] StatusChangeRequestDTO statusChangeRequestDTO)
        {
            await _statusesUpdaterService.ChangeStatus(id, statusChangeRequestDTO.StatusId, ObjectTypeEnum.Document);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(Guid id)
        {
            await _documentsDeleterService.DeleterDocument(id);
            return NoContent();
        }
    }
}
