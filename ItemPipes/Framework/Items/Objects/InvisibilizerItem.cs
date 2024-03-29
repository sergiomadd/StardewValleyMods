﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemPipes.Framework.Model;
using ItemPipes.Framework.Nodes;
using ItemPipes.Framework.Items.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Objects;
using StardewValley.Tools;
using System.Xml.Serialization;
using StardewValley.Network;
using SObject = StardewValley.Object;
using ItemPipes.Framework.Factories;
using ItemPipes.Framework.Nodes.ObjectNodes;
using ItemPipes.Framework.Util;


namespace ItemPipes.Framework.Items.Objects
{
	public class InvisibilizerItem : PipeBigCraftableItem
	{
		public Texture2D SignalTexture { get; set; }
		public Texture2D OnTextureR { get; set; }
		public Texture2D OnTextureL { get; set; }
		public Texture2D OnTextureC { get; set; }
		public Texture2D OffTextureR { get; set; }
		public Texture2D OffTextureL { get; set; }
		public Texture2D OffTextureC { get; set; }

		public InvisibilizerItem() : base()
		{
			State = "off";
			DataAccess DataAccess = DataAccess.GetDataAccess();
			OffTextureC = DataAccess.Sprites[IDName + "_signal_offC"];
			OffTextureL = DataAccess.Sprites[IDName + "_signal_offL"];
			OffTextureR = DataAccess.Sprites[IDName + "_signal_offR"];
			OnTextureC = DataAccess.Sprites[IDName + "_signal_onC"];
			OnTextureL = DataAccess.Sprites[IDName + "_signal_onL"];
			OnTextureR = DataAccess.Sprites[IDName + "_signal_onR"];
			SignalTexture = OffTextureC;
		}

		public InvisibilizerItem(Vector2 position) : base(position)
		{
			State = "off";
			DataAccess DataAccess = DataAccess.GetDataAccess();
			OffTextureC = DataAccess.Sprites[IDName + "_signal_offC"];
			OffTextureL = DataAccess.Sprites[IDName + "_signal_offL"];
			OffTextureR = DataAccess.Sprites[IDName + "_signal_offR"];
			OnTextureC = DataAccess.Sprites[IDName + "_signal_onC"];
			OnTextureL = DataAccess.Sprites[IDName + "_signal_onL"];
			OnTextureR = DataAccess.Sprites[IDName + "_signal_onR"];
			SignalTexture = OffTextureC;
		}

		public override void LoadObject(Item item)
		{
			base.LoadObject(item);
			State = modData["State"];
			if (State.Equals("on"))
			{
				Passable = true;
			}
			else
			{
				Passable = false;
			}
		}

		public void ChangeSignal()
        {
			DataAccess DataAccess = DataAccess.GetDataAccess();
			List<Node> nodes = DataAccess.LocationNodes[Game1.currentLocation];
			InvisibilizerNode pipo = (InvisibilizerNode)nodes.Find(n => n.Position.Equals(this.TileLocation));
			if (pipo.ChangeState())
			{
				Passable = true;
			}
			else
			{
				Passable = false;
			}
		}

		public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
		{
			Rectangle srcRect = new Rectangle(0, 0, 16, 32);
			spriteBatch.Draw(ItemTexture, objectPosition, srcRect, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 3) / 10000f));
		}

		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
		{
			bool shouldDrawStackNumber = ((drawStackNumber == StackDrawType.Draw && this.maximumStackSize() > 1 && this.Stack > 1)
				|| drawStackNumber == StackDrawType.Draw_OneInclusive) && (double)scaleSize > 0.3 && this.Stack != int.MaxValue;
			Rectangle srcRect = new Rectangle(0, 0, 16, 32);
			spriteBatch.Draw(ItemTexture, location + new Vector2((int)(32f * scaleSize), (int)(16f * scaleSize)), srcRect, color * transparency, 0f,
				new Vector2(8f, 8f) * scaleSize, 2f * scaleSize, SpriteEffects.None, layerDepth);

			if (shouldDrawStackNumber)
			{
				var loc = location + new Vector2((float)(64 - Utility.getWidthOfTinyDigitString(this.Stack, 3f * scaleSize)) + 3f * scaleSize, 64f - 18f * scaleSize + 2f);
				Utility.drawTinyDigits(this.Stack, spriteBatch, loc, 3f * scaleSize, 1f, color);

			}
		}

		public override void drawAsProp(SpriteBatch b)
		{
			int x = (int)this.TileLocation.X;
			int y = (int)this.TileLocation.Y;

			Vector2 scaleFactor = Vector2.One;
			scaleFactor *= 4f;
			Vector2 position = Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64));
			Rectangle srcRect = new Rectangle(16 * 2, 0, 16, 32);
			b.Draw(destinationRectangle: new Rectangle((int)(position.X - scaleFactor.X / 2f), (int)(position.Y - scaleFactor.Y / 2f),
				(int)(64f + scaleFactor.X), (int)(128f + scaleFactor.Y / 2f)),
				texture: ItemTexture,
				sourceRectangle: srcRect,
				color: Color.White,
				rotation: 0f,
				origin: Vector2.Zero,
				effects: SpriteEffects.None,
				layerDepth: Math.Max(0f, (float)((y + 1) * 64 - 1) / 10000f));
		}

		public override void draw(SpriteBatch spriteBatch, int xNonTile, int yNonTile, float layerDepth, float alpha = 1)
		{
			base.draw(spriteBatch, xNonTile, yNonTile, layerDepth, alpha);
		}

		public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1)
		{
			base.draw(spriteBatch, x, y);
			DataAccess DataAccess = DataAccess.GetDataAccess();
			List<Node> nodes = DataAccess.LocationNodes[Game1.currentLocation];
			Node node = nodes.Find(n => n.Position.Equals(TileLocation));
			if (node != null && node is InvisibilizerNode)
			{
				InvisibilizerNode invis = (InvisibilizerNode)node;
				State = invis.State;
				float transparency = 1f;
				if (State.Equals("on"))
                {
					//Look right
					if (TileLocation.X < Game1.player.getTileLocation().X)
					{
						SignalTexture = OnTextureR;
					}
					//look left
					else if (TileLocation.X > Game1.player.getTileLocation().X)
					{
						SignalTexture = OnTextureL;
					}
					//center
					else if (TileLocation.X == Game1.player.getTileLocation().X)
					{
						SignalTexture = OnTextureC;
					}
					transparency = 0.5f;
					Rectangle srcRect = new Rectangle(0, 0, 16, 32);
					spriteBatch.Draw(SignalTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64)), srcRect, Color.White * transparency, 0f, Vector2.Zero, 4f, SpriteEffects.None, ((float)(y * 64 + 32) / 10000f) + 0.002f);
				}
				else if(State.Equals("off"))
                {
					//Look right
					if (TileLocation.X < Game1.player.getTileLocation().X)
					{
						SignalTexture = OffTextureR;
					}
					//look left
					else if (TileLocation.X > Game1.player.getTileLocation().X)
					{
						SignalTexture = OffTextureL;
					}
					//center
					else if (TileLocation.X == Game1.player.getTileLocation().X)
					{
						SignalTexture = OffTextureC;
					}
					transparency = 1f;
					Rectangle srcRect = new Rectangle(0, 0, 16, 32);
					spriteBatch.Draw(SignalTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64)), srcRect, Color.White * transparency, 0f, Vector2.Zero, 4f, SpriteEffects.None, ((float)(y * 64 + 32) / 10000f) + 0.002f);
				}
			}
		}

	}
}
