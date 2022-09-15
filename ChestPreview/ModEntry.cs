using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley;
using StardewValley.Objects;
using StardewValley.Menus;
using MaddUtil;
using StardewValley.Tools;
using StardewValley.Locations;
using SObject = StardewValley.Object;


namespace ChestPreview
{
    public class ModEntry : Mod
    {
        public static IModHelper helper;
        //public static ModConfig config;
        public SpriteBatch batch { get; set; }
        public override void Entry(IModHelper helper)
        {
            ModEntry.helper = helper;
            
            Printer.SetMonitor(this.Monitor);
            Helpers.SetModHelper(helper);
            Helpers.SetContentHelper(helper.Content);
            Helpers.SetModContentHelper(helper.ModContent);
            

            //helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            //helper.Events.Input.CursorMoved += this.OnCursorMoved;
            helper.Events.Display.Rendered += this.OnRendered;

        }


        private void OnRendered(object sender, RenderedEventArgs e)
        {
            if (Context.IsWorldReady && Game1.activeClickableMenu == null)
            {
                Vector2 tile = Game1.currentCursorTile;
                if(Game1.currentLocation.Objects.ContainsKey(tile)
                    && Game1.currentLocation.Objects[tile] != null 
                    && Game1.currentLocation.Objects[tile] is Chest)
                {
                    Chest chest = Game1.currentLocation.Objects[tile] as Chest;
                    //Util para la opcion del rango?
                    //Utility.isOnScreen
                    //Utility.tileWithinRadiusOfPlayer

                    //Para coger el height desde donde tiene que drawearse el handle, 
                    //usar el maxHeight de la textura del objeto en custion
                    //chest.getBoundingBox();

                    //handle multiplayer?
                    InventoryMenu menu = CreatePreviewMenu(tile, GetItemList(chest).ToList(), chest.GetActualCapacity());

                    /*
                    Game1.InUIMode(() =>
                    {
                        menu2.draw(e.SpriteBatch);

                    });
                    */
                    menu.draw(e.SpriteBatch);
                    
                }
                else if(Game1.currentLocation is FarmHouse
                    && (Game1.currentLocation as FarmHouse).fridgePosition.Equals(tile.ToPoint()))
                {
                    InventoryMenu menu = CreatePreviewMenu(tile, (Game1.currentLocation as FarmHouse).fridge.First().items.ToList(), 36);
                    menu.draw(e.SpriteBatch);
                }
            }
        }

        public int GetSpriteYOffset(Item item)
        {
            int offset = 0;

            if(item is Chest)
            {
                if((item as Chest).SpecialChestType == Chest.SpecialChestTypes.MiniShippingBin)
                {

                }
                else if((item as Chest).SpecialChestType == Chest.SpecialChestTypes.JunimoChest)
                {

                }
                else if ((item as Chest).SpecialChestType == Chest.SpecialChestTypes.MiniShippingBin)
                {

                }
                else if ((item as Chest).SpecialChestType == Chest.SpecialChestTypes.Enricher)
                {

                }
                else if ((item as Chest).SpecialChestType == Chest.SpecialChestTypes.AutoLoader)
                {

                }
                else
                {

                }
            }

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

        public InventoryMenu CreatePreviewMenu(Vector2 tile, List<Item> items, int capacity)
        {
            Vector2 position = new Vector2(
                (tile.X * Game1.tileSize) - Game1.viewport.X + Game1.tileSize / 2,
                (tile.Y * Game1.tileSize) - Game1.viewport.Y);
            position = Utility.ModifyCoordinatesForUIScale(position);

            HoverMenu menu = new HoverMenu((int)position.X, (int)position.Y, false, items, null, capacity);
            menu.populateClickableComponentList();
            return menu;
        }
       
        private void OnCursorMoved(object sender, CursorMovedEventArgs e)
        {
            /*
            if(Context.IsWorldReady)
            {
                Vector2 tile = e.NewPosition.Tile;
                Printer.Info("cursor "+tile.ToString());
                if(Game1.currentLocation.Objects.ContainsKey(tile)
                    && Game1.currentLocation.Objects[tile] != null 
                    && Game1.currentLocation.Objects[tile] is Chest)
                {

                    Chest chest = Game1.currentLocation.Objects[tile] as Chest;
                    //chest.ShowMenu();

                    ItemGrabMenu menu = new ItemGrabMenu(chest.GetItemsForPlayer(Game1.player.UniqueMultiplayerID));
                    //make position with chest tile pixels
                    Vector2 position = new Vector2((tile.X * Game1.tileSize) - Game1.viewport.X, (tile.Y * Game1.tileSize) -Game1.viewport.Y);
                    Printer.Info("screen " + position.ToString());
                    InventoryMenu menu2 = new InventoryMenu((int)position.X, (int)position.Y, false, chest.GetItemsForPlayer(Game1.player.UniqueMultiplayerID));
                    //Game1.spriteBatch.Begin();
                    
                    if (!batch.IsDisposed)
                    {
                        menu2.draw(batch);
                    }
                }
            }
            */
        }

    }
}
