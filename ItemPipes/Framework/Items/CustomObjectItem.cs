﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemPipes.Framework.Model;
using ItemPipes.Framework.Nodes;
using ItemPipes.Framework.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Objects;
using StardewValley.Tools;
using System.Xml.Serialization;
using StardewValley.Network;
using SObject = StardewValley.Object;
using ItemPipes.Framework.Factories;

namespace ItemPipes.Framework.Items
{
	public abstract class CustomObjectItem : SObject
	{
		public string IDName { get; set; }
		public string Description { get; set; }
		[XmlIgnore]
		public Texture2D ItemTexture { get; set; }
		public string State { get; set; }
		public bool Passable { get; set; }


		public CustomObjectItem()
		{
			Init();
			State = "default";
			type.Value = "Crafting";
			canBeSetDown.Value = true;
		}

		public CustomObjectItem(Vector2 position) : this()
		{
			TileLocation = position;
			base.boundingBox.Value = new Rectangle((int)tileLocation.X * 64, (int)tileLocation.Y * 64, 64, 64);
		}

		public void Init()
		{
			IDName = GetType().Name.Substring(0, GetType().Name.Length - 4);
			DataAccess DataAccess = DataAccess.GetDataAccess();
			ItemTexture = DataAccess.Sprites[IDName + "_Item"];
			Name = DataAccess.ItemNames[IDName];
			DisplayName = DataAccess.ItemNames[IDName];
			Description = DataAccess.ItemDescriptions[IDName];
			parentSheetIndex.Value = DataAccess.ItemIDs[IDName];
		}

		public virtual SObject Save()
		{
			if (!modData.ContainsKey("ItemPipes")){ modData.Add("ItemPipes", "true"); }
			else { modData["ItemPipes"] = "true"; }
			if (!modData.ContainsKey("Type")){ modData.Add("Type", IDName); }
			else { modData["Type"] = IDName; }
			if (!modData.ContainsKey("Stack")){ modData.Add("Stack", Stack.ToString()); }
			else { modData["Type"] = IDName; }
			if (!modData.ContainsKey("State")){ modData.Add("State", State); }
			else { modData["State"] = State; }
			Fence fence = new Fence(tileLocation, 1, false);
			fence.modData = modData;
			
			return fence;
		}

		public virtual void Load(ModDataDictionary data)
		{
			modData = data;
		}

		public override string getDescription()
		{
			return Description;
		}

		protected override int getDescriptionWidth()
		{
			int minimum_size = 272;
			if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.fr)
			{
				minimum_size = 384;
			}
			return Math.Max(minimum_size, (int)Game1.dialogueFont.MeasureString((Name == null) ? "" : Name).X);
		}

		public override string getCategoryName()
		{
			return "Item Pipes";
		}

		public override Color getCategoryColor()
		{
			return Color.Black;
		}

		protected override string loadDisplayName()
		{
			return Name;
		}

		public override bool isPassable()
		{
			return Passable;
		}

		public override bool placementAction(GameLocation location, int x, int y, Farmer who = null)
		{
			Vector2 placementTile = new Vector2(x / 64, y / 64);
			if (location.objects.ContainsKey(placementTile))
			{
				return false;
			}
			SObject obj = ItemFactory.CreateObject(placementTile, this.IDName);
			if (obj != null)
			{
				location.objects.Add(placementTile, obj);
				location.playSound("axe");
				return true;
			}
			else
			{
				return false;
			}
		}

		public override bool performToolAction(Tool t, GameLocation location)
		{
			if (t is Pickaxe)
			{
				var who = t.getLastFarmerToUse();
				this.performRemoveAction(this.TileLocation, location);
				Debris deb = new Debris(getOne(), who.GetToolLocation(), new Vector2(who.GetBoundingBox().Center.X, who.GetBoundingBox().Center.Y));
				Game1.currentLocation.debris.Add(deb);
				Game1.currentLocation.objects.Remove(this.TileLocation);
				return false;
			}
			return false;
		}

		public override bool performObjectDropInAction(Item dropIn, bool probe, Farmer who)
		{
			return base.performObjectDropInAction(dropIn, probe, who);
		}

		public override Item getOne()
		{
			return ItemFactory.CreateItem(IDName);
		}
	}
}
