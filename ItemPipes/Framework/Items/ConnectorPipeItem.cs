using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ItemPipes.Framework.Util;
using ItemPipes.Framework.Model;
using ItemPipes.Framework.Nodes;
using StardewValley;
using StardewValley.Tools;
using StardewValley.Objects;
using SObject = StardewValley.Object;
using System.Threading;
using System.Xml.Serialization;


namespace ItemPipes.Framework.Items
{
    public abstract class ConnectorPipeItem : PipeItem
    {
        [XmlIgnore]
        public Texture2D ItemMovingSprite { get; set; }

        public ConnectorPipeItem() : base()
        {
            
        }
        public ConnectorPipeItem(Vector2 position) : base(position)
        {
            
        }
        public override void LoadTextures()
        {
            base.LoadTextures();
            ItemMovingSprite = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{IDName}/{IDName}_item_Sprite.png");
        }

        public override bool performToolAction(Tool t, GameLocation location)
        {
            if (t is Pickaxe)
            {
                var who = t.getLastFarmerToUse();
                this.performRemoveAction(this.TileLocation, location);
                Debris deb = new Debris(getOne(), who.GetToolLocation(), new Vector2(who.GetBoundingBox().Center.X, who.GetBoundingBox().Center.Y));
                Game1.currentLocation.debris.Add(deb);
                DataAccess DataAccess = DataAccess.GetDataAccess();
                List<Node> nodes = DataAccess.LocationNodes[Game1.currentLocation];
                Node node = nodes.Find(n => n.Position.Equals(TileLocation));
                if (node != null && node is ConnectorPipeNode)
                {
                    ConnectorPipeNode pipe = (ConnectorPipeNode)node;
                    if (pipe.StoredItem != null)
                    {
                            
                        Printer.Info($"[T{Thread.CurrentThread.ManagedThreadId}] GET OUT");
                        Printer.Info($"[T{Thread.CurrentThread.ManagedThreadId}] "+pipe.StoredItem.Stack.ToString());
                        pipe.Print();
                        Debris itemDebr = new Debris(pipe.StoredItem, who.GetToolLocation(), new Vector2(who.GetBoundingBox().Center.X, who.GetBoundingBox().Center.Y));
                        Game1.currentLocation.debris.Add(itemDebr);
                        pipe.Broken = true;
                    }
                }
                Game1.currentLocation.objects.Remove(this.TileLocation);
                return false;
            }
            return false;
        }

