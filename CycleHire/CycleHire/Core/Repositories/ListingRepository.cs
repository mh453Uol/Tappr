using CycleHire.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CycleHire.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CycleHire.Core.Repositories
{
    public class ListingRepository : IListingRepository
    {
        public ListingRepository(ApplicationDbContext db)
        {
            this._db = db;
        }

        private readonly ApplicationDbContext _db;

        public async Task AddAsync(Listing listing)
        {
            listing.NewlyCreated();
            listing.Accessories.ToList().ForEach(a => a.NewlyCreated());
            listing.Images.ToList().ForEach(a => a.NewlyCreated());

            await _db.Listings.AddAsync(listing);
        }

        public async Task<Listing> GetById(Guid id)
        {
            return await _db.Listings
                .SingleOrDefaultAsync(l => l.Id == id && l.IsDeleted == false);
        }

        public async Task<Listing> GetByIdWithImagesAndAccessories(Guid id)
        {
            return await _db.Listings
                .Include(la => la.Accessories)
                    .ThenInclude(la => la.Accessory)
                .Include(l => l.Images)
                .Include(la => la.User)
                .SingleOrDefaultAsync(l => l.Id == id && l.IsDeleted == false);
        }

        public async Task<Listing> GetByIdWithAccessories(Guid id)
        {
            return await _db.Listings
                .Include(la => la.Accessories)
                    .ThenInclude(la => la.Accessory)
                .Include(la => la.User)
                .SingleOrDefaultAsync(l => l.Id == id && l.IsDeleted == false);
        }

        public async Task<decimal> GetPriceById(Guid id)
        {
            return await _db.Listings
                .Where(l => l.Id == id && l.IsDeleted == false)
                .Select(l => l.Price).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountByUserId(string userId)
        {
            return await _db.Listings.CountAsync(l => l.UserId == userId &&
                l.IsDeleted == false);
        }

        public async Task<List<Listing>> GetByUserIdWithImages(string userId)
        {
            return await _db.Listings
                    .Include(l => l.Images)
                    .Where(l => l.UserId == userId)
                    .ToListAsync();
        }

        public void SetSpatialLocation(decimal latitude, decimal longitude, Guid listingId)
        {
            //Point is longitude and latitud
            _db.Database.ExecuteSqlCommand("UPDATE Listings SET Location = geography::STPointFromText('POINT(' + CAST({1} AS VARCHAR(20)) + ' ' + CAST({0} AS VARCHAR(20)) + ')', 4326) WHERE(ID = {2})", latitude, longitude, listingId);
        }

        public IEnumerable<Listing> FindByLatLngAsync(decimal lat, decimal lng, float radius)
        {

            var query = String.Format(@"DECLARE @g geography = geography::Point({0},{1}, 4326);
                        SELECT l.Id,l.UserId,l.Title,l.Price,l.Address,
                        l.Latitude,l.Longitude,l.Created,li.ImagePublicId
                        FROM Listings l
                        OUTER APPLY
                        (
                            SELECT TOP 1 ListingImage.ImagePublicId
                            FROM ListingImage
                            WHERE ListingImage.ListingId = l.Id AND ListingImage.IsDeleted = 'false'
                        ) li
                        WHERE @g.STDistance(l.Location) <= {2}
                        ORDER BY @g.STDistance(l.Location) ASC;", lat, lng, radius);

            List<Listing> listings = new List<Listing>();
            using (var command = _db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                _db.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            listings.Add(new Listing()
                            {
                                Id = result.GetGuid(0),
                                UserId = result.GetString(1),
                                Title = result.GetString(2),
                                Price = result.GetDecimal(3),
                                Address = result.GetString(4),
                                Latitude = result.GetDecimal(5),
                                Longitude = result.GetDecimal(6),
                                Created = result.GetDateTime(7),
                                Images = new List<ListingImage>() { new ListingImage() { ImagePublicId = result.GetString(8) } }
                            });
                        }
                    }
                }
            }

            return listings;
        }
    }
}
