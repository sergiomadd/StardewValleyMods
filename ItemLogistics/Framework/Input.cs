using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemLogistics.Framework.Model;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Buildings;

namespace ItemLogistics.Framework
{
    public class Input : Node
    {
        public Container ConnectedContainer { get; set; }
        public List<string> Filter { get; set; }
        public int Priority { get; set; }

        public Input(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            ConnectedContainer = null;
            Filter = new List<string>();
        }

        public override bool AddAdjacent(Side side, Node entity)
        {
            bool added = false;
            if (Adjacents[side] == null)
            {
                if (ConnectedContainer == null && entity is Container)
                {
                    Container container = (Container)entity;
                    if (container.Input == null)
                    {
                        ConnectedContainer = (Container)entity;
                        Printer.Info("CONNECTED CONTAINER ADDED");
                    }
                }
                else
                {
                    Printer.Info("Didnt add adj container");
                }
                added = true;
                Adjacents[side] = entity;
                entity.AddAdjacent(Sides.GetInverse(side), this);
                UpdateFilter();
            }
            return added;
        }
        public override bool RemoveAdjacent(Side side, Node entity)
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

        public bool HasFilter()
        {
            bool hasFilter = false;
            if (Filter.Count > 0)
            {
                hasFilter = true;
            }
            return hasFilter;
        }
        public virtual void UpdateFilter()
        {
        }
    }
}
