using RealEstateApp.Core.Application.Dtos.Common;

namespace RealEstateApp.Core.Application.Dtos.Catalog;

public class PropertyTypeDto : BasicDto
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int PropertiesCount { get; set; }
}