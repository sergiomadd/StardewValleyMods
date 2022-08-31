using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using StardewValley;
using SObject = StardewValley.Object;
using Microsoft.Xna.Framework;
using StardewValley.Objects;
using StardewValley.Tools;
using ItemPipes.Framework.Factories;


namespace ItemPipes.Framework.Util
{
    public static class Utilities
    {
        public static string GetNetworkLegend()
        {
            StringBuilder legend = new StringBuilder();

            legend.AppendLine("Legend: ");
            legend.AppendLine("C: Connector Pipe");
            legend.AppendLine("O: Output Pipe");
            legend.AppendLine("I: Input Pipe");
            legend.AppendLine("P: PIPO");

            return legend.ToString();
        }


        public static void DropItem(Item item, Vector2 position, GameLocation location)
        {
            Vector2 convertedPosition = new Vector2(position.X * 64, position.Y * 64);
            Debris itemDebr = new Debris(item, convertedPosition);
            location.debris.Add(itemDebr);
        }

        public static string GetIDName(string name)
        {
            string trimmed = "";
            if (name.Equals("PIPO"))
            {
                trimmed = name.ToLower();
            }
            else
            {
                trimmed = String.Concat(name.Where(c => !Char.IsWhiteSpace(c))).ToLower();
            }
            return trimmed;
        }

        public static string GetIDNameFromType(Type type)
        {
            string name = type.Name;
            string trimmed = name.Substring(0, name.Length - 4).ToLower();
            return trimmed;
        }

        
        public static string GetIndexFromItem(Item item)
        {
            string index = "";
            string idTag = item.GetContextTagList()[0];
            string type = idTag.Split("_")[1];
            string tileSheetId = idTag.Split("_")[2];
            if (item is PipeItem)
            {
                type = "ip";
                tileSheetId = (item as PipeItem).ParentSheetIndex.ToString();
            }
            //no compara entre sub tipos. Tomato juice -> juice al crear el obj onLoad
            if(item is SObject)
            {
                index += type + "-" + tileSheetId+"-"+(item as SObject).Quality.ToString();
            }
            else
            {
                index += type + "-" + tileSheetId;
            }
            return index;
        }
        
        public static Item GetItemFromIndex(string index)
        {
            string type = index.Split("-")[0];
            int tileSheetId = 1;
            if (index.Split("-")[1] != "")
            {
                tileSheetId = Int32.Parse(index.Split("-")[1]);
            }
            Item item = null;
			switch(type)
            {
				case "b"://boots
                    item = new Boots(tileSheetId);
					break;
                case "bbl"://big craftable recipe TODO
                    break;
                case "bl"://object recipe TODO
                    break;
                case "bo"://big craftable
                    item = new SObject(Vector2.Zero, tileSheetId, false);
                    break;
                case "c"://clothing
                    item = new Clothing(tileSheetId);
                    break;
                case "f"://furniture
                    item = new Furniture(tileSheetId, Vector2.Zero);
                    break;
                case "h"://hat
                    item = new Hat(tileSheetId);
                    break;
                case "o"://object
                    item = new SObject(Vector2.Zero, tileSheetId, 1);
                    if (index.Split("-")[2] != "")
                    {
                        (item as SObject).Quality = Int32.Parse(index.Split("-")[2]);
                    }
                    break;
                case "r"://ring
                    item = new Ring(tileSheetId);
                    break;
                case "w"://melee weapon
                    item = new MeleeWeapon(tileSheetId);
                    break;
                case "ip"://item pipe
                    item = ItemFactory.CreateItemFromID(tileSheetId);
                    break;
                default:
                    Printer.Warn("Item type not supported in fitlers");
                    break;
            }
            return item;
		}

        public static void ShowInGameMessage(string message, string type)
        {
            int numType = 0;
            switch(type)
            {
                case "achievement":
                    numType = 1;
                    break;
                case "quest":
                    numType = 2;
                    break;
                case "error":
                    numType = 3;
                    break;
                case "stamina":
                    numType = 4;
                    break;
                case "health":
                    numType = 5;
                    break;
                case "screenshot":
                    numType = 6;
                    break;
            }
            Game1.addHUDMessage(new HUDMessage(message, numType));
        }

        //Needs more checks
        public static bool IsVanillaItem(Item item)
        {
            DataAccess data = DataAccess.GetDataAccess();
            bool itis = false;
            string idTag = item.GetContextTagList()[0];
            string type = idTag.Split("_")[1];
            int id = Int32.Parse(idTag.Split("_")[2]);
            if (item is PipeItem)
            {
                type = "ip";
                id = (item as PipeItem).ParentSheetIndex;
            }
            Printer.Info(item.getCategoryName());
            if(type == "")
            {
                type = item.getCategoryName();
            }
            switch (type)
            {
                case "b"://boots
                    if(data.VanillaBoots.Contains(id))
                    {
                        itis = true;
                    }
                    break;
                case "bbl"://big craftable recipe TODO
                    break;
                case "bl"://object recipe TODO
                    break;
                case "bo"://big craftable
                    if (data.VanillaBigCraftables.Contains(id))
                    {
                        itis = true;
                    }
                    break;
                case "c"://clothing
                    if (data.VanillaClothing.Contains(id))
                    {
                        itis = true;
                    }
                    break;
                case "f"://furniture
                    if (data.VanillaFurniture.Contains(id))
                    {
                        itis = true;
                    }
                    break;
                case "h"://hat
                    if (data.VanillaHats.Contains(id))
                    {
                        itis = true;
                    }
                    break;
                case "o"://object
                    if (data.VanillaObjects.Contains(id))
                    {
                        itis = true;
                    }
                    break;
                case "r"://ring
                    if (data.VanillaObjects.Contains(id))
                    {
                        itis = true;
                    }
                    break;
                case "w"://melee weapon
                    if (data.VanillaWeapons.Contains(id))
                    {
                        itis = true;
                    }
                    break;
                case "Tool"://tool
                    id = (item as Tool).InitialParentTileIndex;
                    if (data.VanillaTools.Contains(id))
                    {
                        itis = true;
                    }
                    break;
            }
            return itis;
        }
    }
}
