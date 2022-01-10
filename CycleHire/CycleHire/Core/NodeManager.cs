using CycleHire.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Core
{
    public static class NodeManager
    {
        //O(N)
        public static List<Node> BuildHierarchy(List<Node> nodes)
        {
            //unsorted list of children and parents
            Dictionary<Guid, Node> lookup = nodes.ToDictionary(n => n.Id, n => n);

            //O(N) 
            foreach (var node in lookup.Values)
            {
                if (node.ParentId != null)
                {
                    //O(1)
                    if (lookup.ContainsKey(node.ParentId.Value))
                    {
                        var parentOfNode = lookup[node.ParentId.Value];

                        parentOfNode.Children.Add(node);

                        node.Parent = parentOfNode;
                    }
                }
            }

            //O(N)
            var roots = lookup.Where(n => n.Value.ParentId == null)
                .Select(n => n.Value).ToList();

            return roots;
        }

        public static Node Traverse(List<Node> nodes, Guid id)
        {
            Node found = null;

            if (nodes == null) { return null; }

            foreach (Node node in nodes)
            {
                if (node.Id == id)
                {
                    return node;
                }
                else
                {
                    var x = Traverse(node.Children, id);

                    if (x != null)
                    {
                        found = x;
                    }
                };
            }

            return found;
        }

        public static IEnumerable<Node> Flatten(IEnumerable<Node> nodes)
        {
            var stack = new Stack<Node>();

            foreach (var node in nodes)
            {
                //add all parents
                stack.Push(node);
            }

            while (stack.Count > 0)
            {
                //remove each parent
                var current = stack.Pop();

                //return each parent node one at a time since we use IEnumerable
                yield return current;

                //parent may have children so add them to stack
                if (current.Children != null)
                {
                    foreach (var nodeChild in current.Children)
                    {
                        stack.Push(nodeChild);
                    }
                }
            }
        }
    }
}
