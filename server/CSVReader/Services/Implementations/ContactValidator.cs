using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces;

namespace Services.Implementations;

public class ContactValidator : IContactValidator
{
    public void Validate(ContactDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new DataInvalidException();

        if (dto.Name.Length > 100)
            throw new DataInvalidException("Name cannot exceed 100 characters.");

        if (dto.DateOfBirth >= DateOnly.FromDateTime(DateTime.UtcNow))
            throw new DataInvalidException("Date of birth must be in the past.");

        if (dto.Salary < 0 || dto.Salary > 1_000_000_000)
            throw new DataInvalidException("Salary must be between 0 and 1,000,000,000.");

        if (string.IsNullOrEmpty(dto.Phone))
            throw new DataInvalidException("Phone number is required.");

        if (!System.Text.RegularExpressions.Regex.IsMatch(dto.Phone, @"^[0-9+\- ]+$"))
            throw new DataInvalidException("Invalid phone number.");

        if (dto.Phone.Length > 20)
            throw new DataInvalidException("Phone number cannot exceed 20 characters.");
    }
}
