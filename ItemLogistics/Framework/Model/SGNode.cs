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
        public bool Reached { get; set; }

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
            if ((bool)GetPathRecursive(target, looked, false)[2])
            {
                Printer.Info("CAN CONNECT");
                connected = true;
            }
            return connected;
        }

        public List<SGNode> GetPath(SGNode target)
        {
            List<SGNode> looked = new List<SGNode>();
            Reached = false;
            System.Object[] returns = GetPathRecursive(target, looked, false);
            List<SGNode> path = (List<SGNode>)returns[1];
            return path;
        }

        public System.Object[] GetPathRecursive(SGNode target, List<SGNode> looked, bool reached)
        {
            System.Object[] returns = new System.Object[3];
            returns[2] = reached;
            SGNode adj;
            if (this.Equals(target))
            {
                reached = true;
                Printer.Info("Reached");
                Printer.Info(looked.Count.ToString());
                returns[0] = this;
                returns[1] = looked;
                returns[2] = reached;
                return returns;
            }
            else
            {
                looked.Add(this);
                if (Adjacents.TryGetValue(Sides.North, out adj) && !(bool)returns[2])
                {
                    if (adj != null && !looked.Contains(adj))
                    {
                        returns = adj.GetPathRecursive(target, looked, reached);
                    }
                }
                if (Adjacents.TryGetValue(Sides.South, out adj) && !(bool)returns[2])
                {
                    if (adj != null && !looked.Contains(adj))
                    {
                        returns = adj.GetPathRecursive(target, looked, reached);
                    }
                }
                if (Adjacents.TryGetValue(Sides.West, out adj) && !(bool)returns[2])
                {
                    if (adj != null && !looked.Contains(adj))
                    {
                        returns = adj.GetPathRecursive(target, looked, reached);
                    }
                }

                if (Adjacents.TryGetValue(Sides.East, out adj) && !(bool)returns[2])
                {
                    if (adj != null && !looked.Contains(adj))
                    {
                        returns = adj.GetPathRecursive(target, looked, reached);
                    }
                }
                if(!(bool)returns[2])
                {
                    looked.Remove(this);
                }
                return returns;
            }
        }
         
        public void AnimatePath(List<SGNode> path)
        {
            foreach(SGNode node in path)
            {
                if(node is Connector)
                {
                    Connector conn = (Connector)node;
                    Animate(conn);
                }
            }
        }
        
        public void Animate(Connector conn)
        {
            conn.PassingItem = true;
            System.Threading.Thread.Sleep(500);
            conn.PassingItem = false;
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

        public virtual bool RemoveAdjacent(Side side, SGNode entity)
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

        
        public virtual bool RemoveAllAdjacents()
        {
            bool removed = false;
            foreach(KeyValuePair<Side, SGNode> adj in Adjacents.ToList())
            {
                adj.Value.Print();
                if(adj.Value != null)
                {
                    removed = true;
                    RemoveAdjacent(adj.Key, adj.Value);
                    Adjacents[adj.Key] = null;
                }
            }
            return removed;
        }

        public bool Same(SGNode node)
        {
            if(this.Obj.name.Equals(node.Obj.name) && Position.Equals(node.Position))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Print()
        {
            Printer.Info(Obj.Name + Position.X.ToString() + Position.Y.ToString());
            Printer.Info(GetHashCode().ToString());
        }
    }
}
