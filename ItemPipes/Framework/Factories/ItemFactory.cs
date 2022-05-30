using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Buildings;
using ItemPipes.Framework.Items;
using ItemPipes.Framework.Util;
using SObject = StardewValley.Object;
using ItemPipes.Framework.Items.Objects;
using ItemPipes.Framework.Items.Tools;



namespace ItemPipes.Framework.Factories
{
    public static class ItemFactory
    {
        public static CustomObjectItem CreateItem(string name)
        {
            if (name.Equals("ExtractorPipe"))
            {
                return new ExtractorPipeItem();
            }
            else if (name.Equals("GoldExtractorPipe"))
            {
                return new GoldExtractorPipeItem();
            }
            else if (name.Equals("IridiumExtractorPipe"))
            {
                return new IridiumExtractorPipeItem();
            }
            else if (name.Equals("InserterPipe"))
            {
                return new InserterPipeItem();
            }
            else if (name.Equals("PolymorphicPipe"))
            {
                return new PolymorphicPipeItem();
            }
            else if (name.Equals("FilterPipe"))
            {
                return new FilterPipeItem();
            }
            else if (name.Equals("IronPipe"))
            {
                return new IronPipeItem();
            }
            else if (name.Equals("GoldPipe"))
            {
                return new GoldPipeItem();
            }
            else if (name.Equals("IridiumPipe"))
            {
                return new IridiumPipeItem();
            }
            else if (name.Equals("PIPO"))
            {
                return new PIPOItem();
            }
            else
            {
                Printer.Info($"Item creation for {name} failed.");
                return null;
            }
        }

        public static CustomToolItem CreateTool(string name)
        {
            if (name.Equals("Wrench"))
            {
                return new WrenchItem();
            }
            else
            {
                Printer.Info($"Item creation for {name} failed.");
                return null;
            }
        }

        public static CustomObjectItem CreateObject(Vector2 position, string name)
        {
            if (name.Equals("ExtractorPipe"))
            {
                return new ExtractorPipeItem(position);
            }
            else if (name.Equals("GoldExtractorPipe"))
            {
                return new GoldExtractorPipeItem(position);
            }
            else if (name.Equals("IridiumExtractorPipe"))
            {
                return new IridiumExtractorPipeItem(position);
            }
            else if (name.Equals("InserterPipe"))
            {
                return new InserterPipeItem(position);
            }
            else if (name.Equals("PolymorphicPipe"))
            {
                return new PolymorphicPipeItem(position);
            }
            else if (name.Equals("FilterPipe"))
            {
                return new FilterPipeItem(position);
            }
            else if (name.Equals("IronPipe"))
            {
                return new IronPipeItem(position);
            }
            else if (name.Equals("GoldPipe"))
            {
                return new GoldPipeItem(position);
            }
            else if (name.Equals("IridiumPipe"))
            {
                return new IridiumPipeItem(position);
            }
            else if (name.Equals("PIPO"))
            {
                return new PIPOItem(position);
            }
            else
            {
                Printer.Info($"Object creation for {name} failed.");
                return null;
            }
        }
    }
}
