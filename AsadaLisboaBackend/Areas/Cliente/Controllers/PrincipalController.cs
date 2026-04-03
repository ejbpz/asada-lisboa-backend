using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Principal;
using AsadaLisboaBackend.ServiceContracts.Principals;

namespace AsadaLisboaBackend.Areas.Cliente.Controllers
{
    [ApiController]
    [Area("Cliente")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class PrincipalController : ControllerBase
    {
        private readonly IPrincipalsGetterService _principalsGetterService;

        public PrincipalController(IPrincipalsGetterService principalsGetterService)
        {
            _principalsGetterService = principalsGetterService;
        }

        [HttpGet("")]
        public async Task<ActionResult<PrincipalRequestDTO>> GetPrincipalInformation()
        {
            return Ok(await _principalsGetterService.GetPrincipalInformation());
        }
    }
}
