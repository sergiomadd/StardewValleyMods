using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace ItemPipes.Framework.APIs
{
    public interface IChestPreviewAPI
    {
        void SendIDs(List<int> list);
        void DrawInMenu(int id, Action<SpriteBatch, Vector2, float, float, float, StackDrawType, Color, bool> draw);

    }
}
