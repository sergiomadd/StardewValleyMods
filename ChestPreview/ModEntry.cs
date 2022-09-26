using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley;
using StardewValley.Objects;
using StardewValley.Menus;
using MaddUtil;
using StardewValley.Tools;
using StardewValley.Locations;
using SObject = StardewValley.Object;
using HarmonyLib;
using ChestPreview.Framework;
using ChestPreview.Framework.APIs;

namespace ChestPreview
{
    public class ModEntry : Mod
    {
        public static IModHelper helper;
        public static ModConfig config;
        public static Size CurrentSize { get; set; }
        public static IDynamicGameAssetsApi DGAAPI { get; set; }

        public override void Entry(IModHelper helper)
        {
            ModEntry.helper = helper;
            Printer.SetMonitor(this.Monitor);
            Helpers.SetModHelper(helper);

            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            helper.Events.Display.Rendered += this.OnRendered;
            helper.Events.Display.WindowResized += this.OnWindowResized;
        }

        public override object GetApi()
        {
            return new ChestPreviewAPI();
        }

        public static string GetSizeFromEnum(Size size)
        {
            if (size == Size.Small)
            {
                return "Small";
            }
            else if (size == Size.Medium)
            {
                return "Medium";
            }
            else if (size == Size.Big)
            {
                return "Big";
            }
            else if (size == Size.Huge)
            {
                return "Huge";
            }
            else
            {
                return "Medium";
            }
        }

        public static Size GetSizeFromString(string size)
        {
            if (size.Equals("Small"))
            {
                return Size.Small;
            }
            else if (size.Equals("Medium"))
            {
                return Size.Medium;
            }
            else if (size.Equals("Big"))
            {
                return Size.Big;
            }
            else if (size.Equals("Huge"))
            {
                return Size.Huge;
            }
            else
            {
                return Size.Medium;
            }
        }

        public static float GetSizeValue()
        {
            if (CurrentSize == Size.Small)
            {
                return 0.4f;
            }
            else if(CurrentSize == Size.Medium)
            {
                return 0.5f;
            }
            else if(CurrentSize == Size.Big)
            {
                return 0.6f;
            }
            else if (CurrentSize == Size.Huge)
            {
                return 0.7f;
            }
            else
            {
                return 0.5f;
            }
        }

        public static string UpdateSize(string size)
        {
            CurrentSize = GetSizeFromString(size);
            return size;
        }

        private void OnWindowResized(object sender, WindowResizedEventArgs e)
        {
            Printer.Trace($"Old screen size: {e.OldSize}");
            Printer.Trace($"New screen size: {e.NewSize}");
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            config = null;
            try
            {
                config = ModEntry.helper.ReadConfig<ModConfig>();
                if (config == null)
                {
                    Printer.Error($"The config file seems to be empty or invalid. Data class returned null.");
                }
            }
            catch (Exception ex)
            {
                Printer.Error($"The config file seems to be missing or invalid.\n{ex}");
            }
            config.RegisterModConfigMenu(helper, this.ModManifest);
            CurrentSize = GetSizeFromString(config.Size);
            if (this.Helper.ModRegistry.IsLoaded("spacechase0.DynamicGameAssets"))
            {
                DGAAPI = helper.ModRegistry.GetApi<IDynamicGameAssetsApi>("spacechase0.DynamicGameAssets");
                if (DGAAPI != null)
                {
                    Printer.Info("dga preview api loadsed.");
                }
                else
                {
                    Printer.Error("dga preview api error.");
                }
            }
        }

