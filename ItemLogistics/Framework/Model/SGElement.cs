using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;

namespace ItemLogistics.Framework.Model
{
    class SGElement
    {
        public Vector2 Position { get; set; }
        public GameLocation Location { get; set; }
        public StardewValley.Object Obj { get; set; }
        public Dictionary<string, SGElement> Adjacents { get; set; }
        public SGraph parentGraph { get; set; }

        public SGElement(Vector2 position, GameLocation location, StardewValley.Object obj)
        {
            Position = position;
            Location = location;
            Obj = obj;

            Adjacents = new Dictionary<string, SGElement>();
            Adjacents.Add("Up", null);
            Adjacents.Add("Down", null);
            Adjacents.Add("Right", null);
            Adjacents.Add("Left", null);

            parentGraph = null;
        }

        public SGElement TryReach(SGElement elem, List<SGElement> looked)
        {
            SGElement adj;
            SGElement last = null;
            if(last == elem)
            {
                return last;
            }
            else
            {
                if (Adjacents.TryGetValue("Up", out adj) && last != elem)
                {
                    if (adj != null && !looked.Contains(adj))
                    {
                        last = adj.TryReach(elem, looked);
                    }
                }
                if (Adjacents.TryGetValue("Down", out adj) && last != elem)
                {
                    if (adj != null && !looked.Contains(adj))
                    {
                        last = adj.TryReach(elem, looked);
                    }
                }
                if (Adjacents.TryGetValue("Right", out adj) && last != elem)
                {
                    if (adj != null && !looked.Contains(adj))
                    {
                        last = adj.TryReach(elem, looked);
                    }
                }
                if (Adjacents.TryGetValue("Left", out adj) && last != elem)
                {
                    if (adj != null && !looked.Contains(adj))
                    {
                        last = adj.TryReach(elem, looked);
                    }
                }
                return last;
            }
        }

        public List<SGraph> Scan()
        {
            List<SGraph> retList = new List<SGraph>();
            foreach(KeyValuePair<string, SGElement> adj in Adjacents)
            {
                if(adj.Value != null)
                {
                    retList.Add(adj.Value.parentGraph);
                }
            }
            return retList;
        }

        public SGElement GetAdjacent(string orientation)
        {
            return Adjacents[orientation];
        }

        public bool AddAdjacent(string orientation, SGElement entity)
        {
            bool added = false;
            if (Adjacents[orientation] == null)
            {
                added = true;
                Adjacents[orientation] = entity;
                entity.AddAdjacent(GetInverseOrientation(orientation), this);
            }
            return added;
        }

        public bool RemoveAdjacent(string orientation, SGElement entity)
        {
            bool removed = false;

            if (Adjacents[orientation] != null)
            {
                removed = true;
                Adjacents[orientation] = null;
                entity.RemoveAdjacent(GetInverseOrientation(orientation), this);
            }
            return removed;
        }

        
        public bool RemoveAllAdjacents()
        {
            bool removed = false;
            foreach(KeyValuePair<string, SGElement> adj in Adjacents)
            {
                if(adj.Value != null)
                {
                    removed = true;
                    RemoveAdjacent(GetInverseOrientation(adj.Key), this);
                    Adjacents[adj.Key] = null;
                }
            }
            return removed;
        }

        private string GetInverseOrientation(string orientation)
        {
            string inverse = "";
            switch(orientation)
            {
                case "Up":
                    inverse = "Down";
                    break;
                case "Down":
                    inverse = "Up";
                    break;
                case "Right":
                    inverse = "Left";
                    break;
                case "Left":
                    inverse = "Right";
                    break;
            }
            return inverse;
        }
    }
}
