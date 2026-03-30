using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.ServiceContracts.Document;
using AsadaLisboaBackend.ServiceContracts.Statuses;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Status;
using AsadaLisboaBackend.Models.Enums;
using AsadaLisboaBackend.Utils.OptionsPattern;


namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    public class DocumentoController : ControllerBase
    {
        private readonly IDocumentAdderService _documentAdderService;
        private readonly IDocumentUpdaterService _documentUpdaterService;
        private readonly IDocumentGetterService _documentGetterService;
        private readonly IDocumentDeleterService _documentDeleterService;
        private readonly IStatusesUpdaterService _statusesUpdaterService;

        public DocumentoController(IWebHostEnvironment env, IDocumentAdderService documentAdderService, IDocumentGetterService documentGetterService, IDocumentUpdaterService documentUpdaterService, IDocumentDeleterService documentDeleterService, IStatusesUpdaterService statusesUpdaterService)
        {
            _documentAdderService = documentAdderService;
            _documentUpdaterService = documentUpdaterService;
            _documentGetterService = documentGetterService;
            _documentDeleterService = documentDeleterService;
        }

        [HttpGet("")]
        public async Task<ActionResult<PageResponseDTO<DocumentResponseDTO>>> GetDocument([FromQuery] SearchSortRequestDTO searchSortRequestDTO)
        {
            return Ok(await _documentGetterService.GetDocument(searchSortRequestDTO));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PageResponseDTO<DocumentResponseDTO>>> GetDocument([FromRoute] Guid id)
        {
            return Ok(await _documentGetterService.GetDocument(id));
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateDocument([FromForm] DocumentRequestDTO documentRequestDTO)
        {
            return Created("~/api/admin/documentos", await _documentAdderService.CreateDocument(documentRequestDTO));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> EditDocument([FromRoute] Guid id, [FromForm] DocumentUpdateRequestDTO documentUpdateRequestDTO)
        {
            return Ok(await _documentUpdaterService.UpdateDocument(id, documentUpdateRequestDTO));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(Guid id)
        {
            await _documentDeleterService.DeleterDocument(id);
            return NoContent();
        }

        [HttpPatch("cambiar-estado/{id}")]
            public async Task<IActionResult> ChangeDocumentStatus ([FromRoute]Guid id, [FromBody] StatusChangeRequestDTO statusChangeRequestDTO)
            {
            await _statusesUpdaterService.ChangeStatus(id, statusChangeRequestDTO.StatusID, ObjectTypeEnum.Documento);
            return NoContent();
            }
    }
}
