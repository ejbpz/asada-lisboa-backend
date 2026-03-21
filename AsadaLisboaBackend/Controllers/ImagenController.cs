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
        [Route("creacion")]
        public async Task<IActionResult> CreateImage([FromForm] ImageRequestDTO imageRequestDTO)
        {
            var result = await _imageService.CreateImage(imageRequestDTO);
            return Ok(result);
        }

        [HttpPut]
        [Route("edicion")]
        public async Task<IActionResult> Edit([FromForm] ImageUpdateRequestDTO ImageUpdateRequestDTO)
        {
            try
            {
                var result = await _imageService.UpdateImage(ImageUpdateRequestDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete]
        [Route("delete/{id}/{slug}")]
        public async Task<IActionResult> Delete(Guid id, string slug)
        {
            try
            {
                var result = await _imageService.DeleteImage(id, slug);
                return Ok(new { success = result, message = "Imagen eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
