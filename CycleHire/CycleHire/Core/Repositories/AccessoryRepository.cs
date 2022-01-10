using System.Collections.Generic;
using System.Linq;
using CycleHire.Persistence;
using CycleHire.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CycleHire.Core.Repositories
{
    public class AccessoryRepository : IAccessoryRepository
    {
        private readonly ApplicationDbContext _db;
        public AccessoryRepository(ApplicationDbContext db)
        {
            this._db = db;
        }

        public async Task<List<Accessory>> GetAllAsync()
        {
            return await _db.Accessories.AsNoTracking().ToListAsync();
        }
    }
}
