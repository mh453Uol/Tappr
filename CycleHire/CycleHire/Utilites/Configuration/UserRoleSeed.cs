using CycleHire.Core.Models;
using CycleHire.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Utilites.Configuration
{
    public class UserRoleSeed
    {
        private readonly ApplicationDbContext _context;
        public UserRoleSeed(ApplicationDbContext context)
        {
            _context = context;
        }

        public async void SeedAsync()
        {

            string[] roles = new string[] { "Host", "Tenant" };

            foreach (string role in roles)
            {
                var roleStore = new RoleStore<IdentityRole>(_context);

                if (!_context.Roles.Any(r => r.Name == role))
                {
                    await roleStore.CreateAsync(new IdentityRole { Name = role, NormalizedName = role });
                }
            }

            Accessory[] accessories = new Accessory[] {
                new Accessory("Helmet"),
                new Accessory("Child Bike Seats"),
                new Accessory("Bike Lock"),
                new Accessory("Bicycle trailer")
            };

            if (!_context.Accessories.Any())
            {
                await _context.Accessories.AddRangeAsync(accessories);
            }

            await _context.SaveChangesAsync();
        }
    }
}


