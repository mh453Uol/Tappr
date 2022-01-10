using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.Models
{
    public abstract class Node : Audit
    {
        public Guid Id { get; set; }
        public bool ReadOnly { get; set; }
        public Guid? ParentId { get; set; }
        public Node Parent { get; set; }
        public List<Node> Children { get; set; }

        [StringLength(maximumLength: 300)]
        [Required]
        public string Name { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
