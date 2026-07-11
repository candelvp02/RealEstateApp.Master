using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Entities;

public class PropertyImage : BasicEntity
{
    public string Path { get; set; } = default!;

    public int PropertyId { get; set; }
    public Property Property { get; set; } = default!;
}