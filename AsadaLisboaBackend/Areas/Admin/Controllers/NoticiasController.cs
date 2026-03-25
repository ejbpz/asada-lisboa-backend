using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.ServiceContracts.News;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    public class NoticiasController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly INewsAdderService _newsAdderService;
        private readonly INewsUpdaterService _newsUpdaterService;
        private readonly INewsDeleterService _newsDeleterService;

        public NoticiasController(INewsAdderService newsAdderService, INewsUpdaterService newsUpdaterService, INewsDeleterService newsDeleterService, IWebHostEnvironment env)
        {
            _env = env;
            _newsAdderService = newsAdderService;
            _newsUpdaterService = newsUpdaterService;
            _newsDeleterService = newsDeleterService;
        }

        [HttpPost("")]
        public async Task<ActionResult<NewResponseDTO>> CreateNew([FromBody] NewRequestDTO newRequestDTO)
        {
            var options = new FileStorageOptions
            {
                BasePath = Path.Combine(_env.WebRootPath, "news"),
                BaseUrl = "/news"
            };

            return Created("~/api/admin/noticias", await _newsAdderService.CreateNew(newRequestDTO, options));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<NewResponseDTO>> UpdateNew([FromRoute] Guid id, [FromBody] NewRequestDTO newRequestDTO)
        {
            var options = new FileStorageOptions
            {
                BasePath = Path.Combine(_env.WebRootPath, "news"),
                BaseUrl = "/news"
            };

            return Ok(await _newsUpdaterService.UpdateNew(id, newRequestDTO, options));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<NewResponseDTO>> DeleteNew([FromRoute] Guid id)
        {
            var options = new FileStorageOptions
            {
                BasePath = Path.Combine(_env.WebRootPath, "news"),
                BaseUrl = "/news"
            };

            await _newsDeleterService.DeleteNew(id, options);
            return NoContent();
        }
    }
}
