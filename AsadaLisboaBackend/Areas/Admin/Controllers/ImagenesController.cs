using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.Enums;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Status;
using AsadaLisboaBackend.ServiceContracts.Image;
using AsadaLisboaBackend.ServiceContracts.Statuses;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    public class ImagenesController : ControllerBase
    {
        private readonly IImagesAdderService _imagesAdderService;
        private readonly IImagesGetterService _imagesGetterService;
        private readonly IImagesDeleterService _imagesDeleterService;
        private readonly IImagesUpdaterService _imagesUpdaterService;
        private readonly IStatusesUpdaterService _statusesUpdaterService;

        public ImagenesController(IImagesAdderService imagesAdderService, IImagesGetterService imagesGetterService, IImagesDeleterService imagesDeleterService, IImagesUpdaterService imagesUpdaterService, IStatusesUpdaterService statusesUpdaterService)
        {
            _imagesAdderService = imagesAdderService;
            _imagesGetterService = imagesGetterService;
            _imagesDeleterService = imagesDeleterService;
            _imagesUpdaterService = imagesUpdaterService;
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
            return Created("~/api/admin/imagenes", await _imagesAdderService.CreateImage(imageRequestDTO));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditImage([FromRoute] Guid id, [FromForm] ImageUpdateRequestDTO ImageUpdateRequestDTO)
        {
            return Ok(await _imagesUpdaterService.UpdateImage(id, ImageUpdateRequestDTO));
        }

        [HttpPatch("cambiar-estado/{id}")]
        public async Task<IActionResult> ChangeImageStatus([FromRoute] Guid id, [FromBody] StatusChangeRequestDTO statusChangeRequestDTO)
        {
            await _statusesUpdaterService.ChangeStatus(id, statusChangeRequestDTO.StatusId, ObjectTypeEnum.Image);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            await _imagesDeleterService.DeleteImage(id);
            return NoContent();
        }

    }
}
