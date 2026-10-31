using PetSit.Common.Enums;

namespace PetSit.Data.Entities;

public class SupportTicket : BaseEntity
{
    public string Body { get; set; } = string.Empty;
    public TicketCategory Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }

    public User User { get; set; } = null!;
    public Admin? Admin { get; set; }
}

