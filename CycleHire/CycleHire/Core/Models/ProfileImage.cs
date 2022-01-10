using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.Models
{
    public class ProfileImage
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(450)]
        public string ImagePublicId { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public bool IsCurrent { get; set; }
    }
}
