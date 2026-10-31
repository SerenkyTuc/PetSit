namespace PetSit.Data.Entities;

public class Admin : BaseEntity
{
    public string Email { get; set; } = string.Empty;

    public ICollection<SupportTicket> Tickets { get; set; } = new List<SupportTicket>();
}
