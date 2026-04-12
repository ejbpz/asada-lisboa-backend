using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.Enums;
using AsadaLisboaBackend.Models.DTOs.Status;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.ServiceContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.Documents;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for manage documents, only accessible by admin users.
    /// </summary>
    [ApiController]
    [Area("Admin")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class DocumentosController : ControllerBase
    {
        private readonly IDocumentsAdderService _documentsAdderService;
        private readonly IDocumentsGetterService _documentsGetterService;
        private readonly IStatusesUpdaterService _statusesUpdaterService;
        private readonly IDocumentsUpdaterService _documentsUpdaterService;
        private readonly IDocumentsDeleterService _documentsDeleterService;

        /// <summary>
        /// Constructor for DocumentosController.
        /// </summary>
        /// <param name="documentsAdderService">Service for adding documents.</param>
        /// <param name="documentsGetterService">Service for getting documents.</param>
        /// <param name="statusesUpdaterService">Service for updating statuses.</param>
        /// <param name="documentsUpdaterService">Service for updating documents.</param>
        /// <param name="documentsDeleterService">Service for deleting documents.</param>
        public DocumentosController(IDocumentsAdderService documentsAdderService, IDocumentsGetterService documentsGetterService, IDocumentsUpdaterService documentsUpdaterService, IDocumentsDeleterService documentsDeleterService, IStatusesUpdaterService statusesUpdaterService)
        {
            _documentsAdderService = documentsAdderService;
            _documentsGetterService = documentsGetterService;
            _statusesUpdaterService = statusesUpdaterService;
            _documentsUpdaterService = documentsUpdaterService;
            _documentsDeleterService = documentsDeleterService;
        }

        /// <summary>
        /// Gets a paginated list of documents based on the provided search and sort criteria.
        /// </summary>
        /// <param name="searchSortRequestDTO">An object containing search filters, sorting options, and pagination.</param>
        /// <returns>An ActionResult containing a PageResponseDTO of DocumentResponseDTO objects that match the provided criteria.</returns>
        [HttpGet("")]
        public async Task<ActionResult<PageResponseDTO<DocumentResponseDTO>>> GetDocuments([FromQuery] SearchSortRequestDTO searchSortRequestDTO)
        {
            return Ok(await _documentsGetterService.GetDocuments(searchSortRequestDTO));
        }

        /// <summary>
        /// Gets a specific document by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the document to retrieve.</param>
        /// <returns>An ActionResult containing the DocumentResponseDTO object.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentResponseDTO>> GetDocument([FromRoute] Guid id)
        {
            return Ok(await _documentsGetterService.GetDocument(id));
        }

        /// <summary>
        /// Creates a new document based on the provided DocumentRequestDTO object.
        /// </summary>
        /// <param name="documentRequestDTO">An object containing the details of the document to be created. Cannot be null.</param>
        /// <returns>An ActionResult containing the created DocumentResponseDTO object.</returns>
        [HttpPost("")]
        public async Task<IActionResult> CreateDocument([FromForm] DocumentRequestDTO documentRequestDTO)
        {
            return Created("~/api/admin/documentos", await _documentsAdderService.CreateDocument(documentRequestDTO));
        }

        /// <summary>
        /// Updates an existing document identified by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the document to update.</param>
        /// <param name="documentUpdateRequestDTO">An object containing the updated details of the document.</param>
        /// <returns>An ActionResult containing the updated DocumentResponseDTO object.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditDocument([FromRoute] Guid id, [FromForm] DocumentUpdateRequestDTO documentUpdateRequestDTO)
        {
            return Ok(await _documentsUpdaterService.UpdateDocument(id, documentUpdateRequestDTO));
        }

        /// <summary>
        /// Updates the status of the specified document using the provided status change request.
        /// </summary>
        /// <param name="id">The unique identifier of the document whose status is to be changed.</param>
        /// <param name="statusChangeRequestDTO">An object containing the new status identifier.</param>
        /// <returns>No content.</returns>
        [HttpPatch("cambiar-estado/{id}")]
        public async Task<IActionResult> ChangeDocumentStatus([FromRoute] Guid id, [FromBody] StatusChangeRequestDTO statusChangeRequestDTO)
        {
            await _statusesUpdaterService.ChangeStatus(id, statusChangeRequestDTO.StatusId, ObjectTypeEnum.Document);
            return NoContent();
        }

        /// <summary>
        /// Deletes the specified document by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the document to delete.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(Guid id)
        {
            await _documentsDeleterService.DeleterDocument(id);
            return NoContent();
        }
    }
}
