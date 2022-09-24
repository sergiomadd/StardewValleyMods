using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaddUtil;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace ChestPreview.Framework.APIs
{
    public class ChestPreviewAPI : IChestPreviewAPI
    {
        public void LoadIDs(List<int> list)
        {
            Printer.Info("in api");
            Printer.Info(list.Count.ToString());
            ModEntry.ModdedIDs.AddRange(list);
        }
        public void DrawInPreview(int id, Action<SpriteBatch, Vector2, float, float, float, StackDrawType, Color, bool> draw)
        {
            ModEntry.DrawFunctions.Add(id, draw);
        }
    }
}
