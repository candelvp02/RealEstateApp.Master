using AutoMapper;
using RealEstateApp.Core.Application.Dtos.Offer;
using RealEstateApp.Core.Application.Interfaces.Persistence;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Enums;

namespace RealEstateApp.Core.Application.Services;

public class OfferService : IOfferService
{
    private readonly IOfferRepository _offerRepository;
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;

    public OfferService(IOfferRepository offerRepository, IPropertyRepository propertyRepository, IMapper mapper)
    {
        _offerRepository = offerRepository;
        _propertyRepository = propertyRepository;
        _mapper = mapper;
    }

    public async Task<List<OfferDto>> GetByPropertyAndClientAsync(int propertyId, string clientId)
    {
        var list = await _offerRepository.GetByPropertyAndClientAsync(propertyId, clientId);
        return _mapper.Map<List<OfferDto>>(list);
    }

    public async Task<List<OfferDto>> GetByPropertyAsync(int propertyId)
    {
        var list = await _offerRepository.GetByPropertyAsync(propertyId);
        return _mapper.Map<List<OfferDto>>(list);
    }

    public async Task<bool> CanSendNewOfferAsync(int propertyId, string clientId)
    {
        var property = await _propertyRepository.GetByIdAsync(propertyId);
        if (property is null || property.Status != PropertyStatus.Available)
            return false;

        var hasAccepted = await _offerRepository.HasAcceptedOfferAsync(propertyId);
        if (hasAccepted) return false;

        var hasPending = await _offerRepository.HasPendingOfferAsync(propertyId, clientId);
        return !hasPending;
    }

    public async Task<OfferDto> CreateAsync(OfferDto dto)
    {
        if (dto.Amount <= 0)
            throw new InvalidOperationException("The offer amount must be a numeric value greater than zero.");

        var canSend = await CanSendNewOfferAsync(dto.PropertyId, dto.ClientId);
        if (!canSend)
            throw new InvalidOperationException("You cannot send a new offer for this property right now.");

        var entity = new Offer
        {
            Amount = dto.Amount,
            ClientId = dto.ClientId,
            PropertyId = dto.PropertyId,
            Status = OfferStatus.Pending
        };

        var created = await _offerRepository.AddAsync(entity);
        return _mapper.Map<OfferDto>(created);
    }

    public async Task AcceptAsync(int offerId)
    {
        var offer = await _offerRepository.GetByIdAsync(offerId)
            ?? throw new KeyNotFoundException("The requested offer does not exist.");

        if (offer.Status != OfferStatus.Pending)
            throw new InvalidOperationException("This offer has already been answered.");

        var property = await _propertyRepository.GetByIdAsync(offer.PropertyId)
            ?? throw new KeyNotFoundException("The related property does not exist.");

        if (property.Status != PropertyStatus.Available)
            throw new InvalidOperationException("A property that has already been sold cannot accept a new offer.");

        // Aceptar esta oferta
        offer.Status = OfferStatus.Accepted;
        await _offerRepository.UpdateAsync(offer.Id, offer);

        // Rechazar el resto de ofertas pendientes de la propiedad
        var otherOffers = await _offerRepository.GetByPropertyAsync(offer.PropertyId);
        foreach (var other in otherOffers.Where(o => o.Id != offer.Id && o.Status == OfferStatus.Pending))
        {
            other.Status = OfferStatus.Rejected;
            await _offerRepository.UpdateAsync(other.Id, other);
        }

        // Marcar la propiedad como vendida
        property.Status = PropertyStatus.Sold;
        await _propertyRepository.UpdateAsync(property.Id, property);
    }

    public async Task RejectAsync(int offerId)
    {
        var offer = await _offerRepository.GetByIdAsync(offerId)
            ?? throw new KeyNotFoundException("The requested offer does not exist.");

        if (offer.Status != OfferStatus.Pending)
            throw new InvalidOperationException("This offer has already been answered.");

        offer.Status = OfferStatus.Rejected;
        await _offerRepository.UpdateAsync(offer.Id, offer);
    }
}