namespace PetSit.Domain.Entities;

public class Owner
{
    public Guid UserId { get; set; }
    public string? Address { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    public User User { get; set; } = null!;
    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}