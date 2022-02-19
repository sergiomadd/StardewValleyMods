using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;

namespace ItemPipes.Framework.Items
{
    public abstract class InputPipeItem : IOPipeItem
    {
        public InputPipeItem() : base()
        {

        }
        public InputPipeItem(Vector2 position) : base(position)
        {

        }
    }
}
