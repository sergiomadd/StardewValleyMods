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
using ItemLogistics.Framework;
using ItemLogistics.Framework.Model;
using ItemLogistics.Framework.Patches;
using HarmonyLib;

namespace ItemLogistics
{
    public interface IJsonAssetsApi
    {
        int GetObjectId(string name);
        void LoadAssets(string path);
    }

    class ModEntry : Mod
    {
        private IJsonAssetsApi JsonAssets;
        public Dictionary<string, int> LogisticItemIds;
        public DataAccess DataAccess { get; set; }


        public override void Entry(IModHelper helper)
        {
            Framework.Printer.SetMonitor(this.Monitor);
            LogisticItemIds = new Dictionary<string, int>();
            DataAccess = DataAccess.GetDataAccess();

            const string dataPath = "assets/data.json";
            DataModel data = null;
            try
            {
                data = this.Helper.Data.ReadJsonFile<DataModel>(dataPath);
                if (data.ValidNetworkItems == null)
                {
                    this.Monitor.Log($"The {dataPath} file seems to be missing or invalid.", LogLevel.Error);
                }
            }
            catch (Exception ex)
            {
                this.Monitor.Log($"The {dataPath} file seems to be invalid.\n{ex}", LogLevel.Error);
            }

            DataAccess.ValidNetworkItems = data.ValidNetworkItems;
            DataAccess.ValidPipeNames = data.ValidPipeNames;
            DataAccess.ValidIOPipeNames = data.ValidIOPipeNames;
            DataAccess.ValidLocations = data.ValidLocations;
            DataAccess.ValidExtraNames = data.ValidExtraNames;
            DataAccess.ValidItems = data.ValidItems;


            var harmony = new Harmony(this.ModManifest.UniqueID);
            FencePatcher.Apply(harmony);
            //ChestPatcher.Apply(harmony);


            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
            helper.Events.World.ObjectListChanged += this.OnObjectListChanged;
            helper.Events.GameLoop.OneSecondUpdateTicked += this.OnOneSecondUpdateTicked;

        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            Globals.Debug = true;
            JsonAssets = Helper.ModRegistry.GetApi<IJsonAssetsApi>("spacechase0.JsonAssets");
            if (JsonAssets == null)
            {
                Monitor.Log("Can't load Json Assets API, which is needed for Home Sewing Kit to function", LogLevel.Error);
            }
            else
            {
                JsonAssets.LoadAssets(Path.Combine(Helper.DirectoryPath, "assets"));

            }
        }



        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            if (JsonAssets != null)
            {
                foreach (KeyValuePair<string, int> item in LogisticItemIds)
                {
                    LogisticItemIds.Add(item.Key, JsonAssets.GetObjectId(item.Key));
                    if (item.Value == -1)
                    {
                        Printer.Warn($"Can't get ID for {item.Key}");
                    }
                    else
                    {
                        Printer.Info($"{item.Key} ID is {item.Key}");
                    }
                }
            }

            
            foreach (GameLocation location in Game1.locations)
            {
                if (DataAccess.ValidLocations.Contains(location.Name))
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
                }
            }
        }

        private void OnOneSecondUpdateTicked(object sender, OneSecondUpdateTickedEventArgs e)
        {
            if (Context.IsWorldReady)
            {
                if (e.IsMultipleOf(120))
                {
                    DataAccess DataAccess = DataAccess.GetDataAccess();
                    List<Network> networks;
                    if (DataAccess.LocationNetworks.TryGetValue(Game1.currentLocation, out networks))
                    {
                        Printer.Info("Network amount: "+networks.Count.ToString());
                        foreach (Network network in networks)
                        {
                            network.ProcessExchanges();
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
            RepairPipes();
        }

        private void RepairPipes()
        {
            foreach (GameLocation location in Game1.locations)
            {
                foreach (Fence fence in location.Objects.Values.OfType<Fence>())
                {
                    if (DataAccess.ValidPipeNames.Contains(fence.name))
                    {
                        fence.health.Value = 100f;
                        fence.maxHealth.Value = fence.health.Value;
                    }
                }
            }
        }
    }
}
