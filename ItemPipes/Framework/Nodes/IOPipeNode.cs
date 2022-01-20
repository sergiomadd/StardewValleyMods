using System;
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
        public bool Connecting { get; set; }


        public IOPipeNode() : base()
        {
            ConnectedContainer = null;
            State = "unconnected";
            Connecting = false;
        }
        
        public IOPipeNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            ConnectedContainer = null;
            State = "unconnected";
            Connecting = false;

        }
        
        public override string GetState()
        {
            if (Connecting)
            {
                return "connecting_"+State;
            }
            else
            {
                return State;
            }
        }

        public override bool AddAdjacent(Side side, Node entity)
        {
            bool added = false;
            if (Adjacents[side] == null)
            {
                if (ConnectedContainer == null && entity is ContainerNode)
                {
                    ContainerNode container = (ContainerNode)entity;
                    if ((this is OutputNode && container.Output == null) ||
                        (this is InputNode && container.Input == null))
                    {
                        ConnectedContainer = (ContainerNode)entity;
                        State = "on";
                        if (Globals.Debug) { Printer.Info($"[?] CONNECTED CONTAINER ADDED"); }
                    }

                }
                else
                {
                    if (Globals.Debug) { Printer.Info($"[?] Didnt add adj container"); }
                }
                added = true;
                Adjacents[side] = entity;
                entity.AddAdjacent(Sides.GetInverse(side), this);
            }
            return added;
        }

        public override bool RemoveAdjacent(Side side, Node entity)
        {
            bool removed = false;
            if (Adjacents[side] != null)
            {
                removed = true;
                if (ConnectedContainer != null && entity is ContainerNode)
                {
                    ConnectedContainer = null;
                    State = "unconnected";
                    if (Globals.Debug) { Printer.Info($"[?] CONNECTED CONTAINER REMOVED"); }
                }
                Adjacents[side] = null;
                entity.RemoveAdjacent(Sides.GetInverse(side), this);
            }
            return removed;
        }

        public override bool RemoveAllAdjacents()
        {
            bool removed = false;
            foreach (KeyValuePair<Side, Node> adj in Adjacents.ToList())
            {
                if (adj.Value != null)
                {
                    removed = true;
                    RemoveAdjacent(adj.Key, adj.Value);
                    Adjacents[adj.Key] = null;
                }
            }
            return removed;
        }
    }
}
