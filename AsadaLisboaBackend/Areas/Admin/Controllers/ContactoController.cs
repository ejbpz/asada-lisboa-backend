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
        private readonly IContactsAdderService _contactsAdderService;
        private readonly IContactsGetterService _contactsGetterService;

        public ContactoController(IContactsGetterService contactsGetterService, IContactsAdderService contactsAdderService)
        {
            _contactsGetterService = contactsGetterService;
            _contactsAdderService = contactsAdderService;
        }

        [HttpGet("")]
        public async Task<ActionResult<PageResponseDTO<ContactResponseDTO>>> GetContacts([FromQuery] SearchSortRequestDTO searchSortRequestDTO)
        {
            return Ok(await _contactsGetterService.GetContacts(searchSortRequestDTO));
        }

        [HttpPost("")]
        public async Task<ActionResult<ContactResponseDTO>> CreateContact([FromBody] ContactRequestDTO contactRequestDTO)
        {
            return Created("~/api/admin/contacto", await _contactsAdderService.CreateContact(contactRequestDTO));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ContactResponseDTO>> UpdateContact([FromQuery] Guid id, [FromBody] ContactRequestDTO contactRequestDTO)
        {
            return Ok();
        }
    }
}
