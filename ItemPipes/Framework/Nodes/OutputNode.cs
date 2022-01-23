using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemPipes.Framework.Model;
using ItemPipes.Framework.Nodes;
using ItemPipes.Framework.Util;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;

namespace ItemPipes.Framework
{
    public abstract class OutputNode : IOPipeNode
    {
        public int Tier { get; set; }
        public Dictionary<InputNode, List<Node>> ConnectedInputs { get; set; }
        public OutputNode() : base()
        {

        }
        public OutputNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            ConnectedInputs = new Dictionary<InputNode, List<Node>>();
            Tier = 1;
        }

        public void ProcessExchanges()
        {
            if (Globals.Debug) { Printer.Info($"[{ParentNetwork.ID}] Procesing Exchanges..."); }
            if (Globals.Debug) { Printer.Info($"[{ParentNetwork.ID}] Are there connected input? " + (ConnectedInputs.Count > 0).ToString()); }
            if (ConnectedContainer != null && !ConnectedContainer.IsEmpty()
                && ConnectedInputs.Count > 0 && State.Equals("on"))
            {
                if (Globals.Debug) { Printer.Info($"[{ ParentNetwork.ID}] Is output empty? " + ConnectedContainer.IsEmpty().ToString()); }
                if (Globals.Debug) { Printer.Info($"[{ParentNetwork.ID}] CREATED NEW THREAD"); }
                Thread thread = new Thread(new ThreadStart(StartExchage));
                thread.Start();
            }
        }

        public void StartExchage()
        {
            if (Globals.Debug) { Printer.Info($"[{Thread.CurrentThread.ManagedThreadId}],[{ParentNetwork.ID}] Number of inputs: " + ConnectedInputs.Count.ToString()); }
            Item item = null;
            int index = 0;
            Dictionary<InputNode, List<Node>> priorityInputs = ConnectedInputs;
            priorityInputs = priorityInputs.
                OrderByDescending(pair => pair.Key.Priority).
                ThenBy(pair => pair.Value.Count).
                ToDictionary(x => x.Key, x => x.Value);
            index = 0;
            while (index < priorityInputs.Count && item == null)
            {
                InputNode input = priorityInputs.Keys.ToList()[index];
                if (input.State.Equals("on"))
                {
                    List<Node> path = priorityInputs.Values.ToList()[index];
                    input.UpdateFilter();
                    if (ConnectedContainer is ChestContainerNode && input.ConnectedContainer is ChestContainerNode)
                    {
                        ChestContainerNode outChest = (ChestContainerNode)ConnectedContainer;
                        ChestContainerNode inChest = (ChestContainerNode)input.ConnectedContainer;
                        item = outChest.CanSendItem(inChest);
                        if (Globals.Debug) { Printer.Info($"[T{Thread.CurrentThread.ManagedThreadId}] [{ParentNetwork.ID}] Can send? " + (item != null).ToString()); }
                        if (item != null)
                        {
                            Printer.Info($"[T{Thread.CurrentThread.ManagedThreadId}] PATH---------------");
                            foreach(Node node in path)
                            {
                                node.Print();
                            }
                            Printer.Info($"[T{Thread.CurrentThread.ManagedThreadId}] PATH---------------");
                            Node broken = SendItem(item, input, 0, path);
                            Printer.Info($"[T{Thread.CurrentThread.ManagedThreadId}] IS IT BROKEN " +broken);
                            if (Globals.Debug) { Printer.Info($"[T{Thread.CurrentThread.ManagedThreadId}][{ParentNetwork.ID}] ITEM CORRECTLY SENT"); }
                            //Check with try connect/discoonnect also
                            if (outChest != null && inChest != null)
                            {
                                if(broken == null)
                                {
                                    bool sent = outChest.SendItem(inChest, item);
                                    if (!sent)
                                    {
                                        if (Globals.Debug) { Printer.Info($"T[{Thread.CurrentThread.ManagedThreadId}][{ParentNetwork.ID}] CANT ENTER, REVERSE"); }
                                        List<Node> reversePath = path;
                                        reversePath.Reverse();
                                        input.SendItem(item, this, 0, reversePath);
                                    }
                                }
                                else
                                {
                                    Printer.Info($"[T{Thread.CurrentThread.ManagedThreadId}][{ParentNetwork.ID}] PATH BROKEN, REVERSE");
                                    List<Node> reversePath = path;
                                    reversePath.Reverse();
                                    int brokenIndex = reversePath.IndexOf(broken);
                                    input.SendItem(item, this, brokenIndex+1, reversePath);
                                    inChest.SendItem(outChest, item);
                                }
                            }
                        }
                    }
                }
                index++;
            }
        }

        public bool IsInputConnected(InputNode input)
        {
            bool connected = false;
            if (ConnectedInputs.Keys.Contains(input))
            {
                connected = true;
            }
            return connected;
        }

        public bool AddConnectedInput(InputNode input)
        {

            bool added = false;
            if (!ConnectedInputs.Keys.Contains(input))
            {
                if (Globals.Debug) { Printer.Info($"[{ParentNetwork.ID}] Output container null? " + (ConnectedContainer == null).ToString()); }
                if (Globals.Debug) { Printer.Info($"[{ParentNetwork.ID}] Input container null? " + (input.ConnectedContainer == null).ToString()); }
                if (ConnectedContainer != null && input.ConnectedContainer != null)
                {
                    added = true;
                    List<Node> path;
                    path = GetPath(input.ConnectedContainer);
                    Animator.AnimateInputConnection(path);
                    ConnectedInputs.Add(input, path);
                }
            }
            return added;
        }

        public bool RemoveConnectedInput(InputNode input)
        {
            bool removed = false;
            if (ConnectedInputs.Keys.Contains(input))
            {
                removed = true;
                ConnectedInputs.Remove(input);
            }
            return removed;
        }
    }
}
