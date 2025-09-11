namespace Core.DTOs;

public record ContactDto(
    string Name,
    DateTime DateOfBirth,
    bool Married,
    string Phone,
    decimal Salary
);
