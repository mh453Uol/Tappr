using CycleHire.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CycleHire.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CycleHire.Core.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _db;
        public ReviewRepository(ApplicationDbContext db)
        {
            this._db = db;
        }

        public Task<List<HostReview>> GetHostReviewsByUserId(string id)
        {
            return _db.HostReviews
                .Include(b => b.Booking)
                .Include(b => b.Listing)
                .Where(r => r.UserId == id)
                .ToListAsync();
        }


        public Task<List<TenantReview>> GetTenantReviewsByUserId(string id)
        {
            return _db.TenantReviews
                .Include(b => b.Booking)
                .Include(b => b.Listing)
                .Where(r => r.UserId == id)
                .ToListAsync();
        }
        public async Task<List<Booking>> GetPendingReviewsAboutHost(string id)
        {
            //get all booking where today date is > To and To date less than 14 days from todays date
            DateTime rangeEnd = DateTime.Now.Date;
            DateTime rangeStart = rangeEnd.AddDays(-14);

            return await _db.Bookings
                .Include(b => b.Owner)
                .Include(b => b.HostReview)
                .Where(b => b.To <= rangeEnd &&
                    b.To >= rangeStart &&
                    b.UserId == id &&
                    b.HostReview == null &&
                    b.Status == BookingStatus.ACCEPTED)
                    .ToListAsync();
        }

        public async Task<List<Booking>> GetPendingReviewsAboutTenant(string id)
        {
            //get all booking where today date is > To and To date less than 14 days from todays date
            DateTime rangeEnd = DateTime.Now.Date;
            DateTime rangeStart = rangeEnd.AddDays(-14);

            return await _db.Bookings
                .Include(b => b.User)
                .Include(b => b.TenantReview)
                 .Where(b => b.To <= rangeEnd &&
                     b.To >= rangeStart &&
                     b.OwnerId == id &&
                     b.TenantReview == null &&
                     b.Status == BookingStatus.ACCEPTED)
                     .ToListAsync();
        }

        public async void AddHostReview(HostReview review)
        {
            review.NewlyCreated();
            await _db.HostReviews.AddAsync(review);
        }

        public async void AddTenantReview(TenantReview review)
        {
            review.NewlyCreated();
            await _db.TenantReviews.AddAsync(review);
        }

        public async Task<List<HostReview>> GetByListingId(Guid id)
        {
            // we only show reviews 14 days after the listing so both parties reviews are not influenced
            // so we check if created day of review + 14 is less than today date

            var today = DateTime.Now.Date;

            return await _db.HostReviews
                .AsNoTracking()
                .Include(r => r.User)
                .Where(r => r.ListingId == id && r.Created.AddDays(14) < today)
                .ToListAsync();
        }
    }
}
