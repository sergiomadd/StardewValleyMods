using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using StardewValley;
using StardewModdingAPI;

namespace ConnectedFences.Patches
{
    [HarmonyPatch(typeof(Fence))]
    class FencePatcher
    {
        private static IMonitor Monitor;

        public static void Initialize(IMonitor monitor)
        {
            Monitor = monitor;
        }
        static void Postfix(ref bool __result)
        {
            __result = true;
        }

        public static void Apply(Harmony harmony)
        {
            try
            {
                harmony.Patch(
                    original: AccessTools.Method(typeof(Fence), nameof(Fence.countsForDrawing)),
                    postfix: new HarmonyMethod(typeof(FencePatcher), nameof(FencePatcher.Fence_countsForDrawing_Postfix))
                );
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed to add fences postfix: {ex}", LogLevel.Error);
            }
        }

        private static void Fence_countsForDrawing_Postfix(ref bool __result)
        {
            __result = true;
        }
    }
}
