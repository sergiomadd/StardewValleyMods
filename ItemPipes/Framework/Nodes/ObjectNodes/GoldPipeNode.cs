﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StardewValley;

namespace ItemPipes.Framework.Nodes.ObjectNodes
{
    public class GoldPipeNode : ConnectorPipeNode
    {
        public GoldPipeNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            ItemTimer = 500;
        }
    }
}
