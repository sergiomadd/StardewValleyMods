using Microsoft.Xna.Framework;
using StardewValley;


namespace ItemPipes.Framework.Nodes.ObjectNodes
{
    public class ExtractorPipeNode : OutputNode
    {
        public ExtractorPipeNode() { }
        public ExtractorPipeNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            Tier = 1;
        }
    }
}
