using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.ServiceContracts.Document;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.Utils.OptionsPattern;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    public class DocumentoController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IDocumentService _documentService;

        public DocumentoController(IWebHostEnvironment env, IDocumentService documentService)
        {
            _env = env;
            _documentService = documentService;
        }



        [HttpPost("")]
        public async Task<IActionResult> CreateImage([FromForm] DocumentRequestDTO documentRequestDTO)
        {
            var options = new FileStorageOptions
            {
                BasePath = Path.Combine(_env.WebRootPath, "uploads"),
                BaseUrl = "/uploads"
            };

            var result = await _documentService.CreateDocument(documentRequestDTO, options);
            return Ok(result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> EditDocument([FromRoute] Guid id, [FromForm] DocumentUpdateRequestDTO documentUpdateRequestDTO)
        {
            var options = new FileStorageOptions
            {
                BasePath = Path.Combine(_env.WebRootPath, "uploads"),
                BaseUrl = "/uploads"
            };

            var result = await _documentService.UpdateImage(id, DocumentUpdateRequestDTO, options);
            return Ok(result);
        }
    }
}
