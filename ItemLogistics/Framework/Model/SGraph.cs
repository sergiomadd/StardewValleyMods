using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;


namespace ItemLogistics.Framework.Model
{
    public class SGraph
    {
        public List<SGNode> Nodes { get; set; }

        public SGraph()
        {
            Nodes = new List<SGNode>();
        }

        public bool AddNode(SGNode node)
        {
            bool added = false;
            if (!Nodes.Contains(node))
            {
                added = true;
                Nodes.Add(node);
            }
            return added;
        }

        public virtual bool RemoveNode(SGNode node)
        {
            bool removed = false;
            if (Nodes.Contains(node))
            {
                removed = true;
                Nodes.Remove(node);
            }
            return removed;
        }

        public bool ContainsVector2(Vector2 position)
        {
            bool contains = false;
            if(Nodes.Any(x => x.Position== position))
            {
                contains = true;
            }
            return contains;
        }
        public void Delete()
        {
            foreach (SGNode node in Nodes)
            {
                node.ParentGraph = null;
            }
        }

        public virtual string Print()
        {
            StringBuilder graph = new StringBuilder();
            graph.Append("\nGroup: \n");
            graph.Append("Nodes: \n");
            foreach (SGNode node in Nodes)
            {
                graph.Append(node.Obj.Name + node.Position.ToString() + ", ");
            }
            graph.Append("\n");
            return graph.ToString();
        }
    }
}

