using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.ServiceContracts.Documents;

namespace AsadaLisboaBackend.Areas.Cliente.Controllers
{
    [ApiController]
    [Area("Cliente")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class DocumentosController : ControllerBase
    {
        private readonly IDocumentsGetterService _documentsGetterService;

        public DocumentosController(IDocumentsGetterService documentsGetterService)
        {
            _documentsGetterService = documentsGetterService;
        }

        [HttpGet("")]
        public async Task<ActionResult<PageResponseDTO<DocumentResponseDTO>>> GetDocuments([FromQuery] SearchSortRequestDTO searchSortRequestDTO)
        {
            return Ok(await _documentsGetterService.GetDocuments(searchSortRequestDTO));
        }

        [HttpGet("{slug}")]
        public async Task<ActionResult<DocumentResponseDTO>> GetDocument([FromRoute] string slug)
        {
            return Ok(await _documentsGetterService.GetDocumentBySlug(slug));
        }
    }
}
