using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.ServiceContracts.Images;

namespace AsadaLisboaBackend.Areas.Cliente.Controllers
{
    /// <summary>
    /// Controller for getting images, public access.
    /// </summary>
    [ApiController]
    [Area("Cliente")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class ImagenesController : ControllerBase
    {
        private readonly IImagesGetterService _imagesGetterService;

        /// <summary>
        /// Constructor for ImagenesController.
        /// </summary>
        /// <param name="imagesGetterService">Service for getting images.</param>
        public ImagenesController(IImagesGetterService imagesGetterService)
        {
            _imagesGetterService = imagesGetterService;
        }

        /// <summary>
        /// Retrieves a paginated list of images based on the specified search and sort criteria.
        /// </summary>
        /// <param name="searchSortRequestDTO">An object containing the search filters, sorting options, and pagination parameters to apply when retrieving.</param>
        /// <returns>An ActionResult containing a PageResponseDTO of ImageResponseDTO objects that match the provided criteria.</returns>
        [HttpGet("")]
        public async Task<ActionResult<PageResponseDTO<ImageResponseDTO>>> GetImages([FromQuery] SearchSortRequestDTO searchSortRequestDTO)
        {
            return Ok(await _imagesGetterService.GetImages(searchSortRequestDTO));
        }

        /// <summary>
        /// Retrieves the image associated with the specified slug.
        /// </summary>
        /// <param name="slug">The unique identifier for the image to retrieve. Cannot be null or empty.</param>
        /// <returns>An ActionResult of ImageResponseDTO containing the image data.</returns>
        [HttpGet("{slug}")]
        public async Task<ActionResult<ImageResponseDTO>> GetImage([FromRoute] string slug)
        {
            return Ok(await _imagesGetterService.GetImageBySlug(slug));
        }
    }
}
