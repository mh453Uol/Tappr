using CycleHire.Core.Models;
using CycleHire.Persistence;
using CycleHire.Utilites.DatesTime;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _db;
        public BookingRepository(ApplicationDbContext db)
        {
            this._db = db;
        }

        public async Task<IEnumerable<DateTime>> GetBookedDays(Guid listingId,
            DateTime startDate, DateTime endDate)
        {
            // To get available days we need to consider which days are booked
            // and which days the bike is available on since users when listing
            // their bike can select the availabilty. Calendar will only show 12 months 
            // in advance so only need to consider this time period.

            var booked = new List<DateTime>();

            var bookings = await _db.Bookings.Where(l => l.From >= startDate
                    && l.To <= endDate
                    && l.ListingId == listingId
                    && (l.Status == BookingStatus.ACCEPTED || l.Status == BookingStatus.PENDING)
                    && l.IsDeleted == false)
                    .Select(l => new { l.From, l.To })
                    .AsNoTracking()
                    .ToListAsync();

            bookings.ForEach(b => booked.AddRange(b.From.GetDateRange(b.To)));

            return booked;
        }

        public async Task AddAsync(Booking booking)
        {
            booking.NewlyCreated();

            await _db.Bookings.AddAsync(booking);
        }

        public async Task<Boolean> IsAvailable(Guid listingId, DateTime From, DateTime To)
        {
            return !await _db.Bookings
                .AsNoTracking()
                    .AnyAsync(b => b.ListingId == listingId && b.IsDeleted == false &&
                        (b.Status == BookingStatus.ACCEPTED || b.Status == BookingStatus.PENDING) &&
                        !(From > b.To || To < b.From));
        }

        public async Task<Booking> GetById(Guid listingId)
        {
            return await _db.Bookings.SingleOrDefaultAsync(b => b.Id == listingId);
        }

        public async Task<List<Booking>> GetByUserIdWithListingAndOwner(string userId)
        {
            return await _db.Bookings
                .AsNoTracking()
                .Where(b => b.UserId == userId && b.IsDeleted == false)
                .Include(b => b.Listing)
                .Include(b => b.Owner)
                .OrderByDescending(b => b.From)
                .ToListAsync();
        }

        public async Task<Booking> GetByIdWithTenant(Guid listingId)
        {
            return await _db.Bookings
                .AsNoTracking()
                .Include(b => b.User)
                .SingleOrDefaultAsync(b => b.Id == listingId);

        }
    }
}

