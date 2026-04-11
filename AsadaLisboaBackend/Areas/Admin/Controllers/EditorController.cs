using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Editor;
using AsadaLisboaBackend.ServiceContracts.Editors;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for enricher text editor, only accessible by admin users. 
    /// </summary>
    [ApiController]
    [Area("Admin")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class EditorController : ControllerBase
    {
        private readonly IEditorsAdderService _editorsAdderService;

        /// <summary>
        /// Constructor for EditorController.
        /// </summary>
        /// <param name="editorsAdderService">Service for adding editor content.</param>
        public EditorController(IEditorsAdderService editorsAdderService)
        {
            _editorsAdderService = editorsAdderService;
        }

        /// <summary>
        /// Create a temporal image for the enricher text editor. Uploads a image file to a temporal folder.
        /// </summary>
        /// <param name="editorRequestDTO">File object with the new temporal image.</param>
        /// <returns>ActionResult containing the created temporal image url.</returns>
        [HttpPost("imagen-temp")]
        public async Task<IActionResult> CreateTemporalImage([FromForm] EditorRequestDTO editorRequestDTO)
        {
            return Created("~/api/admin/editor", await _editorsAdderService.CreateTemporalImage(editorRequestDTO));
        }
    }
}
