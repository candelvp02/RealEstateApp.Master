namespace RealEstateApp.Core.Application.Dtos.Property;

public class SavePropertyDto
{
    public int Id { get; set; }
    public int PropertyTypeId { get; set; }
    public int SaleTypeId { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; } = default!;
    public decimal Size { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public List<int> ImprovementIds { get; set; } = new();
    public List<string> ImagePaths { get; set; } = new();
    public string AgentId { get; set; } = default!;
}