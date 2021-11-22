using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using StardewValley;
using StardewValley.Objects;
using StardewModdingAPI;
using ItemLogistics.Framework.Model;
using ItemLogistics.Framework.Objects;
using Netcode;
using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Graphics;

namespace ItemLogistics.Framework.Patches
{
	[HarmonyPatch(typeof(Fence))]
	public static class FencePatcher
    {

		public static void Apply(Harmony harmony)
		{
			try
			{
				harmony.Patch(
					original: AccessTools.Method(typeof(StardewValley.Object), nameof(StardewValley.Object.clicked)),
					postfix: new HarmonyMethod(typeof(FencePatcher), nameof(FencePatcher.Object_clicked_Postfix))
				);
				harmony.Patch(
					original: AccessTools.Method(typeof(Fence), nameof(Fence.getDrawSum)),
					prefix: new HarmonyMethod(typeof(FencePatcher), nameof(FencePatcher.Fence_getDrawSum_Prefix))
				);
				harmony.Patch(
					original: AccessTools.Method(typeof(Fence), nameof(Fence.drawInMenu), new Type[] { typeof(SpriteBatch), typeof(Vector2), typeof(float), typeof(float), typeof(float), typeof(StackDrawType), typeof(Color), typeof(bool) }),
					prefix: new HarmonyMethod(typeof(FencePatcher), nameof(FencePatcher.Fence_drawInMenu_Prefix))
				);
				harmony.Patch(
					original: AccessTools.Method(typeof(Fence), nameof(Fence.draw), new Type[] { typeof(SpriteBatch), typeof(int), typeof(int), typeof(float) }),
					prefix: new HarmonyMethod(typeof(FencePatcher), nameof(FencePatcher.Fence_draw_Prefix))
				);
				harmony.Patch(
					original: AccessTools.Method(typeof(Fence), nameof(Fence.isPassable)),
					prefix: new HarmonyMethod(typeof(FencePatcher), nameof(FencePatcher.Fence_isPassable_Prefix))
				);
				harmony.Patch(
					original: AccessTools.Method(typeof(Fence), nameof(Fence.countsForDrawing)),
					prefix: new HarmonyMethod(typeof(FencePatcher), nameof(FencePatcher.Fence_countsForDrawing_Prefix))
				);

			}
			catch (Exception ex)
			{
				Printer.Info($"Failed to add fence patch: {ex}");
			}
		}

		private static bool Fence_countsForDrawing_Prefix(Fence __instance, ref bool __result, int type)
		{
			__result = false;
			DataAccess DataAccess = DataAccess.GetDataAccess();
			//Add when JA obj IDs is done
			//if (DataAccess.ValidPipeNames.Contains(__instance.Name) && DataAccess.ValidPipeIDs.Contains(type))
			if (DataAccess.ValidPipeNames.Contains(__instance.Name) && IsDefaultFence(type))
			{
				__result = true;
				return false;
			}
			else
            {
				return true;
            }
		}

		private static bool IsDefaultFence(int type)
        {
			if(type == 1 || type == 2 || type == 3 || type == 4 || type == 5)
            {
				return false;
            }
			else
            {
				return true;
            }
        }

		private static bool Fence_isPassable_Prefix(ref bool __result, Fence __instance)
		{
			__result = false;
			DataAccess DataAccess = DataAccess.GetDataAccess();
			if (DataAccess.ValidPipeNames.Contains(__instance.Name))
			{
				Node[,] locationMatrix;
				if(DataAccess.LocationMatrix.TryGetValue(Game1.currentLocation, out locationMatrix))
                {
					if(locationMatrix[(int)__instance.tileLocation.X, (int)__instance.tileLocation.Y] != null)
                    {
						Printer.Info("NOT NULL");
						if (locationMatrix[(int)__instance.tileLocation.X, (int)__instance.tileLocation.Y].ParentNetwork is Network)
						{
							Printer.Info("HAS LG");
							Network lg = (Network)locationMatrix[(int)__instance.tileLocation.X, (int)__instance.tileLocation.Y].ParentNetwork;
							if (lg.IsPassable)
							{
								__result = true;
							}
						}
					}
				}
				return false;
			}
			else
            {
				return true;
			}
		}

