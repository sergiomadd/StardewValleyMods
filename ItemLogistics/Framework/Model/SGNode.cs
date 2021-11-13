using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;

namespace ItemLogistics.Framework.Model
{
    public class SGNode
    {
        public Vector2 Position { get; set; }
        public GameLocation Location { get; set; }
        public StardewValley.Object Obj { get; set; }
        public Dictionary<Side, SGNode> Adjacents { get; set; }
        public SGraph ParentGraph { get; set; }
        public SideStruct Sides { get; set; }

        public SGNode(Vector2 position, GameLocation location, StardewValley.Object obj)
        {
            Position = position;
            Location = location;
            Obj = obj;

            Sides = SideStruct.GetSides();

            Adjacents = new Dictionary<Side, SGNode>();
            Adjacents.Add(Sides.North, null);
            Adjacents.Add(Sides.South, null);
            Adjacents.Add(Sides.West, null);
            Adjacents.Add(Sides.East, null);

            ParentGraph = null;
        }

        public bool CanConnectedWith(SGNode target)
        {
            bool connected = false;
            List<SGNode> looked = new List<SGNode>();
            if (this.TryReach(target, null, looked).Equals(target))
            {
                connected = true;
            }
            return connected;
        }

        public SGNode TryReach(SGNode target, SGNode last, List<SGNode> looked)
        {
            SGNode adj;
            if(this == target)
            {
                return this;
            }
            else
            {
                looked.Add(this);
                if (Adjacents.TryGetValue(Sides.North, out adj))
                {
                    if (adj != null && !looked.Contains(adj))
                    {
                        last = adj.TryReach(target, this, looked);
                    }
                }
                if (Adjacents.TryGetValue(Sides.South, out adj) && last != target)
                {
                    if (adj != null && !looked.Contains(adj))
                    {
                        last = adj.TryReach(target, this, looked);
                    }
                }
                if (Adjacents.TryGetValue(Sides.West, out adj) && last != target)
                {
                    if (adj != null && !looked.Contains(adj))
                    {
                        Printer.Info("W");
                        last = adj.TryReach(target, this, looked);
                    }
                }
                
                if (Adjacents.TryGetValue(Sides.East, out adj) && last != target)
                {
                    if (adj != null && !looked.Contains(adj))
                    {
                        Printer.Info("E");
                        last = adj.TryReach(target, this, looked);
                    }
                }
                return last;
            }
        }

        public List<SGraph> Scan()
        {
            List<SGraph> retList = new List<SGraph>();
            foreach(KeyValuePair<Side, SGNode> adj in Adjacents)
            {
                if(adj.Value != null)
                {
                    retList.Add(adj.Value.ParentGraph);
                }
            }
            return retList;
        }

        public SGNode GetAdjacent(Side side)
        {
            return Adjacents[side];
        }

        public virtual bool AddAdjacent(Side side, SGNode entity)
        {
            bool added = false;
            if (Adjacents[side] == null)
            {
                added = true;
                Adjacents[side] = entity;
                entity.AddAdjacent(Sides.GetInverse(side), this);
            }
            return added;
        }

        public bool RemoveAdjacent(Side side, SGNode entity)
        {
            bool removed = false;
            if (Adjacents[side] != null)
            {
                removed = true;
                Adjacents[side] = null;
                entity.RemoveAdjacent(Sides.GetInverse(side), this);
            }
            return removed;
        }

        
        public bool RemoveAllAdjacents()
        {
            bool removed = false;
            foreach(KeyValuePair<Side, SGNode> adj in Adjacents.ToList())
            {
                if(adj.Value != null)
                {
                    removed = true;
                    RemoveAdjacent(Sides.GetInverse(adj.Key), this);
                    Adjacents[adj.Key] = null;
                }
            }
            return removed;
        }
    }
}
