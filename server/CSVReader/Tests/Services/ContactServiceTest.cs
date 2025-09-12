using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Services.Implementations;
using System.Globalization;
using System.Text;
using Xunit;


namespace Services.Tests
{
    public class ContactServiceTests
    {
        private static AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task UploadContactsAsync_Should_Save_Contacts_From_Csv()
        {
            // Arrange
            var csvContent = "Id,Name,DateOfBirth,IsMarried,Phone,Salary\n" +
                             $"{Guid.NewGuid()},John Doe,1990-01-01,true,1234567890,5000.00";

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
            using var context = GetInMemoryDbContext();
            var validator = new ContactValidator();
            var service = new ContactService(context, validator);

            // Act
            await service.UploadContactsAsync(stream);
            var contacts = await context.Contacts.ToListAsync();

            // Assert
            Assert.Single(contacts);
            Assert.Equal("John Doe", contacts[0].Name);
            Assert.Equal("1234567890", contacts[0].Phone);
            Assert.Equal(5000.00m, contacts[0].Salary);
        }

        [Fact]
        public async Task UpdateContactAsync_Should_Update_Existing_Contact()
        {
            using var context = GetInMemoryDbContext();
            var validator = new ContactValidator();
            var service = new ContactService(context, validator);

            var contact = new Contact
            {
                Id = Guid.NewGuid(),
                Name = "Jane Doe",
                DateOfBirth = new DateOnly(1985, 5, 5),
                IsMarried = false,
                Phone = "111222333",
                Salary = 3000m
            };

            context.Contacts.Add(contact);
            await context.SaveChangesAsync();

            var dto = new ContactDto(
                "Jane Smith",
                contact.DateOfBirth,
                true,
                "999888777",
                4500m
            );

            // Act
            await service.UpdateContactAsync(contact.Id, dto);
            var updated = await context.Contacts.FindAsync(contact.Id);

            // Assert
            Assert.Equal("Jane Smith", updated!.Name);
            Assert.True(updated.IsMarried);
            Assert.Equal("999888777", updated.Phone);
            Assert.Equal(4500m, updated.Salary);
        }

        [Fact]
        public async Task DeleteContactAsync_Should_Remove_Contact()
        {
            using var context = GetInMemoryDbContext();
            var validator = new ContactValidator();
            var service = new ContactService(context, validator);

            var contact = new Contact
            {
                Id = Guid.NewGuid(),
                Name = "Mark Spencer",
                DateOfBirth = new DateOnly(1970, 7, 7),
                IsMarried = true,
                Phone = "444555666",
                Salary = 6000m
            };

            context.Contacts.Add(contact);
            await context.SaveChangesAsync();

            // Act
            await service.DeleteContactAsync(contact.Id);

            // Assert
            Assert.False(context.Contacts.Any());
        }
    }
}
