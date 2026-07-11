namespace RealEstateApp.Core.Application.ViewModels.Property;

public class PropertyListItemViewModel
{
    public int Id { get; set; }
    public string Code { get; set; } = default!;
    public string PropertyTypeName { get; set; } = default!;
    public string SaleTypeName { get; set; } = default!;
    public string? MainImagePath { get; set; }
    public decimal Price { get; set; }
    public decimal Size { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public string Status { get; set; } = default!;
    public bool IsFavorite { get; set; }
}