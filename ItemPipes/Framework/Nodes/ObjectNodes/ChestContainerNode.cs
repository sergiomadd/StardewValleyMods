﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;
using SObject = StardewValley.Object;
using ItemPipes.Framework.Util;
using Netcode;
using MaddUtil;
using System.Threading;


namespace ItemPipes.Framework.Nodes.ObjectNodes
{
    public class ChestContainerNode : ContainerNode
    {
        public Chest Chest { get; set; }
        public ChestContainerNode() { }
        public ChestContainerNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            if (obj is Chest)
            {
                Chest = (Chest)obj;
            }
            Type = "Chest";
        }

        public override bool CanSendItems()
        {
            bool canSend = false;
            if (!IsEmpty())
            {
                canSend = true;
            }
            return canSend;
        }

        public override bool CanRecieveItems()
        {
            bool canReceive = false;
            NetObjectList<Item> itemList = GetItemList();
            if (itemList.Count < Chest.GetActualCapacity())
            {
                canReceive = true;
            }
            return canReceive;
        }
        public override bool CanStackItems()
        {
            bool canStack = false;
            NetObjectList<Item> itemList = GetItemList();
            int index = itemList.Count - 1;
            while (index >= 0 && !canStack)
            {
                if (itemList[index] != null)
                {
                    if (itemList[index].getRemainingStackSpace() > 0)
                    {
                        canStack = true;
                    }
                }
                index--;
            }
            return canStack;
        }

        public override bool CanStackItem(Item item)
        {
            bool canStack = false;
            NetObjectList<Item> itemList = GetItemList();
            foreach (Item i in itemList.ToList())
            {
                if (i.canStackWith(item))
                {
                    canStack = true;
                }
            }
            return canStack;
        }

        public override bool CanRecieveItem(Item item)
        {
            bool canReceive = false;
            if (CanRecieveItems() || CanStackItem(item))
            {
                canReceive = true;
            }
            return canReceive;
        }

        public override bool InsertItem(Item item)
        {
            bool sent = false;
            if (CanRecieveItem(item))
            {
                RecieveItem(item);
                sent = true;
            }
            return sent;
        }

        public override Item GetItemForInput(InputPipeNode input, int flux)
        {
            Item item = null;
            if (input != null)
            {
                NetObjectList<Item> itemList = GetItemList();
                int index = itemList.Count - 1;
                while (index >= 0 && item == null)
                {
                    if (itemList[index] != null)
                    {
                        if (input.HasFilter())
                        {
                            if (input.Filter.IsItemOnFilter(itemList[index]))
                            {
                                item = TryExtractItem(input.ConnectedContainer, itemList, index, flux);
                            }
                        }
                        else
                        {
                            item = TryExtractItem(input.ConnectedContainer, itemList, index, flux);
                        }
                    }
                    index--;
                }
            }
            return item;
        }

        public Item TryExtractItem(ContainerNode input, NetObjectList<Item> itemList, int index, int flux)
        {
            Item source = itemList[index];
            Item tosend = null;
            if (source is SObject)
            {
                SObject obj = (SObject)source;
                SObject tosendObject = (SObject)tosend;
                if (input.CanRecieveItem(source) && !IsEmpty())
                {
                    if (obj.Stack <= flux)
                    {
                        tosendObject = obj;
                        itemList.RemoveAt(index);
                    }
                    else
                    {
                        obj.stack.Value -= flux;
                        tosendObject = (SObject)obj.getOne();
                        tosendObject.stack.Value = flux;
                    }
                    Chest.clearNulls();
                    return tosendObject;
                }
            }
            else if (source is Tool)
            {
                Tool tool = (Tool)source;
                Tool tosendTool = (Tool)tosend;
                if (input.CanRecieveItem(tool))
                {
                    tosendTool = tool;
                    itemList.RemoveAt(index);
                }
                Chest.clearNulls();
                return tosendTool;
            }
            return null;
        }

        public void ReceiveStack(Item item)
        {
            Chest.addToStack(item);
        }

        public void RecieveItem(Item item)
        {
            Chest.addItem(item);
        }

        public NetObjectList<Item> GetItemList()
        {
            NetObjectList<Item> itemList;
            if (Chest.SpecialChestType == Chest.SpecialChestTypes.MiniShippingBin || Chest.SpecialChestType == Chest.SpecialChestTypes.JunimoChest)
            {
                itemList = Chest.GetItemsForPlayer(Game1.MasterPlayer.UniqueMultiplayerID);
            }
            else
            {
                itemList = Chest.items;
            }
            return itemList;
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

    }
}
