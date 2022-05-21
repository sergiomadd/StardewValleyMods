﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Tools;
using StardewValley.Objects;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using SObject = StardewValley.Object;
using ItemPipes.Framework;
using ItemPipes.Framework.Util;
using ItemPipes.Framework.Model;
using ItemPipes.Framework.Patches;
using ItemPipes.Framework.Factories;
using ItemPipes.Framework.Items;
using ItemPipes.Framework.Items.Objects;
using ItemPipes.Framework.Items.Tools;
using HarmonyLib;
using System.Diagnostics;
using System.Threading;


namespace ItemPipes
{
    class ModEntry : Mod, IAssetEditor
    {
        public static IModHelper helper;
        public Dictionary<string, int> LogisticItemIds;
        public DataAccess DataAccess { get; set; }

        internal static readonly string ContentPackPath = Path.Combine("assets", "DGAItemLogistics");

        public override void Entry(IModHelper helper)
        {
            ModEntry.helper = helper;
            Printer.SetMonitor(this.Monitor);
            Framework.Util.Helper.SetHelper(helper);
            LogisticItemIds = new Dictionary<string, int>();
            DataAccess = DataAccess.GetDataAccess();

            string ItemIDs = "assets/Data/ItemIDs.json";
            DataModel data = null;
            ModConfig config = null;
            try
            {
                data = this.Helper.Data.ReadJsonFile<DataModel>(ItemIDs);
                if (data == null)
                {
                    this.Monitor.Log($"The {ItemIDs} file seems to be missing or invalid.", LogLevel.Error);
                }
                config = this.Helper.ReadConfig<ModConfig>();
                if (config == null)
                {
                    this.Monitor.Log($"The config file seems to be missing or invalid.", LogLevel.Error);
                }
            }
            catch (Exception ex)
            {
                this.Monitor.Log($"The {ItemIDs} file seems to be invalid.\n{ex}", LogLevel.Error);
            }

            DataAccess.ModItems = data.ModItems;
            DataAccess.NetworkItems = data.NetworkItems;
            DataAccess.Buildings = data.Buildings;

            //Normal debug = only errors
            if(config.DebugMode)
            {
                Globals.Debug = true;
                if (Globals.Debug) { Printer.Info("Debug mode ENABLED"); }
            }
            else
            {
                Globals.Debug = false;
                if (Globals.Debug) { Printer.Info("Debug mode DISABLED"); }
            }
            //Ultra debug = all the prints like step by step
            if (config.UltraDebugMode)
            {
                Globals.UltraDebug = true;
                if (Globals.Debug) { Printer.Info("UltraDebug mode ENABLED"); }
            }
            else
            {
                Globals.UltraDebug = false;
                if (Globals.Debug) { Printer.Info("UltraDebug mode DISABLED"); }
            }
            if (config.ItemSending)
            {
                Globals.ItemSending = true;
                if (Globals.Debug) { Printer.Info("Item sending ENABLED"); }
            }
            else
            {
                Globals.ItemSending = false;
                if (Globals.Debug) { Printer.Info("Item sending DISABLED"); }
            }
            if (config.IOPipeStatePopup)
            {
                Globals.IOPipeStatePopup = true;
                if (Globals.Debug) { Printer.Info("IOPipe state bubble popup ENABLED"); }
            }
            else
            {
                Globals.IOPipeStatePopup = false;
                if (Globals.Debug) { Printer.Info("IOPipe state bubble popup DISABLED"); }
            }

            var harmony = new Harmony(this.ModManifest.UniqueID);
            //FencePatcher.Apply(harmony);
            //ChestPatcher.Apply(harmony);
            CraftingPatcher.Apply(harmony);
            

            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
            helper.Events.World.ObjectListChanged += this.OnObjectListChanged;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.GameLoop.Saving += this.OnSaving;
            helper.Events.GameLoop.Saved += this.OnSaved;

        }



        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            Helper.Content.AssetEditors.Add(this);
        }

        public bool CanEdit<T>(IAssetInfo asset)
        {
            if (asset.AssetNameEquals("Data/CraftingRecipes"))
            {
                return true;
            }
            if (asset.AssetNameEquals("Data\\mail"))
            {
                return true;
            }

            return false;
        }

