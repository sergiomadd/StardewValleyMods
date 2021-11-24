using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;
using ItemLogistics.Framework;
using ItemLogistics.Framework.Model;

namespace ItemLogistics.Framework.Objects
{
    class FilterPipe : Input
    {
        public Chest Filter { get; set; }
        public FilterPipe(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            ConnectedContainer = null;
            Filter = new Chest(true, position, 130);
            Priority = 3;
        }
        public override bool AddAdjacent(Side side, Node entity)
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
                        UpdateFilter();
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
        public void UpdateFilter()
        {
            ConnectedContainer.UpdateFilter(Filter.items);
        }


    }
}
