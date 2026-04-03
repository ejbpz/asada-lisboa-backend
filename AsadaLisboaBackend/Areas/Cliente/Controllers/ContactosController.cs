using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.ServiceContracts.Contacts;

namespace AsadaLisboaBackend.Areas.Cliente.Controllers
{
    [ApiController]
    [Area("Cliente")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class ContactosController : ControllerBase
    {
        private readonly IContactsGetterService _contactsGetterService;

        public ContactosController(IContactsGetterService contactsGetterService)
        {
            _contactsGetterService = contactsGetterService;
        }

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
