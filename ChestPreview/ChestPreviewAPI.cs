using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaddUtil;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace ChestPreview
{
    public class ChestPreviewAPI : IChestPreviewAPI
    {
        public void SendIDs(List<int> list)
        {
            Printer.Info("in api");
            Printer.Info(list.Count.ToString());
            ModEntry.ModdedIDs.AddRange(list);
        }
        //Pasar draw function?
        public void DrawInMenu(int id, Action<SpriteBatch, Vector2, float, float, float, StackDrawType, Color, bool> draw)
        {
            ModEntry.DrawFunctions.Add(id, draw);
        }
    }
}
