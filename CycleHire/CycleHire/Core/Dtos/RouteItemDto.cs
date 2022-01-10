using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.Dtos
{
    public class RouteItemDto
    {
        [Required]
        public CoordinatesDto Origin { get; set; }

        [Required]
        public CoordinatesDto Destination { get; set; }

        public CoordinatesDto[] Waypoints { get; set; }

        [Required]
        public string Polyline { get; set; }

        [Required]
        public string Name { get; set; }

        public Guid? Parent { get; set; }
    }

    public class CoordinatesDto
    {
        [Required]
        [Range(-85.05115, +85, ErrorMessage = "Latitude is not valid")]
        public decimal Latitude { get; set; }

        [Required]
        [Range(-180, +180, ErrorMessage = "Longitude is not valid")]
        public decimal Longitude { get; set; }
    }
}
