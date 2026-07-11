using RealEstateApp.Core.Application.Dtos.Common;

namespace RealEstateApp.Core.Application.Dtos.Property;

public class PropertyImageDto : BasicDto
{
    public string Path { get; set; } = default!;
}