using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.Dtos
{
    public class NodeDto
    {
        public Guid Id { get; set; }
        public bool ReadOnly { get; set; }
        public string Parent { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
    }
}
