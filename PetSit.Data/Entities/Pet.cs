namespace PetSit.Data.Entities;

public class Pet : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public DateOnly? Birthday { get; set; }
    public char? Gender { get; set; }
    public int? Weight { get; set; }
    public string? Character { get; set; }
    public string? Notes { get; set; }

    public Owner Owner { get; set; } = null!;
    public PetSpecies Species { get; set; } = null!;
    public ICollection<BookingPet> BookingPets { get; set; } = new List<BookingPet>();
}
