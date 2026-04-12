using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Status;
using AsadaLisboaBackend.ServiceContracts.Statuses;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for managing the statuses of the objects in the system, only accessible by admin users.
    /// </summary>
    [ApiController]
    [Area("Admin")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class EstadosController : ControllerBase
    {
        private readonly IStatusesGetterService _statusesGetterService;

        /// <summary>
        /// Constructor for the EstadosController.
        /// </summary>
        /// <param name="statusesGetterService">Service for getting statuses.</param>
        public EstadosController(IStatusesGetterService statusesGetterService)
        {
            _statusesGetterService = statusesGetterService;
        }

        /// <summary>
        /// Retrieves the list of all the statuses in the system.
        /// </summary>
        /// <returns>An ActionResult containing a list of StatusResponseDTO objects.</returns>
        [HttpGet("")]
        public async Task<ActionResult<List<StatusResponseDTO>>> GetStatuses()
        {
            return Ok(await _statusesGetterService.GetStatuses());
        }
    }
}
