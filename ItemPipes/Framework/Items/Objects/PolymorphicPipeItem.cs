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

    public class PolymorphicPipeItem : InputPipeItem
    {
        public PolymorphicPipeItem() : base()
        {
            Init();
        }
        public PolymorphicPipeItem(Vector2 position) : base(position)
        {
            Init();
        }
    }
}
