using RealEstateApp.Core.Application.Dtos.Common;

namespace RealEstateApp.Core.Application.Dtos.Property;

public class PropertyDto : BasicDto
{
    public string Code { get; set; } = default!;
    public string PropertyTypeName { get; set; } = default!;
    public string SaleTypeName { get; set; } = default!;
    public decimal Price { get; set; }
    public decimal Size { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public string Description { get; set; } = default!;
    public string Status { get; set; } = default!;

    public string AgentId { get; set; } = default!;
    public string AgentFullName { get; set; } = default!;
    public string? AgentPhone { get; set; }
    public string? AgentEmail { get; set; }
    public string? AgentProfilePicturePath { get; set; }

    public List<string> Improvements { get; set; } = new();
    public List<PropertyImageDto> Images { get; set; } = new();
}