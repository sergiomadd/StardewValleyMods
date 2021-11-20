using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemLogistics.Framework.Model;
using Microsoft.Xna.Framework;
using StardewValley;

namespace ItemLogistics.Framework
{
    public class Input : SGNode
    {
        public Container ConnectedContainer { get; set; }

        public Input(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            ConnectedContainer = null;
        }

        public override bool AddAdjacent(Side side, SGNode entity)
        {
            bool added = false;
            if (Adjacents[side] == null)
            {
                added = true;
                Adjacents[side] = entity;
                entity.AddAdjacent(Sides.GetInverse(side), this);
                try
                {
                    if (ConnectedContainer == null && entity is Container)
                    {
                        ConnectedContainer = (Container)entity;
                    }
                    else
                    {
                        Printer.Info("More than 1 container adjacent.");
                    }
                }
                catch (Exception e)
                {
                    Printer.Info("More than 1 container adjacent.");
                    Printer.Info(e.StackTrace);
                }
            }
            return added;
        }
        public override bool RemoveAdjacent(Side side, SGNode entity)
        {
            bool removed = false;
            if (Adjacents[side] != null)
            {
                removed = true;
                if (ConnectedContainer != null && entity is Container)
                {
                    ConnectedContainer = null;
                }
                Adjacents[side] = null;
                entity.RemoveAdjacent(Sides.GetInverse(side), this);
            }
            return removed;
        }

        public override bool RemoveAllAdjacents()
        {
            bool removed = false;
            foreach (KeyValuePair<Side, SGNode> adj in Adjacents.ToList())
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
