namespace RealEstateApp.Core.Application.ViewModels.Property;

public class DeletePropertyViewModel
{
    public int Id { get; set; }
    public string Code { get; set; } = default!;
    public string PropertyTypeName { get; set; } = default!;
    public string? MainImagePath { get; set; }
}