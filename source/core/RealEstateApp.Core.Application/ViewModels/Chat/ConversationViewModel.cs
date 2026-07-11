namespace RealEstateApp.Core.Application.ViewModels.Chat;

public class ConversationViewModel
{
    public int Id { get; set; }
    public string Content { get; set; } = default!;
    public string SenderRole { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}