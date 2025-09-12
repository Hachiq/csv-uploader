using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController(IContactService _contactService) : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            // TODO: validate file type and size
            // TODO: validate data inside the file
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();
            await _contactService.UploadContactsAsync(stream);

            return Ok(new { message = "Contacts uploaded successfully." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _contactService.GetAllContactsAsync(cancellationToken);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ContactDto dto)
        {
            try
            {
                // TODO: return updated entity
                await _contactService.UpdateContactAsync(id, dto);
                return Ok();
            }
            catch (Exception ex) when (ex is KeyNotFoundException || ex is DataInvalidException)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _contactService.DeleteContactAsync(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
