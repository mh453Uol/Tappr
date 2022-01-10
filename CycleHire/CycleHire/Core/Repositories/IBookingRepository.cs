using CycleHire.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleHire.Core.Repositories
{
    public interface IBookingRepository
    {
        Task<IEnumerable<DateTime>> GetBookedDays(Guid listingId, DateTime startDate, DateTime endDate);
        Task<Boolean> IsAvailable(Guid listingId, DateTime To, DateTime From);
        Task AddAsync(Booking booking);
        Task<Booking> GetById(Guid listingId);
        Task<Booking> GetByIdWithTenant(Guid listingId);
        Task<List<Booking>> GetByUserIdWithListingAndOwner(string userId);
    }
}
