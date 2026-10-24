namespace PetSit.Domain.Entities;

public class Admin
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;

    public ICollection<SupportTicket> Tickets { get; set; } = new List<SupportTicket>();

}