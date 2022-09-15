using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley.Menus;
using StardewValley;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SObject = StardewValley.Object;
using StardewValley.Tools;
using StardewValley.Objects;
using MaddUtil;

namespace ChestPreview
{
    public class HoverMenu : InventoryMenu
    {
        public float Scale { get; set; }
        public int SourceX { get; set; }
		public int SourceY { get; set; }

		public HoverMenu(int xPosition, int yPosition, bool playerInventory, IList<Item> actualInventory, highlightThisItem highlightMethod = null, int capacity = -1, int rows = 3, int horizontalGap = 0, int verticalGap = 0, bool drawSlots = true)
		: base(xPosition, yPosition, playerInventory, actualInventory, null, capacity)
		{
			/*Scales
			 * 0.4 Small
			 * 0.5 Normal
			 * 0.6 Big
			 * */
			Scale = 0.6f;
			SourceX = xPosition;
			SourceY = yPosition;
			this.width = (int)(width * Scale);
			this.height = (int)(height * Scale);
			this.xPositionOnScreen = (int)(this.xPositionOnScreen - width / 2);
			this.yPositionOnScreen = this.yPositionOnScreen - this.height * 2;
		}
		
		public override void draw(SpriteBatch b, int red = -1, int green = -1, int blue = -1)
		{

			int bWidth = this.width + (int)(IClickableMenu.spaceToClearSideBorder + IClickableMenu.spaceToClearSideBorder*Scale) ;
			int bHeight = this.height + (int)((IClickableMenu.spaceToClearSideBorder + IClickableMenu.spaceToClearSideBorder * Scale));
			int xPosBox = (int)(this.xPositionOnScreen - (IClickableMenu.spaceToClearSideBorder * Scale) - (IClickableMenu.spaceToClearSideBorder * Scale)/2);
			int yPosBox = (int)(this.yPositionOnScreen - (IClickableMenu.spaceToClearSideBorder * Scale) - (IClickableMenu.spaceToClearSideBorder * Scale)/2 - 4);
			Game1.DrawBox(xPosBox, yPosBox, bWidth, bHeight);
			/*
			PERFECT
			int bWidth = this.width + (int)(IClickableMenu.spaceToClearSideBorder + IClickableMenu.spaceToClearSideBorder*Scale) ;
			int bHeight = this.height + (int)((IClickableMenu.spaceToClearSideBorder + IClickableMenu.spaceToClearSideBorder * Scale));
			int xPosBox = (int)(this.xPositionOnScreen - (IClickableMenu.spaceToClearSideBorder * Scale) - (IClickableMenu.spaceToClearSideBorder * Scale)/2);
			int yPosBox = (int)(this.yPositionOnScreen - (IClickableMenu.spaceToClearSideBorder * Scale) - (IClickableMenu.spaceToClearSideBorder * Scale)/2 - 4); 
			*/

			//line
			Texture2D menu_texture = Game1.menuTexture;
			Microsoft.Xna.Framework.Rectangle sourceRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 64, 64);
			/*Inner texture
			sourceRect.X = 64;
			sourceRect.Y = 128;
			//Top
			sourceRect.X = 128;
			sourceRect.Y = 0;
			//Left
			sourceRect.X = 0;
			sourceRect.Y = 128;
			//Right
			sourceRect.X = 192;
			sourceRect.Y = 128;
			*/
			int spriteYOffset = 0;
			
			Microsoft.Xna.Framework.Color inner_color = Microsoft.Xna.Framework.Color.White;
			int xLine = xPosBox + bWidth/2;
			int yLine = yPosBox + bHeight + IClickableMenu.borderWidth/4+6;
			int wLine = 64;
			//Needs height adapting for zoom level
			//Also maybe make it always at the top of bigcraftables -> height 128
			int hLine = SourceY - yLine - 32;
			//Left
			sourceRect.X = 0;
			sourceRect.Y = 128;
			b.Draw(menu_texture, new Microsoft.Xna.Framework.Rectangle(xLine-wLine/2-4, yLine, wLine, hLine), sourceRect, inner_color);
			//Right
			sourceRect.X = 192;
			sourceRect.Y = 128;
			b.Draw(menu_texture, new Microsoft.Xna.Framework.Rectangle(xLine-wLine/2+6, yLine, wLine, hLine), sourceRect, inner_color);

