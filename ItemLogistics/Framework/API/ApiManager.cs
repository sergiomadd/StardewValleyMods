using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;

namespace ItemLogistics.Framework.API
{
    public static class ApiManager
    {
        private static DynamicGameAssetsApi dynamicGameAssets;

        public static void HookIntoDynamicGameAssets()
        {
            // Attempt to hook into the IMobileApi interface
            dynamicGameAssets = Helper.GetHelper().ModRegistry.GetApi<DynamicGameAssetsApi>("spacechase0.DynamicGameAssets");

            if (dynamicGameAssets is null)
            {
                Printer.Info("Failed to hook into spacechase0.DynamicGameAssets.");
                return;
            }
            Printer.Info("Successfully hooked into spacechase0.DynamicGameAssets.");
        }

        public static DynamicGameAssetsApi GetDynamicGameAssetsInterface()
        {
            return dynamicGameAssets;
        }
    }
}
