using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Editor;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.ServiceContracts.Editor;
using Microsoft.AspNetCore.Authorization;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    public class EditorController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IEditorAdderService _editorAdderService;
        private readonly INewsAdderService _newsAdderService;

        public EditorController(IWebHostEnvironment env, IEditorAdderService editorAdderService, INewsAdderService newsAdderService)
        {
            _env = env;
            _editorAdderService = editorAdderService;
            _newsAdderService = newsAdderService;
        }

        [AllowAnonymous]
        [HttpPost("imagen-temp")]
        public async Task<IActionResult> CreateTemporalImage([FromForm] EditorRequestDTO editorRequestDTO)
        {
            var options = new FileStorageOptions
            {
                BasePath = Path.Combine(_env.WebRootPath, "temp"),
                BaseUrl = "/temp"
            };

            return Created("~/api/admin/editor", await _editorAdderService.CreateTemporalImage(editorRequestDTO, options));
        }
    }
}
