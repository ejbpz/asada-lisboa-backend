using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.ServiceContracts.AboutUsSections;

namespace AsadaLisboaBackend.Areas.Cliente.Controllers
{
    [ApiController]
    [Area("Cliente")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class NosotrosController : ControllerBase
    {
        private readonly IAboutUsSectionsGetterService _aboutUsSectionsGetterService;

        public NosotrosController(IAboutUsSectionsGetterService aboutUsSectionsGetterService)
        {
            _aboutUsSectionsGetterService = aboutUsSectionsGetterService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAboutUsSections()
        {
            return Ok(await _aboutUsSectionsGetterService.GetAboutUsSections(new SearchSortRequestDTO()
            {
                Page = 1,
                Take = 100,
                Offset = 0,
                Search = null,
                FilterBy = null,
                SortBy = "order",
                SortDirection = "asc",
            }));
        }
    }
}
