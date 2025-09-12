using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces;

public interface IContactService
{
    // TODO: add cancellation tokens
    Task UploadContactsAsync(Stream csvStream);
    Task<IEnumerable<Contact>> GetAllContactsAsync(CancellationToken cancellationToken);
    Task UpdateContactAsync(Guid id, ContactDto dto);
    Task DeleteContactAsync(Guid id);
}
