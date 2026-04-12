using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.ServiceContracts.SearchGlobal;

namespace AsadaLisboaBackend.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class BuscadorController : ControllerBase
    {
        private readonly ISearchGlobalService _searchGlobal;

        public BuscadorController(ISearchGlobalService searchGlobal)
        {
            _searchGlobal = searchGlobal;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string q)
        {
            var result = await _searchGlobal.SearchAsync(q);
            return Ok(result);
        }

    }
}
