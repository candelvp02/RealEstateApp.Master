using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Entities;

public class Improvement : BasicEntity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;

    public ICollection<PropertyImprovement> PropertyImprovements { get; set; } = new List<PropertyImprovement>();
}