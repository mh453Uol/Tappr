using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CycleHire.Persistence.UnitOfWork;
using CycleHire.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using CycleHire.Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using CycleHire.Core;


namespace CycleHire.WebApiControllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class RoutePlannerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoutePlannerController(IUnitOfWork unitOfWork, IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetNodes()
        {
            var userId = _userManager.GetUserId(User);
            var nodes = await _unitOfWork.RoutePlanner.GetAllNodes(userId);
            var nodeDto = _mapper.Map<IEnumerable<NodeDto>>(nodes);
            return Ok(nodeDto);
        }


        // POST: api/routeplanner/route/create
        [HttpPost("route/create")]
        public IActionResult CreateRoute([FromBody] RouteItemDto route)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            string jsonWaypoints = null;

            if (route.Waypoints != null)
            {
                jsonWaypoints = JsonConvert.SerializeObject(route.Waypoints);
            }

            var userId = _userManager.GetUserId(User);

            var model = new RouteItem()
            {
                OriginLatitude = route.Origin.Latitude,
                OriginLongitude = route.Origin.Longitude,
                DestinationLatitude = route.Destination.Latitude,
                DestinationLongitude = route.Destination.Longitude,
                Waypoints = jsonWaypoints,
                UserId = userId,
                Name = route.Name,
                Polyline = route.Polyline,
                ParentId = route.Parent
            };

            _unitOfWork.RoutePlanner.AddRoute(model);

            _unitOfWork.Complete();

            var nodeDto = _mapper.Map<NodeDto>(model);

            var dto = new { route = route, node = nodeDto };

            return Ok(dto);
        }

        // POST: api/routeplanner/folder/create
        [HttpPost("folder/create")]
        public async Task<IActionResult> CreateFolder(Guid? parentId, string folderName)
        {
            if (String.IsNullOrEmpty(folderName)) { return BadRequest(); }

            var userId = _userManager.GetUserId(User);

            var folder = new FolderItem()
            {
                ParentId = parentId,
                Name = folderName,
                UserId = userId
            };

            if (parentId != null)
            {
                //Its a child folder so check parent exists
                var parent = await _unitOfWork.RoutePlanner.GetFolderById(parentId.Value, userId);

                if (parent == null) { return NotFound(); }
            }

            _unitOfWork.RoutePlanner.AddFolder(folder);

            _unitOfWork.Complete();

            var nodeDto = _mapper.Map<NodeDto>(folder);

            return Ok(nodeDto);
        }

        [HttpPost("folder/edit")]
        public async Task<IActionResult> EditFolder(Guid id, string folderName)
        {
            if (id == Guid.Empty || String.IsNullOrEmpty(folderName))
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            var folder = await _unitOfWork.RoutePlanner.GetFolderById(id, userId);

            folder.Name = folderName;

            _unitOfWork.RoutePlanner.UpdateFolder(folder);

            _unitOfWork.Complete();

            var dto = _mapper.Map<NodeDto>(folder);

            return Ok(dto);
        }

        [HttpGet("route/{id}")]
        public async Task<IActionResult> GetRoute(Guid id)
        {
            if (id == Guid.Empty) { return NotFound(); }

            var userId = _userManager.GetUserId(User);

            var route = await _unitOfWork.RoutePlanner.GetRouteById(id, userId);

            var dto = new RouteItemDto()
            {
                Origin = new CoordinatesDto()
                {
                    Latitude = route.OriginLatitude,
                    Longitude = route.OriginLongitude
                },
                Destination = new CoordinatesDto()
                {
                    Latitude = route.DestinationLatitude,
                    Longitude = route.DestinationLongitude,
                },
                Polyline = route.Polyline,
                Name = route.Name
            };

            dto.Waypoints = JsonConvert.DeserializeObject<CoordinatesDto[]>(route.Waypoints);

            return Ok(dto);
        }

        [HttpDelete("node/{id}")]
        public async Task<IActionResult> DeleteNodes(Guid id)
        {
            if (id == Guid.Empty) { return NotFound(); }

            var userId = _userManager.GetUserId(User);

            var nodes = await _unitOfWork.RoutePlanner.GetAllNodes(userId);

            var roots = nodes.Where(n => n.ParentId == null).ToList();

            var found = NodeManager.Traverse(roots, id);

            if (found == null) { return NotFound(); }

            //if it doesnt have children its a route or a folder with no children
            if (found.Children == null || found.Children.Count == 0)
            {
                _unitOfWork.RoutePlanner.DeleteNodes(new List<Node> { found });
            }
            else
            {
                var children = NodeManager.Flatten(found.Children).ToList();

                children.Add(found);

                _unitOfWork.RoutePlanner.DeleteNodes(children);
            }

            _unitOfWork.Complete();

            return Ok();
        }

        [HttpPost("node/move")]
        public async Task<IActionResult> MoveNode(Guid id, Guid? parentId)
        {
            var userId = _userManager.GetUserId(User);

            var node = await _unitOfWork.RoutePlanner.GetNodeById(id, userId);

            if (node == null) { return NotFound(); }

            node.ParentId = parentId;

            _unitOfWork.RoutePlanner.UpdateNode(node);

            _unitOfWork.Complete();

            return Ok();
        }
    }
}
