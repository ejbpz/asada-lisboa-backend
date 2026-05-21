using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Search;
using AsadaLisboaBackend.ServiceContracts.Searches;

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
        private readonly ISearchesGetterService _searchesGetterService;

        /// <summary>
        /// Constructor for BuscadorController.
        /// </summary>
        /// <param name="searchesGetterService">Service for getting search data.</param>
        public BuscadorController(ISearchesGetterService searchesGetterService)
        {
            _searchesGetterService = searchesGetterService;
        }

        /// <summary>
        /// Retrieve the data from news, documents and/or images.
        /// </summary>
        /// <param name="query">Query to search into documents, images and news.</param>
        /// <returns>ActionResult for List of SearchResponseDTO.</returns>
        [HttpGet("")]
        public async Task<ActionResult<List<SearchResponseDTO>>> Search([FromQuery] string query)
        {
            return Ok(await _searchesGetterService.Search(query));
        }

    }
}
