using Microsoft.Xna.Framework;
using StardewValley;


namespace ItemPipes.Framework.Nodes.ObjectNodes
{
    public class ExtractorPipeNode : OutputPipeNode
    {
        public ExtractorPipeNode() { }
        public ExtractorPipeNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            Tier = 1;
            ItemTimer = 500;
            Flux = 1;
        }
    }
}
