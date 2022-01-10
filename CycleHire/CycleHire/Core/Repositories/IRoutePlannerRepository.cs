using CycleHire.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CycleHire.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CycleHire.Core.Repositories
{
    public interface IRoutePlannerRepository
    {
        void AddFolder(FolderItem folder);
        void AddRoute(RouteItem route);
        void UpdateFolder(FolderItem folder);
        void UpdateNode(Node node);
        void DeleteNodes(List<Node> nodes);
        Task<FolderItem> GetFolderById(Guid parentId, string userId);
        Task<List<Node>> GetAllNodes(string userId);
        Task<RouteItem> GetRouteById(Guid id, string userId);
        Task<Node> GetNodeById(Guid id, string userId);
        Task<Node[]> GetNodeByParentId(Guid parentId, string userId);
    }
}
