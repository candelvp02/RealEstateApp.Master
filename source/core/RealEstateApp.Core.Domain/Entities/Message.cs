using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Entities;

public class Message : BasicEntity
{
    public string Content { get; set; } = default!;

    public string SenderRole { get; set; } = default!;

    public string ClientId { get; set; } = default!;
    public string AgentId { get; set; } = default!;

    public int PropertyId { get; set; }
    public Property Property { get; set; } = default!;
}