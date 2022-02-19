using Microsoft.Xna.Framework;
using System.Xml.Serialization;

namespace ItemPipes.Framework.Items.Objects
{
    [XmlType("Mods_sergiomadd.ItemPipes_ExtractorPipe")]
    public class ExtractorPipeItem : OutputPipeItem
    {

        public ExtractorPipeItem() : base()
        {
            Name = "Extractor Pipe";
            IDName = "ExtractorPipe";
            Description = "Type: Output Pipe\nExtracts items from an adjacent container, and sends them through the network.";
            LoadTextures();
        }

        public ExtractorPipeItem(Vector2 position) : base(position)
        {
            Name = "Extractor Pipe";
            IDName = "ExtractorPipe";
            Description = "Type: Output Pipe\nExtracts items from an adjacent container, and sends them through the network.";
            LoadTextures();
        }


    }
}
