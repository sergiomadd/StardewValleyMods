using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using StardewValley;

namespace ItemPipes.Framework.Items.Objects
{

    public class InserterPipeItem : InputPipeItem
    {
        public InserterPipeItem() : base()
        {
        }
        public InserterPipeItem(Vector2 position) : base(position)
        {
        }
    }
}
