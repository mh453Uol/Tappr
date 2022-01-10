using CycleHire.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleHire.Core.Repositories
{
    public interface IAccessoryRepository
    {
        Task<List<Accessory>> GetAllAsync();
    }
}
