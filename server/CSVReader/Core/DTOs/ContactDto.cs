namespace Core.DTOs;

public record ContactDto(
    string Name,
    DateOnly DateOfBirth,
    bool IsMarried,
    string Phone,
    decimal Salary
);
