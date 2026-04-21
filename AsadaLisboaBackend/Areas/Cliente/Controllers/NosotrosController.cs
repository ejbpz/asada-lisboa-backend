using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.ServiceContracts.AboutUsSections;

namespace AsadaLisboaBackend.Areas.Cliente.Controllers
{
    /// <summary>
    /// Controller for getting about us news, public access.
    /// </summary>
    [ApiController]
    [Area("Cliente")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class NosotrosController : ControllerBase
    {
        private readonly IAboutUsSectionsGetterService _aboutUsSectionsGetterService;

        /// <summary>
        /// Constructor for NosotrosController.
        /// </summary>
        /// <param name="aboutUsSectionsGetterService">Service for getting about us sections.</param>
        public NosotrosController(IAboutUsSectionsGetterService aboutUsSectionsGetterService)
        {
            _aboutUsSectionsGetterService = aboutUsSectionsGetterService;
        }

        /// <summary>
        /// Retrieves a list of 'About Us' sections ordered by their display order.
        /// </summary>
        /// <returns>An IActionResult containing a collection of 'About Us' section data.</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetAboutUsSections()
        {
            return Ok(await _aboutUsSectionsGetterService.GetAboutUsSections(new SearchSortRequestDTO()
            {
                Take = 100,
                Offset = 0,
                Search = null,
                FilterBy = null,
                SortBy = "order",
                SortDirection = "asc",
            }));
        }
    }
}
