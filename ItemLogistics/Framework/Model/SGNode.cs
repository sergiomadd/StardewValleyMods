using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;

namespace ItemLogistics.Framework.Model
{
    class SGNode : SGElement
    {
        public List<SGNode> ConnectedNodes { get; set; }
        public List<SGEdgeUnit> ConnectedEdgeUnits { get; set; }

        public SGNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            ConnectedNodes = new List<SGNode>();
            ConnectedEdgeUnits = new List<SGEdgeUnit>();
        }

        public bool AddConnectedNode(SGNode node)
        {
            bool connected = false;
            if (!ConnectedNodes.Contains(node))
            {
                connected = true;
                ConnectedNodes.Add(node);
            }
            return connected;
        }

        public bool RemoveConnectedNode(SGNode node)
        {
            bool removed = false;
            if (ConnectedNodes.Contains(node))
            {
                removed = true;
                ConnectedNodes.Remove(node);
            }
            return removed;
        }

        public bool RemoveAllConnectedNodes()
        {
            bool removed = false;
            foreach (SGNode node in ConnectedNodes)
            {
                ConnectedNodes.Remove(node);
            }
            if (ConnectedNodes.Count == 0)
            {
                removed = true;
            }
            return removed;
        }

        public string Print()
        {
            /*StringBuilder graph = new StringBuilder();
            graph.AppendLine(Data + " has this connected nodes:");
            foreach (SGNode node in ConnectedNodes.Keys)
            {
                graph.AppendLine("-> " + node.Data);
            }
            return graph.ToString();*/
            return "";
        }
    }
}
