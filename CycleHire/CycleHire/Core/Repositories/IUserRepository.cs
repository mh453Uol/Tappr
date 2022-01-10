using CycleHire.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetByIdWithListingAndReview(string id);
    }
}
