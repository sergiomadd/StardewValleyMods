using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using SVObject = StardewValley.Objects;
using ItemPipes.Framework;
using ItemPipes.Framework.Util;
using ItemPipes.Framework.Model;
using ItemPipes.Framework.Patches;
using ItemPipes.Framework.Nodes;
using ItemPipes.Framework.Items;
using HarmonyLib;

namespace ItemPipes
{
    public interface ISpaceCoreApi
    {
        void RegisterSerializerType(Type type);
    }
    class ModEntry : Mod
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

            string dataPath = "assets/data.json";
            DataModel data = null;
            ModConfig config = null;
            try
            {
                data = this.Helper.Data.ReadJsonFile<DataModel>(dataPath);
                if (data == null)
                {
                    this.Monitor.Log($"The {dataPath} file seems to be missing or invalid.", LogLevel.Error);
                }
                config = this.Helper.ReadConfig<ModConfig>();
                if (config == null)
                {
                    this.Monitor.Log($"The config file seems to be missing or invalid.", LogLevel.Error);
                }
            }
            catch (Exception ex)
            {
                this.Monitor.Log($"The {dataPath} file seems to be invalid.\n{ex}", LogLevel.Error);
            }

            DataAccess.ModItems = data.ModItems;
            DataAccess.NetworkItems = data.NetworkItems;
            DataAccess.PipeNames = data.PipeNames;
            DataAccess.IOPipeNames = data.IOPipeNames;
            DataAccess.ExtraNames = data.ExtraNames;
            DataAccess.Buildings = data.Buildings;
            DataAccess.Locations = data.Locations;

            if(config.DebugMode)
            {
                Globals.Debug = true;
            }
            else
            {
                Globals.Debug = false;
            }
            if (!config.DisableItemSending)
            {
                Globals.DisableItemSending = true;
            }
            else
            {
                Globals.DisableItemSending = false;
            }
            //REMOVE
            Globals.Debug = false;

            var harmony = new Harmony(this.ModManifest.UniqueID);
            //FencePatcher.Apply(harmony);
            //ChestPatcher.Apply(harmony);
            

            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
            helper.Events.World.ObjectListChanged += this.OnObjectListChanged;
            helper.Events.GameLoop.OneSecondUpdateTicked += this.OnOneSecondUpdateTicked;

        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            //Helper.Content.AssetEditors.Add(this);
            var spaceCore = this.Helper.ModRegistry.GetApi<ISpaceCoreApi>("spacechase0.SpaceCore");
            spaceCore.RegisterSerializerType(typeof(IronPipeItem));
            spaceCore.RegisterSerializerType(typeof(ExtractorPipeItem));
            spaceCore.RegisterSerializerType(typeof(InserterPipeItem));
            spaceCore.RegisterSerializerType(typeof(PolymorphicPipeItem));
            spaceCore.RegisterSerializerType(typeof(FilterPipeItem));
            spaceCore.RegisterSerializerType(typeof(WrenchItem));
        }

        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            Reload();
            
            foreach (GameLocation location in Game1.locations)
            {
                //Monitor.Log("LOADING " + location.Name, LogLevel.Info);
                DataAccess.LocationNetworks.Add(location, new List<Network>());
                DataAccess.LocationNodes.Add(location, new List<Node>());
                NetworkBuilder.BuildLocationNetworks(location);
                NetworkManager.UpdateLocationNetworks(location);
                //Monitor.Log(location.Name + " LOADED!", LogLevel.Info);
                /*
                if (Globals.Debug)
                {
                    NetworkManager.PrintLocationNetworks(location);
                }
                */
                /*
                if (DataAccess.Locations.Contains(location.Name))
                {
                    Monitor.Log("LOADING " + location.Name, LogLevel.Info);
                    DataAccess.LocationNetworks.Add(location, new List<Network>());
                    DataAccess.LocationMatrix.Add(location, new Node[location.map.DisplayWidth, location.map.DisplayHeight]);
                    NetworkBuilder.BuildLocationNetworks(location);
                    NetworkManager.UpdateLocationNetworks(location);
                    Monitor.Log(location.Name + " LOADED!", LogLevel.Info);
                    if(Globals.Debug)
                    {
                        NetworkManager.PrintLocationNetworks(location);
                    }
                }*/
            }
        }

        private void Reload()
        {
            DataAccess.LocationNodes.Clear();
            DataAccess.LocationNetworks.Clear();
            DataAccess.UsedNetworkIDs.Clear();
        }

        private void OnOneSecondUpdateTicked(object sender, OneSecondUpdateTickedEventArgs e)
        {
            if(Globals.DisableItemSending)
            {
                if (Context.IsWorldReady)
                {
                    /*if (e.IsMultipleOf(30))
                    {
                        if (Globals.Debug) { Printer.Info($"[X] UPDATETICKET"); }
                        Animator.updated = true;
                    }*/
                    //Tier 1 Extractors
                    if (e.IsMultipleOf(120))
                    {
                        DataAccess DataAccess = DataAccess.GetDataAccess();
                        List<Network> networks;
                        foreach (GameLocation location in Game1.locations)
                        {
                            if (DataAccess.LocationNetworks.TryGetValue(location, out networks))
                            {
                                if(networks.Count > 0)
                                {
                                    if (Globals.Debug) { Printer.Info("Network amount: " + networks.Count.ToString()); }
                                    foreach (Network network in networks)
                                    {
                                        network.ProcessExchanges(1);
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
                NetworkManager.AddObject(obj);
                NetworkManager.UpdateLocationNetworks(Game1.currentLocation);
            }

            List<KeyValuePair<Vector2, StardewValley.Object>> removedObjects = e.Removed.ToList();
            foreach (KeyValuePair<Vector2, StardewValley.Object> obj in removedObjects)
            {
                NetworkManager.RemoveObject(obj);
                NetworkManager.UpdateLocationNetworks(Game1.currentLocation);
            }
        }

        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            for(int i=0;i<15;i++)
            {
                //Game1.player.addItemToInventory(new Test());
                Game1.player.addItemToInventory(new IronPipeItem());
                Game1.player.addItemToInventory(new ExtractorPipeItem());
                Game1.player.addItemToInventory(new InserterPipeItem());
                Game1.player.addItemToInventory(new PolymorphicPipeItem());
                Game1.player.addItemToInventory(new FilterPipeItem());
                //Game1.player.addItemToInventory(new Pipe());
            }
            if(!Game1.player.hasItemInInventoryNamed("Wrench"))
            {
                Game1.player.addItemToInventory(new WrenchItem());
            }
            RepairPipes();
        }

        private void RepairPipes()
        {
            foreach (GameLocation location in Game1.locations)
            {
                foreach (Fence fence in location.Objects.Values.OfType<Fence>())
                {
                    if (DataAccess.PipeNames.Contains(fence.name))
                    {
                        fence.health.Value = 100f;
                        fence.maxHealth.Value = fence.health.Value;
                    }
                }
            }
        }
    }
}
