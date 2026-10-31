namespace PetSit.Data.Entities;

public class Sitter : BaseEntity
{
    public string? Description { get; set; }
    public string? Address { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public int RadiusWalk { get; set; }
    public bool IsActive { get; set; } = true;

    public User User { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
