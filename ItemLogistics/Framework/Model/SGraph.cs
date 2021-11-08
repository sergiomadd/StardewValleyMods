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
        public List<SGNode> Outputs { get; set; }
        public List<SGNode> Inputs { get; set; }
        public List<SGNode> Conectors { get; set; }

        public SGraph()
        {
            Nodes = new List<SGNode>();
            Outputs = new List<SGNode>();
            Inputs = new List<SGNode>();
            Conectors = new List<SGNode>();
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

        public bool AddConector(SGNode node)
        {
            bool added = false;
            if (Nodes.Contains(node))
            {
                if (!Conectors.Contains(node))
                {
                    added = true;
                    Conectors.Add(node);
                }
            }
            return added;
        }

        public bool AddOutput(SGNode node)
        {
            bool added = false;
            if (Nodes.Contains(node))
            {
                if (!Outputs.Contains(node))
                {
                    added = true;
                    Outputs.Add(node);
                }
            }
            return added;
        }
        public bool AddInput(SGNode node)
        {
            bool added = false;
            if (Nodes.Contains(node))
            {
                if (!Inputs.Contains(node))
                {
                    added = true;
                    Inputs.Add(node);
                }
            }
            return added;
        }

        public bool TryConnectNodes(SGNode output, SGNode input)
        {
            bool canConnect = false;
            if (output != null && input != null)
            {
                if (!output.ConnectedNodes.Contains(input))
                {
                    if(output.TryReach(input, new List<SGNode>()) == input)
                    {
                        ConnectNodes(output, input);
                        canConnect = true;
                    } 
                }
            }
            return canConnect;
        }

        public bool TryConnectOutput(SGNode output)
        {
            bool canConnect = false;
            if (output != null)
            {
                foreach(SGNode input in Inputs)
                {
                    if (!output.ConnectedNodes.Contains(input))
                    {
                        foreach (SGNode unit in output.ConnectedNodes)
                        {
                            /*if (input.Adjacents.Values.Contains(unit))
                            {
                                canConnect = true;
                                ConnectNodes(output, input);
                            }*/
                        }
                    }
                }

            }
            return canConnect;
        }

        public void ConnectNodes(SGNode output, SGNode input)
        {
            output.AddConnectedNode(input);
        }

        public bool RemoveNode(SGNode node)
        {
            bool removed = false;
            if (Nodes.Contains(node))
            {
                removed = true;
                Nodes.Remove(node);
                if(Outputs.Contains(node))
                {
                    Outputs.Remove((SGNode)node);
                }
                if (Inputs.Contains(node))
                {
                    Inputs.Remove((SGNode)node);
                }
                if (Conectors.Contains(node))
                {
                    Conectors.Remove(node);
                }
            }
            return removed;
        }

        public bool DisconnectNodes(int from, int to)
        {
            bool disconnected = false;
            /*SGNode nFrom = Find(from);
            SGNode nTo = Find(to);
            if (nFrom == null || nTo == null)
            {
                disconnected = false;
            }
            else if (nFrom.ConnectedNodes.Keys.Contains(nTo))
            {
                disconnected = false;
            }
            else
            {
                nFrom.RemoveConnectedNode(nTo);
            }*/
            return disconnected;
        }

        public string Print()
        {
            StringBuilder graph = new StringBuilder();
            graph.Append("\nGroup: \n");
            graph.Append("Inputs: \n");
            foreach (SGNode input in Inputs)
            {
                graph.Append(input.Obj.Name + input.Position.ToString() + ", ");
            }
            graph.Append("\n");
            graph.Append("Outputs: \n");
            foreach (SGNode output in Outputs)
            {
                graph.Append(output.Obj.Name + output.Position.ToString() + ", \n");
                foreach(SGNode input in output.ConnectedNodes)
                {
                    graph.Append("Output Connected Inputs: \n");
                    graph.Append(input.Obj.Name + input.Position.ToString() + " | ");
                }
                graph.Append("\n");
            }
            graph.Append("Connectors: \n");
            foreach (SGNode conn in Conectors)
            {
                graph.Append(conn.Obj.Name + conn.Position.ToString() + ", ");
            }
            graph.Append("\n");
            return graph.ToString();
        }

        public bool Contains(Vector2 position)
        {
            bool contains = false;
            if(Nodes.Any(x => x.Position== position))
            {
                contains = true;
            }
            return contains;
        }
    }
}
