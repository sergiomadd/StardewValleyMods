using ItemLogistics.Framework.Model;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Buildings;
using Netcode;

namespace ItemLogistics.Framework
{
    public class ShipBin : Node
    {
        public ShippingBin ShippingBin { get; set; }
        public Farm Farm { get; set; }
        public ShipBin(Vector2 position, GameLocation location, StardewValley.Object obj, Building building) : base(position, location, obj)
        {
            Name = building.buildingType.ToString();
            if(building is ShippingBin)
            {
                ShippingBin = (ShippingBin)building;
            }
			Farm = Game1.getFarm();
        }



        public void ShipItem(Item item, Farmer who)
		{
			if (item != null && item is StardewValley.Object && Farm != null)
            {
				Farm.getShippingBin(who).Add(item);
				ShippingBin.showShipment(item as StardewValley.Object, playThrowSound: false);
				Farm.lastItemShipped = item;
			}

		}

    }
}