		private static bool Fence_getDrawSum_Prefix(ref int __result, Fence __instance, GameLocation location)
		{
			DataAccess DataAccess = DataAccess.GetDataAccess();
			if (DataAccess.ValidPipeNames.Contains(__instance.Name))
			{
				bool CN = false;
				bool CS = false;
				bool CW = false;
				bool CE = false;
				int drawSum = 0;
				Vector2 surroundingLocations = __instance.tileLocation;
				surroundingLocations.X += 1f;
				//0 = 6
				//West = 100 = 11
				if (location.objects.ContainsKey(surroundingLocations) && location.objects[surroundingLocations] is Fence && ((Fence)location.objects[surroundingLocations]).countsForDrawing(__instance.whichType))
				{
					drawSum += 100;
				}
				else if (location.objects.ContainsKey(surroundingLocations) && location.objects[surroundingLocations] is Chest)
				{
					CW = true;
				}
				//East = 10 = 10
				//W + E = 110 = 8
				surroundingLocations.X -= 2f;
				if (location.objects.ContainsKey(surroundingLocations) && location.objects[surroundingLocations] is Fence && ((Fence)location.objects[surroundingLocations]).countsForDrawing(__instance.whichType))
				{
					drawSum += 10;
				}
				else if (location.objects.ContainsKey(surroundingLocations) && location.objects[surroundingLocations] is Chest)
				{
					CE = true;
				}
				//South = 500 = 6
				//S + E = 600 = 1
				//S + W = 510 = 3
				//S + E + W = 610
				surroundingLocations.X += 1f;
				surroundingLocations.Y += 1f;
				if (location.objects.ContainsKey(surroundingLocations) && location.objects[surroundingLocations] is Fence && ((Fence)location.objects[surroundingLocations]).countsForDrawing(__instance.whichType))
				{
					drawSum += 500;
				}
				else if (location.objects.ContainsKey(surroundingLocations) && location.objects[surroundingLocations] is Chest)
				{
					CS = true;
				}
				surroundingLocations.Y -= 2f;
				//North = 1000 = 4
				//N + E = 1100 = 7
				//N + W = 1010 = 9
				//N + S = 1500 = 4
				//N + E + W = 1110 = 8
				//N + E + S = 1600 = 1
				//N + S + W = 1510 = 3
				//N + E + W  + S = 1610 = 5
				if (location.objects.ContainsKey(surroundingLocations) && location.objects[surroundingLocations] is Fence && ((Fence)location.objects[surroundingLocations]).countsForDrawing(__instance.whichType))
				{
					drawSum += 1000;
				}
				else if (location.objects.ContainsKey(surroundingLocations) && location.objects[surroundingLocations] is Chest)
				{
					CN = true;
				}
				if (DataAccess.ValidIOPipeNames.Contains(__instance.Name))
				{
					//Que pasa si no hay ningun chest adjacent
					//Que pasa si un connector tiene un chest adjacent
					if (CN || CS || CW || CE)
					{
						drawSum = GetAdjChestsSum(drawSum, CN, CS, CW, CE);
					}
				}
				if (__instance.Name.Equals("Connector Pipe"))
				{
					Node[,] locationMatrix;
					if (DataAccess.LocationMatrix.TryGetValue(Game1.currentLocation, out locationMatrix))
					{
						if(locationMatrix[(int)__instance.tileLocation.X, (int)__instance.tileLocation.Y] is Connector)
                        {
							//Printer.Info("IS CONNECTOR");
							Connector connector = (Connector) locationMatrix[(int)__instance.tileLocation.X, (int)__instance.tileLocation.Y];
							if(connector.PassingItem)
                            {
								//Printer.Info("PASSING ITEM");
								drawSum += 5;
							}

						}
					}
				}
				__result = drawSum;
				return false;
			}
			else
            {
				return true;
			}
		}

