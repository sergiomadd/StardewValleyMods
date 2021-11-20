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
using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;

namespace ItemLogistics.Framework.Patches
{
	[HarmonyPatch(typeof(StardewValley.Object))]
	public static class ObjectPatcher
    {
		public static void Apply(Harmony harmony)
		{
			try
			{
				/*harmony.Patch(
					original: AccessTools.Method(typeof(StardewValley.Object), nameof(StardewValley.Object.performUseAction)),
					prefix: new HarmonyMethod(typeof(ObjectPatcher), nameof(ObjectPatcher.Object_performUseAction_Prefix))
				);*/
			}
			catch (Exception ex)
			{
				Printer.Info($"Failed to add object postfix: {ex}");
			}
		}

		private static void Object_performUseAction_Prefix(ref bool __result, StardewValley.Object __instance)
		{
			Printer.Info("OBJ: CHECKING FOR ACTION");

			if (__instance.Name.Equals("Filter Pipe"))
			{
				Printer.Info("Filterpipe PATCH");
			}
		}

	}
}
