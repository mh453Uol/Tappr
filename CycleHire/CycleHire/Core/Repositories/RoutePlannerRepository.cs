using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CycleHire.Core.Models;
using CycleHire.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CycleHire.Core.Repositories
{
    public class RoutePlannerRepository : IRoutePlannerRepository
    {
        public RoutePlannerRepository(ApplicationDbContext db)
        {
            this._db = db;
        }

        private readonly ApplicationDbContext _db;

        public void AddFolder(FolderItem folder)
        {
            folder.NewlyCreated();
            _db.Routes.Add(folder);
        }

        public void AddRoute(RouteItem route)
        {
            route.NewlyCreated();
            _db.Routes.Add(route);
        }

        public Task<FolderItem> GetFolderById(Guid parentId, string userId)
        {
            return _db.Routes
                .AsNoTracking()
                .OfType<FolderItem>()
                .SingleOrDefaultAsync(f => f.Id == parentId && f.UserId == userId);
        }

        public Task<List<Node>> GetAllNodes(string userId)
        {
            return _db.Routes.Where(n => n.UserId == userId && n.IsDeleted == false).ToListAsync();
        }

        public Task<RouteItem> GetRouteById(Guid id, string userId)
        {
            return _db.Routes.AsNoTracking()
                .OfType<RouteItem>()
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
        }

        public void UpdateFolder(FolderItem folder)
        {
            folder.Updated();
            _db.Routes.Update(folder);
        }

        public void DeleteNodes(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                node.IsDeleted = true;
                node.Updated();
            };

            _db.Routes.UpdateRange(nodes);
        }

        public Task<Node> GetNodeById(Guid id, string userId)
        {
            return _db.Routes.AsNoTracking()
                .SingleOrDefaultAsync(n => n.Id == id && n.UserId == userId);
        }

        public Task<Node[]> GetNodeByParentId(Guid parentId, string userId)
        {
            return _db.Routes.AsNoTracking()
                        .Where(n => n.ParentId == parentId && n.UserId == userId)
                        .ToArrayAsync();
        }

        public void UpdateNode(Node node)
        {
            _db.Routes.Update(node);
        }
    }
}
