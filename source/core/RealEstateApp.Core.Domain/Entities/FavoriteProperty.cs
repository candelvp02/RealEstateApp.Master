using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Entities;

public class FavoriteProperty : BasicEntity
{
    public string ClientId { get; set; } = default!;
    public int PropertyId { get; set; }
    public Property Property { get; set; } = default!;
}