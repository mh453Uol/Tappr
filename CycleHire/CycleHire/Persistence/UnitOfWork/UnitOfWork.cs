using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CycleHire.Core.Repositories;

namespace CycleHire.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(ApplicationDbContext db,
            IAccessoryRepository accessoryRepository,
            IListingRepository listingRepository,
            IBookingRepository bookingRepository,
            IAvailabilityRepository availabiltyRepository,
            IHostRepository hostRepository,
            IUserRepository userRepository,
            IReviewRepository reviewRepository,
            IRoutePlannerRepository routePlannerRepository)
        {
            this._db = db;
            this.Accessories = accessoryRepository;
            this.Listings = listingRepository;
            this.Bookings = bookingRepository;
            this.Availability = availabiltyRepository;
            this.Host = hostRepository;
            this.Users = userRepository;
            this.Reviews = reviewRepository;
            this.RoutePlanner = routePlannerRepository;
        }

        private readonly ApplicationDbContext _db;

        public IAccessoryRepository Accessories { get; }
        public IListingRepository Listings { get; }
        public IBookingRepository Bookings { get; }
        public IAvailabilityRepository Availability { get; }
        public IHostRepository Host { get; }
        public IUserRepository Users { get; }
        public IReviewRepository Reviews { get; }
        public IRoutePlannerRepository RoutePlanner { get; }


        public void Complete()
        {
            _db.SaveChanges();
        }
    }
}
