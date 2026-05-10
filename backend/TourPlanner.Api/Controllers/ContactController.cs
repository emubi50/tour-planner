using Microsoft.AspNetCore.Mvc;
using TourPlanner.Bll.Interfaces;

namespace TourPlanner.Api.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await _contactService.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetContactById(int id)
        {
            var contact = await _contactService.GetByIdAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact([FromBody] Models.Contact contact)
        {
            var createdContact = await _contactService.CreateAsync(contact);
            return CreatedAtAction(
                nameof(GetContactById),
                new { id = createdContact?.Id },
                createdContact
            );
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateContact(int id, [FromBody] Models.Contact contact)
        {
            var success = await _contactService.UpdateByIdAsync(id, contact);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var success = await _contactService.DeleteByIdAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
