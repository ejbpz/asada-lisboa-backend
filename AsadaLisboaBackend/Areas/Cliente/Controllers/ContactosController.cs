using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.ServiceContracts.Contacts;

namespace AsadaLisboaBackend.Areas.Cliente.Controllers
{
    /// <summary>
    /// Controller for getting contact information, public access.
    /// </summary>
    [ApiController]
    [Area("Cliente")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class ContactosController : ControllerBase
    {
        private readonly IContactsGetterService _contactsGetterService;

        /// <summary>
        /// Constructor for ContactosController.
        /// </summary>
        /// <param name="contactsGetterService">Service for getting contacts.</param>
        public ContactosController(IContactsGetterService contactsGetterService)
        {
            _contactsGetterService = contactsGetterService;
        }

        /// <summary>
        /// Retrieves a paginated list of contacts using default search and sort parameters.
        /// </summary>
        /// <returns>An ActionResult containing a PageResponseDTO of ContactResponseDTO, sorted in ascending order by the 'order' field.</returns>
        [HttpGet("")]
        public async Task<ActionResult<PageResponseDTO<ContactResponseDTO>>> GetContacts()
        {
            return Ok(await _contactsGetterService.GetContacts(new SearchSortRequestDTO()
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
