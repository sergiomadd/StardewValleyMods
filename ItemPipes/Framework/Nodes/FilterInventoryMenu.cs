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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ItemPipes.Framework.Nodes
{
    public class FilterInventoryMenu : IClickableMenu
	{
		/*
		public delegate bool highlightThisItem(Item i);

		public enum BorderSide
		{
			Top,
			Left,
			Right,
			Bottom
		}

		public const int region_inventorySlot0 = 0;

		public const int region_inventorySlot1 = 1;

		public const int region_inventorySlot2 = 2;

		public const int region_inventorySlot3 = 3;

		public const int region_inventorySlot4 = 4;

		public const int region_inventorySlot5 = 5;

		public const int region_inventorySlot6 = 6;

		public const int region_inventorySlot7 = 7;

		public const int region_inventorySlot8 = 8;

		public const int region_inventorySlot9 = 9;

		public const int region_inventorySlot10 = 10;

		public const int region_inventorySlot11 = 11;

		public const int region_inventorySlot12 = 12;

		public const int region_inventorySlot13 = 13;

		public const int region_inventorySlot14 = 14;

		public const int region_inventorySlot15 = 15;

		public const int region_inventorySlot16 = 16;

		public const int region_inventorySlot17 = 17;

		public const int region_inventorySlot18 = 18;

		public const int region_inventorySlot19 = 19;

		public const int region_inventorySlot20 = 20;

		public const int region_inventorySlot21 = 21;

		public const int region_inventorySlot22 = 22;

		public const int region_inventorySlot23 = 23;

		public const int region_inventorySlot24 = 24;

		public const int region_inventorySlot25 = 25;

		public const int region_inventorySlot26 = 26;

		public const int region_inventorySlot27 = 27;

		public const int region_inventorySlot28 = 28;

		public const int region_inventorySlot29 = 29;

		public const int region_inventorySlot30 = 30;

		public const int region_inventorySlot31 = 31;

		public const int region_inventorySlot32 = 32;

		public const int region_inventorySlot33 = 33;

		public const int region_inventorySlot34 = 34;

		public const int region_inventorySlot35 = 35;

		public const int region_dropButton = 107;

		public const int region_inventoryArea = 9000;

		public string hoverText = "";

		public string hoverTitle = "";

		public string descriptionTitle = "";

		public string descriptionText = "";

		public List<ClickableComponent> inventory = new List<ClickableComponent>();

		protected Dictionary<int, double> _iconShakeTimer = new Dictionary<int, double>();

		public IList<Item> actualInventory;

		public highlightThisItem highlightMethod;

		public ItemGrabMenu.behaviorOnItemSelect onAddItem;

		public bool playerInventory;

		public bool drawSlots;

		public bool showGrayedOutSlots;

		public int capacity;

		public int rows;

		public int horizontalGap;

		public int verticalGap;

		public ClickableComponent dropItemInvisibleButton;

		public string moveItemSound = "dwop";

		public FilterInventoryMenu(int xPosition, int yPosition, bool playerInventory, IList<Item> actualInventory = null, highlightThisItem highlightMethod = null, int capacity = -1, int rows = 3, int horizontalGap = 0, int verticalGap = 0, bool drawSlots = true)
			: base(xPosition, yPosition, 64 * (((capacity == -1) ? 36 : capacity) / rows), 64 * rows + 16)
		{
			this.drawSlots = drawSlots;
			this.horizontalGap = horizontalGap;
			this.verticalGap = verticalGap;
			this.rows = rows;
			this.capacity = ((capacity == -1) ? 36 : capacity);
			this.playerInventory = playerInventory;
			this.actualInventory = actualInventory;
			if (actualInventory == null)
			{
				this.actualInventory = Game1.player.items;
			}
			for (int j = 0; j < (int)Game1.player.maxItems; j++)
			{
				if (Game1.player.items.Count <= j)
				{
					Game1.player.items.Add(null);
				}
			}
			for (int i = 0; i < this.capacity; i++)
			{
				this.inventory.Add(new ClickableComponent(new Rectangle(xPosition + i % (this.capacity / rows) * 64 + horizontalGap * (i % (this.capacity / rows)), base.yPositionOnScreen + i / (this.capacity / rows) * (64 + verticalGap) + (i / (this.capacity / rows) - 1) * 4 - ((i <= this.capacity / rows && playerInventory && verticalGap == 0) ? 12 : 0), 64, 64), string.Concat(i))
				{
					myID = i,
					leftNeighborID = ((i % (this.capacity / rows) != 0) ? (i - 1) : 107),
					rightNeighborID = (((i + 1) % (this.capacity / rows) != 0) ? (i + 1) : 106),
					downNeighborID = ((i >= this.actualInventory.Count - this.capacity / rows) ? 102 : (i + this.capacity / rows)),
					upNeighborID = ((i < this.capacity / rows) ? (12340 + i) : (i - this.capacity / rows)),
					region = 9000,
					upNeighborImmutable = true,
					downNeighborImmutable = true,
					leftNeighborImmutable = true,
					rightNeighborImmutable = true
				});
			}
			this.highlightMethod = highlightMethod;
			if (highlightMethod == null)
			{
				this.highlightMethod = highlightAllItems;
			}
			this.dropItemInvisibleButton = new ClickableComponent(new Rectangle(xPosition - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - 128, base.yPositionOnScreen - 12, 64, 64), "")
			{
				myID = (playerInventory ? 107 : (-500)),
				rightNeighborID = 0
			};
			foreach (ClickableComponent item in this.GetBorder(BorderSide.Top))
			{
				item.upNeighborImmutable = false;
			}
			foreach (ClickableComponent item2 in this.GetBorder(BorderSide.Bottom))
			{
				item2.downNeighborImmutable = false;
			}
			foreach (ClickableComponent item3 in this.GetBorder(BorderSide.Left))
			{
				item3.leftNeighborImmutable = false;
			}
			foreach (ClickableComponent item4 in this.GetBorder(BorderSide.Right))
			{
				item4.rightNeighborImmutable = false;
			}
		}

		public List<ClickableComponent> GetBorder(BorderSide side)
		{
			List<ClickableComponent> inventory_slots = new List<ClickableComponent>();
			int row_size = this.capacity / this.rows;
			switch (side)
			{
				case BorderSide.Bottom:
					{
						for (int l = 0; l < this.inventory.Count; l++)
						{
							if (l >= this.actualInventory.Count - row_size)
							{
								inventory_slots.Add(this.inventory[l]);
							}
						}
						break;
					}
				case BorderSide.Top:
					{
						for (int k = 0; k < this.inventory.Count; k++)
						{
							if (k < row_size)
							{
								inventory_slots.Add(this.inventory[k]);
							}
						}
						break;
					}
				case BorderSide.Left:
					{
						for (int j = 0; j < this.inventory.Count; j++)
						{
							if (j % row_size == 0)
							{
								inventory_slots.Add(this.inventory[j]);
							}
						}
						break;
					}
				case BorderSide.Right:
					{
						for (int i = 0; i < this.inventory.Count; i++)
						{
							if (i % row_size == row_size - 1)
							{
								inventory_slots.Add(this.inventory[i]);
							}
						}
						break;
					}
			}
			return inventory_slots;
		}

		public static bool highlightAllItems(Item i)
		{
			return true;
		}

		public static bool highlightNoItems(Item i)
		{
			return false;
		}

		public void movePosition(int x, int y)
		{
			base.xPositionOnScreen += x;
			base.yPositionOnScreen += y;
			foreach (ClickableComponent item in this.inventory)
			{
				item.bounds.X += x;
				item.bounds.Y += y;
			}
			this.dropItemInvisibleButton.bounds.X += x;
			this.dropItemInvisibleButton.bounds.Y += y;
		}

		public void ShakeItem(Item item)
		{
			this.ShakeItem(this.actualInventory.IndexOf(item));
		}

		public void ShakeItem(int index)
		{
			if (index >= 0 && index < this.inventory.Count)
			{
				this._iconShakeTimer[index] = Game1.currentGameTime.TotalGameTime.TotalSeconds + 0.5;
			}
		}

		public Item tryToAddItem(Item toPlace, string sound = "coin")
		{
			if (toPlace == null)
			{
				return null;
			}
			int originalStack = toPlace.Stack;
			foreach (ClickableComponent item in this.inventory)
			{
				int slotNumber2 = Convert.ToInt32(item.name);
				if (slotNumber2 >= this.actualInventory.Count || this.actualInventory[slotNumber2] == null || !this.highlightMethod(this.actualInventory[slotNumber2]) || !this.actualInventory[slotNumber2].canStackWith(toPlace))
				{
					continue;
				}
				toPlace.Stack = this.actualInventory[slotNumber2].addToStack(toPlace);
				if (toPlace.Stack > 0)
				{
					continue;
				}
				try
				{
					Game1.playSound(sound);
					if (this.onAddItem != null)
					{
						this.onAddItem(toPlace, this.playerInventory ? Game1.player : null);
					}
				}
				catch (Exception)
				{
				}
				return null;
			}
			foreach (ClickableComponent item2 in this.inventory)
			{
				int slotNumber = Convert.ToInt32(item2.name);
				if (slotNumber >= this.actualInventory.Count || (this.actualInventory[slotNumber] != null && !this.highlightMethod(this.actualInventory[slotNumber])) || this.actualInventory[slotNumber] != null)
				{
					continue;
				}
				if (!string.IsNullOrEmpty(sound))
				{
					try
					{
						Game1.playSound(sound);
					}
					catch (Exception)
					{
					}
				}
				return Utility.addItemToInventory(toPlace, slotNumber, this.actualInventory, this.onAddItem);
			}
			if (toPlace.Stack < originalStack)
			{
				Game1.playSound(sound);
			}
			return toPlace;
		}

		public int getInventoryPositionOfClick(int x, int y)
		{
			for (int i = 0; i < this.inventory.Count; i++)
			{
				if (this.inventory[i] != null && this.inventory[i].bounds.Contains(x, y))
				{
					return Convert.ToInt32(this.inventory[i].name);
				}
			}
			return -1;
		}

		public Item leftClick(int x, int y, Item toPlace, bool playSound = true)
		{
			foreach (ClickableComponent c in this.inventory)
			{
				if (!c.containsPoint(x, y))
				{
					continue;
				}
				int slotNumber = Convert.ToInt32(c.name);
				if (slotNumber >= this.actualInventory.Count || (this.actualInventory[slotNumber] != null && !this.highlightMethod(this.actualInventory[slotNumber]) && !this.actualInventory[slotNumber].canStackWith(toPlace)))
				{
					continue;
				}
				if (this.actualInventory[slotNumber] != null)
				{
					if (toPlace != null)
					{
						if (playSound)
						{
							Game1.playSound("stoneStep");
						}
						return Utility.addItemToInventory(toPlace, slotNumber, this.actualInventory, this.onAddItem);
					}
					if (playSound)
					{
						Game1.playSound(this.moveItemSound);
					}
					return Utility.removeItemFromInventory(slotNumber, this.actualInventory);
				}
				if (toPlace != null)
				{
					if (playSound)
					{
						Game1.playSound("stoneStep");
					}
					return Utility.addItemToInventory(toPlace, slotNumber, this.actualInventory, this.onAddItem);
				}
			}
			return toPlace;
		}

		public Vector2 snapToClickableComponent(int x, int y)
		{
			foreach (ClickableComponent c in this.inventory)
			{
				if (c.containsPoint(x, y))
				{
					return new Vector2(c.bounds.X, c.bounds.Y);
				}
			}
			return new Vector2(x, y);
		}

		public Item getItemAt(int x, int y)
		{
			foreach (ClickableComponent c in this.inventory)
			{
				if (c.containsPoint(x, y))
				{
					return this.getItemFromClickableComponent(c);
				}
			}
			return null;
		}

		public Item getItemFromClickableComponent(ClickableComponent c)
		{
			if (c != null)
			{
				int slotNumber = Convert.ToInt32(c.name);
				if (slotNumber < this.actualInventory.Count)
				{
					return this.actualInventory[slotNumber];
				}
			}
			return null;
		}

		public Item rightClick(int x, int y, Item toAddTo, bool playSound = true, bool onlyCheckToolAttachments = false)
		{
			foreach (ClickableComponent item in this.inventory)
			{
				int slotNumber = Convert.ToInt32(item.name);
				if (!item.containsPoint(x, y) || slotNumber >= this.actualInventory.Count || (this.actualInventory[slotNumber] != null && !this.highlightMethod(this.actualInventory[slotNumber])) || slotNumber >= this.actualInventory.Count || this.actualInventory[slotNumber] == null)
				{
					continue;
				}
				if (this.actualInventory[slotNumber] is Tool && (toAddTo == null || toAddTo is Object) && (this.actualInventory[slotNumber] as Tool).canThisBeAttached((Object)toAddTo))
				{
					return (this.actualInventory[slotNumber] as Tool).attach((toAddTo == null) ? null : ((Object)toAddTo));
				}
				if (onlyCheckToolAttachments)
				{
					return toAddTo;
				}
				if (toAddTo == null)
				{
					if (this.actualInventory[slotNumber].maximumStackSize() != -1)
					{
						if (slotNumber == Game1.player.CurrentToolIndex && this.actualInventory[slotNumber] != null && this.actualInventory[slotNumber].Stack == 1)
						{
							this.actualInventory[slotNumber].actionWhenStopBeingHeld(Game1.player);
						}
						Item tmp = this.actualInventory[slotNumber].getOne();
						if (this.actualInventory[slotNumber].Stack > 1 && Game1.isOneOfTheseKeysDown(Game1.oldKBState, new InputButton[1]
						{
							new InputButton(Keys.LeftShift)
						}))
						{
							tmp.Stack = (int)Math.Ceiling((double)this.actualInventory[slotNumber].Stack / 2.0);
							this.actualInventory[slotNumber].Stack = this.actualInventory[slotNumber].Stack / 2;
						}
						else if (this.actualInventory[slotNumber].Stack == 1)
						{
							this.actualInventory[slotNumber] = null;
						}
						else
						{
							this.actualInventory[slotNumber].Stack--;
						}
						if (this.actualInventory[slotNumber] != null && this.actualInventory[slotNumber].Stack <= 0)
						{
							this.actualInventory[slotNumber] = null;
						}
						if (playSound)
						{
							Game1.playSound(this.moveItemSound);
						}
						return tmp;
					}
				}
				else
				{
					if (!this.actualInventory[slotNumber].canStackWith(toAddTo) || toAddTo.Stack >= toAddTo.maximumStackSize())
					{
						continue;
					}
					if (Game1.isOneOfTheseKeysDown(Game1.oldKBState, new InputButton[1]
					{
						new InputButton(Keys.LeftShift)
					}))
					{
						int amountToAdd = (int)Math.Ceiling((double)this.actualInventory[slotNumber].Stack / 2.0);
						amountToAdd = Math.Min(toAddTo.maximumStackSize() - toAddTo.Stack, amountToAdd);
						toAddTo.Stack += amountToAdd;
						this.actualInventory[slotNumber].Stack -= amountToAdd;
					}
					else
					{
						toAddTo.Stack++;
						this.actualInventory[slotNumber].Stack--;
					}
					if (playSound)
					{
						Game1.playSound(this.moveItemSound);
					}
					if (this.actualInventory[slotNumber].Stack <= 0)
					{
						if (slotNumber == Game1.player.CurrentToolIndex)
						{
							this.actualInventory[slotNumber].actionWhenStopBeingHeld(Game1.player);
						}
						this.actualInventory[slotNumber] = null;
					}
					return toAddTo;
				}
			}
			return toAddTo;
		}

		public Item hover(int x, int y, Item heldItem)
		{
			this.descriptionText = "";
			this.descriptionTitle = "";
			this.hoverText = "";
			this.hoverTitle = "";
			Item toReturn = null;
			foreach (ClickableComponent c in this.inventory)
			{
				int slotNumber = Convert.ToInt32(c.name);
				c.scale = Math.Max(1f, c.scale - 0.025f);
				if (c.containsPoint(x, y) && slotNumber < this.actualInventory.Count && (this.actualInventory[slotNumber] == null || this.highlightMethod(this.actualInventory[slotNumber])) && slotNumber < this.actualInventory.Count && this.actualInventory[slotNumber] != null)
				{
					this.descriptionTitle = this.actualInventory[slotNumber].DisplayName;
					this.descriptionText = Environment.NewLine + this.actualInventory[slotNumber].getDescription();
					c.scale = Math.Min(c.scale + 0.05f, 1.1f);
					string s = this.actualInventory[slotNumber].getHoverBoxText(heldItem);
					if (s != null)
					{
						this.hoverText = s;
						this.hoverTitle = this.actualInventory[slotNumber].DisplayName;
					}
					else
					{
						this.hoverText = this.actualInventory[slotNumber].getDescription();
						this.hoverTitle = this.actualInventory[slotNumber].DisplayName;
					}
					if (toReturn == null)
					{
						toReturn = this.actualInventory[slotNumber];
					}
				}
			}
			if (toReturn != null && toReturn is Object && (Game1.getLocationFromName("CommunityCenter") as CommunityCenter).couldThisIngredienteBeUsedInABundle(toReturn as Object))
			{
				GameMenu.bundleItemHovered = true;
			}
			return toReturn;
		}

		public override void setUpForGamePadMode()
		{
			base.setUpForGamePadMode();
			if (this.inventory != null && this.inventory.Count > 0)
			{
				Game1.setMousePosition(this.inventory[0].bounds.Right - this.inventory[0].bounds.Width / 8, this.inventory[0].bounds.Bottom - this.inventory[0].bounds.Height / 8);
			}
		}

		public override void draw(SpriteBatch b)
		{
			this.draw(b, -1, -1, -1);
		}

		public override void draw(SpriteBatch b, int red = -1, int green = -1, int blue = -1)
		{
			for (int i = 0; i < this.inventory.Count; i++)
			{
				if (this._iconShakeTimer.ContainsKey(i) && Game1.currentGameTime.TotalGameTime.TotalSeconds >= this._iconShakeTimer[i])
				{
					this._iconShakeTimer.Remove(i);
				}
			}
			Color tint = ((red == -1) ? Color.White : new Color((int)Utility.Lerp(red, Math.Min(255, red + 150), 0.65f), (int)Utility.Lerp(green, Math.Min(255, green + 150), 0.65f), (int)Utility.Lerp(blue, Math.Min(255, blue + 150), 0.65f)));
			Texture2D texture = ((red == -1) ? Game1.menuTexture : Game1.uncoloredMenuTexture);
			if (this.drawSlots)
			{
				for (int l = 0; l < this.capacity; l++)
				{
					Vector2 toDraw = new Vector2(base.xPositionOnScreen + l % (this.capacity / this.rows) * 64 + this.horizontalGap * (l % (this.capacity / this.rows)), base.yPositionOnScreen + l / (this.capacity / this.rows) * (64 + this.verticalGap) + (l / (this.capacity / this.rows) - 1) * 4 - ((l < this.capacity / this.rows && this.playerInventory && this.verticalGap == 0) ? 12 : 0));
					b.Draw(texture, toDraw, Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 10), tint, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
					if ((this.playerInventory || this.showGrayedOutSlots) && l >= (int)Game1.player.maxItems)
					{
						b.Draw(texture, toDraw, Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 57), tint * 0.5f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
					}
					if (l < 12 && this.playerInventory)
					{
						string strToDraw = l switch
						{
							11 => "=",
							10 => "-",
							9 => "0",
							_ => string.Concat(l + 1),
						};
						Vector2 strSize = Game1.tinyFont.MeasureString(strToDraw);
						b.DrawString(Game1.tinyFont, strToDraw, toDraw + new Vector2(32f - strSize.X / 2f, 0f - strSize.Y), (l == Game1.player.CurrentToolIndex) ? Color.Red : Color.DimGray);
					}
				}
				for (int k = 0; k < this.capacity; k++)
				{
					Vector2 toDraw2 = new Vector2(base.xPositionOnScreen + k % (this.capacity / this.rows) * 64 + this.horizontalGap * (k % (this.capacity / this.rows)), base.yPositionOnScreen + k / (this.capacity / this.rows) * (64 + this.verticalGap) + (k / (this.capacity / this.rows) - 1) * 4 - ((k < this.capacity / this.rows && this.playerInventory && this.verticalGap == 0) ? 12 : 0));
					if (this.actualInventory.Count > k && this.actualInventory.ElementAt(k) != null)
					{
						bool highlight2 = this.highlightMethod(this.actualInventory[k]);
						if (this._iconShakeTimer.ContainsKey(k))
						{
							toDraw2 += 1f * new Vector2(Game1.random.Next(-1, 2), Game1.random.Next(-1, 2));
						}
						this.actualInventory[k].drawInMenu(b, toDraw2, (this.inventory.Count > k) ? this.inventory[k].scale : 1f, (!this.highlightMethod(this.actualInventory[k])) ? 0.25f : 1f, 0.865f, StackDrawType.Draw, Color.White, highlight2);
					}
				}
				return;
			}
			for (int j = 0; j < this.capacity; j++)
			{
				Vector2 toDraw3 = new Vector2(base.xPositionOnScreen + j % (this.capacity / this.rows) * 64 + this.horizontalGap * (j % (this.capacity / this.rows)), base.yPositionOnScreen + j / (this.capacity / this.rows) * (64 + this.verticalGap) + (j / (this.capacity / this.rows) - 1) * 4 - ((j < this.capacity / this.rows && this.playerInventory && this.verticalGap == 0) ? 12 : 0));
				if (this.actualInventory.Count > j && this.actualInventory.ElementAt(j) != null)
				{
					bool highlight = this.highlightMethod(this.actualInventory[j]);
					if (this._iconShakeTimer.ContainsKey(j))
					{
						toDraw3 += 1f * new Vector2(Game1.random.Next(-1, 2), Game1.random.Next(-1, 2));
					}
					this.actualInventory[j].drawInMenu(b, toDraw3, (this.inventory.Count > j) ? this.inventory[j].scale : 1f, (!highlight) ? 0.25f : 1f, 0.865f, StackDrawType.Draw, Color.White, highlight);
				}
			}
		}

		public List<Vector2> GetSlotDrawPositions()
		{
			List<Vector2> slot_draw_positions = new List<Vector2>();
			for (int i = 0; i < this.capacity; i++)
			{
				slot_draw_positions.Add(new Vector2(base.xPositionOnScreen + i % (this.capacity / this.rows) * 64 + this.horizontalGap * (i % (this.capacity / this.rows)), base.yPositionOnScreen + i / (this.capacity / this.rows) * (64 + this.verticalGap) + (i / (this.capacity / this.rows) - 1) * 4 - ((i < this.capacity / this.rows && this.playerInventory && this.verticalGap == 0) ? 12 : 0)));
			}
			return slot_draw_positions;
		}

		public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			base.gameWindowSizeChanged(oldBounds, newBounds);
		}

		public override void receiveLeftClick(int x, int y, bool playSound = true)
		{
		}

		public override void receiveRightClick(int x, int y, bool playSound = true)
		{
		}

		public override void performHoverAction(int x, int y)
		{
		}
		*/
	}
}
