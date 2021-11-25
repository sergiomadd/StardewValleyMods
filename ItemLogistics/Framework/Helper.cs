using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;

namespace ItemLogistics.Framework
{
    class Helper
    {
        private static IModHelper _helper;

        public static void SetHelper(IModHelper helper)
        {
            _helper = helper;
        }

        public static IModHelper GetHelper()
        {
            return _helper;
        }
    }
}
