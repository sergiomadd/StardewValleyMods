using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;
using ItemLogistics.Framework.Model;

namespace ItemLogistics.Framework
{
    public class Container : SGNode
    {
        public Chest Chest { get; set; }
        public List<Item> Filter { get; set; }
        public Container(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            if(obj is Chest)
            {
                Chest = (Chest)obj;
            }
        }

        public bool IsEmpty()
        {
            bool isEmpty = false;
            if (Chest.items.Count < 1)
            {
                isEmpty = true;
            }
            return isEmpty;
        }

        public void SendItems(Container target)
        {
            foreach (Item item in Chest.items.ToList())
            {
                Printer.Info($"SENDING: {item.Name}");
                if (target.CanStackItem(item))
                {
                    target.ReceiveStack(item);
                }
                else if(target.CanReceiveItems())
                {
                    target.ReceiveItem(item);
                }
            }
        }

        public void SendItem(Container target)
        {
            Printer.Info(Chest.items.Count.ToString());
            if(!IsEmpty())
            {
                if (Chest.items[0] != null)
                {
                    Item item = Chest.items[0];
                    Printer.Info($"SENDING: {item.Name}");
                    if (target.CanStackItem(item))
                    {
                        Printer.Info("Stacking");
                        target.ReceiveStack(item);
                    }
                    else if (target.CanReceiveItems())
                    {
                        Printer.Info("New Stack");
                        target.ReceiveItem(item);
                    }
                    Chest.items.RemoveAt(0);
                    Chest.clearNulls();
                }
            }
        }

        public bool CanReceiveItems()
        {
            bool canReceive = false;
            if (Chest.items.Capacity < Chest.GetActualCapacity())
            {
                canReceive = true;
            }
            return canReceive;
        }

        public bool CanStackItem(Item item)
        {
            bool canStack = false;
            if(Chest.items.Contains(item))
            {
                int index = Chest.items.IndexOf(item);
                if (Chest.items[index].canStackWith(item))
                {
                    canStack = true;
                }
            }
            return canStack;
        }

        public void ReceiveItem(Item item)
        {
            Chest.addItem(item);
        }

        public void ReceiveStack(Item item)
        {
            Chest.addToStack(item);
        }

    }
}
