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
        public List<string> Filter { get; set; }
        public Container(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            if(obj is Chest)
            {
                Chest = (Chest)obj;
            }
            Filter = new List<string>();
        }

        public void SendItem(Container input)
        {
            if(!IsEmpty())
            {
                int index = Chest.items.Count-1;
                bool sent = false;
                while (index >= 0 && sent == false)
                {
                    Printer.Info(index.ToString());
                    if (Chest.items[index] != null)
                    {
                        Printer.Info(Chest.items[index].Name);
                        Item item = Chest.items[index];
                        if (input.HasFilter())
                        {
                            Printer.Info("FILTERED");
                            if (input.Filter.Contains(item.Name))
                            {
                                Printer.Info($"SENDING FILTERED: {item.Name}");
                                if (input.CanStackItem(item))
                                {
                                    Printer.Info("Stacking");
                                    input.ReceiveStack(item);
                                }
                                else if (input.CanReceiveItems())
                                {
                                    Printer.Info("New Stack");
                                    input.ReceiveItem(item);
                                }
                                Chest.items.RemoveAt(index);
                                Chest.clearNulls();
                                sent = true;
                            }
                        }
                        else
                        {
                            Printer.Info($"SENDING: {item.Name}");
                            if (input.CanStackItem(item))
                            {
                                Printer.Info("Stacking");
                                input.ReceiveStack(item);
                            }
                            else if (input.CanReceiveItems())
                            {
                                Printer.Info("New Stack");
                                input.ReceiveItem(item);
                            }
                            Chest.items.RemoveAt(index);
                            Chest.clearNulls();
                            sent = true;
                        }
                    }
                    index--;
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

        public void UpdateFilter()
        {
            Printer.Info($"Filter items: {Chest.items.Count}");
            Filter.Clear();
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
    }
}
