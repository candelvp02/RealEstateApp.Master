namespace RealEstateApp.Core.Application.Dtos.Chat;

public class MessageDto
{
    public int Id { get; set; }
    public string Content { get; set; } = default!;
    public string SenderRole { get; set; } = default!;
    public string ClientId { get; set; } = default!;
    public string AgentId { get; set; } = default!;
    public int PropertyId { get; set; }
    public DateTime CreatedAt { get; set; }
}