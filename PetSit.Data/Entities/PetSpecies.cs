namespace PetSit.Data.Entities;

public class PetSpecies : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
