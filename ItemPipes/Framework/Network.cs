using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemPipes.Framework.Model;
using ItemPipes.Framework.Nodes;
using ItemPipes.Framework.Util;
using Microsoft.Xna.Framework;
using System.Threading;
using StardewModdingAPI;
using StardewValley;
using ItemPipes.Framework.Nodes.ObjectNodes;

namespace ItemPipes.Framework
{
    public class Network
    {
        public long ID { get; set; }
        public List<Node> Nodes { get; set; }
        public List<OutputPipeNode> Outputs { get; set; }
        public List<InputPipeNode> Inputs { get; set; }
        public List<ConnectorPipeNode> Connectors { get; set; }
        public bool IsPassable { get; set; }
        public List<PIPONode> PIPOs { get; set; }

        public Network() { }
        public Network(long id)
        {
            ID = id;
            Nodes = new List<Node>();
            Outputs = new List<OutputPipeNode>();
            Inputs = new List<InputPipeNode>();
            Connectors = new List<ConnectorPipeNode>();
            PIPOs = new List<PIPONode>();
            IsPassable = false;
        }

        public void Update()
        {
            foreach(OutputPipeNode output in Outputs)
            {
                foreach (InputPipeNode input in output.ConnectedInputs.Keys.ToList())
                {
                    TryDisconnectInput(input);
                    input.UpdateSignal();
                }
                TryConnectOutput(output);
                output.UpdateSignal();
            }
            if (PIPOs.Count == 0)
            {
                Deinvisibilize(null);
            }
        }

        public void ProcessExchanges(int tier)
        {
            //Update();
            foreach (OutputPipeNode output in Outputs)
            {
                if (output.Tier == tier)
                {
                    output.ProcessExchanges();
                }
            }

        }

        public bool AddNode(Node node)
        {
            bool added = false;
            if (node.ParentNetwork == this && !Nodes.Contains(node))
            {
                added = true;
                if (IsPassable && node is not PIPONode)
                {
                    node.Passable = true;
                }
                Nodes.Add(node);
                if (node is OutputPipeNode && !Outputs.Contains(node))
                {
                    Outputs.Add((OutputPipeNode)node);
                }
                else if (node is InputPipeNode && !Inputs.Contains(node))
                {
                    Inputs.Add((InputPipeNode)node);
                }
                else if (node is ConnectorPipeNode && !Connectors.Contains(node))
                {
                    Connectors.Add((ConnectorPipeNode)node);
                }
                else if (node is PIPONode && !PIPOs.Contains(node))
                {
                    PIPOs.Add((PIPONode)node);
                    if (!IsPassable && (node as PIPONode).State == "on")
                    {
                        Invisibilize((PIPONode)node);
                    }
                    else if (IsPassable && (node as PIPONode).State == "off")
                    {
                        Deinvisibilize((PIPONode)node);
                    }
                }
            }
            else if(node.ParentNetwork != this)
            {
                Printer.Warn($"Tried to add {node.Print()} to N{ID}, but they dont match.");
            }
            return added;
        }

        public bool RemoveNode(Node node)
        {
            bool removed = false;
            if (Nodes.Contains(node))
            {
                removed = true;
                Nodes.Remove(node);
                if (Outputs.Contains(node))
                {
                    Outputs.Remove((OutputPipeNode)node);
                }
                else if (Inputs.Contains(node))
                {
                    Inputs.Remove((InputPipeNode)node);
                }
                else if (Connectors.Contains(node))
                {
                    Connectors.Remove((ConnectorPipeNode)node);
                }
                else if (node is PIPONode && PIPOs != null)
                {
                    PIPOs.Remove((PIPONode)node);
                    if (!IsPassable && (node as PIPONode).State == "on")
                    {
                        Invisibilize((PIPONode)node);
                    }
                    else if (IsPassable && (node as PIPONode).State == "off")
                    {
                        Deinvisibilize((PIPONode)node);
                    }
                }
            }
            return removed;
        }

