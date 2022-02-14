﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemPipes.Framework.Model;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Buildings;
using Netcode;

namespace ItemPipes.Framework
{
    public abstract class InputNode : IOPipeNode
    {
        public NetObjectList<Item> Filter { get; set; }
        public int Priority { get; set; }

        public InputNode() : base()
        {

        }

        public InputNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            ConnectedContainer = null;
            Filter = new NetObjectList<Item>();
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
