using System;
using System.ComponentModel.DataAnnotations;

namespace CycleHire.Core.Models
{
    public class Accessory : Audit
    {
        public Accessory() { }
        public Accessory(string name)
        {
            Name = name;
            NewlyCreated();
        }
        public Guid Id { get; set; }

        [MaxLength(50)]
        public String Name { get; set; }
    }
}
