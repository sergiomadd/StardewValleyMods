using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;

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
