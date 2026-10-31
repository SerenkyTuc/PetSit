using PetSit.Data.Enums;

namespace PetSit.Data.Entities;

public class Booking : BaseEntity
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Comment { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    public Owner Owner { get; set; } = null!;
    public Sitter Sitter { get; set; } = null!;
    public ICollection<BookingPet> BookingPets { get; set; } = new List<BookingPet>();
}
