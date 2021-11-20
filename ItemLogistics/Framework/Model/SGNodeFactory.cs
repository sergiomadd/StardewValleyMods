﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;

namespace ItemLogistics.Framework.Model
{
    public static class SGNodeFactory
    {
        public static SGNode CreateElement(Vector2 position, GameLocation location, StardewValley.Object obj)
        {
            if (obj.name.Equals("Extractor Pipe"))
            {
                return new ExtractorPipe(position, location, obj);
            }
            else if (obj.name.Equals("Inserter Pipe"))
            {
                return new InserterPipe(position, location, obj);
            }
            else if (obj.name.Equals("Polymorphic Pipe"))
            {
                return new PolymorphicPipe(position, location, obj);
            }
            else if (obj.name.Equals("Filter Pipe"))
            {
                return new FilterPipe(position, location, obj);
            }
            else if (obj.name.Equals("Connector Pipe"))
            {
                return new Pipe(position, location, obj);
            }
            else if (obj.name.Equals("Chest"))
            {
                return new Container(position, location, obj);
            }
            else
            {
                return null;
            }
        }
    }
}
