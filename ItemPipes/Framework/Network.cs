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
        public int ID { get; set; }
        public List<Node> Nodes { get; set; }
        public List<OutputNode> Outputs { get; set; }
        public List<InputNode> Inputs { get; set; }
        public List<ConnectorNode> Connectors { get; set; }
        public bool IsPassable { get; set; }
        public PPMNode Invis { get; set; }

        public Network() { }
        public Network(int id)
        {
            ID = id;
            Nodes = new List<Node>();
            Outputs = new List<OutputNode>();
            Inputs = new List<InputNode>();
            Connectors = new List<ConnectorNode>();
            IsPassable = false;
        }

        public void Update()
        {
            foreach(OutputNode output in Outputs)
            {
                foreach (InputNode input in output.ConnectedInputs.Keys.ToList())
                {
                    TryDisconnectInput(input);
                    input.UpdateSignal();
                }
                TryConnectOutput(output);
                output.UpdateSignal();
            }
        }

        public void ProcessExchanges(int tier)
        {
            Update();
            foreach (OutputNode output in Outputs)
            {
                if (output.Tier == tier)
                {
                    Printer.Info(tier.ToString());

                    output.ProcessExchanges();
                }
            }
        }

        public bool AddNode(Node node)
        {
            bool added = false;
            if (!Nodes.Contains(node))
            {
                added = true;
                Nodes.Add(node);
                if (node is OutputNode && !Outputs.Contains(node))
                {
                    Outputs.Add((OutputNode)node);
                }
                else if (node is InputNode && !Inputs.Contains(node))
                {
                    Inputs.Add((InputNode)node);
                }
                else if (node is ConnectorNode && !Connectors.Contains(node))
                {
                    Connectors.Add((ConnectorNode)node);
                }
                else if (node is PPMNode && Invis == null)
                {
                    Invis = (PPMNode)node;
                }
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
                    Outputs.Remove((OutputNode)node);
                }
                else if (Inputs.Contains(node))
                {
                    Inputs.Remove((InputNode)node);
                }
                else if (Connectors.Contains(node))
                {
                    Connectors.Remove((ConnectorNode)node);
                }
                else if (node is PPMNode && Invis != null)
                {
                    Invis = null;
                    if(IsPassable)
                    {
                        Deinvisibilize((PPMNode)node);
                    }
                }
            }
            return removed;
        }

        public bool TryConnectNodes(OutputNode output, InputNode input)
        {
            bool connected = false;
            if (output != null && input != null)
            {
                if (!output.IsInputConnected(input))
                {
                    if (output.CanConnectedWith(input))
                    {
                        output.AddConnectedInput(input);

                        connected = true;
                    }
                }
            }
            return connected;
        }

        public bool TryConnectOutput(OutputNode output)
        {
            bool canConnect = false;
            if (output != null)
            {
                if (Globals.UltraDebug) { Printer.Info($"[N{ID}] Trying connecting {output.Print()}"); }
                if(Inputs.Count == 0)
                {
                    if (Globals.UltraDebug) { Printer.Info($"[N{ID}] No inputs to connect."); }
                }
                else
                {
                    if (Globals.UltraDebug) { Printer.Info($"[N{ID}] {Inputs.Count} inputs to connect."); }
                    foreach (InputNode input in Inputs)
                    {
                        if (!output.IsInputConnected(input))
                        {
                            if (Globals.UltraDebug) { Printer.Info($"[N{ID}] {input.Print()} not already connected"); }
                            if (output.CanConnectedWith(input))
                            {
                                canConnect = output.AddConnectedInput(input);
                                if (Globals.UltraDebug) { Printer.Info($"[N{ID}] Can connect with {input.Print()}? -> {canConnect}"); }
                            }
                            else
                            {
                                if (Globals.UltraDebug) { Printer.Info($"[N{ID}] Cannot connect with {input.Print()}"); }
                            }
                        }
                        else
                        {
                            if (Globals.UltraDebug) { Printer.Info($"[N{ID}] {input.Print()} already connected"); }
                        }
                        input.UpdateSignal();
                    }
                    output.UpdateSignal();
                }
            }
            return canConnect;
        }

        public bool TryDisconnectInput(InputNode input)
        {
            bool canDisconnect = false;
            if (input != null)
            {
                if (Globals.UltraDebug) { Printer.Info($"[N{ID}] Trying disconnecting {input.Print()}"); }
                foreach (OutputNode output in Outputs)
                {
                    if (output.IsInputConnected(input))
                    {
                        if (Globals.UltraDebug) { Printer.Info($"[N{ID}] {input.Print()} already connected"); }
                        if (!output.CanConnectedWith(input))
                        {
                            canDisconnect = output.RemoveConnectedInput(input);
                            if (Globals.UltraDebug) { Printer.Info($"[N{ID}] Can disconnect with {input.Print()}? -> {canDisconnect}"); }
                        }
                        else
                        {
                            if (Globals.UltraDebug) { Printer.Info($"[N{ID}] Cannot disconnect with {input.Print()}"); }
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

        public void Invisibilize(PPMNode invis)
        {
            Invis = invis;
            IsPassable = true;
            foreach(Node node in Nodes)
            {
                node.Passable = true;
            }
        }

        public void Deinvisibilize(PPMNode invis)
        {
            Invis = invis;
            IsPassable = false;
            foreach (Node node in Nodes)
            {
                node.Passable = false;
            }
        }

        public string Print()
        {
            StringBuilder graph = new StringBuilder();
            if (!Nodes.All(n=>n is ContainerNode))
            {
                graph.Append($"\n----------------------------");
                graph.Append($"\nPriting Network [{ID}]: \n");
                graph.Append("Networks: \n");
                graph.Append("Inputs: \n");
                foreach (InputNode input in Inputs)
                {
                    graph.Append(input.Obj.Name + input.Position.ToString() + input.GetHashCode().ToString() + ", ");
                }
                graph.Append("\n");
                graph.Append("Outputs: \n");
                foreach (OutputNode output in Outputs)
                {
                    graph.Append(output.Obj.Name + output.Position.ToString() + output.GetHashCode().ToString() + ", \n");
                    foreach (InputNode input in output.ConnectedInputs.Keys)
                    {
                        graph.Append("Output Connected Inputs: \n");
                        graph.Append(input.Obj.Name + input.Position.ToString() + input.GetHashCode().ToString() + " | ");
                    }
                    graph.Append("\n");
                }
                graph.Append("Connectors: \n");
                foreach (ConnectorNode conn in Connectors)
                {
                    graph.Append(conn.Obj.Name + conn.Position.ToString() + conn.GetHashCode().ToString() + ", ");
                }
                graph.Append("\n");
            }
            return graph.ToString();
        }
    }
}
