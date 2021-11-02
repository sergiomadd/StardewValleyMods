using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;

namespace ItemLogistics.Framework.Model
{
    class SGraph
    {
        public List<SGElement> Elements { get; set; }
        public List<SGNode> Outputs { get; set; }
        public List<SGNode> Inputs { get; set; }
        public List<SGElement> Conectors { get; set; }

        public SGraph()
        {
            Elements = new List<SGElement>();
            Outputs = new List<SGNode>();
            Inputs = new List<SGNode>();
            Conectors = new List<SGElement>();
        }

        public string Build()
        {
            string ret = "BUILDING.. \n";
            foreach (SGNode output in Outputs)
            {
                foreach (SGNode input in Inputs)
                {
                    if(TryConnectNodes(output, input))
                    {
                        ret = ret + "CONNECTED " + output.Position.ToString() + "WITH " + input.Position.ToString() + " | ";
                    }
                }
            }
            return ret;
        }

        public bool AddElement(SGElement elem)
        {
            bool added = false;
            if (!Elements.Contains(elem))
            {
                added = true;
                Elements.Add(elem);
            }
            return added;
        }

        public bool AddConector(SGElement edge)
        {
            bool added = false;
            if (!Conectors.Contains(edge))
            {
                added = true;
                Conectors.Add(edge);
            }
            return added;
        }

        public bool AddOutput(SGNode node)
        {
            bool added = false;
            if (!Outputs.Contains(node))
            {
                added = true;
                Outputs.Add(node);
                //TryConnectOutput(node);
            }
            return added;
        }
        public bool AddInput(SGNode node)
        {
            bool added = false;
            if (!Inputs.Contains(node))
            {
                added = true;
                Inputs.Add(node);
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
                    if(output.TryReach(input, new List<SGElement>()) == input)
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
                        foreach (SGEdgeUnit unit in output.ConnectedEdgeUnits)
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
            Update();
        }

        public bool RemoveNode(int id)
        {
            bool removed = false;
            /*SGNode node = Find(id);
            if (node != null)
            {
                removed = true;
                Conectors.Remove(node);
                foreach (SGNode n in Conectors)
                {
                    if (n.ConnectedNodes.Keys.Contains(node))
                    {
                        n.ConnectedNodes.Remove(node);
                    }
                }
            }*/
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
            foreach (SGElement conn in Conectors)
            {
                graph.Append(conn.Obj.Name + conn.Position.ToString() + ", ");
            }
            return graph.ToString();
        }

        public bool IsValidPath(SGNode from, SGNode to)
        {
            return false;
        }
        
        public void Update()
        {
            //Construit todo el grafo
        }




    }
}
