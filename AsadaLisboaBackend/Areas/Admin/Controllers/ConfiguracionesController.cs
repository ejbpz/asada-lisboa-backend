using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Configuration;
using AsadaLisboaBackend.ServiceContracts.Configurations;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    public class ConfiguracionesController : ControllerBase
    {
        private readonly IConfigurationsAdderService _configurationsAdderService;
        private readonly IConfigurationsGetterService _configurationsGetterService;
        private readonly IConfigurationsUpdaterService _configurationsUpdaterService;
        private readonly IConfigurationsDeleterService _configurationsDeleterService;

        public ConfiguracionesController(IConfigurationsGetterService configurationsGetterService, IConfigurationsAdderService configurationsAdderService, IConfigurationsUpdaterService configurationsUpdaterService, IConfigurationsDeleterService configurationsDeleterService)
        {
            _configurationsAdderService = configurationsAdderService;
            _configurationsGetterService = configurationsGetterService;
            _configurationsUpdaterService = configurationsUpdaterService;
            _configurationsDeleterService = configurationsDeleterService;
        }

        [HttpGet("")]
        public async Task<ActionResult<PageResponseDTO<ConfigurationResponseDTO>>> GetConfigurations([FromQuery] SearchSortRequestDTO searchSortRequestDTO)
        {
            return Ok(await _configurationsGetterService.GetConfigurations(searchSortRequestDTO));
        }

        [HttpPost("")]
        public async Task<ActionResult<ConfigurationResponseDTO>> CreateConfiguration([FromBody] ConfigurationsRequestDTO ConfigurationRequestDTO)
        {
            return Created("~/api/admin/configuracion", await _configurationsAdderService.CreateConfiguration(ConfigurationRequestDTO));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ConfigurationResponseDTO>> UpdateConfiguration([FromRoute] Guid id, [FromBody] ConfigurationsRequestDTO configurationRequestDTO)
        {
            return Ok(await _configurationsUpdaterService.UpdateConfiguration(id, configurationRequestDTO));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ConfigurationResponseDTO>> DeleteConfiguration([FromRoute] Guid id)
        {
            await _configurationsDeleterService.UpdateConfiguration(id);
            return NoContent();
        }
    }
}
