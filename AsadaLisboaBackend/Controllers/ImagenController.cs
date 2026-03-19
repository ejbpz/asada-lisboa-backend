using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.ServiceContracts.Image;
using AsadaLisboaBackend.Models.DTOs.Image;

namespace AsadaLisboaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagenController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImagenController(IImageService imageService)
        {

            _imageService = imageService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateImage([FromForm] ImageRequestDTO imageRequestDTO)
        {
            var result = await _imageService.CreateImage(imageRequestDTO);
            return Ok(result);
        }


    }
}