			Color tint = ((red == -1) ? Color.White : new Color((int)Utility.Lerp(red, Math.Min(255, red + 150), 0.65f), (int)Utility.Lerp(green, Math.Min(255, green + 150), 0.65f), (int)Utility.Lerp(blue, Math.Min(255, blue + 150), 0.65f)));
			Texture2D texture = ((red == -1) ? Game1.menuTexture : Game1.uncoloredMenuTexture);
			if (this.drawSlots)
			{
				for (int l = 0; l < this.capacity; l++)
				{
					Vector2 toDraw = new Vector2(base.xPositionOnScreen + l % (this.capacity / this.rows) * (64*Scale) + this.horizontalGap * (l % (this.capacity / this.rows)),
						base.yPositionOnScreen + l / (this.capacity / this.rows) * ((64*Scale) + this.verticalGap) + (l / (this.capacity / this.rows) - 1) * 4 - ((l < this.capacity / this.rows && this.playerInventory && this.verticalGap == 0) ? 12 : 0));
					b.Draw(texture, toDraw, Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 10), tint, 0f, Vector2.Zero, 1f*Scale, SpriteEffects.None, float.MaxValue-1);
				}
				for (int k = 0; k < this.capacity; k++)
				{
					Vector2 toDraw2 = new Vector2(
						base.xPositionOnScreen + k % (this.capacity / this.rows) * (64*Scale) 
						+ this.horizontalGap * (k % (this.capacity / this.rows)) * Scale - 16,
						base.yPositionOnScreen + k / (this.capacity / this.rows) * ((64*Scale) 
						+ this.verticalGap) + (k / (this.capacity / this.rows) - 1) * 4
						- ((k < this.capacity / this.rows && this.playerInventory && this.verticalGap == 0) ? 12 : 0) - 16);

					if (this.actualInventory.Count > k && this.actualInventory.ElementAt(k) != null)
					{
						bool highlight2 = this.highlightMethod(this.actualInventory[k]);
						if (this._iconShakeTimer.ContainsKey(k))
						{
							toDraw2 += 1f * new Vector2(Game1.random.Next(-1, 2), Game1.random.Next(-1, 2));
						}
						//this.actualInventory[k].drawInMenu(b, toDraw2, 1f*Scale, (!this.highlightMethod(this.actualInventory[k])) ? 0.25f : 1f, float.MaxValue, StackDrawType.Draw, Color.White, highlight2);
						DrawItem(b, this.actualInventory[k], (int)toDraw2.X, (int)toDraw2.Y, Scale, 1f, float.MaxValue, toDraw2);
					}
				}
			}
			
		}

		public void DrawItem(SpriteBatch spriteBatch, Item item, int x, int y, float scaleSize,  float transparency, float layer, Vector2 normal)
		{
			//Item item = pipe.StoredItem;
			Texture2D SpriteSheet;
			Rectangle srcRect;
			Vector2 originalPosition;
			Vector2 position = new Vector2(x, y);

			/*
			if(item is ModdedItem)
            {
				//usar 
				item.drawInMenu(spriteBatch, normal, scaleSize, transparency, layer, StackDrawType.Draw, Color.White, false);
				//o
				//draw itemnotfound sprite
			}
			*/
			if (item is SObject)
            {
				DrawInMenu.drawInMenuObject((item as SObject), spriteBatch, position, scaleSize, transparency, layer, StackDrawType.Draw, Color.White, false);
			}
			else if (item is Boots)
			{
				DrawInMenu.drawInMenuBoots((item as Boots), spriteBatch, position, scaleSize, transparency, layer, StackDrawType.Draw, Color.White, false);
			}
			else if (item is WateringCan)
            {
				
				DrawInMenu.drawInMenuWateringCan((item as WateringCan), spriteBatch, position, scaleSize, transparency, layer, StackDrawType.Draw, Color.White, false);
            }
			
			else if (item is MeleeWeapon && (item as MeleeWeapon).type.TargetValue == 1)
			{
				DrawInMenu.drawInMenuDagger((item as MeleeWeapon), spriteBatch, position, scaleSize, transparency, layer, StackDrawType.Draw, Color.White, false);
			}
			
			else
            {
				item.drawInMenu(spriteBatch, normal, scaleSize, transparency, layer, StackDrawType.Draw, Color.White, false);
			}
			//FURNITURE NO SE SHOWEA <<<<<<<<<<<-----------------------------------------
			/*
			 //REPASAR SI TODAS FUNCIONAN
			if (item is Tool)
			{
				Tool tool = (Tool)item;
				if (item is MeleeWeapon || item is Slingshot || item is Sword)
				{
					SpriteSheet = Tool.weaponsTexture;
					srcRect = Game1.getSquareSourceRectForNonStandardTileSheet(SpriteSheet, 16, 16, tool.IndexOfMenuItemView);
					originalPosition = Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64));
					position = new Vector2(originalPosition.X + 19, originalPosition.Y + 64 + 19);
					spriteBatch.Draw(SpriteSheet, position, srcRect, Color.White * transparency, 0f, Vector2.Zero, 1.7f, SpriteEffects.None,
						((float)(y * 64 + 32) / 10000f) + float.MaxValue);
				}
				else
				{
					SpriteSheet = Game1.toolSpriteSheet;
					srcRect = Game1.getSquareSourceRectForNonStandardTileSheet(SpriteSheet, 16, 16, tool.IndexOfMenuItemView);
					originalPosition = Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64));
					position = new Vector2(originalPosition.X + 19, originalPosition.Y + 64 + 18);
					spriteBatch.Draw(SpriteSheet, position, srcRect, Color.White * transparency, 0f, Vector2.Zero, 1.7f, SpriteEffects.None,
						((float)(y * 64 + 32) / 10000f) + float.MaxValue);
				}
			}
			/*
			//rings = standard
			else if (item is Ring)
			{
				SpriteSheet = Game1.objectSpriteSheet;
				srcRect = Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, item.ParentSheetIndex, 16, 16);
				originalPosition = Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64));
				position = new Vector2(originalPosition.X + 10, originalPosition.Y + 64 + 14);
				spriteBatch.Draw(SpriteSheet, position, srcRect, Color.White * transparency, 0f, Vector2.Zero, 2.5f, SpriteEffects.None,
					((float)(y * 64 + 32) / 10000f) + float.MaxValue);
			}
			
			else if (item != null)
			{
				SpriteSheet = Game1.objectSpriteSheet;
				srcRect = Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, item.ParentSheetIndex, 16, 16);
				originalPosition = Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64));
				position = new Vector2(originalPosition.X + 17, originalPosition.Y + 64 + 17);
				spriteBatch.Draw(SpriteSheet, position, srcRect, Color.White * transparency, 0f, Vector2.Zero, 1.9f, SpriteEffects.None,
					((float)(y * 64 + 32) / 10000f) + float.MaxValue);
			}
			*/
			
		}

	}
}
