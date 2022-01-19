using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemPipes.Framework.Model;
using ItemPipes.Framework.Nodes;
using ItemPipes.Framework.Util;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;


namespace ItemPipes.Framework.Nodes
{
    public class InvisibilizerNode : Node
    {
        public InvisibilizerNode() { }
        public InvisibilizerNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
        }
    }
}
