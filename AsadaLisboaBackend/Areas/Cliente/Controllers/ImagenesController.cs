using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.ServiceContracts.Images;

namespace AsadaLisboaBackend.Areas.Cliente.Controllers
{
    [ApiController]
    [Area("Cliente")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class ImagenesController : ControllerBase
    {
        private readonly IImagesGetterService _imagesGetterService;

        public ImagenesController(IImagesGetterService imagesGetterService)
        {
            _imagesGetterService = imagesGetterService;
        }

        [HttpGet("")]
        public async Task<ActionResult<PageResponseDTO<ImageResponseDTO>>> GetImages([FromQuery] SearchSortRequestDTO searchSortRequestDTO)
        {
            return Ok(await _imagesGetterService.GetImages(searchSortRequestDTO));
        }

        [HttpGet("{slug}")]
        public async Task<ActionResult<ImageResponseDTO>> GetImage([FromRoute] string slug)
        {
            return Ok(await _imagesGetterService.GetImageBySlug(slug));
        }
    }
}
