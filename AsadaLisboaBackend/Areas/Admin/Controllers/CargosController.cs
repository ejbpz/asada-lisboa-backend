using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Charge;
using AsadaLisboaBackend.ServiceContracts.Charges;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for manage user charges, only accessible by admin users.
    /// </summary>
    [ApiController]
    [Area("Admin")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class CargosController : ControllerBase
    {
        private readonly IChargesAdderService _chargesAdderService;
        private readonly IChargesGetterService _chargesGetterService;
        private readonly IChargesUpdaterService _chargesUpdaterService;
        private readonly IChargesDeleterService _chargesDeleterService;

        /// <summary>
        /// Constructor for CargosController.
        /// </summary>
        /// <param name="chargesAdderService">Service for adding charges.</param>
        /// <param name="chargesGetterService">Service for getting charges.</param>
        /// <param name="chargesDeleterService">Service for deleting charges.</param>
        /// <param name="chargesUpdaterService">Service for updating charges.</param>
        public CargosController(IChargesGetterService chargesGetterService, IChargesAdderService chargesAdderService, IChargesDeleterService chargesDeleterService, IChargesUpdaterService chargesUpdaterService)
        {
            _chargesAdderService = chargesAdderService;
            _chargesGetterService = chargesGetterService;
            _chargesUpdaterService = chargesUpdaterService;
            _chargesDeleterService = chargesDeleterService;
        }

        /// <summary>
        /// Get all existing charges in the system.
        /// </summary>
        /// <returns>List of charges.</returns>
        [HttpGet("")]
        public async Task<ActionResult<List<ChargeResponseDTO>>> GetCharges()
        {
            return Ok(await _chargesGetterService.GetCharges());
        }

        /// <summary>
        /// Create a new charge in the system.
        /// </summary>
        /// <param name="chargeRequest">The charge request data.</param>
        /// <returns>The created charge.</returns>
        [HttpPost("")]
        public async Task<ActionResult<ChargeResponseDTO>> CreateCharge([FromBody] string chargeRequest)
        {
            return Created("~/api/admin/cargos", await _chargesAdderService.CreateCharge(chargeRequest));
        }

        /// <summary>
        /// Updates an specific charge in the system.
        /// </summary>
        /// <param name="id">The ID of the charge to update.</param>
        /// <param name="chargeRequest">The updated charge data.</param>
        /// <returns>The updated charge.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ChargeResponseDTO>> UpdateCharge([FromRoute] Guid id, [FromBody] string chargeRequest)
        {
            return Ok(await _chargesUpdaterService.UpdateCharge(id, chargeRequest));
        }

        /// <summary>
        /// Delete a charge from the system by its ID.
        /// </summary>
        /// <param name="id">The ID of the charge to delete.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCharge([FromRoute] Guid id)
        {
            await _chargesDeleterService.DeleteCharge(id);
            return NoContent();
        }
    }
}
