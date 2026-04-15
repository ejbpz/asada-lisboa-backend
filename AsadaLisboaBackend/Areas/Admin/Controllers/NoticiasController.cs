using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.Enums;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Status;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.ServiceContracts.Statuses;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for manage news, only accessible by admin users.
    /// </summary>
    [ApiController]
    [Area("Admin")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class NoticiasController : ControllerBase
    {
        private readonly INewsAdderService _newsAdderService;
        private readonly INewsGetterService _newsGetterService;
        private readonly INewsUpdaterService _newsUpdaterService;
        private readonly INewsDeleterService _newsDeleterService;
        private readonly IStatusesUpdaterService _statusesUpdaterService;

        /// <summary>
        /// Constructor for NoticiasController.
        /// </summary>
        /// <param name="newsAdderService">Service for adding news.</param>
        /// <param name="newsGetterService">Service for getting news.</param>
        /// <param name="newsUpdaterService">Service for updating news.</param>
        /// <param name="newsDeleterService">Service for deleting news.</param>
        /// <param name="statusesUpdaterService">Service for updating statuses.</param>
        public NoticiasController(INewsAdderService newsAdderService, INewsUpdaterService newsUpdaterService, INewsDeleterService newsDeleterService, INewsGetterService newsGetterService, IStatusesUpdaterService statusesUpdaterService)
        {
            _newsAdderService = newsAdderService;
            _newsGetterService = newsGetterService;
            _newsUpdaterService = newsUpdaterService;
            _newsDeleterService = newsDeleterService;
            _statusesUpdaterService = statusesUpdaterService;
        }

        /// <summary>
        /// Gets a paginated list of news based on the provided search and sort criteria.
        /// </summary>
        /// <param name="searchSortRequestDTO">An object containing search filters, sorting options, and pagination.</param>
        /// <returns>An ActionResult containing a PageResponseDTO of NewResponseDTO objects that match the provided criteria.</returns>
        [HttpGet("")]
        public async Task<ActionResult<PageResponseDTO<NewResponseDTO>>> GetNews([FromQuery] SearchSortRequestDTO searchSortRequestDTO)
        {
            return Ok(await _newsGetterService.GetNews(searchSortRequestDTO));
        }

        /// <summary>
        /// Gets a specific new by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the new to retrieve.</param>
        /// <returns>An ActionResult containing the NewResponseDTO object.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<NewResponseDTO>> GetNew([FromRoute] Guid id)
        {
            return Ok(await _newsGetterService.GetNew(id));
        }

        /// <summary>
        /// Creates a new based on the provided NewRequestDTO object.
        /// </summary>
        /// <param name="newRequestDTO">An object containing the details of the new to be created. Cannot be null.</param>
        /// <returns>An ActionResult containing the created NewResponseDTO object.</returns>
        [HttpPost("")]
        public async Task<ActionResult<NewResponseDTO>> CreateNew([FromForm] NewRequestDTO newRequestDTO)
        {
            return Created("~/api/admin/noticias", await _newsAdderService.CreateNew(newRequestDTO));
        }

        /// <summary>
        /// Updates an existing new identified by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the new to update.</param>
        /// <param name="newRequestDTO">An object containing the updated details of the new.</param>
        /// <returns>An ActionResult containing the updated NewResponseDTO object.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<NewResponseDTO>> UpdateNew([FromRoute] Guid id, [FromForm] NewRequestDTO newRequestDTO)
        {
            return Ok(await _newsUpdaterService.UpdateNew(id, newRequestDTO));
        }

        /// <summary>
        /// Updates the status of the specified new using the provided status change request.
        /// </summary>
        /// <param name="id">The unique identifier of the new whose status is to be changed.</param>
        /// <param name="statusChangeRequestDTO">An object containing the new status identifier.</param>
        /// <returns>No content.</returns>
        [HttpPatch("cambiar-estado/{id}")]
        public async Task<IActionResult> ChangeNewStatus([FromRoute] Guid id, [FromForm] StatusChangeRequestDTO statusChangeRequestDTO)
        {
            await _statusesUpdaterService.ChangeStatus(id, statusChangeRequestDTO.StatusId, ObjectTypeEnum.New);
            return NoContent();
        }

        /// <summary>
        /// Deletes the specified new by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the new to delete.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNew([FromRoute] Guid id)
        {
            await _newsDeleterService.DeleteNew(id);
            return NoContent();
        }
    }
}
