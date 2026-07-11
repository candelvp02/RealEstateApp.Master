using AutoMapper;
using RealEstateApp.Core.Application.Dtos.Chat;
using RealEstateApp.Core.Application.Interfaces.Persistence;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _repository;
    private readonly IMapper _mapper;

    public MessageService(IMessageRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<MessageDto>> GetConversationAsync(int propertyId, string clientId, string agentId)
    {
        var list = await _repository.GetConversationAsync(propertyId, clientId, agentId);
        return _mapper.Map<List<MessageDto>>(list);
    }

    public async Task<List<string>> GetClientsWithConversationAsync(int propertyId, string agentId)
    {
        return await _repository.GetClientsWithConversationAsync(propertyId, agentId);
    }

    public async Task<MessageDto> SendAsync(MessageDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Content))
            throw new InvalidOperationException("You must write a message before sending it.");

        var entity = _mapper.Map<Message>(dto);
        var created = await _repository.AddAsync(entity);
        return _mapper.Map<MessageDto>(created);
    }
}