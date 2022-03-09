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

namespace ItemPipes.Framework.Items.Objects
{
    public class GoldPipeItem : ConnectorPipeItem
    {
        public GoldPipeItem() : base()
        {
        }

        public GoldPipeItem(Vector2 position) : base(position)
        {
        }
    }
}
