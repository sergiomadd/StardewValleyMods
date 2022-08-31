using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Netcode;
using StardewValley;
using SObject = StardewValley.Object;
using ItemPipes.Framework.Util;


namespace ItemPipes.Framework.Nodes.ObjectNodes
{
    public class FilterNode
    {
        public NetObjectList<Item> Items { get; set; }
        public bool Active { get; set; }
        public bool Quality { get; set; }

        public FilterNode(bool active)
        {
            Active = active;
            Items = new NetObjectList<Item>();
            Quality = false;
        }

        public void UpdateOption(string option)
        {
            if(option.Equals("quality"))
            {
                Quality = !Quality;
            }
        }

        public bool IsItemOnFilter(Item item)
        {
            bool itis = false;
            if (Quality)
            {
                if (item is SObject)
                {
                    if (Items.Any(i => i.Name.Equals(item.Name) && (i as SObject).Quality.Equals((item as SObject).Quality)))
                    {
                        itis = true;
                    }
                }
            }
            else
            {
                if (Items.Any(i => i.Name.Equals(item.Name)))
                {
                    itis = true;
                }
            }
            return itis;
        }

    }
}
