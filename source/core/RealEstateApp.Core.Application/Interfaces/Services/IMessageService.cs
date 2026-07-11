using RealEstateApp.Core.Application.Dtos.Chat;

namespace RealEstateApp.Core.Application.Interfaces.Services;

public interface IMessageService
{
    Task<List<MessageDto>> GetConversationAsync(int propertyId, string clientId, string agentId);
    Task<List<string>> GetClientsWithConversationAsync(int propertyId, string agentId);
    Task<MessageDto> SendAsync(MessageDto dto);
}