		private static int GetAdjChestsSum(int drawSum, bool CN, bool CS, bool CW, bool CE)
        {
			switch(drawSum)
            {
				case 0:
					if (CN) { drawSum = 500; }
					else if (CS) { drawSum = 1000; }
					else if (CW) { drawSum = 10; }
					else if (CE) { drawSum = 100; }
					break;
				case 1000:
					if (CS){drawSum += 2;}
					else if (CW){drawSum += 3;}
					else if (CE) { drawSum += 4; }
					break;
				case 500:
					if (CN) { drawSum += 1; }
					else if (CW) { drawSum += 3; }
					else if (CE) { drawSum += 4; }
					break;
				case 100:
					if (CN) { drawSum += 1; }
					else if (CS) { drawSum += 2; }
					else if (CE) { drawSum += 4; }
					break;
				case 10:
					if (CN) { drawSum += 1; }
					else if (CS) { drawSum += 2; }
					else if (CW) { drawSum += 3; }
					break;
				case 1500:
					if (CW) { drawSum += 3; }
					else if (CE) { drawSum += 4; }
					break;
				case 110:
					if (CN){drawSum += 1;}
					else if (CS){drawSum += 2;}
					break;
				case 1100:
					if (CS){drawSum += 2;}
					else if (CE){drawSum += 4;}
					break;
				case 1010:
					if (CS){drawSum += 2;}
					else if (CW){drawSum += 3;}
					break;
				case 600:
					if (CN){drawSum += 1;}
					else if (CE){drawSum += 4;}
					break;
				case 510:
					if (CN){drawSum += 1;}
					else if (CW){drawSum += 3;}
					break;
			}
			return drawSum;
		}

		private static void Object_clicked_Postfix(Fence __instance, Farmer who)
		{
			Printer.Info("FENCE: CLICKED");
			if (__instance.Name.Equals("Filter Pipe"))
			{

				//Printer.Info("Filterpipe PATCH");
				DataAccess DataAccess = DataAccess.GetDataAccess();
				Node[,] locationMatrix;
				if (DataAccess.LocationMatrix.TryGetValue(Game1.currentLocation, out locationMatrix))
				{
					FilterPipe pipe = (FilterPipe)locationMatrix[(int)__instance.tileLocation.X, (int)__instance.tileLocation.Y];
					//pipe.FilterChest.checkForAction(Game1.player, false);
					pipe.Filter.ShowMenu();
				}
			}
		}
		
