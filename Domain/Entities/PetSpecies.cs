namespace PetSit.Domain.Entities;

public class PetSpecies
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
}