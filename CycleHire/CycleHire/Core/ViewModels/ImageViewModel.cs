using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.ViewModels
{
    public class ImageViewModel
    {
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
    }
}
