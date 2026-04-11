using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.AboutUs;
using AsadaLisboaBackend.ServiceContracts.AboutUsSections;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for manage about us section, only accessible by admin users.
    /// </summary>
    [ApiController]
    [Area("Admin")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class NosotrosController : ControllerBase
    {
        private readonly IAboutUsSectionsAdderService _aboutUsSectionsAdderService;
        private readonly IAboutUsSectionsGetterService _aboutUsSectionsGetterService;
        private readonly IAboutUsSectionsDeleterService _aboutUsSectionsDeleterService;
        private readonly IAboutUsSectionsUpdaterService _aboutUsSectionsUpdaterService;

        /// <summary>
        /// Constructor for NosotrosController.
        /// </summary>
        /// <param name="aboutUsSectionsAdderService">Service for adding about us sections.</param>
        /// <param name="aboutUsSectionsGetterService">Service for getting about us sections.</param>
        /// <param name="aboutUsSectionsDeleterService">Service for deleting about us sections.</param>
        /// <param name="aboutUsSectionsUpdaterService">Service for updating about us sections.</param>
        public NosotrosController(IAboutUsSectionsAdderService aboutUsSectionsAdderService, IAboutUsSectionsGetterService aboutUsSectionsGetterService, IAboutUsSectionsDeleterService aboutUsSectionsDeleterService, IAboutUsSectionsUpdaterService aboutUsSectionsUpdaterService)
        {
            _aboutUsSectionsAdderService = aboutUsSectionsAdderService;
            _aboutUsSectionsGetterService = aboutUsSectionsGetterService;
            _aboutUsSectionsDeleterService = aboutUsSectionsDeleterService;
            _aboutUsSectionsUpdaterService = aboutUsSectionsUpdaterService;
        }

        /// <summary>
        /// Gets a paginated list of about us sections based on the provided search and sort criteria.
        /// </summary>
        /// <param name="searchSortRequestDTO">An object containing search filters, sorting options, and pagination.</param>
        /// <returns>An ActionResult containing a PageResponseDTO of AboutUsResponseDTO objects that match the provided criteria.</returns>
        [HttpGet("")]
        public async Task<ActionResult<PageResponseDTO<AboutUsResponseDTO>>> GetAboutUsSections([FromQuery] SearchSortRequestDTO searchSortRequestDTO)
        {
            return Ok(await _aboutUsSectionsGetterService.GetAboutUsSections(searchSortRequestDTO));
        }

        /// <summary>
        /// Create a new about us section in the system.
        /// </summary>
        /// <param name="aboutUsSectionRequestDTO">The about us request data.</param>
        /// <returns>The created about us section.</returns>
        [HttpPost("")]
        public async Task<ActionResult<AboutUsResponseDTO>> CreateAboutUsSection([FromForm] AboutUsRequestDTO aboutUsSectionRequestDTO)
        {
            return Created("~/api/admin/nosotros", await _aboutUsSectionsAdderService.CreateAboutUsSection(aboutUsSectionRequestDTO));
        }

        /// <summary>
        /// Updates an specific about us section in the system.
        /// </summary>
        /// <param name="id">The ID of the about us section to update.</param>
        /// <param name="aboutUsSectionRequestDTO">The updated about us section data.</param>
        /// <returns>The updated about us section.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<AboutUsResponseDTO>> UpdateAboutUsSection([FromRoute] Guid id, [FromForm] AboutUsRequestDTO aboutUsSectionRequestDTO)
        {
            return Ok(await _aboutUsSectionsUpdaterService.UpdateAboutUsSection(id, aboutUsSectionRequestDTO));
        }

        /// <summary>
        /// Delete an about us section from the system by its ID.
        /// </summary>
        /// <param name="id">The ID of the about us section to delete.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<AboutUsResponseDTO>> DeleteAboutUsSection([FromRoute] Guid id)
        {
            await _aboutUsSectionsDeleterService.DeleteAboutUsSection(id);
            return NoContent();
        }
    }
}
