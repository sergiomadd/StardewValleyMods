﻿using System;
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
        string GetPreviewSizeString();
        int GetPreviewSizeInt();
        float GetPreviewScale();
    }
}
