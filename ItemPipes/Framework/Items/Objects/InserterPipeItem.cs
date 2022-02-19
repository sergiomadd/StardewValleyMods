using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using StardewValley;

namespace ItemPipes.Framework.Items.Objects
{
    [XmlType("Mods_sergiomadd.ItemPipes_InserterPipeItem")]

    public class InserterPipeItem : InputPipeItem
    {
        public InserterPipeItem() : base()
        {
            Name = "Inserter Pipe";
            IDName = "InserterPipe";
            Description = "Type: Input Pipe\nInserts items into an adjacent container, it doesn't filter items.";
            LoadTextures();
        }
        public InserterPipeItem(Vector2 position) : base(position)
        {
            Name = "Inserter Pipe";
            IDName = "InserterPipe";
            Description = "Type: Input Pipe\nInserts items into an adjacent container, it doesn't filter items.";
            LoadTextures();
        }
    }
}
