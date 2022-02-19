using Microsoft.Xna.Framework;
using StardewValley;

namespace ItemPipes.Framework.Nodes.ObjectNodes
{
    public class GoldExtractorPipeNode : OutputPipeNode
    {
        public GoldExtractorPipeNode() 
        {

        }
        public GoldExtractorPipeNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            Tier = 2;
        }
    }
}
