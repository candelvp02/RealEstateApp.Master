using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewModels.Chat;

public class SendMessageViewModel
{
    public int PropertyId { get; set; }
    public string? AgentId { get; set; }
    public string? ClientId { get; set; }

    [Required(ErrorMessage = "You must write a message before sending it.")]
    public string Content { get; set; } = default!;
}