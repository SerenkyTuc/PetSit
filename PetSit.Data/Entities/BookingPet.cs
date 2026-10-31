namespace PetSit.Data.Entities;

public class BookingPet : BaseEntity
{
    public Booking Booking { get; set; } = null!;
    public Pet Pet { get; set; } = null!;
}
