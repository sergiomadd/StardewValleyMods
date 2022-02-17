using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemPipes.Framework.Util;
using ItemPipes.Framework.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;
using StardewValley;

namespace ItemPipes.Framework.Items
{
    [XmlType("Mods_sergiomadd.ItemPipes_IridiumPipeItem")]
    public class IridiumPipeItem : ConnectorItem
    {
        public int Stage { get; set; }
        //Stage 1
        [XmlIgnore]
        public Texture2D ItemMovingSprite1 { get; set; }
        [XmlIgnore]
        public Texture2D DefaultSprite1 { get; set; }
        [XmlIgnore]
        public Texture2D ConnectingSprite1 { get; set; }
        [XmlIgnore]
        public Texture2D ItemMovingSprite_passable1 { get; set; }
        [XmlIgnore]
        public Texture2D DefaultSprite_passable1 { get; set; }
        [XmlIgnore]
        public Texture2D ConnectingSprite_passable1 { get; set; }
        [XmlIgnore]
        public Texture2D ItemTexture1 { get; set; }
        [XmlIgnore]
        public Texture2D SpriteTexture1 { get; set; }
        //Stage 2
        [XmlIgnore]
        public Texture2D ItemMovingSprite2 { get; set; }
        [XmlIgnore]
        public Texture2D DefaultSprite2 { get; set; }
        [XmlIgnore]
        public Texture2D ConnectingSprite2 { get; set; }
        [XmlIgnore]
        public Texture2D ItemMovingSprite_passable2 { get; set; }
        [XmlIgnore]
        public Texture2D DefaultSprite_passable2 { get; set; }
        [XmlIgnore]
        public Texture2D ConnectingSprite_passable2 { get; set; }
        [XmlIgnore]
        public Texture2D ItemTexture2 { get; set; }
        [XmlIgnore]
        public Texture2D SpriteTexture2 { get; set; }
        //Stage 3
        [XmlIgnore]
        public Texture2D ItemMovingSprite3 { get; set; }
        [XmlIgnore]
        public Texture2D DefaultSprite3 { get; set; }
        [XmlIgnore]
        public Texture2D ConnectingSprite3 { get; set; }
        [XmlIgnore]
        public Texture2D ItemMovingSprite_passable3 { get; set; }
        [XmlIgnore]
        public Texture2D DefaultSprite_passable3 { get; set; }
        [XmlIgnore]
        public Texture2D ConnectingSprite_passable3 { get; set; }
        [XmlIgnore]
        public Texture2D ItemTexture3 { get; set; }
        [XmlIgnore]
        public Texture2D SpriteTexture3 { get; set; }

        public IridiumPipeItem() : base()
        {
            Stage = 1;
            Name = "Iridium Pipe";
            IDName = "IridiumPipe";
            Description = "Type: Connector Pipe\nThe link between IO pipes. It moves items at 5 tiles/1 second.";
            LoadStages();
            LoadTextures();
        }

        public IridiumPipeItem(Vector2 position) : base(position)
        {
            Stage = 1;
            Name = "Iridium Pipe";
            IDName = "IridiumPipe";
            Description = "Type: Connector Pipe\nThe link between IO pipes. It moves items at 5 tiles/1 second.";
            LoadStages();
            LoadTextures();
        }

        public void LoadStages()
        {
            //Stage 1
            ItemMovingSprite1 = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{IDName}/1/{IDName}_item_Sprite.png");
            DefaultSprite1 = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{IDName}/1/{IDName}_default_Sprite.png");
            ConnectingSprite1 = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{IDName}/1/{IDName}_connecting_Sprite.png");
            ItemMovingSprite_passable1 = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{IDName}/1/{IDName}_item_passable_Sprite.png");
            DefaultSprite_passable1 = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{IDName}/1/{IDName}_default_passable_Sprite.png");
            ConnectingSprite_passable1 = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{IDName}/1/{IDName}_connecting_passable_Sprite.png");
            ItemTexture1 = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{IDName}/1/{IDName}_Item.png");
            SpriteTexture1 = DefaultSprite;

            //Stage 2
            ItemMovingSprite2 = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{IDName}/2/{IDName}_item_Sprite.png");
            DefaultSprite2 = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{IDName}/2/{IDName}_default_Sprite.png");
            ConnectingSprite2 = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{IDName}/2/{IDName}_connecting_Sprite.png");
            ItemMovingSprite_passable2 = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{IDName}/2/{IDName}_item_passable_Sprite.png");
            DefaultSprite_passable2 = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{IDName}/2/{IDName}_default_passable_Sprite.png");
            ConnectingSprite_passable2 = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{IDName}/2/{IDName}_connecting_passable_Sprite.png");
            ItemTexture2 = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{IDName}/2/{IDName}_Item.png");
            SpriteTexture2 = DefaultSprite2;

            //Stage 3
            ItemMovingSprite3 = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{IDName}/3/{IDName}_item_Sprite.png");
            DefaultSprite3 = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{IDName}/3/{IDName}_default_Sprite.png");
            ConnectingSprite3 = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{IDName}/3/{IDName}_connecting_Sprite.png");
            ItemMovingSprite_passable3 = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{IDName}/3/{IDName}_item_passable_Sprite.png");
            DefaultSprite_passable3 = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{IDName}/3/{IDName}_default_passable_Sprite.png");
            ConnectingSprite_passable3 = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{IDName}/3/{IDName}_connecting_passable_Sprite.png");
            ItemTexture3 = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{IDName}/3/{IDName}_Item.png");
            SpriteTexture3 = DefaultSprite3;
        }

