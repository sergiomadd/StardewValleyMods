﻿using System;
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

        public override void UpdateSignal()
        {
            if(ConnectedContainer == null)
            {
                Signal = "nochest";
            }
            else if (ConnectedInputs.Count < 1)
            {
                Signal = "unconnected";
            }
            else if (ConnectedContainer != null && ConnectedInputs.Count >= 1)
            {
                Signal = "on";
            }
        }

        public void ProcessExchanges()
        {
            if (Globals.UltraDebug) { Printer.Info($"[{ParentNetwork.ID}] Procesing Exchanges..."); }
            if (Globals.UltraDebug) { Printer.Info($"[{ParentNetwork.ID}] Are there connected input? " + (ConnectedInputs.Count > 0).ToString()); }
            if (ConnectedContainer != null && !ConnectedContainer.IsEmpty()
                && ConnectedInputs.Count > 0 && Signal.Equals("on"))
            {
                if (Globals.UltraDebug) { Printer.Info($"[{ ParentNetwork.ID}] Is output empty? " + ConnectedContainer.IsEmpty().ToString()); }
                if (Globals.UltraDebug) { Printer.Info($"[{ParentNetwork.ID}] CREATED NEW THREAD"); }
                try
                {
                    Thread thread = new Thread(new ThreadStart(StartExchage));
                    DataAccess.GetDataAccess().Threads.Add(thread);
                    thread.Start();
                }
                catch (ThreadInterruptedException exception)
                {
                }
            }
        }

        public void StartExchage()
        {
            if (Globals.UltraDebug) { Printer.Info($"[{Thread.CurrentThread.ManagedThreadId}][{ParentNetwork.ID}] Number of inputs: " + ConnectedInputs.Count.ToString()); }
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
                if (input.Signal.Equals("on"))
                {
                    List<Node> path = priorityInputs.Values.ToList()[index];
                    input.UpdateFilter();
                    if (ConnectedContainer is ChestContainerNode && input.ConnectedContainer is ChestContainerNode)
                    {
                        ChestContainerNode outChest = (ChestContainerNode)ConnectedContainer;
                        ChestContainerNode inChest = (ChestContainerNode)input.ConnectedContainer;
                        item = outChest.CanSendItem(inChest);
                        if (Globals.Debug) { Printer.Info($"[T{Thread.CurrentThread.ManagedThreadId}][{ParentNetwork.ID}] Can send {item.Name}? " + (item != null).ToString()); }
                        if (item != null)
                        {
                            if (Globals.UltraDebug)
                            {
                                Printer.Info($"[T{Thread.CurrentThread.ManagedThreadId}] PATH---------------");
                                foreach (Node node in path)
                                {
                                    Printer.Info(node.Print());
                                }
                                Printer.Info($"[T{Thread.CurrentThread.ManagedThreadId}] PATH---------------");
                            }
                            Node broken = MoveItem(item, input, 0, path);
                            //Printer.Info($"[T{Thread.CurrentThread.ManagedThreadId}] IS IT BROKEN " +broken);
                            //Check with try connect/discoonnect also
                            if (outChest != null && inChest != null)
                            {
                                if(broken == null)
                                {
                                    bool sent = outChest.SendItem(inChest, item);
                                    if (!sent)
                                    {
                                        if (Globals.UltraDebug) { Printer.Info($"T[{Thread.CurrentThread.ManagedThreadId}][{ParentNetwork.ID}] {item.Name} CANT ENTER, REVERSE"); }
                                        List<Node> reversePath = path;
                                        reversePath.Reverse();
                                        input.MoveItem(item, this, 0, reversePath);
                                    }
                                    else
                                    {
                                        if (Globals.UltraDebug) { Printer.Info($"[T{Thread.CurrentThread.ManagedThreadId}][{ParentNetwork.ID}] {item.Name} CORRECTLY SENT"); }
                                    }
                                }
                                else
                                {
                                    if (Globals.UltraDebug) { Printer.Info($"[T{Thread.CurrentThread.ManagedThreadId}][{ParentNetwork.ID}] {item.Name} PATH BROKEN, REVERSE"); }
                                    List<Node> reversePath = path;
                                    reversePath.Reverse();
                                    int brokenIndex = reversePath.IndexOf(broken);
                                    input.MoveItem(item, this, brokenIndex+1, reversePath);
                                    inChest.SendItem(outChest, item);
                                }
                            }
                        }
                    }
                    else if (ConnectedContainer is ChestContainerNode && input.ConnectedContainer is ShippingBinContainerNode)
                    {
                        ShippingBinContainerNode shipBin = (ShippingBinContainerNode)input.ConnectedContainer;
                        ChestContainerNode outChest = (ChestContainerNode)ConnectedContainer;
                        item = outChest.GetItemToShip(input);
                        if (item != null)
                        {
                            Node broken = MoveItem(item, input, 0, path);
                            if (outChest != null)
                            {
                                if (broken == null)
                                {
                                    shipBin.ShipItem(item);
                                }
                                else
                                {
                                    if (Globals.UltraDebug) { Printer.Info($"[T{Thread.CurrentThread.ManagedThreadId}][{ParentNetwork.ID}] {item.Name} PATH BROKEN, REVERSE"); }
                                    List<Node> reversePath = path;
                                    reversePath.Reverse();
                                    int brokenIndex = reversePath.IndexOf(broken);
                                    input.MoveItem(item, this, brokenIndex + 1, reversePath);
                                    outChest.InsertItem(item);
                                }
                            }
                        }

                    }
                }
                index++;
            }
            DataAccess.GetDataAccess().Threads.Remove(Thread.CurrentThread);
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
            if (Globals.UltraDebug) { Printer.Info($"[N{ParentNetwork.ID}] Does {Print()} have a valid adjacent container? " + (ConnectedContainer != null).ToString()); }
            if (Globals.UltraDebug) { Printer.Info($"[N{ParentNetwork.ID}] Does {input.Print()} have a valid adjacent container? " + (input.ConnectedContainer != null).ToString()); }
            if (ConnectedContainer != null && input.ConnectedContainer != null)
            {
                List<Node> path;
                path = GetPath(input);
                if(path.Count > 0)
                {
                    added = true;
                    ConnectedInputs.Add(input, path);
                    var t = new Thread(() => AnimateConnection(path));
                    t.Start();
                    DataAccess.GetDataAccess().Threads.Add(t);
                }
            }
            return added;
        }

        private void AnimateConnection(List<Node> path)
        {
            ConnectPipe(path.Last(), 0, path);
            DataAccess.GetDataAccess().Threads.Remove(Thread.CurrentThread);
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
