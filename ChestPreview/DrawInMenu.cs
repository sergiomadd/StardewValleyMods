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
    public static class DrawInMenu
    {
		//ONly in watercan for tools
		public static void drawInMenuTool(Tool item, SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
		{
			spriteBatch.Draw(Game1.toolSpriteSheet, location + new Vector2(32f, 32f), Game1.getSquareSourceRectForNonStandardTileSheet(Game1.toolSpriteSheet, 16, 16, item.IndexOfMenuItemView), color * transparency, 0f, new Vector2(8f, 8f), 4f * scaleSize, SpriteEffects.None, layerDepth);
			if ((bool)item.stackable)
			{
				Game1.drawWithBorder(string.Concat(((Stackable)item).NumberInStack), Color.Black, Color.White, location + new Vector2(64f - Game1.dialogueFont.MeasureString(string.Concat(((Stackable)item).NumberInStack)).X, 64f - Game1.dialogueFont.MeasureString(string.Concat(((Stackable)item).NumberInStack)).Y * 3f / 4f), 0f, 0.5f, 1f);
			}
		}
		//TODO RINGS
		public static void drawInMenuRing(Tool item, SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
		{
			spriteBatch.Draw(Game1.toolSpriteSheet, location + new Vector2(32f, 32f), Game1.getSquareSourceRectForNonStandardTileSheet(Game1.toolSpriteSheet, 16, 16, item.IndexOfMenuItemView), color * transparency, 0f, new Vector2(8f, 8f), 4f * scaleSize, SpriteEffects.None, layerDepth);
			if ((bool)item.stackable)
			{
				Game1.drawWithBorder(string.Concat(((Stackable)item).NumberInStack), Color.Black, Color.White, location + new Vector2(64f - Game1.dialogueFont.MeasureString(string.Concat(((Stackable)item).NumberInStack)).X, 64f - Game1.dialogueFont.MeasureString(string.Concat(((Stackable)item).NumberInStack)).Y * 3f / 4f), 0f, 0.5f, 1f);
			}
		}
		//TODO DAGGERS
		/* Melee weapons types:
		 * 0 Sword
		 * 1 Dagger
		 * 2 Club
		 * 3 Defense?
		 */
		public static void drawInMenuDagger(MeleeWeapon item, SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
		{
			float addedScale = 0f;
			spriteBatch.Draw(Tool.weaponsTexture,
				location + (((int)item.type == 1) ? new Vector2(36f, 28f) : new Vector2(32f, 32f)),
				Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, item.IndexOfMenuItemView, 16, 16),
				color * transparency, 0f, new Vector2(8f, 8f), 4f * (scaleSize + addedScale), SpriteEffects.None, layerDepth);
		}

		public static void drawInMenuWateringCan(WateringCan item, SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
		{
			drawInMenuTool((item as WateringCan), spriteBatch,
				location + (Game1.player.hasWateringCanEnchantment ? new Vector2(0f, 0f) : new Vector2(0f, -8f)),
				scaleSize, transparency, layerDepth, StackDrawType.Draw, Color.White, false);

			if (drawStackNumber != 0 && !Game1.player.hasWateringCanEnchantment)
			{
				//Left
				spriteBatch.Draw(Game1.mouseCursors, 
					location + new Vector2(16f, 38f), 
					new Rectangle
					(
						297, 
						420, 
						(int)(7), 
						(int)(2)
					), 
					Color.White * transparency, 0f, Vector2.Zero, 4f, SpriteEffects.None, layerDepth + 0.0001f);
				
				spriteBatch.Draw(Game1.mouseCursors,
					location + new Vector2(16f, 40f),
					new Rectangle
					(
						297,
						422,
						(int)(7),
						(int)(2)
					),
					Color.White * transparency, 0f, Vector2.Zero, 4f, SpriteEffects.None, layerDepth + 0.0001f);
				//Right
				spriteBatch.Draw(Game1.mouseCursors,
					location + new Vector2(20f, 38f),
					new Rectangle
					(
						304,
						420,
						(int)(7),
						(int)(2)
					),
					Color.White * transparency, 0f, Vector2.Zero, 4f, SpriteEffects.None, layerDepth + 0.0001f);

				spriteBatch.Draw(Game1.mouseCursors,
					location + new Vector2(20f, 40f),
					new Rectangle
					(
						304,
						422,
						(int)(7),
						(int)(2)
					),
					Color.White * transparency, 0f, Vector2.Zero, 4f, SpriteEffects.None, layerDepth + 0.0001f);		
				spriteBatch.Draw(Game1.staminaRect, 
					new Rectangle
					(
						(int)location.X + 8 + 12, 
						(int)location.Y + 64 - 16 - 8, 
						(int)(((float)item.WaterLeft / (float)item.waterCanMax * 48f)*scaleSize), 
						(int)(8*scaleSize)
					),
					item.IsBottomless ? (Color.BlueViolet * 1f * transparency) : (Color.DodgerBlue * 0.7f * transparency));
				
			}
		}

		public static void drawInMenuBoots(Boots item, SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
		{
			spriteBatch.Draw(Game1.objectSpriteSheet,
				location + new Vector2(32f + 16f, 32f + 16f) * scaleSize,
				Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, item.indexInTileSheet.Value, 16, 16),
				color * transparency, 0f, new Vector2(8f, 8f) * scaleSize, scaleSize * 4f, SpriteEffects.None, layerDepth);
		}

		public static void drawInMenuObject(SObject item, SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
		{
			if ((bool)item.isRecipe)
			{
				transparency = 0.5f;
				scaleSize *= 0.75f;
			}
			bool shouldDrawStackNumber = ((drawStackNumber == StackDrawType.Draw && item.maximumStackSize() > 1 && item.Stack > 1) || drawStackNumber == StackDrawType.Draw_OneInclusive) && (double)scaleSize > 0.3 && item.Stack != int.MaxValue;
			if (item.IsRecipe)
			{
				shouldDrawStackNumber = false;
			}
			if ((bool)item.bigCraftable)
			{
				Microsoft.Xna.Framework.Rectangle sourceRect = SObject.getSourceRectForBigCraftable(item.parentSheetIndex);
				spriteBatch.Draw(Game1.bigCraftableSpriteSheet,
					location + new Vector2(32f, 32f), sourceRect,
					color * transparency, 0f, new Vector2(8f, 16f), 4f * (((double)scaleSize < 0.2) ? scaleSize : (scaleSize / 2f)), SpriteEffects.None, layerDepth);
				if (shouldDrawStackNumber)
				{
					Utility.drawTinyDigits(item.stack, spriteBatch,
						location + new Vector2(32, 18f * scaleSize + 64 * scaleSize),
						3f * scaleSize, 1f, color);
				}
			}
			else
			{
				if ((int)item.parentSheetIndex != 590 && drawShadow)
				{
					spriteBatch.Draw(Game1.shadowTexture, location + new Vector2(32f, 48f), Game1.shadowTexture.Bounds, color * 0.5f, 0f, new Vector2(Game1.shadowTexture.Bounds.Center.X, Game1.shadowTexture.Bounds.Center.Y), 3f, SpriteEffects.None, layerDepth - 0.0001f);
				}
				spriteBatch.Draw(Game1.objectSpriteSheet,
					location + new Vector2((int)(32f * scaleSize) + 8, (int)(32f * scaleSize) + 8),
					Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, item.parentSheetIndex, 16, 16),
					color * transparency, 0f, new Vector2(8f, 8f) * scaleSize, 4f * scaleSize, SpriteEffects.None, layerDepth);
				if (shouldDrawStackNumber)
				{
					Utility.drawTinyDigits(item.stack, spriteBatch,
						location + new Vector2(32, 18f * scaleSize + 64 * scaleSize),
						3f * scaleSize, 1f, color);
				}
				if (drawStackNumber != 0 && (int)item.quality > 0)
				{
					Microsoft.Xna.Framework.Rectangle quality_rect = (((int)item.quality < 4) ? new Microsoft.Xna.Framework.Rectangle(338 + ((int)item.quality - 1) * 8, 400, 8, 8) : new Microsoft.Xna.Framework.Rectangle(346, 392, 8, 8));
					Texture2D quality_sheet = Game1.mouseCursors;
					float yOffset = (((int)item.quality < 4) ? 0f : (((float)Math.Cos((double)Game1.currentGameTime.TotalGameTime.Milliseconds * Math.PI / 512.0) + 1f) * 0.05f));
					spriteBatch.Draw(quality_sheet, location + new Vector2(12f, 52f + yOffset), quality_rect, color * transparency, 0f, new Vector2(4f, 4f), 3f * scaleSize * (1f + yOffset), SpriteEffects.None, layerDepth);
				}
				if (item.Category == -22 && item.uses.Value > 0)
				{
					float health = ((float)(FishingRod.maxTackleUses - item.uses.Value) + 0f) / (float)FishingRod.maxTackleUses;
					spriteBatch.Draw(Game1.staminaRect, new Microsoft.Xna.Framework.Rectangle((int)location.X, (int)(location.Y + 56f * scaleSize), (int)(64f * scaleSize * health), (int)(8f * scaleSize)), Utility.getRedToGreenLerpColor(health));
				}
			}
			if ((bool)item.isRecipe)
			{
				spriteBatch.Draw(Game1.objectSpriteSheet, location + new Vector2(16f, 16f), Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 451, 16, 16), color, 0f, Vector2.Zero, 3f, SpriteEffects.None, layerDepth + 0.0001f);
			}
		}
	}
}
