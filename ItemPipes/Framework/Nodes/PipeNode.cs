using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewModdingAPI.Events;
using SObject = StardewValley.Object;
using StardewValley.Tools;
using System.Xml.Serialization;
using ItemPipes.Framework.Util;
using ItemPipes.Framework.Model;

namespace ItemPipes.Framework.Nodes
{
    public class PipeNode : Node
    {
        public Item StoredItem { get; set; }
        public int ItemTimer { get; set; }
        public bool PassingItem { get; set; }

        public PipeNode() : base()
        {

        }
        public PipeNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            ItemTimer = 500;
            PassingItem = false;
        }

        public void SendItem(Item item, Node target, int index, List<Node> path)
        {
            DisplayItem(item);
            if (!this.Equals(target))
            {
                if (index < path.Count)
                {
                    Node nextNode = path[index];
                    index++;
                    if(nextNode is PipeNode)
                    {
                        PipeNode pipe = (PipeNode)nextNode;
                        pipe.SendItem(item, target, index, path);
                    }
                }
            }
        }

        public bool DisplayItem(Item item)
        {
            bool canLoad = false;
            if (StoredItem == null)
            {
                StoredItem = item;
                PassingItem = true;
                Printer.Info("Passing: ");
                Print();
                System.Threading.Thread.Sleep(ItemTimer);
                StoredItem = null;
                PassingItem = false;
            }
            return canLoad;
        }

    }
}