        public bool TryConnectOutput(OutputPipeNode output)
        {
            bool canConnect = false;
            if (output != null)
            {
                if(Inputs.Count > 0)
                {
                    foreach (InputPipeNode input in Inputs)
                    {
                        if (!output.IsInputConnected(input))
                        {
                            if (output.CanConnectedWith(input))
                            {
                                canConnect = output.AddConnectedInput(input);
                                if (Globals.UltraDebug) { Printer.Debug($"[N{ID}] {input.Print()} connected with {output.Print()}"); }
                            }
                        }
                        else
                        {
                            List<PipeNode> path = output.GetPath(input);
                            if (path.Count > 0 && path.Last().Equals(input))
                            {
                                output.ConnectedInputs[input] = path;
                                if (Globals.UltraDebug) { Printer.Debug($"[N{ID}] Path succesfully updated!"); }
                            }
                            else
                            {
                                if (Globals.UltraDebug) { Printer.Warn($"[N{ID}] Error updating path from {output.Print()} to {input.Print()}"); }
                            }
                        }
                        input.UpdateSignal();
                    }
                    output.UpdateSignal();
                }
            }
            return canConnect;
        }

        public bool TryDisconnectInput(InputPipeNode input)
        {
            bool canDisconnect = false;
            if (input != null)
            {
                foreach (OutputPipeNode output in Outputs)
                {
                    if (output.IsInputConnected(input))
                    {
                        if (!output.CanConnectedWith(input) || input.ConnectedContainer == null)
                        {
                            canDisconnect = output.RemoveConnectedInput(input);
                            if (Globals.Debug) { Printer.Debug($"[N{ID}] {input.Print()} disconnected"); }
                        }
                    }
                    output.UpdateSignal();
                }
                input.UpdateSignal();
            }
            return canDisconnect;
        }

        public bool ContainsVector2(Vector2 position)
        {
            bool contains = false;
            if (Nodes.Any(x => x.Position == position))
            {
                contains = true;
            }
            return contains;
        }
        public void Delete()
        {
            foreach (Node node in Nodes)
            {
                node.ParentNetwork = null;
            }
        }

        public void Invisibilize(PIPONode invis)
        {
            if (PIPOs.All(p => p.Passable))
            {
                IsPassable = true;
                foreach (Node node in Nodes)
                {
                    if (node is not PIPONode)
                    {
                        node.Passable = true;
                    }
                }
            }
        }

        public void Deinvisibilize(PIPONode invis)
        {
            if(PIPOs.Count == 0)
            {
                IsPassable = false;
                foreach (Node node in Nodes)
                {
                    if (node is not PIPONode)
                    {
                        node.Passable = false;
                    }
                }
            }
            else if (PIPOs.Any(p => !p.Passable))
            {
                IsPassable = false;
                foreach (Node node in Nodes)
                {
                    if(node is not PIPONode)
                    {
                        node.Passable = false;
                    }
                }
            }
        }

        public void RemoveAllAdjacents()
        {
            foreach (Node node in Nodes.ToList())
            {
                node.RemoveAllAdjacents();
            }
        }
        public string Print()
        {
            StringBuilder graph = new StringBuilder();
            if (!Nodes.All(n=>n is ContainerNode))
            {
                graph.Append($"\n----------------------------");
                graph.Append($"\nPriting Network [{ID}] {this.GetHashCode().ToString()}: \n");
                graph.Append("Networks: \n");
                graph.Append("Inputs: \n");
                foreach (InputPipeNode input in Inputs)
                {
                    graph.Append(input.Print() + ", ");
                }
                graph.Append("\n");
                graph.Append("Outputs: \n");
                foreach (OutputPipeNode output in Outputs)
                {
                    graph.Append(output.Print() + ", \n");
                    graph.Append("Output Connected Inputs: \n");
                    foreach (InputPipeNode input in output.ConnectedInputs.Keys)
                    {
                        graph.Append(input.Print() + " | ");
                    }
                    graph.Append("\n");
                }
                graph.Append("Connectors: \n");
                foreach (ConnectorPipeNode conn in Connectors)
                {
                    graph.Append(conn.Print() + ", ");
                }
                graph.Append("\n");
                graph.Append("PIPOs: \n");
                foreach (PIPONode pipo in PIPOs)
                {
                    graph.Append(pipo.Print() + $" {pipo.State} {pipo.Passable} " + ", ");
                }
                graph.Append("\n");
            }
            else
            {
                graph.Append($"Network {ID} is only chests.");
            }
            return graph.ToString();
        }
    }
}