        public override void LoadTextures()
        {
            ItemTexture = ItemTexture1;
            SpriteTexture = SpriteTexture1;
            ItemMovingSprite = ItemMovingSprite1;
            DefaultSprite = DefaultSprite1;
            ConnectingSprite = ConnectingSprite1;
            ItemMovingSprite_passable = ItemMovingSprite_passable1;
            DefaultSprite_passable = DefaultSprite_passable1;
            ConnectingSprite_passable = ConnectingSprite_passable1;
            ItemTexture = ItemTexture1;
            SpriteTexture = SpriteTexture1;
        }

        public void StageChange()
        {
            if (((int)Game1.currentGameTime.TotalGameTime.TotalSeconds) % 2 == 0 && ((int)Game1.currentGameTime.TotalGameTime.TotalSeconds) % 3 == 0)
            {
                Stage = 3;
                ItemTexture = ItemTexture3;
                SpriteTexture = SpriteTexture3;
                ItemMovingSprite = ItemMovingSprite3;
                DefaultSprite = DefaultSprite3;
                ConnectingSprite = ConnectingSprite3;
                ItemMovingSprite_passable = ItemMovingSprite_passable3;
                DefaultSprite_passable = DefaultSprite_passable3;
                ConnectingSprite_passable = ConnectingSprite_passable3;
            }
            else if (((int)Game1.currentGameTime.TotalGameTime.TotalSeconds) % 2 == 0)
            {
                Stage = 2;
                ItemTexture = ItemTexture2;
                SpriteTexture = SpriteTexture2;
                ItemMovingSprite = ItemMovingSprite2;
                DefaultSprite = DefaultSprite2;
                ConnectingSprite = ConnectingSprite2;
                ItemMovingSprite_passable = ItemMovingSprite_passable2;
                DefaultSprite_passable = DefaultSprite_passable2;
                ConnectingSprite_passable = ConnectingSprite_passable2;
            }
            else
            {
                Stage = 1;
                ItemTexture = ItemTexture1;
                SpriteTexture = SpriteTexture1;
                ItemMovingSprite = ItemMovingSprite1;
                DefaultSprite = DefaultSprite1;
                ConnectingSprite = ConnectingSprite1;
                ItemMovingSprite_passable = ItemMovingSprite_passable1;
                DefaultSprite_passable = DefaultSprite_passable1;
                ConnectingSprite_passable = ConnectingSprite_passable1;
            }
            
        }
        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            StageChange();
            Rectangle srcRect = new Rectangle(0, 0, 16, 16);
            spriteBatch.Draw(ItemTexture, objectPosition, srcRect, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0f, (float)(f.getStandingY() + 3) / 10000f));
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
        {
            StageChange();
            bool shouldDrawStackNumber = ((drawStackNumber == StackDrawType.Draw && this.maximumStackSize() > 1 && this.Stack > 1)
                || drawStackNumber == StackDrawType.Draw_OneInclusive) && (double)scaleSize > 0.3 && this.Stack != int.MaxValue;
            Rectangle srcRect = new Rectangle(0, 0, 16, 16);
            spriteBatch.Draw(ItemTexture, location + new Vector2((int)(32f * scaleSize), (int)(32f * scaleSize)), srcRect, color * transparency, 0f,
                new Vector2(8f, 8f) * scaleSize, 4f * scaleSize, SpriteEffects.None, layerDepth);

            if (shouldDrawStackNumber)
            {
                var loc = location + new Vector2((float)(64 - Utility.getWidthOfTinyDigitString(this.Stack, 3f * scaleSize)) + 3f * scaleSize, 64f - 18f * scaleSize + 2f);
                Utility.drawTinyDigits(this.Stack, spriteBatch, loc, 3f * scaleSize, 1f, color);

            }
        }

