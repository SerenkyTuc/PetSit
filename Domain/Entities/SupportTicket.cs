using PetSit.Domain.Enums;
namespace PetSit.Domain.Entities;

public class SupportTicket
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? AdminId { get; set; }
    public string Body { get; set; } = string.Empty;
    public TicketCategory Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }

    public User User { get; set; } = null!;
    public Admin? Admin { get; set; }
}