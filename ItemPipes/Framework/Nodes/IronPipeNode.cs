using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemPipes.Framework.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;
using StardewValley;

namespace ItemPipes.Framework.Nodes
{
    public class IronPipeNode : ConnectorNode
    {
        public IronPipeNode() { }
        public IronPipeNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
        }
    }
}
