using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.SearchGlobal;
using AsadaLisboaBackend.ServiceContracts.SearchGlobal;

namespace AsadaLisboaBackend.Areas.Cliente.Controllers
{
    /// <summary>
    /// Controller for searching information, public access.
    /// </summary>
    [ApiController]
    [Area("Cliente")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class BuscadorController : ControllerBase
    {
        private readonly ISearchGlobalService _searchGlobalService;

        /// <summary>
        /// Constructor for BuscadorController.
        /// </summary>
        /// <param name="searchGlobalService">Service for getting search data.</param>
        public BuscadorController(ISearchGlobalService searchGlobalService)
        {
            _searchGlobalService = searchGlobalService;
        }

        /// <summary>
        /// Retrieve the data from news, documents and/or images.
        /// </summary>
        /// <param name="query">Query to search into documents, images and news.</param>
        /// <returns>ActionResult for List of SearchGlobalDocument.</returns>
        [HttpGet("")]
        public async Task<ActionResult<List<SearchGlobalDocument>>> Search([FromQuery] string query)
        {
            return Ok(await _searchGlobalService.Search(query));
        }

    }
}