        public override void drawAsProp(SpriteBatch b)
        {
            StageChange();
            int x = (int)this.TileLocation.X;
            int y = (int)this.TileLocation.Y;

            Vector2 scaleFactor = Vector2.One;
            scaleFactor *= 4f;
            Vector2 position = Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64));
            Rectangle srcRect = new Rectangle(16 * 2, 0, 16, 16);
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
            StageChange();
            base.draw(spriteBatch, xNonTile, yNonTile, layerDepth, alpha);
        }
        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1)
        {
            StageChange();
            int drawSum = this.getDrawSum(Game1.currentLocation);
            int sourceRectPosition = GetNewDrawGuide()[drawSum];
            DataAccess DataAccess = DataAccess.GetDataAccess();
            if (DataAccess.LocationNodes.ContainsKey(Game1.currentLocation))
            {
                List<Node> nodes = DataAccess.LocationNodes[Game1.currentLocation];
                Node node = nodes.Find(n => n.Position.Equals(TileLocation));
                if (node != null && node is ConnectorNode)
                {
                    ConnectorNode pipe = (ConnectorNode)node;
                    if (pipe.Passable)
                    {
                        Passable = true;
                        if (pipe.StoredItem != null)
                        {
                            SpriteTexture = ItemMovingSprite_passable;
                            spriteBatch.Draw(SpriteTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64)), new Rectangle(sourceRectPosition * Fence.fencePieceWidth % SpriteTexture.Bounds.Width, sourceRectPosition * Fence.fencePieceWidth / SpriteTexture.Bounds.Width * Fence.fencePieceHeight, Fence.fencePieceWidth, Fence.fencePieceHeight), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, ((float)(y * 64 + 32) / 10000f) + 0.001f);
                            drawItem(pipe, spriteBatch, x, y, alpha);
                        }
                        else if (State == "connecting")
                        {
                            SpriteTexture = ConnectingSprite_passable;
                            spriteBatch.Draw(SpriteTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64)), new Rectangle(sourceRectPosition * Fence.fencePieceWidth % SpriteTexture.Bounds.Width, sourceRectPosition * Fence.fencePieceWidth / SpriteTexture.Bounds.Width * Fence.fencePieceHeight, Fence.fencePieceWidth, Fence.fencePieceHeight), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, ((float)(y * 64 + 32) / 10000f) + 0.001f);
                        }
                        else
                        {
                            SpriteTexture = DefaultSprite_passable;
                            spriteBatch.Draw(SpriteTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64)), new Rectangle(sourceRectPosition * Fence.fencePieceWidth % SpriteTexture.Bounds.Width, sourceRectPosition * Fence.fencePieceWidth / SpriteTexture.Bounds.Width * Fence.fencePieceHeight, Fence.fencePieceWidth, Fence.fencePieceHeight), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, ((float)(y * 64 + 32) / 10000f) + 0.001f);
                        }
                    }
                    else if (!pipe.Passable)
                    {
                        Passable = false;
                        if (pipe.StoredItem != null)
                        {
                            SpriteTexture = ItemMovingSprite;
                            spriteBatch.Draw(SpriteTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64)), new Rectangle(sourceRectPosition * Fence.fencePieceWidth % SpriteTexture.Bounds.Width, sourceRectPosition * Fence.fencePieceWidth / SpriteTexture.Bounds.Width * Fence.fencePieceHeight, Fence.fencePieceWidth, Fence.fencePieceHeight), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, ((float)(y * 64 + 32) / 10000f) + 0.001f);
                            drawItem(pipe, spriteBatch, x, y, alpha);
                        }
                        else if (State == "connecting")
                        {
                            SpriteTexture = ConnectingSprite;
                            spriteBatch.Draw(SpriteTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64)), new Rectangle(sourceRectPosition * Fence.fencePieceWidth % SpriteTexture.Bounds.Width, sourceRectPosition * Fence.fencePieceWidth / SpriteTexture.Bounds.Width * Fence.fencePieceHeight, Fence.fencePieceWidth, Fence.fencePieceHeight), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, ((float)(y * 64 + 32) / 10000f) + 0.001f);
                        }
                        else
                        {
                            SpriteTexture = DefaultSprite;
                            spriteBatch.Draw(SpriteTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64)), new Rectangle(sourceRectPosition * Fence.fencePieceWidth % SpriteTexture.Bounds.Width, sourceRectPosition * Fence.fencePieceWidth / SpriteTexture.Bounds.Width * Fence.fencePieceHeight, Fence.fencePieceWidth, Fence.fencePieceHeight), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, ((float)(y * 64 + 32) / 10000f) + 0.001f);
                        }
                    }
                }
            }
        }
    }
}
