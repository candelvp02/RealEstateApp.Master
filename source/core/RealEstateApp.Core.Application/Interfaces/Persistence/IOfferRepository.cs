using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Persistence;

public interface IOfferRepository : IGenericRepository<Offer>
{
    Task<List<Offer>> GetByPropertyAndClientAsync(int propertyId, string clientId);
    Task<List<Offer>> GetByPropertyAsync(int propertyId);
    Task<bool> HasPendingOfferAsync(int propertyId, string clientId);
    Task<bool> HasAcceptedOfferAsync(int propertyId);
}