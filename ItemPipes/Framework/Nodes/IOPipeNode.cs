﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemPipes.Framework.Model;
using ItemPipes.Framework.Util;
using ItemPipes.Framework.Nodes;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;

namespace ItemPipes.Framework
{
    public abstract class IOPipeNode : PipeNode
    {
        public ContainerNode ConnectedContainer { get; set; }
        public string Signal { get; set; }

        public IOPipeNode() : base()
        {
            ConnectedContainer = null;
            Signal = "unconnected";
            Connecting = false;
        }
        
        public IOPipeNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            ConnectedContainer = null;
            Signal = "nochest";
            Connecting = false;

        }

        public virtual void UpdateSignal()
        {
            if (ConnectedContainer == null)
            {
                Signal = "nochest";
            }
            else if (ConnectedContainer != null)
            {
                Signal = "on";
            }

        }

        public void ChangeSignal()
        {
            if(Signal.Equals("off"))
            {
                Signal = "on";
            }
            else
            {
                Signal = "off";
            }
        }

        public bool AddConnectedContainer(Node entity)
        {
            bool added = false;
            if (Globals.UltraDebug) { Printer.Info($"[?] Adding {entity.Name} container to {Print()} "); }
            if (Globals.UltraDebug) { Printer.Info($"[?] Alreadyhas a container? {ConnectedContainer != null}"); }
            if (ConnectedContainer == null && entity is ContainerNode)
            {
                if (Globals.UltraDebug) { Printer.Info($"[?] Connecting adjacent container.."); }
                ContainerNode container = (ContainerNode)entity;
                if ((this is OutputNode && container.Output == null) ||
                    (this is InputNode && container.Input == null))
                {
                    ConnectedContainer = (ContainerNode)entity;
                    ConnectedContainer.AddIOPipe(this);
                    if (Globals.UltraDebug) { Printer.Info($"[?] CONNECTED CONTAINER ADDED"); }
                }
                else
                {
                    if (Globals.UltraDebug) { Printer.Info($"[?] Didnt add adj container"); }
                }
            }
            else
            {
                if (Globals.UltraDebug) { Printer.Info($"[?] Didnt add adj container"); }
            }
            UpdateSignal();
            added = true;
            return added;
        }

        public bool RemoveConnectedContainer(Node entity)
        {
            bool removed = false;
            if (Globals.UltraDebug) { Printer.Info($"[?] Removing {entity.Name} container "); }
            if (ConnectedContainer != null && entity is ContainerNode)
            {
                ConnectedContainer.RemoveIOPipe(this);
                ConnectedContainer = null;
                if (Globals.UltraDebug) { Printer.Info($"[?] CONNECTED CONTAINER REMOVED"); }
                removed = true;
            }
            UpdateSignal();
            return removed;
        }

        public override bool AddAdjacent(Side side, Node node)
        {
            bool added = false;
            if (Adjacents[side] == null)
            {
                added = true;
                Adjacents[side] = node;
                node.AddAdjacent(Sides.GetInverse(side), this);
                if(node is ContainerNode)
                {
                    AddConnectedContainer(node);
                }
            }
            return added;
        }

        public override bool RemoveAdjacent(Side side, Node node)
        {
            bool removed = false;
            if (Adjacents[side] != null)
            {
                removed = true;
                Adjacents[side] = null;
                node.RemoveAdjacent(Sides.GetInverse(side), this);
                if (node is ContainerNode)
                {
                    RemoveConnectedContainer(node);
                }
            }
            return removed;
        }


        public override bool RemoveAllAdjacents()
        {
            bool removed = false;
            /*
            if (ConnectedContainer != null)
            {
                ConnectedContainer.RemoveIOPipe(this);
            }
            */
            foreach (KeyValuePair<Side, Node> adj in Adjacents.ToList())
            {
                if (adj.Value != null)
                {
                    removed = true;
                    RemoveAdjacent(adj.Key, adj.Value);
                }
            }
            return removed;
        }
    }
}
