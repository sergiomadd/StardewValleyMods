using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace ChestPreview.Framework.APIs
{
    public interface IChestPreviewAPI
    {
        void LoadIDs(List<int> list);
        void DrawInPreview(int id, Action<SpriteBatch, Vector2, float, float, float, StackDrawType, Color, bool> draw);
    }
}