        public void Edit<T>(IAssetData asset)
        {
            if (asset.AssetNameEquals("Data/CraftingRecipes"))
            {
                IDictionary<string, string> data = asset.AsDictionary<string, string>().Data;
                if (!data.ContainsKey("IronPipe"))
                {
                    data.Add("IronPipe", "0 1//0 1/false/Mining 3/Fake Recipe");
                    data.Add("GoldPipe", "0 1//0 1/false/Mining 6/Fake Recipe");
                    data.Add("IridiumPipe", "0 1//0 1/false/Mining 9/Fake Recipe");
                    data.Add("ExtractorPipe", "0 1//0 1/false/Mining 3/Fake Recipe");
                    data.Add("GoldExtractorPipe", "0 1//0 1/false/Mining 6/Fake Recipe");
                    data.Add("IridiumExtractorPipe", "0 1//0 1/false/Mining 9/Fake Recipe");
                    data.Add("InserterPipe", "0 1//0 1/false/Mining 3/Fake Recipe");
                    data.Add("PolymorphicPipe", "0 1//0 1/false/Mining 3/Fake Recipe");
                    data.Add("FilterPipe", "0 1//0 1/false/Mining 3/Fake Recipe");
                    data.Add("PPM", "0 1//0 1/false/Mining 6/Fake Recipe");
                }
                else
                {
                    data["IronPipe"] = "0 1//0 1/false/Mining 3/Fake Recipe";
                    data["GoldPipe"] = "0 1//0 1/false/Mining 6/Fake Recipe";
                    data["IridiumPipe"] = "0 1//0 1/false/Mining 9/Fake Recipe";
                    data["ExtractorPipe"] = "0 1//0 1/false/Mining 3/Fake Recipe";
                    data["GoldExtractorPipe"] = "0 1//0 1/false/Mining 6/Fake Recipe";
                    data["IridiumExtractorPipe"] = "0 1//0 1/false/Mining 9/Fake Recipe";
                    data["InserterPipe"] = "0 1//0 1/false/Mining 3/Fake Recipe";
                    data["PolymorphicPipe"] = "0 1//0 1/false/Mining 3/Fake Recipe";
                    data["FilterPipe"] = "0 1//0 1/false/Mining 3/Fake Recipe";
                    data["PPM"] = "0 1//0 1/false/Mining 6/Fake Recipe";
                }
            }
            if (asset.AssetNameEquals("Data\\mail"))
            {
                var data = asset.AsDictionary<string, string>().Data;
                data["ItemPipes_SendWrench"] = "Hello @! It has come to Madd Industries knowledge that you have started to use our products!" +
                    "^As a welcome gift, we are pleased to give you a tool to configure item pipes. For now it can only turn on and off IOPipes, but the " +
                    "engineering team is working hard on implementing further manipulations.^Thanks for putting your trust in Madd Industries, " +
                    "the best logistics provider of the Ferngill Republic!";
            }

        }

        private void OnSaving(object sender, SavingEventArgs e)
        {
            if (Context.IsMainPlayer)
            {
                if (Globals.Debug) { Printer.Info("Waiting for all items to arrive at input..."); }
                //Quick end all threads
                /*
                while (DataAccess.Threads.Count > 0)
                {
                    foreach (Thread thread in DataAccess.Threads.ToList())
                    {
                        if (thread != null && thread.IsAlive)
                        {
                            
                            thread.Interrupt();
                        }
                    }
                }
                */
                foreach (Thread thread in DataAccess.Threads.ToList())
                {
                    if (thread != null && thread.IsAlive)
                    {

                        thread.Interrupt();
                    }
                }
                ConvertToVanillaMap();
                ConvertToVanillaPlayer();
                if (Globals.Debug) { Printer.Info("All modded items saved!"); }

            }
        }

