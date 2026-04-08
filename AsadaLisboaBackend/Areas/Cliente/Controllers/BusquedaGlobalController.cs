using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using AsadaLisboaBackend.ServiceContracts.SearchGlobal;

namespace AsadaLisboaBackend.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class BusquedaGlobalController : ControllerBase
    {
        private readonly ISearchGlobalService _searchGlobal;

        public BusquedaGlobalController(ISearchGlobalService searchGlobal)
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
