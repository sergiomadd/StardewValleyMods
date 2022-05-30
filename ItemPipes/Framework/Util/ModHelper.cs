using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;

namespace ItemPipes.Framework.Util
{
    public static class ModHelper
    {
        private static IModContentHelper _helper;

        public static void SetHelper(IModContentHelper helper)
        {
            _helper = helper;
        }

        public static IModContentHelper GetHelper()
        {
            return _helper;
        }
    }
}
