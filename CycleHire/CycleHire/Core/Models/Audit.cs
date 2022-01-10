using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.Models
{
    public class Audit
    {
        public Audit()
        {

        }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public bool IsDeleted { get; set; }

        public void NewlyCreated()
        {
            this.Created = DateTime.Now;
            this.Modified = DateTime.Now;
        }

        public void Updated()
        {
            this.Modified = DateTime.Now;
        }
    }
}
