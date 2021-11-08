using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;

namespace ItemLogistics.Framework.Model
{
    public static class SGNodeFactory
    {
        //private static SGNodeFactory mySGNodeFactory;
        public static List<string> ValidItemNames { get; set; }
        /*private SGNodeFactory()
        {
            ValidItemNames = SGraphDB.GetSGraphDB().ValidItemNames;
        }

        public static SGNodeFactory GetSGNodeFactory()
        {
            if (mySGNodeFactory == null)
            {
                mySGNodeFactory = new SGNodeFactory();
            }
            return mySGNodeFactory;
        }*/

        public static SGNode CreateElement(Vector2 position, GameLocation location, StardewValley.Object obj)
        {
            if(obj.name.Equals("Extractor Pipe"))
            {
                return new OutPipe(position, location, obj);
            }
            else if (obj.name.Equals("Inserter Pipe"))
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
