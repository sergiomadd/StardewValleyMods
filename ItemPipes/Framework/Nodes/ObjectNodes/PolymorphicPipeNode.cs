﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;
using ItemPipes.Framework;
using ItemPipes.Framework.Model;

namespace ItemPipes.Framework.Nodes.ObjectNodes
{
    public class PolymorphicPipeNode : InputPipeNode
    {
        public PolymorphicPipeNode() : base()
        {

        }
        public PolymorphicPipeNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            ConnectedContainer = null;
            Priority = 2;
            ItemTimer = 1000;
        }

        public override void UpdateFilter()
        {
            if(ConnectedContainer != null)
            {
                Filter = ConnectedContainer.UpdateFilter(null);
            }
        }
    }
}
