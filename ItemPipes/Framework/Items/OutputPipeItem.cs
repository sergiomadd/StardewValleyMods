using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;

namespace ItemPipes.Framework.Items
{
    public abstract class OutputPipeItem : IOPipeItem
    {
        public OutputPipeItem() : base()
        {

        }

        public OutputPipeItem(Vector2 position) : base(position)
        {

        }
    }
}
