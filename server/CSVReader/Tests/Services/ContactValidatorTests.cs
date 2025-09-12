using Core.DTOs;
using Core.Exceptions;
using Services.Implementations;

namespace Tests.Services;

public class ContactValidatorTests
{
    private readonly ContactValidator _validator = new();

    private ContactDto CreateValidContact() =>
        new(
            Name: "John Doe",
            DateOfBirth: new DateOnly(1990, 1, 1),
            IsMarried: false,
            Phone: "+1234567890",
            Salary: 50000
        );

    [Fact]
    public void Validate_ValidContact_DoesNotThrow()
    {
        var contact = CreateValidContact();

        var exception = Record.Exception(() => _validator.Validate(contact));

        Assert.Null(exception);
    }

    [Fact]
    public void Validate_EmptyName_ThrowsDataInvalidException()
    {
        var contact = CreateValidContact() with { Name = "" };

        var ex = Assert.Throws<DataInvalidException>(() => _validator.Validate(contact));
        Assert.Equal("Inpud data is not valid.", ex.Message);
    }

    [Fact]
    public void Validate_NameTooLong_ThrowsDataInvalidException()
    {
        var contact = CreateValidContact() with { Name = new string('A', 101) };

        var ex = Assert.Throws<DataInvalidException>(() => _validator.Validate(contact));
        Assert.Equal("Inpud data is not valid. Name cannot exceed 100 characters.", ex.Message);
    }

    [Fact]
    public void Validate_DateOfBirthInFuture_ThrowsDataInvalidException()
    {
        var contact = CreateValidContact() with { DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)) };

        var ex = Assert.Throws<DataInvalidException>(() => _validator.Validate(contact));
        Assert.Equal("Inpud data is not valid. Date of birth must be in the past.", ex.Message);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1_000_000_001)]
    public void Validate_InvalidSalary_ThrowsDataInvalidException(decimal salary)
    {
        var contact = CreateValidContact() with { Salary = salary };

        var ex = Assert.Throws<DataInvalidException>(() => _validator.Validate(contact));
        Assert.Equal("Inpud data is not valid. Salary must be between 0 and 1,000,000,000.", ex.Message);
    }

    [Fact]
    public void Validate_PhoneEmpty_ThrowsDataInvalidException()
    {
        var contact = CreateValidContact() with { Phone = "" };

        var ex = Assert.Throws<DataInvalidException>(() => _validator.Validate(contact));
        Assert.Equal("Inpud data is not valid. Phone number is required.", ex.Message);
    }

    [Fact]
    public void Validate_InvalidPhoneFormat_ThrowsDataInvalidException()
    {
        var contact = CreateValidContact() with { Phone = "abc123" };

        var ex = Assert.Throws<DataInvalidException>(() => _validator.Validate(contact));
        Assert.Equal("Inpud data is not valid. Invalid phone number.", ex.Message);
    }

    [Fact]
    public void Validate_PhoneTooLong_ThrowsDataInvalidException()
    {
        var contact = CreateValidContact() with { Phone = new string('1', 21) };

        var ex = Assert.Throws<DataInvalidException>(() => _validator.Validate(contact));
        Assert.Equal("Inpud data is not valid. Phone number cannot exceed 20 characters.", ex.Message);
    }
}
