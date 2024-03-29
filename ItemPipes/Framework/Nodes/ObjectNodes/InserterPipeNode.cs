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
    public class InserterPipeNode : InputPipeNode
    {
        public InserterPipeNode() : base()
        {

        }
        public InserterPipeNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            Filter = new FilterNode(false);
            Priority = 1;
            ItemTimer = 1000;
        }
    }
}
