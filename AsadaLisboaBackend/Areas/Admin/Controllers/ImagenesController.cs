using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.Enums;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Status;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.ServiceContracts.Image;
using AsadaLisboaBackend.ServiceContracts.Statuses;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    public class ImagenesController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IImageService _imageService;
        private readonly IImagesGetterService _imagesGetterService;
        private readonly IStatusesUpdaterService _statusesUpdaterService;

        public ImagenesController(IImageService imageService, IWebHostEnvironment env, IImagesGetterService imagesGetterService, IStatusesUpdaterService statusesUpdaterService)
        {
            _env = env;
            _imageService = imageService;
            _imagesGetterService = imagesGetterService;
            _statusesUpdaterService = statusesUpdaterService;
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

        [HttpPut("{id}")]
        public async Task<IActionResult> EditImage([FromRoute] Guid id, [FromForm] ImageUpdateRequestDTO ImageUpdateRequestDTO)
        {
            var options = new FileStorageOptions
            {
                BasePath = Path.Combine(_env.WebRootPath, "uploads"),
                BaseUrl = "/uploads"
            };

            var result = await _imageService.UpdateImage(id, ImageUpdateRequestDTO, options);
            return Ok(result);
        }

        [HttpPut("cambiar-estado/{id}")]
        public async Task<IActionResult> ChangeImageStatus([FromRoute] Guid id, [FromBody] StatusChangeRequestDTO statusChangeRequestDTO)
        {
            await _statusesUpdaterService.ChangeStatus(id, statusChangeRequestDTO.StatusId, ObjectTypeEnum.Image);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            await _imageService.DeleteImage(id);
            return NoContent();
        }

    }
}
