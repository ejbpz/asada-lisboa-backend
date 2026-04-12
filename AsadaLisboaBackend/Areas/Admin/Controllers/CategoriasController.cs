using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Category;
using AsadaLisboaBackend.ServiceContracts.Categories;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for manage categories, only accessible by admin users.
    /// </summary>
    [ApiController]
    [Area("Admin")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriesGetterService _categoriesGetterService;
        private readonly ICategoriesDeleterService _categoriesDeleterService;

        /// <summary>
        /// Constructor for CategoriasController.
        /// </summary>
        /// <param name="categoriesGetterService">Service for getting categories.</param>
        /// <param name="categoriesDeleterService">Service for deleting categories.</param>
        public CategoriasController(ICategoriesDeleterService categoriesDeleterService, ICategoriesGetterService categoriesGetterService)
        {
            _categoriesGetterService = categoriesGetterService;
            _categoriesDeleterService = categoriesDeleterService;
        }

        /// <summary>
        /// Get all existing categories in the system.
        /// </summary>
        /// <returns>List of categories.</returns>
        [HttpGet("")]
        public async Task<ActionResult<List<CategoryResponseDTO>>> GetCategories()
        {
            return Ok(await _categoriesGetterService.GetCategories());
        }

        /// <summary>
        /// Search for categories by name. The search is case-insensitive and matches any category that contains the search string in its name.
        /// </summary>
        /// <param name="search">The search string to filter categories by name.</param>
        /// <returns>List of categories that match the search string.</returns>
        [HttpGet("buscar")]
        public async Task<ActionResult<List<CategoryResponseDTO>>> SearchCategories([FromQuery] string search)
        {
            return Ok(await _categoriesGetterService.SearchCategories(search));
        }

        /// <summary>
        /// Delete a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            await _categoriesDeleterService.DeleteCategory(id);
            return NoContent();
        }
    }
}
