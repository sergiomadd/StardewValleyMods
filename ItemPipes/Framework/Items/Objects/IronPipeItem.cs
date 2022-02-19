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
    [XmlType("Mods_sergiomadd.ItemPipes_IronPipeItem")]
    public class IronPipeItem : ConnectorPipeItem
    {
        public IronPipeItem() : base()
        {
            Name = "Iron Pipe";
            IDName = "IronPipe";
            Description = "Type: Connector Pipe\nThe link between IO pipes. It moves items at 2 tiles/1 second.";
            base.LoadTextures();
        }

        public IronPipeItem(Vector2 position) : base(position)
        {
            Name = "Iron Pipe";
            IDName = "IronPipe";
            Description = "Type: Connector Pipe\nThe link between IO pipes. It moves items at 2 tiles/1 second.";
            base.LoadTextures();
        }
    }
}
