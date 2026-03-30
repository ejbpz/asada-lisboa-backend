using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Editor;
using AsadaLisboaBackend.ServiceContracts.Editors;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    public class EditorController : ControllerBase
    {
        private readonly IEditorsAdderService _editorsAdderService;

        public EditorController(IEditorsAdderService editorsAdderService)
        {
            _editorsAdderService = editorsAdderService;
        }

        [HttpPost("imagen-temp")]
        public async Task<IActionResult> CreateTemporalImage([FromForm] EditorRequestDTO editorRequestDTO)
        {
            return Created("~/api/admin/editor", await _editorsAdderService.CreateTemporalImage(editorRequestDTO));
        }
    }
}
