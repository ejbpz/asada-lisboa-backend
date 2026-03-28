using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Charge;
using AsadaLisboaBackend.ServiceContracts.Charges;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    public class CargosController : ControllerBase
    {
        private readonly IChargesGetterService _chargesGetterService;

        public CargosController(IChargesGetterService chargesGetterService)
        {
            _chargesGetterService = chargesGetterService;
        }

        [HttpGet("")]
        public async Task<ActionResult<List<ChargeResponseDTO>>> GetCharges()
        {
            return Ok(await _chargesGetterService.GetCharges());
        }
    }
}
