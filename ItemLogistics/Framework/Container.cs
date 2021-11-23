using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;
using ItemLogistics.Framework.Model;
using Netcode;

namespace ItemLogistics.Framework
{
    public class Container : Node
    {
        public Chest Chest { get; set; }
        public List<string> Filter { get; set; }
        public Container(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            if(obj is Chest)
            {
                Chest = (Chest)obj;
            }
            Filter = new List<string>();
        }

        public Item CanSendItem(Container input)
        {
            Item item = null;
            if (!IsEmpty() && input != null)
            {
                NetObjectList<Item> sourceItemList;
                if (Chest.SpecialChestType == Chest.SpecialChestTypes.MiniShippingBin || Chest.SpecialChestType == Chest.SpecialChestTypes.JunimoChest)
                {
                    Printer.Info("JUMINO");
                    sourceItemList = Chest.GetItemsForPlayer(Game1.player.UniqueMultiplayerID);
                }
                else
                {
                    sourceItemList = Chest.items;
                }
                int index = sourceItemList.Count - 1;
                while (index >= 0 && item == null)
                {
                    Printer.Info(sourceItemList[index].Name);
                    if (input.HasFilter())
                    {
                        if (input.Filter.Contains(item.Name))
                        {
                            if(input.CanStackItem(sourceItemList[index]))
                            {
                                item = sourceItemList[index];
                                sourceItemList.RemoveAt(index);
                                Chest.clearNulls();
                            }
                            else if (input.CanReceiveItems())
                            {
                                item = sourceItemList[index];
                                sourceItemList.RemoveAt(index);
                                Chest.clearNulls();
                            }
                        }
                    }
                    else
                    {
                        if (input.CanStackItem(sourceItemList[index]))
                        {
                            item = sourceItemList[index];
                            sourceItemList.RemoveAt(index);
                            Chest.clearNulls();
                        }
                        else if (input.CanReceiveItems())
                        {
                            item = sourceItemList[index];
                            sourceItemList.RemoveAt(index);
                            Chest.clearNulls();
                        }
                    }
                    index--;
                }
            }
            return item;
        }

        public bool SendItem(Container input, Item item)
        {
            bool sent = false;
            if (!IsEmpty() && input != null && input.HasFilter())
            {
                Printer.Info("FILTERED");
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
                    Printer.Info("RETURNING ITEM");
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
                }

            }
            else
            {
                Printer.Info($"SENDING: {item.Name}");
                if (input.CanStackItem(item))
                {
                    Printer.Info("Stacking");
                    input.ReceiveStack(item);
                    sent = true;
                }
                else if (input.CanReceiveItems())
                {
                    Printer.Info("New Stack");
                    input.ReceiveItem(item);
                    sent = true;
                }
                else
                {
                    if (CanStackItem(item))
                    {
                        Printer.Info("Stacking");
                        ReceiveStack(item);
                        sent = false;
                    }
                    else if (CanReceiveItems())
                    {
                        Printer.Info("New Stack");
                        ReceiveItem(item);
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
            NetObjectList<Item> itemList;
            if (Chest.SpecialChestType == Chest.SpecialChestTypes.MiniShippingBin || Chest.SpecialChestType == Chest.SpecialChestTypes.JunimoChest)
            {
                itemList = Chest.GetItemsForPlayer(Game1.player.UniqueMultiplayerID);
            }
            else
            {
                itemList = Chest.items;
            }

            int index = itemList.Count - 1;
            while (index >= 0 && !canStack)
            {
                if (itemList[index] != null)
                {
                    Printer.Info(itemList[index].getRemainingStackSpace().ToString());
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
            NetObjectList<Item> itemList;
            if (Chest.SpecialChestType == Chest.SpecialChestTypes.MiniShippingBin || Chest.SpecialChestType == Chest.SpecialChestTypes.JunimoChest)
            {
                itemList = Chest.GetItemsForPlayer(Game1.player.UniqueMultiplayerID);
            }
            else
            {
                itemList = Chest.items;
            }
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
            NetObjectList<Item> itemList;
            if (Chest.SpecialChestType == Chest.SpecialChestTypes.MiniShippingBin || Chest.SpecialChestType == Chest.SpecialChestTypes.JunimoChest)
            {
                itemList = Chest.GetItemsForPlayer(Game1.player.UniqueMultiplayerID);
            }
            else
            {
                itemList = Chest.items;
            }
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
            Printer.Info("Receiving item");
            Chest.addItem(item);
        }

        public void UpdateFilter()
        {
            Printer.Info($"Filter items: {Chest.items.Count}");
            Filter = new List<string>();
            foreach (Item item in Chest.items.ToList())
            {
                Filter.Add(item.Name);
            }
        }
        public bool HasFilter()
        {
            bool hasFilter = false;
            if (Filter.Count > 0)
            {
                hasFilter = true;
            }
            return hasFilter;
            /*bool hasFilter = false;
            if (Filter != null)
            {
                hasFilter = true;
            }
            return hasFilter;*/
        }

        public bool IsEmpty()
        {
            bool isEmpty = false;
            NetObjectList<Item> itemList;
            if (Chest.SpecialChestType == Chest.SpecialChestTypes.MiniShippingBin || Chest.SpecialChestType == Chest.SpecialChestTypes.JunimoChest)
            {
                itemList = Chest.GetItemsForPlayer(Game1.player.UniqueMultiplayerID);
            }
            else
            {
                itemList = Chest.items;
            }
            if (itemList.Count < 1)
            {
                isEmpty = true;
            }
            return isEmpty;
        }
    }
}
