using Microsoft.Xna.Framework;
using System.Xml.Serialization;

namespace ItemPipes.Framework.Items.Objects
{
    public class ExtractorPipeItem : OutputPipeItem
    {
        public ExtractorPipeItem() : base()
        {
        }

        public ExtractorPipeItem(Vector2 position) : base(position)
        {
        }
    }
}
