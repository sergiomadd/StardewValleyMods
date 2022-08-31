using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;
using ItemPipes.Framework.Model;
using ItemPipes.Framework.Nodes;
using StardewValley.Menus;
using Netcode;
using ItemPipes.Framework.Items.CustomFilter;
using SObject = StardewValley.Object;
using ItemPipes.Framework.Util;
using ItemPipes.Framework.Nodes.ObjectNodes;


namespace ItemPipes.Framework.Items.Objects
{
    public class FilterPipeItem : InputPipeItem
    {
		public Filter Filter { get; set; }

		public FilterPipeItem() : base()
        {
		}

        public FilterPipeItem(Vector2 position) : base(position)
        {
			Filter = new Filter(9, this);
		}

		public override SObject Save()
		{
			Fence fence = (Fence)base.Save();
			string filterItems = "";
			if(Filter != null)
            {
				foreach (Item item in Filter.items)
				{
					if (item != null)
					{
						filterItems += "," + Utilities.GetIndexFromItem(item);
					}
				}
				if (!fence.modData.ContainsKey("filter")) { fence.modData.Add("filter", filterItems); }
				else { fence.modData["filter"] = filterItems; }
			}
			return fence;
		}

		public override void Load(ModDataDictionary data)
		{
			modData = data;
			if(modData.ContainsKey("filter"))
            {
				List<string> filterStrings = modData["filter"].Split(",").Skip(1).ToList();
				foreach (string index in filterStrings)
				{
					Item item = Utilities.GetItemFromIndex(index);
					if (item != null)
					{
						Filter.addItem(item);
					}
					else
					{
						Printer.Warn($"Error loading item with index {index}");
					}
				}
			}
		}
	
		public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
		{
			if (justCheckingForActivity)
			{
				return true;
			}
			if (Game1.didPlayerJustRightClick(ignoreNonMouseHeldInput: true))
			{
				Filter.ShowMenu();
				return false;
			}
			if (!justCheckingForActivity && who != null && who.currentLocation.isObjectAtTile(who.getTileX(), who.getTileY() - 1) && who.currentLocation.isObjectAtTile(who.getTileX(), who.getTileY() + 1) && who.currentLocation.isObjectAtTile(who.getTileX() + 1, who.getTileY()) && who.currentLocation.isObjectAtTile(who.getTileX() - 1, who.getTileY()) && !who.currentLocation.getObjectAtTile(who.getTileX(), who.getTileY() - 1).isPassable() && !who.currentLocation.getObjectAtTile(who.getTileX(), who.getTileY() + 1).isPassable() && !who.currentLocation.getObjectAtTile(who.getTileX() - 1, who.getTileY()).isPassable() && !who.currentLocation.getObjectAtTile(who.getTileX() + 1, who.getTileY()).isPassable())
			{
				this.performToolAction(null, who.currentLocation);
			}
			return true;
		}
	}
}
