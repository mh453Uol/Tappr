using CycleHire.Core.Models;
using System.Threading.Tasks;
using System;

namespace CycleHire.Core.Repositories
{
    public interface IAvailabilityRepository
    {
        Task<Availability> GetAvailabilityByIdAsync(Guid listingId);
    }
}
