using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core.Models
{
    public class FolderItem : Node
    {
        public FolderItem()
        {
            Children = new List<Node>();
        }
    }
}
