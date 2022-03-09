using Microsoft.Xna.Framework;
using StardewValley;

namespace ItemPipes.Framework.Nodes.ObjectNodes
{
    public class IridiumExtractorPipeNode : OutputPipeNode
    {
        public IridiumExtractorPipeNode()
        {

        }
        public IridiumExtractorPipeNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            Tier = 3;
            ItemTimer = 150;
            Flux = 50;
        }
    }
}
