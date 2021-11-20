﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;
using ItemLogistics.Framework;
using ItemLogistics.Framework.Model;

namespace ItemLogistics.Framework
{
    class InserterPipe : Input
    {
        public InserterPipe(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
        }
    }
}
