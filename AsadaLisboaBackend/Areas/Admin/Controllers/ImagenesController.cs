using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.Enums;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Status;
using AsadaLisboaBackend.ServiceContracts.Images;
using AsadaLisboaBackend.ServiceContracts.Statuses;

namespace AsadaLisboaBackend.Areas.Admin.Controllers;

/// <summary>
/// Controller for managing images, only accessible by admin users.
/// </summary>
[ApiController]
[Area("Admin")]
[ApiVersion("1.0")]
[Route("api/[area]/[controller]")]
public class ImagenesController : ControllerBase
{
    private readonly IImagesAdderService _imagesAdderService;
    private readonly IImagesGetterService _imagesGetterService;
    private readonly IImagesDeleterService _imagesDeleterService;
    private readonly IImagesUpdaterService _imagesUpdaterService;
    private readonly IStatusesUpdaterService _statusesUpdaterService;

    /// <summary>
    /// Constructor for the ImagenesController.
    /// </summary>
    /// <param name="imagesAdderService">Service for adding images.</param>
    /// <param name="imagesGetterService">Service for getting images.</param>
    /// <param name="imagesDeleterService">Service for deleting images.</param>
    /// <param name="imagesUpdaterService">Service for updating images.</param>
    /// <param name="statusesUpdaterService">Service for updating statuses.</param>
    public ImagenesController(IImagesAdderService imagesAdderService, IImagesGetterService imagesGetterService, IImagesDeleterService imagesDeleterService, IImagesUpdaterService imagesUpdaterService, IStatusesUpdaterService statusesUpdaterService)
    {
        _imagesAdderService = imagesAdderService;
        _imagesGetterService = imagesGetterService;
        _imagesDeleterService = imagesDeleterService;
        _imagesUpdaterService = imagesUpdaterService;
        _statusesUpdaterService = statusesUpdaterService;
    }

    /// <summary>
    /// Gets a paginated list of images based on the provided search and sort criteria.
    /// </summary>
    /// <param name="searchSortRequestDto">The search and sort criteria for retrieving images.</param>
    /// <returns>An ActionResult containing a paginated list of ImageResponseDTO objects.</returns>
    [HttpGet("")]
    public async Task<ActionResult<PageResponseDto<ImageResponseDto>>> GetImages([FromQuery] SearchSortRequestDto searchSortRequestDto)
    {
        searchSortRequestDto.IsPublic = false;
        return Ok(await _imagesGetterService.GetImages(searchSortRequestDto));
    }

    /// <summary>
    /// Get a specific image by its unique identifier (ID).
    /// </summary>
    /// <param name="id">The unique identifier of the image.</param>
    /// <returns>An ActionResult containing the ImageResponseDTO object.</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ImageResponseDto>> GetImage([FromRoute] Guid id)
    {
        return Ok(await _imagesGetterService.GetImage(id));
    }

    /// <summary>
    /// Creates a new image based on the provided ImageRequestDTO object.
    /// </summary>
    /// <param name="imageRequestDto">The ImageRequestDTO object containing the image details.</param>
    /// <returns>An ActionResult indicating the result of the create operation.</returns>
    [HttpPost("")]
    public async Task<IActionResult> CreateImage([FromForm] ImageRequestDto imageRequestDto)
    {
        return Created("~/api/admin/imagenes", await _imagesAdderService.CreateImage(imageRequestDto));
    }

    /// <summary>
    /// Updates an existing image identified by its unique identifier (ID) with the provided ImageUpdateRequestDTO object.
    /// </summary>
    /// <param name="id">The unique identifier of the image to be updated.</param>
    /// <param name="imageUpdateRequestDto">The ImageUpdateRequestDTO object containing the updated image details.</param>
    /// <returns>An ActionResult indicating the result of the update operation.</returns>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> EditImage([FromRoute] Guid id, [FromForm] ImageUpdateRequestDto imageUpdateRequestDto)
    {
        return Ok(await _imagesUpdaterService.UpdateImage(id, imageUpdateRequestDto));
    }

    /// <summary>
    /// Updates the status of the specified image using the provided status change request.
    /// </summary>
    /// <param name="id">The unique identifier of the image whose status is to be updated.</param>
    /// <param name="statusChangeRequestDto">An object containing the new status identifier to apply to the image. Cannot be null.</param>
    /// <returns>No content.</returns>
    [HttpPatch("cambiar-estado/{id:guid}")]
    public async Task<IActionResult> ChangeImageStatus([FromRoute] Guid id, [FromForm] StatusChangeRequestDto statusChangeRequestDto)
    {
        await _statusesUpdaterService.ChangeStatus(id, statusChangeRequestDto.StatusId, ObjectTypeEnum.Image);
        return NoContent();
    }
    
    /// <summary>
    /// Deletes the specified image by its unique identifier (ID).
    /// </summary>
    /// <param name="id">The unique identifier of the image to be deleted.</param>
    /// <returns>No content.</returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteImage(Guid id)
    {
        await _imagesDeleterService.DeleteImage(id);
        return NoContent();
    }
}
