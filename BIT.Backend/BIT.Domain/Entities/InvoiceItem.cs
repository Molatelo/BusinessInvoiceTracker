namespace BIT.Domain.Entities;

public class InvoiceItem : Entity
{
    public long InvoiceId { get; set; }
    public required string Description { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; private set; }

    public virtual required Invoice Invoice { get; set; }
}
