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
        public InvisibilizerNode Invis { get; set; }

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
                }
                TryConnectOutput(output);
            }
            
        }

        public void ProcessExchanges(int tier)
        {
            Update();
            switch (tier)
            {
                case 1:
                    foreach (OutputNode output in Outputs)
                    {
                        if(output.Tier == 1)
                        {
                            output.ProcessExchanges();
                        }
                    }
                    break;
                case 2:
                    break;
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
                else if (node is InvisibilizerNode && Invis == null)
                {
                    Invis = (InvisibilizerNode)node;
                    Thread thread = new Thread(new ThreadStart(ChangePassable));
                    thread.Start();
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
                else if (node is InvisibilizerNode && Invis != null)
                {
                    Thread thread = new Thread(new ThreadStart(ChangePassable));
                    thread.Start();
                    Invis = null;
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
            if (Globals.Debug) { Printer.Info($"[{ID}] Trying output connection..."); }
            bool canConnect = false;
            if (output != null)
            {
                foreach (InputNode input in Inputs)
                {
                    if (!output.IsInputConnected(input))
                    {
                        if (Globals.Debug) { Printer.Info($"[{ID}] Input not connected"); }
                        if (Globals.Debug) { input.Print(); }
                        if (output.CanConnectedWith(input))
                        {
                            if (Globals.Debug) { Printer.Info($"[{ID}] Can connect with input"); }
                            canConnect = output.AddConnectedInput(input);
                            if (Globals.Debug) { Printer.Info($"[{ID}] CONNECTED? " + canConnect.ToString()); }
                        }
                    }
                }
            }
            return canConnect;
        }

        public bool TryDisconnectInput(InputNode input)
        {
            if (Globals.Debug) { Printer.Info($"[{ID}] Trying input disconnection"); Print(); }
            bool canDisconnect = false;
            if (input != null)
            {
                if (Globals.Debug) { Printer.Info($"[{ID}] Input not null"); }
                foreach (OutputNode output in Outputs)
                {
                    if (Globals.Debug) { Printer.Info($"[{ID}] Output has input? " + output.IsInputConnected(input).ToString()); }
                    if (output.IsInputConnected(input))
                    {
                        if (!output.CanConnectedWith(input))
                        {
                            if (Globals.Debug) { Printer.Info($"[{ID}] Can connect with input"); }
                            canDisconnect = output.RemoveConnectedInput(input);
                            if (Globals.Debug) { Printer.Info($"[{ID}] Disconnected?  " + canDisconnect.ToString()); }
                        }

                    }
                }
            }
            return canDisconnect;
        }

        public void ChangePassable()
        {
            List<Node> path = Invis.TraverseAll();
            Animator.AnimateChangingPassable(path);
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

        public string Print()
        {
            /*
            StringBuilder graph = new StringBuilder();
            graph.Append("\nPriting Networks: \n");
            graph.Append("Networks: \n");
            graph.Append("Inputs: \n");
            foreach (Input input in Inputs)
            {
                graph.Append(input.Obj.Name + input.Position.ToString() + input.GetHashCode().ToString() + ", ");
            }
            graph.Append("\n");
            graph.Append("Outputs: \n");
            foreach (Output output in Outputs)
            {
                graph.Append(output.Obj.Name + output.Position.ToString() + output.GetHashCode().ToString() + ", \n");
                foreach (Input input in output.ConnectedInputs.Keys)
                {
                    graph.Append("Output Connected Inputs: \n");
                    graph.Append(input.Obj.Name + input.Position.ToString() + input.GetHashCode().ToString() + " | ");
                }
                graph.Append("\n");
            }
            graph.Append("Connectors: \n");
            foreach (Connector conn in Connectors)
            {
                graph.Append(conn.Obj.Name + conn.Position.ToString() + conn.GetHashCode().ToString() + ", ");
            }
            graph.Append("\n");
            return graph.ToString();
            */
            return "";
        }
    }
}
