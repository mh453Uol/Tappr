using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.ViewModels.ManageViewModels
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            UploadImageModel = new ImageViewModel();
        }

        [Required]
        [MinLength(2)]
        public string Firstname { get; set; }

        [Required]
        [MinLength(2)]
        public string Surname { get; set; }

        //public bool IsEmailConfirmed { get; set; }

        //[Required]
        //[EmailAddress]
        //[Display(Name = "Email")]
        //public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }

        [Display(Name = "About Me")]
        [StringLength(maximumLength: 1000)]
        public string AboutMe { get; set; }

        public string ProfileImageId { get; set; }

        public ImageViewModel UploadImageModel { get; set; }
    }
}
