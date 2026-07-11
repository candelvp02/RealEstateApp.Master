using RealEstateApp.Core.Application.Dtos.Common;

namespace RealEstateApp.Core.Application.Dtos.Catalog;

public class ImprovementDto : BasicDto
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int PropertiesCount { get; set; }
}