using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;

namespace MaddUtil
{
    public static class Helpers
    {
        private static IModContentHelper modContentHelper;
        private static IGameContentHelper gameContentHelper;
        private static ITranslationHelper translationHelper;
        private static IModHelper modHelper;

        public static void SetModHelper(IModHelper helper)
        {
            modHelper = helper;
            SetContentHelper(helper.GameContent);
            SetModContentHelper(helper.ModContent);
            SetTranslationHelper(helper.Translation);
        }

        public static IModHelper GetModHelper()
        {
            return modHelper;
        }
        public static void SetModContentHelper(IModContentHelper helper)
        {
            modContentHelper = helper;
        }

        public static IModContentHelper GetModContentHelper()
        {
            return modContentHelper;
        }
        public static void SetContentHelper(IGameContentHelper helper)
        {
            gameContentHelper = helper;
        }

        public static IGameContentHelper GetContentHelper()
        {
            return gameContentHelper;
        }
        public static void SetTranslationHelper(ITranslationHelper helper)
        {
            translationHelper = helper;
        }

        public static ITranslationHelper GetTranslationHelper()
        {
            return translationHelper;
        }
    }
}
