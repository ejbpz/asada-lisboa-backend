using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Category;
using AsadaLisboaBackend.ServiceContracts.Categories;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriesGetterService _categoriesGetterService;
        private readonly ICategoriesDeleterService _categoriesDeleterService;

        public CategoriasController(ICategoriesDeleterService categoriesDeleterService, ICategoriesGetterService categoriesGetterService)
        {
            _categoriesGetterService = categoriesGetterService;
            _categoriesDeleterService = categoriesDeleterService;
        }
        [HttpGet("")]
        public async Task<ActionResult<List<CategoryResponseDTO>>> GetCategories()
        {
            return Ok(await _categoriesGetterService.GetCategories());
        }

        [HttpGet("buscar")]
        public async Task<ActionResult<List<CategoryResponseDTO>>> SearchCategories([FromQuery] string search)
        {
            return Ok(await _categoriesGetterService.SearchCategories(search));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            await _categoriesDeleterService.DeleteCategory(id);
            return NoContent();
        }
    }
}
