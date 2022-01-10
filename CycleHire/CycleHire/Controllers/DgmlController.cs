using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CycleHire.Persistence.UnitOfWork;
using System.IO;
using CycleHire.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CycleHire.Controllers
{
    public class DgmlController : Controller
    {
        private ApplicationDbContext _context;
        public DgmlController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            System.IO.File.WriteAllText(Directory.GetCurrentDirectory() + "\\Entities.dgml",
                _context.AsDgml(), System.Text.Encoding.UTF8);

            var file = System.IO.File.OpenRead(Directory.GetCurrentDirectory() + "\\Entities.dgml");
            var response = File(file, "application/octet-stream", "Entities.dgml");
            return response;
        }
    }
}