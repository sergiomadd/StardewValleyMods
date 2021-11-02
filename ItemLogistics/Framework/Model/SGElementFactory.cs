using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;

namespace ItemLogistics.Framework.Model
{
    class SGElementFactory
    {
        public List<string> LogisticItemNames { get; set; }
        public SGElementFactory(List<string> logisticItemNames)
        {
            LogisticItemNames = logisticItemNames;
        }

        public SGElement CreateElement(Vector2 position, GameLocation location, StardewValley.Object obj)
        {
            if(obj.name.Equals("Hardwood Fence"))
            {
                return new OutPipe(position, location, obj);
            }
            else if (obj.name.Equals("Wood Fence"))
            {
                return new InPipe(position, location, obj);
            }
            else
            {
                return new Pipe(position, location, obj);
            }
        }

}
}
