using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Configuration;
using AsadaLisboaBackend.ServiceContracts.Configurations;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for manage system configurations, only accessible by admin users.
    /// </summary>
    [ApiController]
    [Area("Admin")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class ConfiguracionesController : ControllerBase
    {
        private readonly IConfigurationsAdderService _configurationsAdderService;
        private readonly IConfigurationsGetterService _configurationsGetterService;
        private readonly IConfigurationsUpdaterService _configurationsUpdaterService;
        private readonly IConfigurationsDeleterService _configurationsDeleterService;

        /// <summary>
        /// Constructor for ConfiguracionesController.
        /// </summary>
        /// <param name="configurationsAdderService">Service for adding configurations.</param>
        /// <param name="configurationsGetterService">Service for getting configurations.</param>
        /// <param name="configurationsUpdaterService">Service for updating configurations.</param>
        /// <param name="configurationsDeleterService">Service for deleting configurations.</param>
        public ConfiguracionesController(IConfigurationsGetterService configurationsGetterService, IConfigurationsAdderService configurationsAdderService, IConfigurationsUpdaterService configurationsUpdaterService, IConfigurationsDeleterService configurationsDeleterService)
        {
            _configurationsAdderService = configurationsAdderService;
            _configurationsGetterService = configurationsGetterService;
            _configurationsUpdaterService = configurationsUpdaterService;
            _configurationsDeleterService = configurationsDeleterService;
        }

        /// <summary>
        /// Retrieves a paginated list of configuration records based on the specified search and sort criteria.
        /// </summary>
        /// <param name="searchSortRequestDTO">An object containing search filters, sorting options, and pagination parameters to apply to the
        /// configuration query. Cannot be null.</param>
        /// <returns>An ActionResult containing a PageResponseDTO of ConfigurationResponseDTO objects that match the provided
        /// criteria. The result includes pagination metadata and may be empty if no configurations match.</returns>
        [HttpGet("")]
        public async Task<ActionResult<PageResponseDTO<ConfigurationResponseDTO>>> GetConfigurations([FromQuery] SearchSortRequestDTO searchSortRequestDTO)
        {
            return Ok(await _configurationsGetterService.GetConfigurations(searchSortRequestDTO));
        }

        /// <summary>
        /// Creates a new configuration record.
        /// </summary>
        /// <param name="configurationRequestDTO">An object containing the details of the configuration to be created. Cannot be null.</param>
        /// <returns>An ActionResult containing the created ConfigurationResponseDTO object.</returns>
        [HttpPost("")]
        public async Task<ActionResult<ConfigurationResponseDTO>> CreateConfiguration([FromForm] ConfigurationsRequestDTO configurationRequestDTO)
        {
            return Created("~/api/admin/configuracion", await _configurationsAdderService.CreateConfiguration(configurationRequestDTO));
        }

        /// <summary>
        /// Update an existing configuration record by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the configuration to update.</param>
        /// <param name="configurationRequestDTO">An object containing the updated details of the configuration. Cannot be null.</param>
        /// <returns>An ActionResult containing the updated ConfigurationResponseDTO object.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ConfigurationResponseDTO>> UpdateConfiguration([FromRoute] Guid id, [FromForm] ConfigurationsRequestDTO configurationRequestDTO)
        {
            return Ok(await _configurationsUpdaterService.UpdateConfiguration(id, configurationRequestDTO));
        }

        /// <summary>
        /// Deletes the configuration with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the configuration to delete.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ConfigurationResponseDTO>> DeleteConfiguration([FromRoute] Guid id)
        {
            await _configurationsDeleterService.UpdateConfiguration(id);
            return NoContent();
        }
    }
}
