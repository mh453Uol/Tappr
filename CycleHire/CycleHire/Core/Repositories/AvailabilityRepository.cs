using CycleHire.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CycleHire.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CycleHire.Core.Repositories
{
    public class AvailabiltyRepository : IAvailabilityRepository
    {
        public AvailabiltyRepository(ApplicationDbContext db)
        {
            this._db = db;
        }

        private readonly ApplicationDbContext _db;

        public async Task<Availability> GetAvailabilityByIdAsync(Guid listingId)
        {
            return await _db.Availabilities.AsNoTracking()
                .SingleOrDefaultAsync(l => l.ListingId == listingId);
        }
    }
}
