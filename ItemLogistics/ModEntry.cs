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
        public SGraphDB DataAccess { get; set; }


        public override void Entry(IModHelper helper)
        {
            Framework.Model.Printer.SetMonitor(this.Monitor);
            LogisticItemIds = new Dictionary<string, int>();
            DataAccess = SGraphDB.GetSGraphDB();

            const string dataPath = "assets/data.json";
            DataModel data = null;
            try
            {
                data = this.Helper.Data.ReadJsonFile<DataModel>(dataPath);
                if (data.ValidItemNames == null)
                {
                    this.Monitor.Log($"The {dataPath} file seems to be missing or invalid.", LogLevel.Error);
                }
            }
            catch (Exception ex)
            {
                this.Monitor.Log($"The {dataPath} file seems to be invalid.\n{ex}", LogLevel.Error);
            }

            DataAccess.ValidItemNames = data.ValidItemNames;
            DataAccess.ValidLocations = data.ValidLocations;

            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.World.ObjectListChanged += this.OnObjectListChanged;
            helper.Events.GameLoop.OneSecondUpdateTicked += this.OnOneSecondUpdateTicked;
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
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
                        Framework.Model.Printer.Warn($"Can't get ID for {item.Key}");
                    }
                    else
                    {
                        Framework.Model.Printer.Info($"{item.Key} ID is {item.Key}");
                    }
                }
            }


            foreach (GameLocation location in Game1.locations)
            {
                if (DataAccess.ValidLocations.Contains(location.Name))
                {
                    Monitor.Log("LOADING " + location.Name, LogLevel.Info);
                    DataAccess.LocationGroups.Add(location, new List<LogisticGroup>());
                    DataAccess.LocationMatrix.Add(location, new SGNode[location.map.DisplayWidth, location.map.DisplayHeight]);
                    LogisticGroupBuilder.BuildLocationGraphs(location);
                    LogisticGroupManager.UpdateLocationGroups(location);
                    Monitor.Log(location.Name + " LOADED!", LogLevel.Info);
                    //SGraphManager.PrintLocationGraphs(location);
                    LogisticGroupManager.PrintLocationGroups(location);
                }
            }
        }

        private void OnObjectListChanged(object sender, ObjectListChangedEventArgs e)
        {
            List<KeyValuePair<Vector2, StardewValley.Object>> addedObjects = e.Added.ToList();
            foreach (KeyValuePair<Vector2, StardewValley.Object> obj in addedObjects)
            {
                SGraphManager.AddObject(obj);
                LogisticGroupManager.UpdateLocationGroups(Game1.currentLocation);
            }

            List<KeyValuePair<Vector2, StardewValley.Object>> removedObjects = e.Removed.ToList();
            foreach (KeyValuePair<Vector2, StardewValley.Object> obj in removedObjects)
            {
                SGraphManager.RemoveObject(obj);
                LogisticGroupManager.UpdateLocationGroups(Game1.currentLocation);
            }
        }

        private void OnOneSecondUpdateTicked(object sender, OneSecondUpdateTickedEventArgs e)
        {
            if(Context.IsWorldReady)
            {
                if(e.IsMultipleOf(180))
                {
                    SGraphDB DataAccess = SGraphDB.GetSGraphDB();
                    List<LogisticGroup> logisticGroups;
                    if (DataAccess.LocationGroups.TryGetValue(Game1.currentLocation, out logisticGroups))
                    {
                        foreach (LogisticGroup group in logisticGroups)
                        {
                            Printer.Info("Group");
                            group.ProcessExchanges();    
                        }
                    }
                }
            }
        }
    }
}
