using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemLogistics.Framework.Model;
using Microsoft.Xna.Framework;
using StardewValley;

namespace ItemLogistics.Framework
{
    class ExtractorPipe : Output
    {
        public ExtractorPipe(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {

        }
    }
}
