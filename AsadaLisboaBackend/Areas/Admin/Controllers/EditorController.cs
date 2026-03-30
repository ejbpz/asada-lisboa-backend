using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Editor;
using AsadaLisboaBackend.ServiceContracts.Editor;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    public class EditorController : ControllerBase
    {
        private readonly IEditorAdderService _editorAdderService;

        public EditorController(IEditorAdderService editorAdderService)
        {
            _editorAdderService = editorAdderService;
        }

        [HttpPost("imagen-temp")]
        public async Task<IActionResult> CreateTemporalImage([FromForm] EditorRequestDTO editorRequestDTO)
        {
            return Created("~/api/admin/editor", await _editorAdderService.CreateTemporalImage(editorRequestDTO));
        }
    }
}
