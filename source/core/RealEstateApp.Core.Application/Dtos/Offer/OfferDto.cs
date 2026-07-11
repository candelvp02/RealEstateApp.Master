namespace RealEstateApp.Core.Application.Dtos.Offer;

public class OfferDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = default!;
    public string ClientId { get; set; } = default!;
    public string? ClientFullName { get; set; }
    public int PropertyId { get; set; }
    public DateTime CreatedAt { get; set; }
}