        public override bool countsForDrawing(SObject adj)
        {
            if (adj is PipeItem && !(adj is ConnectorPipeItem))
            {
                return true;
            }
            else if(adj is PipeItem && adj is ConnectorPipeItem)
            {
                if(adj.GetType().Equals(this.GetType()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1)
        {
            DataAccess DataAccess = DataAccess.GetDataAccess();
            if (DataAccess.LocationNodes.ContainsKey(Game1.currentLocation))
            {
                List<Node> nodes = DataAccess.LocationNodes[Game1.currentLocation];
                Node node = nodes.Find(n => n.Position.Equals(TileLocation));
                if (node != null && node is ConnectorPipeNode)
                {
                    ConnectorPipeNode pipe = (ConnectorPipeNode)node;
                    if (pipe.StoredItem != null)
                    {
                        SpriteTexture = ItemMovingSprite;
                        base.draw(spriteBatch, x, y);
                        drawItem(pipe, spriteBatch, x, y);
                    }
                    else if (State == "connecting")
                    {
                        SpriteTexture = ConnectingSprite;
                        base.draw(spriteBatch, x, y);
                    }
                    else
                    {
                        SpriteTexture = DefaultSprite;
                        base.draw(spriteBatch, x, y);
                    }
                }
            }
        }

        public void drawItem(PipeNode pipe, SpriteBatch spriteBatch, int x, int y)
        {
            Item item = pipe.StoredItem;
            Texture2D SpriteSheet;
            Rectangle srcRect;
            Vector2 originalPosition;
            Vector2 position;
            //How to handle drawing custom mod items
            if (item is PipeItem)
            {
                PipeItem pipeItem = (PipeItem)item;
                SpriteSheet = pipeItem.ItemTexture;
                srcRect = new Rectangle(0, 0, 16, 16);
                originalPosition = Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64));
                position = new Vector2(originalPosition.X + 16, originalPosition.Y + 64 + 16);
                spriteBatch.Draw(SpriteSheet, position, srcRect, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None,
                    ((float)(y * 64 + 32) / 10000f) + 0.002f);
            }
            else if (item is SObject && (item as SObject).bigCraftable.Value)
            {
                SpriteSheet = Game1.bigCraftableSpriteSheet;
                srcRect = SObject.getSourceRectForBigCraftable(item.ParentSheetIndex);
                originalPosition = Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64));
                position = new Vector2(originalPosition.X + 23, originalPosition.Y + 64 + 10);
                spriteBatch.Draw(SpriteSheet, position, srcRect, Color.White, 0f, Vector2.Zero, 1.2f, SpriteEffects.None,
                    ((float)(y * 64 + 32) / 10000f) + 0.002f);
            }
            else if (item is Tool)
            {
                Tool tool = (Tool)item;
                if (item is MeleeWeapon || item is Slingshot || item is Sword)
                {
                    SpriteSheet = Tool.weaponsTexture;
                    srcRect = Game1.getSquareSourceRectForNonStandardTileSheet(SpriteSheet, 16, 16, tool.IndexOfMenuItemView);
                    originalPosition = Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64));
                    position = new Vector2(originalPosition.X + 19, originalPosition.Y + 64 + 19);
                    spriteBatch.Draw(SpriteSheet, position, srcRect, Color.White, 0f, Vector2.Zero, 1.7f, SpriteEffects.None,
                        ((float)(y * 64 + 32) / 10000f) + 0.002f);
                }
                else
                {
                    SpriteSheet = Game1.toolSpriteSheet;
                    srcRect = Game1.getSquareSourceRectForNonStandardTileSheet(SpriteSheet, 16, 16, tool.IndexOfMenuItemView);
                    originalPosition = Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64));
                    position = new Vector2(originalPosition.X + 19, originalPosition.Y + 64 + 18);
                    spriteBatch.Draw(SpriteSheet, position, srcRect, Color.White, 0f, Vector2.Zero, 1.7f, SpriteEffects.None,
                        ((float)(y * 64 + 32) / 10000f) + 0.002f);
                }
            }
            //Boots = standard
            else if (item is Boots)
            {
                Boots boot = (Boots)item;
                SpriteSheet = Game1.objectSpriteSheet;
                srcRect = Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, boot.indexInTileSheet.Value, 16, 16);
                originalPosition = Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64));
                position = new Vector2(originalPosition.X + 18, originalPosition.Y + 64 + 16);
                spriteBatch.Draw(SpriteSheet, position, srcRect, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None,
                    ((float)(y * 64 + 32) / 10000f) + 0.002f);
            }
            //rings = standard
            else if (item is Ring)
            {
                SpriteSheet = Game1.objectSpriteSheet;
                srcRect = Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, item.ParentSheetIndex, 16, 16);
                originalPosition = Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64));
                position = new Vector2(originalPosition.X + 10, originalPosition.Y + 64 + 14);
                spriteBatch.Draw(SpriteSheet, position, srcRect, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None,
                    ((float)(y * 64 + 32) / 10000f) + 0.002f);
            }
            else if (item is Hat)
            {
                Hat hat = (Hat)item;
                SpriteSheet = FarmerRenderer.hatsTexture;
                srcRect = new Rectangle((int)hat.which * 20 % FarmerRenderer.hatsTexture.Width, (int)hat.which * 20 / FarmerRenderer.hatsTexture.Width * 20 * 4, 20, 20);
                originalPosition = Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64));
                position = new Vector2(originalPosition.X + 12, originalPosition.Y + 64 + 18);
                spriteBatch.Draw(SpriteSheet, position, srcRect, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None,
                    ((float)(y * 64 + 32) / 10000f) + 0.002f);
            }
            else if (item is Clothing)
            {
                Clothing cloth = (Clothing)item;
                Color clothes_color = cloth.clothesColor;
                if (cloth.isPrismatic.Value)
                {
                    clothes_color = Utility.GetPrismaticColor();
                }
                if (cloth.clothesType.Value == 0)
                {
                    SpriteSheet = FarmerRenderer.shirtsTexture;
                    srcRect = new Rectangle(cloth.indexInTileSheetMale.Value * 8 % 128, cloth.indexInTileSheetMale.Value * 8 / 128 * 32, 8, 8);
                    originalPosition = Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64));
                    position = new Vector2(originalPosition.X + 20, originalPosition.Y + 64 + 20);
                    spriteBatch.Draw(SpriteSheet, position, srcRect, clothes_color, 0f, Vector2.Zero, 3f, SpriteEffects.None,
                        ((float)(y * 64 + 32) / 10000f) + 0.002f);
                }
                else if (cloth.clothesType.Value == 1)
                {
                    SpriteSheet = FarmerRenderer.pantsTexture;
                    srcRect = new Rectangle(192 * (cloth.indexInTileSheetMale.Value % (FarmerRenderer.pantsTexture.Width / 192)), 688 * (cloth.indexInTileSheetMale.Value / (FarmerRenderer.pantsTexture.Width / 192)) + 672, 16, 16);
                    originalPosition = Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64));
                    position = new Vector2(originalPosition.X + 8, originalPosition.Y + 64 + 10);
                    spriteBatch.Draw(SpriteSheet, position, srcRect, clothes_color, 0f, Vector2.Zero, 3f, SpriteEffects.None,
                        ((float)(y * 64 + 32) / 10000f) + 0.002f);
                }
            }
            else
            {
                SpriteSheet = Game1.objectSpriteSheet;
                srcRect = Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, item.ParentSheetIndex, 16, 16);
                originalPosition = Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64));
                position = new Vector2(originalPosition.X + 17, originalPosition.Y + 64 + 17);
                spriteBatch.Draw(SpriteSheet, position, srcRect, Color.White, 0f, Vector2.Zero, 1.9f, SpriteEffects.None,
                    ((float)(y * 64 + 32) / 10000f) + 0.002f);
            }
        }
    }
}
