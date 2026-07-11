using RealEstateApp.Core.Domain.Common;
using RealEstateApp.Core.Domain.Enums;

namespace RealEstateApp.Core.Domain.Entities;

public class Offer : BasicEntity
{
    public decimal Amount { get; set; }
    public OfferStatus Status { get; set; } = OfferStatus.Pending;

    public string ClientId { get; set; } = default!;

    public int PropertyId { get; set; }
    public Property Property { get; set; } = default!;
}