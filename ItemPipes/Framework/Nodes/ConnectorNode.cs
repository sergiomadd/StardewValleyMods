using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemPipes.Framework.Model;
using Microsoft.Xna.Framework;
using StardewValley;

namespace ItemPipes.Framework
{
    public abstract class ConnectorNode : Node
    {
        public bool PassingItem { get; set; }
        public bool Connecting { get; set; }

        public ConnectorNode() : base()
        {

        }
        public ConnectorNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            PassingItem = false;
            Connecting = false;
        }

        public override string GetState()
        {
            if(PassingItem)
            {
                return "item";
            }
            else if (Connecting)
            {
                return "connecting";
            }
            else
            {
                return "default";
            }

        }

    }
}
