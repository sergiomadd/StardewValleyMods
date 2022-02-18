using ItemPipes.Framework.Model;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Buildings;
using Netcode;
using System.Collections.Generic;
using System.Linq;

namespace ItemPipes.Framework.Nodes.ObjectNodes
{
    public class ShippingBinContainerNode : ContainerNode
    {
        public ShippingBin ShippingBin { get; set; }
        public Farm Farm { get; set; }
        public ShippingBinContainerNode() { }
        public ShippingBinContainerNode(Vector2 position, GameLocation location, StardewValley.Object obj, Building building) : base(position, location, obj)
        {
            Name = building.buildingType.ToString();
            if(building is ShippingBin)
            {
                ShippingBin = (ShippingBin)building;
            }
			Farm = Game1.getFarm();
            Filter = new NetObjectList<Item>();
            Type = "ShippingBin";
        }



        public void ShipItem(Item item)
		{
			if (item != null && item is StardewValley.Object && Farm != null)
            {
				Farm.getShippingBin(Game1.MasterPlayer).Add(item);
				ShippingBin.showShipment(item as StardewValley.Object, playThrowSound: false);
				Farm.lastItemShipped = item;
			}

		}
        public override NetObjectList<Item> UpdateFilter(NetObjectList<Item> filteredItems)
        {
            Filter = new NetObjectList<Item>();
            if (filteredItems == null)
            {
                Filter.Add(Farm.lastItemShipped);
            }
            else
            {
                foreach (Item item in filteredItems.ToList())
                {
                    Filter.Add(item);
                }
            }
            return Filter;

        }

        public override bool IsEmpty()
        {
            return false;
        }
    }
}
