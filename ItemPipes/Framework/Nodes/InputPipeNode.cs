﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemPipes.Framework.Nodes.ObjectNodes;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Buildings;
using Netcode;

namespace ItemPipes.Framework
{
    public abstract class InputPipeNode : IOPipeNode
    {
        public FilterNode Filter { get; set; }
        public int Priority { get; set; }

        public InputPipeNode() : base()
        {

        }

        public InputPipeNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            ConnectedContainer = null;
        }

        public bool CanRecieveItems()
        {
            bool canReceive = false;
            if (ConnectedContainer.CanRecieveItems())
            {
                canReceive = true;
            }
            return canReceive;
        }

        public bool HasFilter()
        {
            bool hasFilter = false;
            if (Filter.Active)
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
