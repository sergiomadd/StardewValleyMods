﻿using Microsoft.Xna.Framework;
using System.Xml.Serialization;

namespace ItemPipes.Framework.Items.Objects
{
    [XmlType("Mods_sergiomadd.ItemPipes_GoldExtractorPipe")]
    public class GoldExtractorPipeItem : OutputItem
    {

        public GoldExtractorPipeItem() : base()
        {
            Name = "Gold Extractor Pipe";
            IDName = "GoldExtractorPipe";
            Description = "Type: Output Pipe\nExtracts items from an adjacent container, and sends them through the network.";
            LoadTextures();
        }

        public GoldExtractorPipeItem(Vector2 position) : base(position)
        {
            Name = "Gold Extractor Pipe";
            IDName = "GoldExtractorPipe";
            Description = "Type: Output Pipe\nExtracts items from an adjacent container, and sends them through the network.";
            LoadTextures();
        }
    }
}