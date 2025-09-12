using Core.DTOs;

namespace Core.Interfaces;

public interface IContactValidator
{
    void Validate(ContactDto dto);
}
