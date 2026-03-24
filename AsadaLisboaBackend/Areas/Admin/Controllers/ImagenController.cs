using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.ServiceContracts.Image;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    public class ImagenController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IImageService _imageService;
        private readonly IImagesGetterService _imagesGetterService;

        public ImagenController(IImageService imageService, IWebHostEnvironment env, IImagesGetterService imagesGetterService)
        {
            _env = env;
            _imageService = imageService;
            _imagesGetterService = imagesGetterService;
        }

        [HttpGet("")]
        public async Task<ActionResult<PageResponseDTO<ImageResponseDTO>>> GetImages([FromQuery] SearchSortRequestDTO searchSortRequestDTO)
        {
            return Ok(await _imagesGetterService.GetImages(searchSortRequestDTO));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PageResponseDTO<ImageResponseDTO>>> GetImage([FromRoute] Guid id)
        {
            return Ok(await _imagesGetterService.GetImage(id));
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateImage([FromForm] ImageRequestDTO imageRequestDTO)
        {
            var options = new FileStorageOptions
            {
                BasePath = Path.Combine(_env.WebRootPath, "uploads"),
                BaseUrl = "/uploads"
            };

            var result = await _imageService.CreateImage(imageRequestDTO, options);
            return Ok(result);
        }

        [HttpPut("")]
        public async Task<IActionResult> EditImage([FromForm] ImageUpdateRequestDTO ImageUpdateRequestDTO)
        {
            var options = new FileStorageOptions
            {
                BasePath = Path.Combine(_env.WebRootPath, "uploads"),
                BaseUrl = "/uploads"
            };

            var result = await _imageService.UpdateImage(ImageUpdateRequestDTO, options);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            await _imageService.DeleteImage(id);
            return NoContent();
        }

    }
}
