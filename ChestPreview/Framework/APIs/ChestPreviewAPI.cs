using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChestPreview.Framework;

namespace ChestPreview.Framework.APIs
{
    public class ChestPreviewAPI : IChestPreviewAPI
    {
        public string GetPreviewSizeString()
        {
            return Conversor.GetSizeName(ModEntry.config.Size);
        }
        public int GetPreviewSizeInt()
        {
            return (int)ModEntry.CurrentSize;
        }
        public float GetPreviewScale()
        {
            return Conversor.GetCurrentSizeValue();
        }
    }
}
