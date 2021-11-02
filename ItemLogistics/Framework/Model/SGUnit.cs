using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;

namespace ItemLogistics.Framework.Model
{
    class SGUnit
    {
        public Vector2 Position { get; set; }
        public GameLocation Location { get; set; }
        public Dictionary<string, SGUnit> Adjacents { get; set; }
        public SGraph ParentGraph { get; set; }

        public SGUnit()
        {
            Adjacents = new Dictionary<string, SGUnit>();
            Adjacents.Add("Up", null);
            Adjacents.Add("Down", null);
            Adjacents.Add("Right", null);
            Adjacents.Add("Left", null);
            ParentGraph = new SGraph();
        }

        public SGUnit GetAdjacent(string orientation)
        {
            return Adjacents[orientation];
        }

        public bool AddAdjacent(string orientation, SGUnit entity)
        {
            bool added = false;
            if(Adjacents[orientation] == null)
            {
                added = true;
                Adjacents[orientation] = entity;
            }
            return added;
        }

        public bool RemoveAdjacent(string orientation, SGUnit entity)
        {
            bool removed = false;

            if (Adjacents[orientation] != null)
            {
                removed = true;
                Adjacents[orientation] = null;
            }
            return removed;
        }

        public bool Scan()
        {
            return false;
        }
    }
}
