using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.ServiceContracts.Contacts;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    public class ContactoController : ControllerBase
    {
        private readonly IContactsGetterService _contactsGetterService;

        public ContactoController(IContactsGetterService contactsGetterService)
        {
            _contactsGetterService = contactsGetterService;
        }

        [HttpGet("")]
        public async Task<ActionResult<PageResponseDTO<ContactResponseDTO>>> GetContacts([FromQuery] SearchSortRequestDTO searchSortRequestDTO)
        {
            return Ok(await _contactsGetterService.GetContacts(searchSortRequestDTO));
        }
    }
}
