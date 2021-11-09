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
        private int PipeID = -1;
        public SGraphDB DataAccess { get; set; }

        public override void Entry(IModHelper helper)
        {
            Framework.Model.Console.SetMonitor(this.Monitor);
            DataAccess = SGraphDB.GetSGraphDB();

            helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;
            helper.Events.World.ObjectListChanged += this.OnObjectListChanged;
            helper.Events.GameLoop.OneSecondUpdateTicked += this.OnOneSecondUpdateTicked;
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
        }

        private void GameLoop_GameLaunched(object sender, GameLaunchedEventArgs e)
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

                PipeID = JsonAssets.GetObjectId("Pipe");
                if (PipeID == -1)
                {
                    Monitor.Log("Can't get ID for Pipe. Some functionality will be lost.", LogLevel.Warn);
                }
                else
                {
                    Monitor.Log($"Sewing Machine ID is {PipeID}.", LogLevel.Info);
                }
            }

            foreach (GameLocation location in Game1.locations)
            {
                if (DataAccess.ValidLocations.Contains(location.Name))
                {
                    Monitor.Log("LOADING " + location.Name, LogLevel.Info);
                    DataAccess.LocationGraphs.Add(location, new List<SGraph>());
                    DataAccess.LocationMatrix.Add(location, new SGNode[location.map.DisplayWidth, location.map.DisplayHeight]);
                    SGraphBuilder.BuildLocationGraphs(location);
                    Monitor.Log(location.Name + " LOADED!", LogLevel.Info);
                    SGraphManager.PrintLocationGraphs(location);
                }
            }
        }

        private void OnObjectListChanged(object sender, ObjectListChangedEventArgs e)
        {
            List<KeyValuePair<Vector2, StardewValley.Object>> addedObjects = e.Added.ToList();
            foreach(KeyValuePair<Vector2, StardewValley.Object> obj in addedObjects)
            {
                SGraphManager.AddObject(obj);
            }

            List<KeyValuePair<Vector2, StardewValley.Object>> removedObjects = e.Removed.ToList();
            foreach (KeyValuePair<Vector2, StardewValley.Object> obj in removedObjects)
            {
                SGraphManager.RemoveObject(obj);
            }
        }

        private void OnOneSecondUpdateTicked(object sender, OneSecondUpdateTickedEventArgs e)
        {

        }
    }
}
