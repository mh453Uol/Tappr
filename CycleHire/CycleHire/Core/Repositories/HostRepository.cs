using System.Collections.Generic;
using System.Linq;
using CycleHire.Persistence;
using CycleHire.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using CycleHire.Core.Dtos;

namespace CycleHire.Core.Repositories
{
    public class HostRepository : IHostRepository
    {
        private readonly ApplicationDbContext _db;
        public HostRepository(ApplicationDbContext db)
        {
            this._db = db;
        }
        public async Task<List<Booking>> GetPendingBookings(string hostId)
        {
            return await _db.Bookings
                .Include(b => b.User)
                .Include(b => b.Listing)
                .Where(b => b.OwnerId == hostId && b.Status == BookingStatus.PENDING)
                .OrderBy(b => b.From)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<FullCalendarEventDto>> GetBookings(string hostId, DateTime start, DateTime end)
        {
            return await _db.Bookings
               .Include(b => b.User)
               .Where(b => b.OwnerId == hostId && b.Status == BookingStatus.ACCEPTED)
               .Select(p => new FullCalendarEventDto(p.Id, p.User.Firstname, p.From, p.To))
               .AsNoTracking()
               .ToListAsync();
        }

    }
}
