using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.Models
{
    public class RouteItem : Item
    {
        public RouteItem()
        {
            Children = new List<Node>();
        }

        [Required]
        [Column(TypeName = "decimal(8,6)")]
        public decimal OriginLatitude { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,6)")]
        public decimal OriginLongitude { get; set; }

        [Required]
        [Column(TypeName = "decimal(8,6)")]
        public decimal DestinationLatitude { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,6)")]
        public decimal DestinationLongitude { get; set; }

        public string Waypoints { get; set; }
        public string Polyline { get; set; }
    }
}
