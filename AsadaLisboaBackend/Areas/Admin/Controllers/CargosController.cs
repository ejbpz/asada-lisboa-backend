using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Charge;
using AsadaLisboaBackend.ServiceContracts.Charges;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class CargosController : ControllerBase
    {
        private readonly IChargesAdderService _chargesAdderService;
        private readonly IChargesGetterService _chargesGetterService;
        private readonly IChargesDeleterService _chargesDeleterService;

        public CargosController(IChargesGetterService chargesGetterService, IChargesAdderService chargesAdderService, IChargesDeleterService chargesDeleterService)
        {
            _chargesAdderService = chargesAdderService;
            _chargesGetterService = chargesGetterService;
            _chargesDeleterService = chargesDeleterService;
        }

        [HttpGet("")]
        public async Task<ActionResult<List<ChargeResponseDTO>>> GetCharges()
        {
            return Ok(await _chargesGetterService.GetCharges());
        }

        [HttpPost("")]
        public async Task<ActionResult<ChargeResponseDTO>> CreateCharge([FromBody] string chargeRequest)
        {
            return Created("~/api/admin/cargos", await _chargesAdderService.CreateCharge(chargeRequest));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCharge([FromRoute] Guid id)
        {
            await _chargesDeleterService.DeleteCharge(id);
            return NoContent();
        }
    }
}
