using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;
using ItemPipes.Framework.Util;
using ItemPipes.Framework.Nodes;
using StardewValley.Menus;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using SObject = StardewValley.Object;
using ItemPipes.Framework.Items.Objects;
using ItemPipes.Framework.Nodes.ObjectNodes;


namespace ItemPipes.Framework.Items.CustomFilter
{
    public class Filter
	{
        public NetObjectList<Item> items { get; set; }
        public string message { get; set; }
        public int Cols { get; set; }
        public int Rows { get; set; }
        public int Capacity { get; set; }
        public bool Quality { get; set; }
        public Dictionary<OptionsElement, string> Options { get; set; }
        public FilterPipeItem FilterPipe { get; set; }
        public Filter()
        {

        }
        public Filter(int capacity, FilterPipeItem filterPipe)
        {
            items = new NetObjectList<Item>();
            message = "Filter";
            Capacity = capacity;
            //Generate cols and rows based on capacity
            Cols = 9;
            Rows = 1;
            Quality = true;
            Options = new Dictionary<OptionsElement, string>();
            FilterPipe = filterPipe;
        }

        public void UpdateOption(OptionsElement elem)
        {
            string changed = Options[elem];
            (FilterPipe.GetNode() as FilterPipeNode).Filter.UpdateOption(changed);
            Printer.Info(changed);
        }

        public void ShowMenu()
        {
			Game1.activeClickableMenu = new FilterItemGrabMenu(this, items, Capacity, reverseGrab: false, showReceivingMenu: true, InventoryMenu.highlightAllItems,
                grabItemFromInventory, message, grabItemFromChest, snapToBottom: true, canBeExitedWithKey: true, playRightClickSound: true,
                allowRightClick: false, showOrganizeButton: false, 1, null, -80, this);
		}

        public void grabItemFromInventory(Item item, Farmer who)
        {
            if(CanAddItem(item))
            {
                addItem(item);
                clearNulls();
            }
        }

        public bool CanAddItem(Item item)
        {
            bool can = true;
            string category = item.getCategoryName();
            if (item is not PipeItem && !Utilities.IsVanillaItem(item))
            {
                can = false;
                Utilities.ShowInGameMessage($"Non vanilla items [{item.Name}] are not allowed in filter pipes!", "error");
                Printer.Debug($"Attempted to place a non vanilla item [{item.Name}] in a filter pipe. Non vanilla items are not allowed in filter pipes!");
            }
            else if (category.Equals("Tool"))
            {
                can = false;
                Utilities.ShowInGameMessage($"Tools [{item.Name}] are not allowed in filter pipes!", "error");
                Printer.Debug($"Attempted to place a tool [{item.Name}] in a filter pipe. Tools are not allowed in filter pipes!");
            }
            else if(category.Equals("Weapon"))
            {
                can = false;
                Utilities.ShowInGameMessage($"Weapons [{item.Name}] are not allowed in filter pipes!", "error");
                Printer.Debug($"Attempted to place a weapon [{item.Name}] in a filter pipe. Weapons are not allowed in filter pipes!");
            }
            else if(category.Equals("Cooking") || category.Equals("Crafting"))
            {
                can = false;
                Utilities.ShowInGameMessage($"Recipes [{item.Name}] are not allowed in filter pipes!", "error");
                Printer.Debug($"Attempted to place a recipe [{item.Name}] in a filter pipe. Recipes are not allowed in filter pipes!");
            }
            else
            {
                can = false;
                if(items.Count < Capacity)
                {
                    if (Quality)
                    {
                        if(item is SObject)
                        {
                            if (!items.Any(i => i.Name.Equals(item.Name) && (i as SObject).Quality.Equals((item as SObject).Quality)))
                            {
                                can = true;
                            }
                            else
                            {
                                if((item as SObject).Quality > 0)
                                {
                                    Utilities.ShowInGameMessage($"{item.Name} of that quality is already in the filter!", "error");
                                    Printer.Debug($"Attempted to place {item.Name} in a filter pipe. {item.Name} of that quality is already in the filter!!");
                                }
                                else
                                {
                                    Utilities.ShowInGameMessage($"{item.Name} is already in the filter!", "error");
                                    Printer.Debug($"Attempted to place {item.Name} in a filter pipe. {item.Name} of that quality is already in the filter!!");
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!items.Any(i => i.Name.Equals(item.Name)))
                        {
                            can = true;
                        }
                        else
                        {
                            Utilities.ShowInGameMessage($"{item.Name} is already in the filter!", "error");
                            Printer.Debug($"Attempted to place {item.Name} in a filter pipe. {item.Name} is already in the filter!!");
                        }
                    }
                }
            }
            return can;
        }

        public void grabItemFromChest(Item item, Farmer who)
        {
            items.Remove(item);
            clearNulls();
            ShowMenu();
        }

        public Item addItem(Item item)
        {
            item.resetState();
            this.clearNulls();
            items.Add(item.getOne());
            return item;
        }

        public void clearNulls()
        {
            for (int j = this.items.Count - 1; j >= 0; j--)
            {
                if (this.items[j] == null)
                {
                    this.items.RemoveAt(j);
                }
            }
        }

        public int GetActualCapacity()
        {
            return items.Capacity;
        }
    }
}
