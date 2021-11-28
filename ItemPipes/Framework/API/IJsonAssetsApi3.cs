﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemPipes.Framework.API
{
    public interface IJsonAssetsApi3
    {
        int GetObjectId(string name);
        void LoadAssets(string path);
    }
}
