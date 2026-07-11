using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Entities;

public class SaleType : BasicEntity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;

    public ICollection<Property> Properties { get; set; } = new List<Property>();
}