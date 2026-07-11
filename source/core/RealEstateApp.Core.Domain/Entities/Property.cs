using RealEstateApp.Core.Domain.Common;
using RealEstateApp.Core.Domain.Enums;

namespace RealEstateApp.Core.Domain.Entities;

public class Property : BasicEntity
{
    public string Code { get; set; } = default!;
    public decimal Price { get; set; }
    public string Description { get; set; } = default!;
    public decimal Size { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public PropertyStatus Status { get; set; } = PropertyStatus.Available;
    public int PropertyTypeId { get; set; }
    public PropertyType PropertyType { get; set; } = default!;
    public int SaleTypeId { get; set; }
    public SaleType SaleType { get; set; } = default!;
    public string AgentId { get; set; } = default!;
    public ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
    public ICollection<PropertyImprovement> PropertyImprovements { get; set; } = new List<PropertyImprovement>();
    public ICollection<FavoriteProperty> FavoriteProperties { get; set; } = new List<FavoriteProperty>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public ICollection<Offer> Offers { get; set; } = new List<Offer>();
}