        private void OnRendered(object sender, RenderedEventArgs e)
        {
            //Printer.Info($"key {(!config.EnableKey || (config.EnableKey && !config.EnableMouse && Helper.Input.IsDown(config.Key)))} | mouse {(!config.EnableMouse || (config.EnableMouse && !config.EnableKey && Helper.Input.IsDown(config.GetMouseButton(config.Mouse))))}");
            if (config.Enabled 
                && Context.IsWorldReady 
                && Game1.activeClickableMenu == null 
                && (!config.EnableKey
                || (config.EnableKey
                && Helper.Input.IsDown(config.Key)))
                && (!config.EnableMouse
                || (config.EnableMouse
                && Helper.Input.IsDown(config.GetMouseButton(config.Mouse)))))
            {
                Vector2 tile = Game1.currentCursorTile;
                Vector2 downTile = new Vector2(tile.X, tile.Y+1);
                if (Game1.currentLocation is FarmHouse
                    && (Game1.currentLocation as FarmHouse).fridgePosition.Equals(tile.ToPoint()))
                {
                    int yOffset = (int)(-94 * Game1.options.zoomLevel);
                    InventoryMenu menu = CreatePreviewMenu(tile, (Game1.currentLocation as FarmHouse).fridge.First().items.ToList(), 36, yOffset);
                    menu.draw(e.SpriteBatch);
                }
                else if ((Game1.currentLocation.Objects.ContainsKey(tile)
                    && Game1.currentLocation.Objects[tile] != null
                    && Game1.currentLocation.Objects[tile] is Chest)
                    && (config.Range <= 0 ||(config.Range > 0
                    && Utility.tileWithinRadiusOfPlayer((int)tile.X, (int)tile.Y, config.Range, Game1.player))))
                {
                    DrawPreview(tile, e.SpriteBatch);
                }
                else if 
                    ((Game1.currentLocation.Objects.ContainsKey(downTile)
                    && Game1.currentLocation.Objects[downTile] != null
                    && Game1.currentLocation.Objects[downTile] is Chest)
                    && (config.Range <= 0 || (config.Range > 0
                    && Utility.tileWithinRadiusOfPlayer((int)downTile.X, (int)downTile.Y, config.Range, Game1.player))))
                {
                    DrawPreview(downTile, e.SpriteBatch);
                }
            }
        }

        public void DrawPreview(Vector2 tile, SpriteBatch b)
        {
            Chest chest = Game1.currentLocation.Objects[tile] as Chest;
            int yOffset = GetSpriteYOffset(chest);
            InventoryMenu menu = CreatePreviewMenu(tile, GetItemList(chest).ToList(), chest.GetActualCapacity(), yOffset);
            menu.draw(b);
        }
        
        public int GetSpriteYOffset(Item item)
        {
            int offset = 0;
            if (item is Chest)
            {
                if ((item as Chest).SpecialChestType == Chest.SpecialChestTypes.MiniShippingBin)
                {
                    offset = -32;
                }
                else if ((item as Chest).SpecialChestType == Chest.SpecialChestTypes.JunimoChest)
                {
                    offset = -50;
                }
                else if ((item as Chest).SpecialChestType == Chest.SpecialChestTypes.Enricher)
                {

                }
                else if ((item as Chest).SpecialChestType == Chest.SpecialChestTypes.AutoLoader)
                {

                }
                else if ((item as Chest).fridge)
                {
                    offset = -74;
                }
                else
                {
                    offset = -42;
                }
            }
            offset = (int)(offset * Game1.options.zoomLevel);
            return offset;
        }

        public NetObjectList<Item> GetItemList(Chest chest)
        {
            NetObjectList<Item> itemList;
            if (chest.SpecialChestType == Chest.SpecialChestTypes.MiniShippingBin || chest.SpecialChestType == Chest.SpecialChestTypes.JunimoChest)
            {
                itemList = chest.GetItemsForPlayer(Game1.player.UniqueMultiplayerID);
            }
            else
            {
                itemList = chest.items;
            }
            return itemList;
        }

        public InventoryMenu CreatePreviewMenu(Vector2 tile, List<Item> items, int capacity, int yOffset)
        {
            //Printer.Info("tile: " + (tile).ToString());
            //Printer.Info("tile "+ (tile.X * Game1.tileSize).ToString());
            //Printer.Info("viewport " + (Game1.viewport.X).ToString());

            Vector2 position = new Vector2(
                (tile.X * Game1.tileSize) - Game1.viewport.X + Game1.tileSize / 2,
                (tile.Y * Game1.tileSize) - Game1.viewport.Y);
            //Printer.Info("pre modify: " + (position).ToString());
            position = Utility.ModifyCoordinatesForUIScale(position);
            Vector2 absolutPosition = new Vector2(
                tile.X + Game1.viewport.X,
                tile.Y + Game1.viewport.Y);
            //Printer.Info("------------");
            //Printer.Info("absolutePre: " + absolutPosition.X.ToString());
            HoverMenu menu = new HoverMenu((int)position.X, (int)position.Y, yOffset, false, items, null, capacity);
            menu.populateClickableComponentList();
            return menu;
        }
        
    }
}
