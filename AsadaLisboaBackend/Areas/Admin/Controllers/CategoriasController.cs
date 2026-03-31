using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.ServiceContracts.Categories;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriesDeleterService _categoriesDeleterService;

        public CategoriasController(ICategoriesDeleterService categoriesDeleterService)
        {
            _categoriesDeleterService = categoriesDeleterService;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            await _categoriesDeleterService.DeleteCategory(id);
            return NoContent();
        }
    }
}
