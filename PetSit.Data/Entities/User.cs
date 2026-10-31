namespace PetSit.Data.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public Owner? Owner { get; set; }
    public Sitter? Sitter { get; set; }
    public ICollection<SupportTicket> Tickets { get; set; } = new List<SupportTicket>();
}
