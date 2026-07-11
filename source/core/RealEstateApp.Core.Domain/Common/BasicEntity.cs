namespace RealEstateApp.Core.Domain.Common;

public abstract class BasicEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}