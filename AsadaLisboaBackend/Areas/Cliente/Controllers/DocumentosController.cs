using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.ServiceContracts.Documents;

namespace AsadaLisboaBackend.Areas.Cliente.Controllers
{
    /// <summary>
    /// Controller for getting documents, public access.
    /// </summary>
    [ApiController]
    [Area("Cliente")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class DocumentosController : ControllerBase
    {
        private readonly IDocumentsGetterService _documentsGetterService;

        /// <summary>
        /// Constructor for DocumentosController.
        /// </summary>
        /// <param name="documentsGetterService">Service for getting documents.</param>
        public DocumentosController(IDocumentsGetterService documentsGetterService)
        {
            _documentsGetterService = documentsGetterService;
        }

        /// <summary>
        /// Retrieves a paginated list of documents based on the specified search and sort criteria.
        /// </summary>
        /// <param name="searchSortRequestDTO">The search and sort parameters to filter and order the documents. Cannot be null.</param>
        /// <returns>ActionResult containing PageResponseDTO of DocumentResponseDTO objects.</returns>
        [HttpGet("")]
        public async Task<ActionResult<PageResponseDTO<DocumentResponseDTO>>> GetDocuments([FromQuery] SearchSortRequestDTO searchSortRequestDTO)
        {
            return Ok(await _documentsGetterService.GetDocuments(searchSortRequestDTO));
        }

        /// <summary>
        /// Retrieves a document by its unique slug identifier.
        /// </summary>
        /// <param name="slug">The slug that uniquely identifies the document to retrieve.</param>
        /// <returns>An ActionResult of DocumentResponseDTO containing the document data.</returns>
        [HttpGet("{slug}")]
        public async Task<ActionResult<DocumentResponseDTO>> GetDocument([FromRoute] string slug)
        {
            return Ok(await _documentsGetterService.GetDocumentBySlug(slug));
        }
    }
}
