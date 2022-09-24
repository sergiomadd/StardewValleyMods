using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MaddUtil;
using SObject = StardewValley.Object;
using StardewValley;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ChestPreview.Framework
{
    public class Patches
    {
		public static void Apply(Harmony harmony)
		{
			try
			{
				/*
				harmony.Patch(
					original: typeof(Item).GetMethod(nameof(Item.drawInMenu)),
					prefix: new HarmonyMethod(typeof(Patches), nameof(Patches.Patches_drawInMenu_Prefix))
				);
				*/
				harmony.Patch(
					original: typeof(SObject).GetMethod(nameof(SObject.drawInMenu), new Type[] 
					{
						typeof(SpriteBatch),
						typeof(Vector2),
						typeof(float),
						typeof(float),
						typeof(float),
						typeof(StackDrawType),
						typeof(Color),
						typeof(bool)
					}),
					prefix: new HarmonyMethod(typeof(Patches), nameof(Patches.Patches_drawInMenu_Prefix))
				);

			}
			catch (Exception ex)
			{
				Printer.Error($"Failed to add patches: {ex}");
			}
		}

		private static bool Patches_drawInMenu_Prefix(Item __instance)
		{
			Printer.Info($"IN ITEM PATCH {__instance.Name}");
			return true;
		}

		private static bool Patches_drawInMenu_Prefix(SObject __instance)
		{
			Printer.Info($"IN obj PATCH {__instance.Name}");
			return true;
		}
	}
}
