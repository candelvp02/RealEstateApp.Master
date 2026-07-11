namespace RealEstateApp.Core.Application.ViewModels.Property;

public class PropertyFilterViewModel
{
    public string? Code { get; set; }
    public int? PropertyTypeId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? Bedrooms { get; set; }
    public int? Bathrooms { get; set; }
    public List<Catalog.PropertyTypeViewModel>? PropertyTypes { get; set; }
}