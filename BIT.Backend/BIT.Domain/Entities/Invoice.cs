using BIT.Common.Enums;

namespace BIT.Domain.Entities;

public class Invoice : Entity<long>
{
    public required string InvoiceNumber { get; set; }
    public int ClientId { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public InvoiceStatus Status { get; set; }
    public required string Notes { get; set; }
    public DateTime? PaidDate { get; set; }

    public virtual required Client Client { get; set; }
    public virtual ICollection<InvoiceItem>? InvoiceItems { get; set; }
}
