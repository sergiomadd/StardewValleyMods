﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemLogistics.Framework.Model;
using Microsoft.Xna.Framework;
using StardewValley;

namespace ItemLogistics.Framework
{
    public class Connector : Node
    {
        public bool PassingItem { get; set; }
        public Connector(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            PassingItem = false;
        }

    }
}