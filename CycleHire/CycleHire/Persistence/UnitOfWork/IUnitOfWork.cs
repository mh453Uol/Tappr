using CycleHire.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Persistence.UnitOfWork
{
    public interface IUnitOfWork
    {
        IAccessoryRepository Accessories { get; }
        IListingRepository Listings { get; }
        IBookingRepository Bookings { get; }
        IAvailabilityRepository Availability { get; }
        IHostRepository Host { get; }
        IUserRepository Users { get; }
        IReviewRepository Reviews { get; }
        IRoutePlannerRepository RoutePlanner { get; }

        void Complete();
    }
}
