namespace PetSit.Domain.Entities;

public class BookingPet
{
    public Guid BookingId { get; set; }
    public Guid PetId { get; set; }

    public Booking Booking { get; set; } = null!;
    public Pet Pet { get; set; } = null!;

}