		private static bool Fence_drawInMenu_Prefix(Fence __instance, SpriteBatch spriteBatch, Vector2 location, float scale, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
		{
			DataAccess DataAccess = DataAccess.GetDataAccess();
			if (DataAccess.ValidPipeNames.Contains(__instance.Name))
			{
				location.Y -= 64f * scale;
				int sourceRectPosition = 1;
				int drawSum = __instance.getDrawSum(Game1.currentLocation);
				sourceRectPosition = GetNewDrawGuide(__instance)[drawSum];
				if ((bool)__instance.isGate)
				{
					switch (drawSum)
					{
						case 110:
							spriteBatch.Draw(__instance.fenceTexture.Value, location + new Vector2(6f, 6f), new Rectangle(0, 512, 88, 24), color, 0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
							return false;
						case 1500:
							spriteBatch.Draw(__instance.fenceTexture.Value, location + new Vector2(6f, 6f), new Rectangle(112, 512, 16, 64), color, 0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
							return false;
					}
				}
				spriteBatch.Draw(__instance.fenceTexture.Value, location + new Vector2(32f, 32f) * scale, Game1.getArbitrarySourceRect(__instance.fenceTexture.Value, 64, 128, sourceRectPosition), color * transparency, 0f, new Vector2(32f, 32f) * scale, scale, SpriteEffects.None, layerDepth);
				return false;
			}
			else
			{
				return true;
			}

		}


		private static bool Fence_draw_Prefix(Fence __instance, SpriteBatch b, int x, int y, float alpha = 1f)
		{
			DataAccess DataAccess = DataAccess.GetDataAccess();
			if (DataAccess.ValidPipeNames.Contains(__instance.Name))
			{
				int sourceRectPosition = 1;
				if ((float)__instance.health > 1f || __instance.repairQueued.Value)
				{
					int drawSum = __instance.getDrawSum(Game1.currentLocation);
					sourceRectPosition = GetNewDrawGuide(__instance)[drawSum];
					if ((bool)__instance.isGate)
					{
						Vector2 offset = new Vector2(0f, 0f);
						_ = (Vector2)__instance.tileLocation;
						_ = __instance.tileLocation + new Vector2(-1f, 0f);
						switch (drawSum)
						{
							case 10:
								b.Draw(__instance.fenceTexture.Value, Game1.GlobalToLocal(Game1.viewport, offset + new Vector2(x * 64 - 16, y * 64 - 128)), new Rectangle(((int)__instance.gatePosition == 88) ? 24 : 0, 192, 24, 48), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)(y * 64 + 32 + 1) / 10000f);
								break;
							case 100:
								b.Draw(__instance.fenceTexture.Value, Game1.GlobalToLocal(Game1.viewport, offset + new Vector2(x * 64 - 16, y * 64 - 128)), new Rectangle(((int)__instance.gatePosition == 88) ? 24 : 0, 240, 24, 48), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)(y * 64 + 32 + 1) / 10000f);
								break;
							case 1000:
								b.Draw(__instance.fenceTexture.Value, Game1.GlobalToLocal(Game1.viewport, offset + new Vector2(x * 64 + 20, y * 64 - 64 - 20)), new Rectangle(((int)__instance.gatePosition == 88) ? 24 : 0, 288, 24, 32), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)(y * 64 - 32 + 2) / 10000f);
								break;
							case 500:
								b.Draw(__instance.fenceTexture.Value, Game1.GlobalToLocal(Game1.viewport, offset + new Vector2(x * 64 + 20, y * 64 - 64 - 20)), new Rectangle(((int)__instance.gatePosition == 88) ? 24 : 0, 320, 24, 32), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)(y * 64 + 96 - 1) / 10000f);
								break;
							case 110:
								b.Draw(__instance.fenceTexture.Value, Game1.GlobalToLocal(Game1.viewport, offset + new Vector2(x * 64 - 16, y * 64 - 64)), new Rectangle(((int)__instance.gatePosition == 88) ? 24 : 0, 128, 24, 32), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)(y * 64 + 32 + 1) / 10000f);
								break;
							case 1500:
								b.Draw(__instance.fenceTexture.Value, Game1.GlobalToLocal(Game1.viewport, offset + new Vector2(x * 64 + 20, y * 64 - 64 - 20)), new Rectangle(((int)__instance.gatePosition == 88) ? 16 : 0, 160, 16, 16), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)(y * 64 - 32 + 2) / 10000f);
								b.Draw(__instance.fenceTexture.Value, Game1.GlobalToLocal(Game1.viewport, offset + new Vector2(x * 64 + 20, y * 64 - 64 + 44)), new Rectangle(((int)__instance.gatePosition == 88) ? 16 : 0, 176, 16, 16), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)(y * 64 + 96 - 1) / 10000f);
								break;
						}
						sourceRectPosition = 17;
					}
					else if (__instance.heldObject.Value != null)
					{
						Vector2 offset2 = Vector2.Zero;
						switch (drawSum)
						{
							case 10:
								if (__instance.whichType.Value == 2)
								{
									offset2.X = -4f;
								}
								else if (__instance.whichType.Value == 3)
								{
									offset2.X = 8f;
								}
								else
								{
									offset2.X = 0f;
								}
								break;
							case 100:
								if (__instance.whichType.Value == 2)
								{
									offset2.X = 0f;
								}
								else if (__instance.whichType.Value == 3)
								{
									offset2.X = -8f;
								}
								else
								{
									offset2.X = -4f;
								}
								break;
						}
						if ((int)__instance.whichType == 2)
						{
							offset2.Y = 16f;
						}
						else if ((int)__instance.whichType == 3)
						{
							offset2.Y -= 8f;
						}
						if ((int)__instance.whichType == 3)
						{
							offset2.X -= 2f;
						}
						__instance.heldObject.Value.draw(b, x * 64 + (int)offset2.X, (y - 1) * 64 - 16 + (int)offset2.Y, (float)(y * 64 + 64) / 10000f, 1f);
					}
				}
				b.Draw(__instance.fenceTexture.Value, Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - 64)), new Rectangle(sourceRectPosition * Fence.fencePieceWidth % __instance.fenceTexture.Value.Bounds.Width, sourceRectPosition * Fence.fencePieceWidth / __instance.fenceTexture.Value.Bounds.Width * Fence.fencePieceHeight, Fence.fencePieceWidth, Fence.fencePieceHeight), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)(y * 64 + 32) / 10000f);
				return false;
			}
			else
            {
				return true;
            }
			
		}

		private static Dictionary<int, int> GetNewDrawGuide(Fence fence)
        {
			Dictionary<int, int> DrawGuide = new Dictionary<int, int>();
			DataAccess DataAccess = DataAccess.GetDataAccess();
			if (DataAccess.ValidItemNames.Contains(fence.Name))
			{
				if (fence.Name.Equals("Connector Pipe"))
				{
					DrawGuide.Add(0, 0);
					DrawGuide.Add(1000, 1);
					DrawGuide.Add(500, 2); ;
					DrawGuide.Add(100, 3);
					DrawGuide.Add(10, 4);
					DrawGuide.Add(1500, 5);
					DrawGuide.Add(1100, 6);
					DrawGuide.Add(1010, 7);
					DrawGuide.Add(600, 8);
					DrawGuide.Add(510, 9);
					DrawGuide.Add(110, 10);
					DrawGuide.Add(1600, 11);
					DrawGuide.Add(1510, 12);
					DrawGuide.Add(1110, 13);
					DrawGuide.Add(610, 14);
					DrawGuide.Add(1610, 15);
					DrawGuide.Add(1505, 17);
					DrawGuide.Add(1105, 18);
					DrawGuide.Add(1015, 19);
					DrawGuide.Add(605, 20);
					DrawGuide.Add(515, 21);
					DrawGuide.Add(115, 22);
					DrawGuide.Add(1605, 23);
					DrawGuide.Add(1515, 24);
					DrawGuide.Add(1115, 25);
					DrawGuide.Add(615, 26);
					DrawGuide.Add(1615, 27);
				}
				else
				{
					DrawGuide.Clear();
					DrawGuide.Add(0, 0);
					DrawGuide.Add(1000, 1);
					DrawGuide.Add(1002, 1);
					DrawGuide.Add(1003, 2);
					DrawGuide.Add(1004, 3);
					DrawGuide.Add(500, 4);
					DrawGuide.Add(501, 4);
					DrawGuide.Add(503, 5);
					DrawGuide.Add(504, 6);
					DrawGuide.Add(100, 9);
					DrawGuide.Add(101, 7);
					DrawGuide.Add(102, 8);
					DrawGuide.Add(104, 9);
					DrawGuide.Add(10, 12);
					DrawGuide.Add(11, 10);
					DrawGuide.Add(12, 11);
					DrawGuide.Add(13, 12);
					DrawGuide.Add(1500, 13);
					DrawGuide.Add(1503, 13);
					DrawGuide.Add(1504, 14);
					DrawGuide.Add(1100, 15);
					DrawGuide.Add(1102, 15);
					DrawGuide.Add(1104, 16);
					DrawGuide.Add(1010, 17);
					DrawGuide.Add(1012, 17);
					DrawGuide.Add(1013, 18);
					DrawGuide.Add(600, 19);
					DrawGuide.Add(601, 19);
					DrawGuide.Add(604, 20);
					DrawGuide.Add(510, 21);
					DrawGuide.Add(511, 21);
					DrawGuide.Add(513, 22);
					DrawGuide.Add(110, 23);
					DrawGuide.Add(111, 23);
					DrawGuide.Add(112, 24);
					DrawGuide.Add(1600, 25);
					DrawGuide.Add(1510, 26);
					DrawGuide.Add(1110, 27);
					DrawGuide.Add(610, 28);
					DrawGuide.Add(1610, 29);
				}
			}
			return DrawGuide;
		}




		/*		private static bool Fence_isPassable_Prefix(ref bool __result, Fence __instance)
		{
			SGraphDB DataAccess = SGraphDB.GetSGraphDB();
			if (DataAccess.ValidItemNames.Contains(__instance.Name))
			{

				return false;
			}
			else
            {
				return true;
			}
		}
		 */
	}
}
