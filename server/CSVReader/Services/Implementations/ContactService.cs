using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Services.Implementations;

public class ContactService(AppDbContext _context, IContactValidator _validator) : IContactService
{
    // TODO: validate file type and size
    // TODO: validate data inside the file
    public async Task UploadContactsAsync(Stream csvStream)
    {
        using var reader = new StreamReader(csvStream);
        using var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture);

        var records = csv.GetRecords<ContactDto>().ToList();

        var entities = records.Select(r => new Contact
        {
            Name = r.Name,
            DateOfBirth = r.DateOfBirth,
            IsMarried = r.IsMarried,
            Phone = r.Phone,
            Salary = r.Salary
        }).ToList();

        await _context.Contacts.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }
    public async Task<IEnumerable<Contact>> GetAllContactsAsync(CancellationToken cancellationToken)
    {
        return await _context.Contacts.ToListAsync(cancellationToken);
    }
    public async Task UpdateContactAsync(Guid id, ContactDto dto)
    {
        var contact = await _context.Contacts.FindAsync(id) ?? throw new KeyNotFoundException("Contact not found");
        _validator.Validate(dto);

        contact.Name = dto.Name;
        contact.DateOfBirth = dto.DateOfBirth;
        contact.IsMarried = dto.IsMarried;
        contact.Phone = dto.Phone;
        contact.Salary = dto.Salary;

        await _context.SaveChangesAsync();
    }
    public async Task DeleteContactAsync(Guid id)
    {
        var contact = await _context.Contacts.FindAsync(id) ?? throw new KeyNotFoundException("Contact not found");
        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();
    }
}
