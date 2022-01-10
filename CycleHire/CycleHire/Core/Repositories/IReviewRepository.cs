using CycleHire.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.Repositories
{
    public interface IReviewRepository
    {
        Task<List<HostReview>> GetHostReviewsByUserId(string id);
        Task<List<TenantReview>> GetTenantReviewsByUserId(string id);
        Task<List<Booking>> GetPendingReviewsAboutHost(string id);
        Task<List<Booking>> GetPendingReviewsAboutTenant(string id);
        void AddHostReview(HostReview review);
        void AddTenantReview(TenantReview review);
        Task<List<HostReview>> GetByListingId(Guid id);
    }
}
