using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemPipes.Framework.Model;
using Microsoft.Xna.Framework;
using StardewValley;

namespace ItemPipes.Framework.Objects
{
    class ConnectorPipe : Connector
    {
        public ConnectorPipe(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {

        }
    }
}
