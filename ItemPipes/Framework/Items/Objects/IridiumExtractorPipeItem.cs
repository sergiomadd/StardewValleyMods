using Microsoft.Xna.Framework;
using System.Xml.Serialization;

namespace ItemPipes.Framework.Items.Objects
{
    [XmlType("Mods_sergiomadd.ItemPipes_IridiumExtractorPipe")]
    public class IridiumExtractorPipeItem : OutputItem
    {

        public IridiumExtractorPipeItem() : base()
        {
            Name = "Iridium Extractor Pipe";
            IDName = "IridiumExtractorPipe";
            Description = "Type: Output Pipe\nExtracts items from an adjacent container, and sends them through the network.";
            LoadTextures();
        }

        public IridiumExtractorPipeItem(Vector2 position) : base(position)
        {
            Name = "Iridium Extractor Pipe";
            IDName = "IridiumExtractorPipe";
            Description = "Type: Output Pipe\nExtracts items from an adjacent container, and sends them through the network.";
            LoadTextures();
        }
    }
}
