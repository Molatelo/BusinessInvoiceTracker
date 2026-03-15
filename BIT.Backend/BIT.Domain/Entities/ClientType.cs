namespace BIT.Domain.Entities;

public class ClientType : Entity
{
    public required string Name { get; set; }
    public string? Code { get; private set; }
    public string? Description { get; set; }

    public virtual ICollection<Client>? Clients { get; set; }
}
