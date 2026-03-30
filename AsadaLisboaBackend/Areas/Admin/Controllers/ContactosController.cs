using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.ServiceContracts.Contacts;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    public class ContactosController : ControllerBase
    {
        private readonly IContactsAdderService _contactsAdderService;
        private readonly IContactsGetterService _contactsGetterService;
        private readonly IContactsUpdaterService _contactsUpdaterService;
        private readonly IContactsDeleterService _contactsDeleterService;

        public ContactosController(IContactsGetterService contactsGetterService, IContactsAdderService contactsAdderService, IContactsUpdaterService contactsUpdaterService, IContactsDeleterService contactsDeleterService)
        {
            _contactsAdderService = contactsAdderService;
            _contactsGetterService = contactsGetterService;
            _contactsUpdaterService = contactsUpdaterService;
            _contactsDeleterService = contactsDeleterService;
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
        public async Task<ActionResult<ContactResponseDTO>> UpdateContact([FromRoute] Guid id, [FromBody] ContactRequestDTO contactRequestDTO)
        {
            return Ok(await _contactsUpdaterService.UpdateContact(id, contactRequestDTO));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ContactResponseDTO>> DeleteContact([FromRoute] Guid id)
        {
            await _contactsDeleterService.DeleteContact(id);
            return NoContent();
        }
    }
}
