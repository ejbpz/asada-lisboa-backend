using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Principal;
using AsadaLisboaBackend.ServiceContracts.Principals;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for main page content, admin access.
    /// </summary>
    [ApiController]
    [Area("Admin")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class PrincipalController : ControllerBase
    {
        private readonly IPrincipalsGetterService _principalsGetterService;

        /// <summary>
        /// Constructor for PrincipalController.
        /// </summary>
        /// <param name="principalsGetterService">Service for getting main content.</param>
        public PrincipalController(IPrincipalsGetterService principalsGetterService)
        {
            _principalsGetterService = principalsGetterService;
        }

        /// <summary>
        /// Get main content information (latest news, documents and images).
        /// </summary>
        /// <returns>An ActionResult containing the main content.</returns>
        [HttpGet("")]
        public async Task<ActionResult<PrincipalResponseDTO>> GetPrincipalAdminInformation()
        {
            return Ok(await _principalsGetterService.GetPrincipalAdminInformation());
        }
    }
}
