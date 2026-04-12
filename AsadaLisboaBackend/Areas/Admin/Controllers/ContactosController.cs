using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.ServiceContracts.Contacts;

namespace AsadaLisboaBackend.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for manage user contacts, only accessible by admin users.
    /// </summary>
    [ApiController]
    [Area("Admin")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class ContactosController : ControllerBase
    {
        private readonly IContactsAdderService _contactsAdderService;
        private readonly IContactsGetterService _contactsGetterService;
        private readonly IContactsUpdaterService _contactsUpdaterService;
        private readonly IContactsDeleterService _contactsDeleterService;

        /// <summary>
        /// Constructor for ContactosController.
        /// </summary>
        /// <param name="contactsAdderService">Service for adding contacts.</param>
        /// <param name="contactsGetterService">Service for getting contacts.</param>
        /// <param name="contactsUpdaterService">Service for updating contacts.</param>
        /// <param name="contactsDeleterService">Service for deleting contacts.</param>
        public ContactosController(IContactsGetterService contactsGetterService, IContactsAdderService contactsAdderService, IContactsUpdaterService contactsUpdaterService, IContactsDeleterService contactsDeleterService)
        {
            _contactsAdderService = contactsAdderService;
            _contactsGetterService = contactsGetterService;
            _contactsUpdaterService = contactsUpdaterService;
            _contactsDeleterService = contactsDeleterService;
        }

        /// <summary>
        /// Retrieves a list of contacts based on the provided search and sort parameters.
        /// </summary>
        /// <param name="searchSortRequestDTO">An object containing search filters, sorting options, and pagination parameters to apply to the
        /// contact query. Cannot be null.</param>
        /// <returns>An ActionResult containing a PageResponseDTO of ContactResponseDTO objects that match the provided
        /// criteria. The result includes pagination metadata and may be empty if no contacts match.</returns>
        [HttpGet("")]
        public async Task<ActionResult<PageResponseDTO<ContactResponseDTO>>> GetContacts([FromQuery] SearchSortRequestDTO searchSortRequestDTO)
        {
            return Ok(await _contactsGetterService.GetContacts(searchSortRequestDTO));
        }

        /// <summary>
        /// Creates a new contact record.
        /// </summary>
        /// <param name="contactRequestDTO">An object containing the details of the contact to be created. Cannot be null.</param>
        /// <returns>An ActionResult containing the created ContactResponseDTO object.</returns>
        [HttpPost("")]
        public async Task<ActionResult<ContactResponseDTO>> CreateContact([FromForm] ContactRequestDTO contactRequestDTO)
        {
            return Created("~/api/admin/contacto", await _contactsAdderService.CreateContact(contactRequestDTO));
        }

        /// <summary>
        /// Updates an existing contact record identified by the provided ID.
        /// </summary>
        /// <param name="id">The unique identifier of the contact to update.</param>
        /// <param name="contactRequestDTO">An object containing the updated details of the contact. Cannot be null.</param>
        /// <returns>An ActionResult containing the updated ContactResponseDTO object.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ContactResponseDTO>> UpdateContact([FromRoute] Guid id, [FromForm] ContactRequestDTO contactRequestDTO)
        {
            return Ok(await _contactsUpdaterService.UpdateContact(id, contactRequestDTO));
        }

        /// <summary>
        /// Deletes an existing contact record identified by the provided ID.
        /// </summary>
        /// <param name="id">The unique identifier of the contact to delete.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ContactResponseDTO>> DeleteContact([FromRoute] Guid id)
        {
            await _contactsDeleterService.DeleteContact(id);
            return NoContent();
        }
    }
}
