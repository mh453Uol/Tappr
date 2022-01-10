using CycleHire.Core.Models;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace CycleHire.Core.Repositories
{
    public interface IListingRepository
    {
        Task AddAsync(Listing listing);
        Task<Listing> GetByIdWithImagesAndAccessories(Guid id);
        Task<Listing> GetByIdWithAccessories(Guid id);
        Task<Listing> GetById(Guid id);
        Task<decimal> GetPriceById(Guid id);
        Task<int> GetCountByUserId(string userId);
        Task<List<Listing>> GetByUserIdWithImages(string userId);
        //Work round since ef core doesnt support spatial types :(
        void SetSpatialLocation(decimal latitude, decimal longitude, Guid listingId);
        IEnumerable<Listing> FindByLatLngAsync(decimal lat, decimal lng, float radius);
    }
}
