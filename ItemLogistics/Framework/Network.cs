using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemLogistics.Framework.Model;
using Microsoft.Xna.Framework;

namespace ItemLogistics.Framework
{
    public class Network
    {
        public int ID { get; set; }
        public List<Node> Nodes { get; set; }
        public List<Output> Outputs { get; set; }
        public List<Input> Inputs { get; set; }
        public List<Connector> Connectors { get; set; }
        public List<Container> Containers { get; set; }
        public bool IsPassable { get; set; }

        public Network()
        {
            Nodes = new List<Node>();
            Outputs = new List<Output>();
            Inputs = new List<Input>();
            Connectors = new List<Connector>();
            Containers = new List<Container>();
            IsPassable = false;
        }

        public void Update()
        {
            foreach(Output output in Outputs)
            {
                TryConnectOutput(output);
            }
            /*foreach(Input input in Inputs)
            {
                TryDisconnectInput(input);
            }*/
        }

        public void ProcessExchanges()
        {
            foreach (Output output in Outputs)
            {
                output.ProcessExchanges();
            }
        }

        public bool AddConnector(Connector node)
        {
            bool added = false;
            if (Nodes.Contains(node))
            {
                if (!Connectors.Contains(node))
                {
                    added = true;
                    Connectors.Add(node);
                }
            }
            return added;
        }

        public bool AddOutput(Output node)
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
        public bool AddInput(Input node)
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

        public bool AddContainer(Container node)
        {
            bool added = false;
            if (Nodes.Contains(node))
            {
                if (!Containers.Contains(node))
                {
                    added = true;
                    Containers.Add(node);
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
                    Outputs.Remove((Output)node);
                }
                if (Inputs.Contains(node))
                {
                    TryDisconnectInput((Input)node);
                    Inputs.Remove((Input)node);
                }
                if (Connectors.Contains(node))
                {
                    Connectors.Remove((ConnectorPipe)node);
                }
            }
            return removed;
        }

        public bool TryConnectNodes(Output output, Input input)
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

        public bool TryConnectOutput(Output output)
        {
            Printer.Info("Trying connection");
            bool canConnect = false;
            if (output != null)
            {
                foreach (Input input in Inputs)
                {
                    input.Print();
                    if (!output.IsInputConnected(input))
                    {
                        Printer.Info("Not connected");
                        if (output.CanConnectedWith(input))
                        {
                            Printer.Info("Connecting..");
                            input.Print();
                            output.AddConnectedInput(input);
                            canConnect = true;
                        }
                    }
                }
            }
            return canConnect;
        }

        public bool TryDisconnectInput(Input input)
        {
            Printer.Info("Trying disconnection");
            bool canDisconnect = false;
            if (input != null)
            {
                foreach (Output output in Outputs)
                {
                    if (output.IsInputConnected(input))
                    {
                        Printer.Info("Disconnecting..");
                        output.RemoveConnectedInput(input);
                        canDisconnect = true;
                    }
                }
            }
            return canDisconnect;
        }

        public bool AddNode(Node node)
        {
            bool added = false;
            if (!Nodes.Contains(node))
            {
                added = true;
                Nodes.Add(node);
            }
            return added;
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
            StringBuilder graph = new StringBuilder();
            graph.Append("\nGroup: \n");
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
            graph.Append("Containers: \n");
            foreach (Container cont in Containers)
            {
                graph.Append(cont.Obj.Name + cont.Position.ToString() + cont.GetHashCode().ToString() + ", ");
            }
            graph.Append("\n");
            graph.Append("Chests: \n");
            return graph.ToString();
        }
    }
}
