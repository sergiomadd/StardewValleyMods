﻿using System;
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


namespace ItemPipes.Framework.Items
{
    [XmlType("Mods_sergiomadd.ItemPipes_FilterPipeItem")]
    public class FilterPipeItem : InputItem
    {
		public Filter Filter { get; set; }

		public FilterPipeItem() : base()
        {
			Name = "Filter Pipe";
			IDName = "FilterPipe";
			Description = "Type: Input Pipe\nInserts items into an adjacent container, it filters only the items already on the Filter Pipe Inventory. Right click the Filter Pipe to open the Inventory.";
			LoadTextures();
		}

        public FilterPipeItem(Vector2 position) : base(position)
        {
			Name = "Filter Pipe";
			IDName = "FilterPipe";
			Description = "Type: Input Pipe\nInserts items into an adjacent container, it filters only the items already on the Filter Pipe Inventory. Right click the Filter Pipe to open the Inventory.";
			LoadTextures();
			Filter = new Filter();
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
