using PetSit.Domain.Enums;
namespace PetSit.Domain.Entities;

public class Booking
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public Guid SitterId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Comment { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    public Owner Owner { get; set; } = null!;
    public Sitter Sitter { get; set; } = null!;
    public ICollection<BookingPet> BookingPets { get; set; } = new List<BookingPet>();

}