using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CycleHire.Core.Models;
using CycleHire.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CycleHire.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(ApplicationDbContext db)
        {
            this._db = db;
        }

        private readonly ApplicationDbContext _db;

        public async Task<ApplicationUser> GetByIdWithListingAndReview(string id)
        {
            return await _db.Users
                .AsNoTracking()
                .Include(u => u.Listings)
                    .ThenInclude(ul => ul.Images)
                .Include(u => u.HostReviews)
                    .ThenInclude(u => u.User)
                .Include(u => u.TenantReviews)
                    .ThenInclude(u => u.User)
                .SingleOrDefaultAsync(u => u.Id == id);
        }
    }
}