        public void OnSaved(object sender, SavedEventArgs args)
        {
            Reset();
            if (Context.IsMainPlayer) 
            {

                foreach (GameLocation location in Game1.locations)
                {
                    DataAccess.LocationNetworks.Add(location, new List<Network>());
                    DataAccess.LocationNodes.Add(location, new List<Node>());
                    DataAccess.UsedNetworkIDs.Add(location, new List<int>());
                    NetworkBuilder.BuildLocationNetworksTEMP(location);
                    NetworkManager.UpdateLocationNetworks(location);
                }

                ConvertFromVanillaMap();
                ConvertFromVanillaPlayer(); 
                
                if (Globals.UltraDebug) { Printer.Info("Location networks loaded!"); }
            }
        }

        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            DataAccess.LoadItems();
            DataAccess.LoadSprites();
            DataAccess.LoadRecipes();
            Reset();
            if (Context.IsMainPlayer)
            {
                foreach (GameLocation location in Game1.locations)
                {
                    DataAccess.LocationNetworks.Add(location, new List<Network>());
                    DataAccess.LocationNodes.Add(location, new List<Node>());
                    DataAccess.UsedNetworkIDs.Add(location, new List<int>());
                    NetworkBuilder.BuildLocationNetworksTEMP(location);
                    NetworkManager.UpdateLocationNetworks(location);
                }

                ConvertFromVanillaMap();
                ConvertFromVanillaPlayer();
            }
            if (Globals.UltraDebug) { Printer.Info("Location networks loaded!"); }
        }


        private void Reset()
        {
            DataAccess.LocationNodes.Clear();
            DataAccess.LocationNetworks.Clear();
            DataAccess.UsedNetworkIDs.Clear();
            DataAccess.Threads.Clear();
        }

        public void ConvertToVanillaMap()
        {
            foreach (GameLocation location in Game1.locations)
            {
                foreach (KeyValuePair<Vector2, SObject> obj in location.Objects.Pairs.ToList())
                {
                    if (obj.Value is CustomObjectItem)
                    {
                        CustomObjectItem customObj = (CustomObjectItem)obj.Value;
                        SObject tempObj = customObj.Save();
                        location.objects.Remove(obj.Key);
                        location.objects.Add(obj.Key, tempObj);
                    }
                    if (obj.Value is Chest && (obj.Value as Chest).items.Any(i => i is CustomObjectItem || i is CustomToolItem))
                    {
                        for (int i = 0; i < (obj.Value as Chest).items.Count; i++)
                        {
                            if ((obj.Value as Chest).items[i] is CustomObjectItem)
                            {
                                CustomObjectItem customObj = (CustomObjectItem)(obj.Value as Chest).items[i];
                                SObject tempObj = customObj.Save();
                                (obj.Value as Chest).items.RemoveAt(i);
                                (obj.Value as Chest).items.Insert(i, tempObj);
                            }
                            else if ((obj.Value as Chest).items[i] is CustomToolItem)
                            {
                                CustomToolItem customTool = (CustomToolItem)(obj.Value as Chest).items[i];
                                Tool tempTool = customTool.Save();
                                (obj.Value as Chest).items.RemoveAt(i);
                                (obj.Value as Chest).items.Insert(i, tempTool);
                            }
                        }
                    }
                }
            }
        }
        public void ConvertToVanillaPlayer()
        {
            if (Game1.player.Items.Any(i => i is CustomObjectItem || i is CustomToolItem))
            {
                for (int i = 0; i < Game1.player.Items.Count; i++)
                {
                    if (Game1.player.Items[i] is CustomObjectItem)
                    {
                        CustomObjectItem customObj = (CustomObjectItem)Game1.player.Items[i];
                        SObject tempObj = customObj.Save();
                        Game1.player.Items.RemoveAt(i);
                        Game1.player.Items.Insert(i, tempObj);
                    }
                    else if (Game1.player.Items[i] is CustomToolItem)
                    {
                        CustomToolItem customTool = (CustomToolItem)Game1.player.Items[i];
                        Tool tempTool = customTool.Save();
                        Game1.player.Items.RemoveAt(i);
                        Game1.player.Items.Insert(i, tempTool);
                    }
                }
            }
        }

        public void ConvertFromVanillaMap()
        {
            foreach (GameLocation location in Game1.locations)
            {
                foreach (KeyValuePair<Vector2, SObject> obj in location.Objects.Pairs.ToList())
                {
                    if (obj.Value is Fence && obj.Value.modData.ContainsKey("ItemPipes"))
                    {
                        if (obj.Value.modData["Type"] != null)
                        {
                            CustomObjectItem customObj = ItemFactory.CreateObject(obj.Key, obj.Value.modData["Type"]);
                            customObj.Load(obj.Value.modData);
                            location.objects.Remove(obj.Key);
                            location.objects.Add(obj.Key, customObj);
                        }
                    }
                    if (obj.Value is Chest && (obj.Value as Chest).items.Any(i => i is Fence))
                    {
                        for (int i = 0; i < (obj.Value as Chest).items.Count; i++)
                        {
                            if ((obj.Value as Chest).items[i] is Fence && (obj.Value as Chest).items[i].modData.ContainsKey("ItemPipes"))
                            {
                                Fence tempObj = (Fence)(obj.Value as Chest).items[i];
                                CustomObjectItem customObj = ItemFactory.CreateItem(tempObj.modData["Type"]);
                                (obj.Value as Chest).items.RemoveAt(i);
                                (obj.Value as Chest).items.Insert(i, customObj);
                            }
                            else if ((obj.Value as Chest).items[i] is Axe && (obj.Value as Chest).items[i].modData.ContainsKey("ItemPipes"))
                            {
                                Axe tempTool = (Axe)(obj.Value as Chest).items[i];
                                CustomToolItem customObj = ItemFactory.CreateTool(tempTool.modData["Type"]);
                                (obj.Value as Chest).items.RemoveAt(i);
                                (obj.Value as Chest).items.Insert(i, customObj);
                            }
                        }
                    }
                    if(obj.Value is Chest)
                    {
                        NetworkManager.AddObject(obj, location);
                    }
                }
            }
        }

        public void ConvertFromVanillaPlayer()
        {
            if (Game1.player.Items.Any(i => 
                (i is Fence && (i as Fence).modData.ContainsKey("ItemPipes"))
                || (i is Axe && (i as Axe).modData.ContainsKey("ItemPipes"))))
            {
                for (int i = 0; i < Game1.player.Items.Count; i++)
                {
                    if (Game1.player.Items[i] is Fence && Game1.player.Items[i].modData.ContainsKey("ItemPipes"))
                    {
                        CustomObjectItem customObj = ItemFactory.CreateItem(Game1.player.Items[i].modData["Type"]);
                        customObj.Load(Game1.player.Items[i].modData);
                        Game1.player.Items.RemoveAt(i);
                        Game1.player.Items.Insert(i, customObj);
                    }
                    else if (Game1.player.Items[i] is Axe && Game1.player.Items[i].modData.ContainsKey("ItemPipes"))
                    {
                        CustomToolItem customTool = ItemFactory.CreateTool(Game1.player.Items[i].modData["Type"]);
                        customTool.Load(Game1.player.Items[i].modData);
                        Game1.player.Items.RemoveAt(i);
                        Game1.player.Items.Insert(i, customTool);
                    }
                }
            }
        }

        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (Globals.ItemSending)
            {
                if (Context.IsWorldReady)
                {
                    //Tier 1 Extractors
                    if (e.IsMultipleOf(120))
                    {
                        foreach (GameLocation location in Game1.locations)
                        {
                            List<Network> networks = DataAccess.LocationNetworks[location];
                            if (networks.Count > 0)
                            {
                                if (Globals.UltraDebug) { Printer.Info("Network amount: " + networks.Count.ToString()); }
                                foreach (Network network in networks)
                                {
                                    //Printer.Info(network.Print());
                                    if (network != null && network.Outputs.Count > 0)
                                    {
                                        //Printer.Info(network.Print());
                                        network.ProcessExchanges(1);
                                    }
                                }
                            }
                        }
                    }
                    //Tier 2 Extractors
                    if (e.IsMultipleOf(60))
                    {
                        foreach (GameLocation location in Game1.locations)
                        {
                            List<Network> networks = DataAccess.LocationNetworks[location];
                            if (networks.Count > 0)
                            {
                                //if (Globals.UltraDebug) { Printer.Info("Network amount: " + networks.Count.ToString()); }
                                foreach (Network network in networks)
                                {
                                    //Printer.Info(network.Print());
                                    if (network != null && network.Outputs.Count > 0)
                                    {
                                        network.ProcessExchanges(2);
                                    }

                                }
                            }
                        }
                    }
                    //Tier 3 Extractors
                    if (e.IsMultipleOf(30))
                    {
                        foreach (GameLocation location in Game1.locations)
                        {
                            List<Network> networks = DataAccess.LocationNetworks[location];
                            if (networks.Count > 0)
                            {
                                foreach (Network network in networks)
                                {
                                    if (network != null && network.Outputs.Count > 0)
                                    {
                                        network.ProcessExchanges(3);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void OnObjectListChanged(object sender, ObjectListChangedEventArgs e)
        {
            List<KeyValuePair<Vector2, StardewValley.Object>> addedObjects = e.Added.ToList();
            foreach (KeyValuePair<Vector2, StardewValley.Object> obj in addedObjects)
            {
                if(obj.Value is CustomObjectItem || obj.Value is Chest)
                {
                    NetworkManager.AddObject(obj, e.Location);
                    NetworkManager.UpdateLocationNetworks(Game1.currentLocation);
                }
            }

            List<KeyValuePair<Vector2, StardewValley.Object>> removedObjects = e.Removed.ToList();
            foreach (KeyValuePair<Vector2, StardewValley.Object> obj in removedObjects)
            {

                if (obj.Value is CustomObjectItem || obj.Value is Chest)
                {
                    NetworkManager.RemoveObject(obj, e.Location);
                    NetworkManager.UpdateLocationNetworks(Game1.currentLocation);
                }
            }
        }

        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            if (Game1.player.craftingRecipes.ContainsKey("Iron Pipe") && Game1.player.craftingRecipes["Iron Pipe"] > 0 && !Game1.player.mailReceived.Contains("ItemPipes_SendWrench"))
            {
                Game1.player.mailbox.Add("ItemPipes_SendWrench");
            }
        }
    }
}
