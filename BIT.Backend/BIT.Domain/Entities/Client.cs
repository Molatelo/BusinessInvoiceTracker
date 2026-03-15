namespace BIT.Domain.Entities;

public class Client : Entity
{
    public int ClientTypeId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }

    public virtual required ClientType ClientType { get; set; }
    public virtual ICollection<Invoice>? Invoices { get; set; }
}
