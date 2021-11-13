using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using StardewValley;
using StardewValley.Objects;
using StardewModdingAPI;
using ItemLogistics.Framework.Model;
using Netcode;


namespace ItemLogistics.Framework.Patches
{
    [HarmonyPatch(typeof(Chest))]
    public static class ChestPatcher
    {
        static void Postfix(ref bool __result)
        {
            __result = true;
        }

        public static void Apply(Harmony harmony)
        {
            try
            {
                harmony.Patch(
                    original: AccessTools.Method(typeof(Chest), nameof(Chest.addItem)),
                    postfix: new HarmonyMethod(typeof(ChestPatcher), nameof(ChestPatcher.Chest_addItem_Postfix))
                );
            }
            catch (Exception ex)
            {
                Framework.Model.Printer.Info($"Failed to add fences postfix: {ex}");
            }
        }

        private static void Chest_addItem_Postfix(Chest __instance, Item item, ref Item __result)
        {

            item.resetState();
            __instance.clearNulls();
            NetObjectList<Item> item_list = __instance.items;
            for (int i = 0; i < item_list.Count; i++)
            {
                if (item_list[i] != null && item_list[i].canStackWith(item))
                {
                    item.Stack = item_list[i].addToStack(item);
                    if (item.Stack <= 0)
                    {
                        __result = null;
                    }
                }
            }
            if (item_list.Count < __instance.GetActualCapacity())
            {
                item_list.Add(item);
                __result = null;
            }
            else
            {
                __result = item;
            }
        }
    }
}
