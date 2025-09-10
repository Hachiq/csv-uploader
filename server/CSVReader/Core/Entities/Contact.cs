namespace Core.Entities;

public class Contact
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public bool IsMarried { get; set; }
    public string Phone { get; set; } = string.Empty;
    public decimal Salary { get; set; }
}
