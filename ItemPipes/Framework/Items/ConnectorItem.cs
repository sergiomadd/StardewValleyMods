using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ItemPipes.Framework.Util;
using ItemPipes.Framework.Model;
using StardewValley;

namespace ItemPipes.Framework.Items
{
    public class ConnectorItem : PipeItem
    {
        public ConnectorItem() : base()
        {

        }
        public ConnectorItem(Vector2 position) : base(position)
        {

        }

        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1)
        {
            DataAccess DataAccess = DataAccess.GetDataAccess();
            List<Node> nodes;
            if (DataAccess.LocationNodes.TryGetValue(Game1.currentLocation, out nodes))
            {
                Node node = nodes.Find(n => n.Position.Equals(TileLocation));
                if (node != null && node is ConnectorNode)
                {
                    ConnectorNode pipe = (ConnectorNode)nodes.Find(n => n.Position.Equals(this.TileLocation));
                    int sourceRectPosition = 1;
                    int drawSum = getDrawSum(Game1.currentLocation);
                    sourceRectPosition = GetNewDrawGuide()[drawSum];
                    SpriteTexture = Helper.GetHelper().Content.Load<Texture2D>($"assets/Pipes/{IDName}/{IDName}_{pipe.GetState()}_Sprite.png");
                    spriteBatch.Draw(SpriteTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64)), new Rectangle(sourceRectPosition * Fence.fencePieceWidth % SpriteTexture.Bounds.Width, sourceRectPosition * Fence.fencePieceWidth / SpriteTexture.Bounds.Width * Fence.fencePieceHeight, Fence.fencePieceWidth, Fence.fencePieceHeight), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, ((float)(y * 64 + 32) / 10000f) + 0.001f);
                }
            }
        }
    }
}
