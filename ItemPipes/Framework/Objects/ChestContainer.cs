using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;
using ItemPipes.Framework.Model;
using Netcode;


namespace ItemPipes.Framework.Objects
{
    public class ChestContainer : Container
    {
        public Chest Chest { get; set; }
        public ChestContainer(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            if (obj is Chest)
            {
                Chest = (Chest)obj;
            }
            Type = "Chest";
        }

        public Item GetItemToShip(Input input)
        {
            Item item = null;
            if (!IsEmpty() && input != null)
            {
                NetObjectList<Item> itemList = GetItemList();
                int index = itemList.Count - 1;
                while (index >= 0 && item == null)
                {
                    if (input.HasFilter())
                    {
                        if (input.Filter.Contains(itemList[index].Name))
                        {
                            item = itemList[index];
                            itemList.RemoveAt(index);
                            Chest.clearNulls();
                        }
                    }
                    else
                    {
                        item = itemList[index];
                        itemList.RemoveAt(index);
                        Chest.clearNulls();
                    }
                    index--;
                }
            }
            return item;
        }

        public Item TrySendItem(ChestContainer input, NetObjectList<Item> itemList, int index)
        {
            Item item = null;
            if (input.CanStackItem(item))
            {
                item = itemList[index];
                itemList.RemoveAt(index);
                Chest.clearNulls();
            }
            else if (input.CanReceiveItems())
            {
                item = itemList[index];
                itemList.RemoveAt(index);
                Chest.clearNulls();
            }
            return item;
        }

        public Item CanSendItem(ChestContainer input)
        {
            Item item = null;
            if (!IsEmpty() && input != null)
            {
                NetObjectList<Item> itemList = GetItemList();
                int index = itemList.Count - 1;
                while (index >= 0 && item == null)
                {
                    Printer.Info(itemList[index].Name);
                    if (input.HasFilter())
                    {
                        Printer.Info("It has filter" + input.Filter.Count.ToString());
                        if (input.Filter.Contains(itemList[index].Name))
                        {
                            item = TrySendItem(input, itemList, index);
                        }
                    }
                    else
                    {
                        item = TrySendItem(input, itemList, index);
                    }
                    index--;
                }
            }
            return item;
        }

        public bool SendItem(ChestContainer input, Item item)
        {
            bool sent = false;
            if (!IsEmpty() && input != null && input.HasFilter())
            {
                //Printer.Info("FILTERED");
                if (input.Filter.Contains(item.Name))
                {
                    //Printer.Info($"SENDING FILTERED: {item.Name}");
                    if (input.CanStackItem(item))
                    {
                        //Printer.Info("Stacking");
                        input.ReceiveStack(item);
                        sent = true;
                    }
                    else if (input.CanReceiveItems())
                    {
                        //Printer.Info("New Stack");
                        input.ReceiveItem(item);
                        sent = true;
                    }
                }
                else
                {
                    //Printer.Info("RETURNING ITEM");
                    if (CanStackItem(item))
                    {
                        //Printer.Info("Stacking");
                        ReceiveStack(item);
                        sent = false;
                    }
                    else if (CanReceiveItems())
                    {
                        //Printer.Info("New Stack");
                        ReceiveItem(item);
                        sent = false;
                    }
                    else
                    {
                        //Drop item
                        //Game1.currentLocation.dropObject(item);
                        sent = false;
                    }
                }

            }
            else
            {
                //Printer.Info($"SENDING: {item.Name}");
                if (input.CanStackItem(item))
                {
                    //Printer.Info("Stacking");
                    input.ReceiveStack(item);
                    sent = true;
                }
                else if (input.CanReceiveItems())
                {
                    //Printer.Info("New Stack");
                    input.ReceiveItem(item);
                    sent = true;
                }
                else
                {
                    if (CanStackItem(item))
                    {
                        //Printer.Info("Stacking");
                        ReceiveStack(item);
                        sent = false;
                    }
                    else if (CanReceiveItems())
                    {
                        //Printer.Info("New Stack");
                        ReceiveItem(item);
                        sent = false;
                    }
                    //If returning chest is full
                    else
                    {
                        //Drop item
                        //Game1.currentLocation.dropObject(item);
                        sent = false;
                    }
                }

            }
            Printer.Info("Item sent? " + sent.ToString());
            return sent;
        }

        //See if any stacks isnt full
        public bool CanStack()
        {
            bool canStack = false;
            NetObjectList<Item> itemList = GetItemList();
            int index = itemList.Count - 1;
            while (index >= 0 && !canStack)
            {
                if (itemList[index] != null)
                {
                    //Printer.Info(itemList[index].getRemainingStackSpace().ToString());
                    if (itemList[index].getRemainingStackSpace() > 0)
                    {
                        canStack = true;
                    }
                }
                index--;
            }

            return canStack;
        }
        //See if an especific item can stack with another
        public bool CanStackItem(Item item)
        {
            bool canStack = false;
            NetObjectList<Item> itemList = GetItemList();
            if (itemList.Contains(item))
            {
                int index = itemList.IndexOf(item);
                if (itemList[index].canStackWith(item))
                {
                    canStack = true;
                }
            }
            return canStack;
        }

        //See if any slot if free
        public bool CanReceiveItems()
        {
            bool canReceive = false;
            NetObjectList<Item> itemList = GetItemList();
            if (itemList.Count < Chest.GetActualCapacity())
            {
                canReceive = true;
            }
            return canReceive;
        }
        public void ReceiveStack(Item item)
        {
            Chest.addToStack(item);
        }

        public void ReceiveItem(Item item)
        {
            Chest.addItem(item);
        }

        public override List<string> UpdateFilter(NetObjectList<Item> filteredItems)
        {
            Filter = new List<string>();
            if (filteredItems == null)
            {
                NetObjectList<Item> itemList = GetItemList();
                foreach (Item item in itemList.ToList())
                {
                    //Printer.Info("NAME:" + item.Name);
                    Filter.Add(item.Name);
                }
            }
            else
            {
                foreach (Item item in filteredItems.ToList())
                {
                    //Printer.Info("NAME:" + item.Name);
                    Filter.Add(item.Name);
                }
            }
            return Filter;

        }
        public bool HasFilter()
        {
            bool hasFilter = false;
            if (Filter.Count > 0)
            {
                hasFilter = true;
            }
            return hasFilter;
        }

        public override bool IsEmpty()
        {
            bool isEmpty = false;
            NetObjectList<Item> itemList = GetItemList();
            if (itemList.Count < 1)
            {
                isEmpty = true;
            }
            return isEmpty;
        }

        public NetObjectList<Item> GetItemList()
        {
            NetObjectList<Item> itemList;
            if (Chest.SpecialChestType == Chest.SpecialChestTypes.MiniShippingBin || Chest.SpecialChestType == Chest.SpecialChestTypes.JunimoChest)
            {
                itemList = Chest.GetItemsForPlayer(Game1.player.UniqueMultiplayerID);
            }
            else
            {
                itemList = Chest.items;
            }
            return itemList;
        }
    }
}
