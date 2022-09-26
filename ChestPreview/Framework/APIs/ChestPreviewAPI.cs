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
        public string GetPreviewSizeString()
        {
            return ModEntry.config.Size;
        }
        public int GetPreviewSizeInt()
        {
            return (int)ModEntry.CurrentSize;
        }
        public float GetPreviewScale()
        {
            return ModEntry.GetSizeValue();
        }
        public void LoadIDs(List<int> list)
        {
            ModEntry.ModdedIDs.AddRange(list);
        }
    }
}
