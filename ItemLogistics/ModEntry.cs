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


        public Dictionary<GameLocation, List<LogisticGroup>> LogisticLocations { get; set; }
        public Dictionary<GameLocation, SGElement[,]> LocationsMatrix { get; set; }
        public List<string> LogisticItemNames { get; set; }
        public List<string> ValidLocations { get; set; }
        public SGElementFactory ElementFactory { get; set; }

        public override void Entry(IModHelper helper)
        {
            LogisticLocations = new Dictionary<GameLocation, List<LogisticGroup>>();
            LocationsMatrix = new Dictionary<GameLocation, SGElement[,]>();

            helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;
            helper.Events.World.ObjectListChanged += this.OnObjectListChanged;
            helper.Events.GameLoop.OneSecondUpdateTicked += this.OnOneSecondUpdateTicked;
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            //Leer de config
            ValidLocations = new List<string>
                {
                "Farm"
                };
            /*ValidLocations = new List<string>
                { 
                "FarmHouse", "Farm", "FarmCave", "Beach", "Mountain", 
                "Forest", "RailRoad", "Greenhouse", "Tunnel", "Cellar", "Cellar2", "Cellar3", "Cellar4"
                };
            */
            LogisticItemNames = new List<string>
                {
                "Wood Fence", "Hardwood Fence", "Iron Fence"
                };
            ElementFactory = new SGElementFactory(LogisticItemNames);
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
                    Monitor.Log("Can't get ID for Sewing Machine. Some functionality will be lost.", LogLevel.Warn);
                }
                else
                {
                    Monitor.Log($"Sewing Machine ID is {PipeID}.", LogLevel.Info);
                }
            }

            foreach (GameLocation location in Game1.locations)
            {
                //Va por cada location haciendo un swep
                //Y crea los grafos de cada location
                //List<LogisticGroup> groups = BuildLogistics(location);
                //LogisticLocations.Add(location, groups);
                //PrintLocationElems(LoadLocationElems(Game1.currentLocation), location);
                //Monitor.Log(location.Name, LogLevel.Info);
                if (ValidLocations.Contains(location.Name))
                {
                    LogisticLocations.Add(location, new List<LogisticGroup>());
                    LocationsMatrix.Add(location, new SGElement[location.map.DisplayWidth, location.map.DisplayHeight]);
                    Monitor.Log(location.Name + " LOADING...", LogLevel.Info);
                    LoadLocationLogistics(location);
                    BuildLocationLogistics(location);
                    Monitor.Log(location.Name + " LOADED!", LogLevel.Info);
                }
            }
        }

        private void BuildLocationLogistics(GameLocation location)
        {
            List<LogisticGroup> logisticList;
            LogisticLocations.TryGetValue(location, out logisticList);
            foreach(LogisticGroup lg in logisticList)
            {
                //Monitor.Log(lg.Build(), LogLevel.Info);
                Monitor.Log(lg.Print(), LogLevel.Info);
            }
        }

        private void LoadLocationLogistics(GameLocation location)
        {
            SGElement[,] logisticMatrix;
            LocationsMatrix.TryGetValue(location, out logisticMatrix);
            for (int x = 0; x < location.map.DisplayWidth; x++)
            {
                for (int y = 0; y < location.map.DisplayHeight; y++)
                {
                    if(location.getObjectAtTile(x, y) != null)
                    {
                        if(LogisticItemNames.Contains(location.getObjectAtTile(x, y).name))
                        {
                            LoadAdjacentsRecursive(location, null, x, y);
                        }
                    }  
                }
            }
        }

        private SGElement LoadAdjacentsRecursive(GameLocation location, SGraph lg, int x, int y)
        {
            SGElement[,] logisticMatrix;
            LocationsMatrix.TryGetValue(location, out logisticMatrix);
            if(logisticMatrix[x, y] == null)
            {
                logisticMatrix[x, y] = ElementFactory.CreateElement(new Vector2(x, y), location, location.getObjectAtTile(x, y));
            }
            SGElement elem = logisticMatrix[x, y];
            if(lg == null && elem.parentGraph == null)
            {
                elem.parentGraph = new LogisticGroup();
                List<LogisticGroup> logisticList;
                LoadElemenToGraph(location, x, y, elem.parentGraph);
                LogisticLocations.TryGetValue(location, out logisticList);
                if (!logisticList.Contains((LogisticGroup)elem.parentGraph))
                {
                    logisticList.Add((LogisticGroup)elem.parentGraph);
                }
            }
            else if(lg != null && elem.parentGraph == null)
            {
                elem.parentGraph = lg;
                //Source
                if (location.getObjectAtTile(x, y) != null)
                {
                    if (LogisticItemNames.Contains(location.getObjectAtTile(x, y).Name))
                    {
                        if (!elem.parentGraph.Elements.Contains(logisticMatrix[x, y]))
                        {
                            LoadElemenToGraph(location, x, y, lg);
                        }
                    }
                }
            }
            Monitor.Log("SOURCE: " + location.getObjectAtTile(x, y).Name + x.ToString() + (y).ToString(), LogLevel.Info);


            //Up
            if (location.getObjectAtTile(x, y - 1) != null && y - 1 >= 0)
            {
                if (LogisticItemNames.Contains(location.getObjectAtTile(x, y - 1).Name))
                {
                    Monitor.Log("Up: "+location.getObjectAtTile(x, y - 1).Name + x.ToString()+ (y-1).ToString(), LogLevel.Info);
                    if (!elem.parentGraph.Elements.Contains(logisticMatrix[x, y - 1]))
                    {
                        Monitor.Log("Inserted!", LogLevel.Info);
                        SGElement adj = LoadAdjacentsRecursive(location, elem.parentGraph, x, y - 1);
                        elem.AddAdjacent("Up", adj);
                    }
                }
            }

            //Down
            if (location.getObjectAtTile(x, y + 1) != null && y + 1 < location.map.DisplayHeight)
            {
                if (LogisticItemNames.Contains(location.getObjectAtTile(x, y + 1).Name))
                {
                    Monitor.Log("Down: " + location.getObjectAtTile(x, y + 1).Name + x.ToString() + (y + 1).ToString(), LogLevel.Info);
                    if (!elem.parentGraph.Elements.Contains(logisticMatrix[x, y + 1]))
                    {
                        SGElement adj = LoadAdjacentsRecursive(location, elem.parentGraph, x, y + 1);
                        elem.AddAdjacent("Down", adj);
                    }
                }
            }
            //Right
            if (location.getObjectAtTile(x + 1, y) != null && x + 1 < location.map.DisplayWidth)
            {
                if (LogisticItemNames.Contains(location.getObjectAtTile(x + 1, y).Name))
                {
                    Monitor.Log("Right: " + location.getObjectAtTile(x + 1, y).Name + (x + 1).ToString() + (y).ToString(), LogLevel.Info);
                    if (!elem.parentGraph.Elements.Contains(logisticMatrix[x + 1, y]))
                    {
                        SGElement adj = LoadAdjacentsRecursive(location, elem.parentGraph, x + 1, y);
                        elem.AddAdjacent("Right", adj);
                    }
                }
            }
            //Left
            if (location.getObjectAtTile(x - 1, y) != null && x - 1 >= 0)
            {
                if (LogisticItemNames.Contains(location.getObjectAtTile(x - 1, y).Name))
                {
                    Monitor.Log("Left: " + location.getObjectAtTile(x - 1, y).Name + (x - 1).ToString() + (y).ToString(), LogLevel.Info);
                    if (!elem.parentGraph.Elements.Contains(logisticMatrix[x - 1, y]))
                    {
                        SGElement adj = LoadAdjacentsRecursive(location, elem.parentGraph, x - 1, y);
                        elem.AddAdjacent("Left", adj);
                    }
                }
            }

            return elem;
        }

        private SGElement LoadAdjacents(GameLocation location, SGraph lg, int x, int y)
        {
            SGElement[,] logisticMatrix;
            LocationsMatrix.TryGetValue(location, out logisticMatrix);
            if (logisticMatrix[x, y] == null)
            {
                logisticMatrix[x, y] = ElementFactory.CreateElement(new Vector2(x, y), location, location.getObjectAtTile(x, y));
            }
            SGElement elem = logisticMatrix[x, y];
            if (lg == null && elem.parentGraph == null)
            {
                elem.parentGraph = new LogisticGroup();
                List<LogisticGroup> logisticList;
                LoadElemenToGraph(location, x, y, elem.parentGraph);
                LogisticLocations.TryGetValue(location, out logisticList);
                if (!logisticList.Contains((LogisticGroup)elem.parentGraph))
                {
                    logisticList.Add((LogisticGroup)elem.parentGraph);
                }
            }
            else if (lg != null && elem.parentGraph == null)
            {
                elem.parentGraph = lg;
                //Source
                if (location.getObjectAtTile(x, y) != null)
                {
                    if (LogisticItemNames.Contains(location.getObjectAtTile(x, y).Name))
                    {
                        if (!elem.parentGraph.Elements.Contains(logisticMatrix[x, y]))
                        {
                            LoadElemenToGraph(location, x, y, lg);
                        }
                    }
                }
            }
            Monitor.Log("SOURCE: " + location.getObjectAtTile(x, y).Name + x.ToString() + (y).ToString(), LogLevel.Info);


            //Up
            if (location.getObjectAtTile(x, y - 1) != null && y - 1 >= 0)
            {
                if (LogisticItemNames.Contains(location.getObjectAtTile(x, y - 1).Name))
                {
                    Monitor.Log("Up: " + location.getObjectAtTile(x, y - 1).Name + x.ToString() + (y - 1).ToString(), LogLevel.Info);
                    if (!elem.parentGraph.Elements.Contains(logisticMatrix[x, y - 1]))
                    {
                        Monitor.Log("Inserted!", LogLevel.Info);
                        SGElement adj = LoadAdjacentsRecursive(location, elem.parentGraph, x, y - 1);
                        elem.AddAdjacent("Up", adj);
                    }
                }
            }

            //Down
            if (location.getObjectAtTile(x, y + 1) != null && y + 1 < location.map.DisplayHeight)
            {
                if (LogisticItemNames.Contains(location.getObjectAtTile(x, y + 1).Name))
                {
                    Monitor.Log("Down: " + location.getObjectAtTile(x, y + 1).Name + x.ToString() + (y + 1).ToString(), LogLevel.Info);
                    if (!elem.parentGraph.Elements.Contains(logisticMatrix[x, y + 1]))
                    {
                        SGElement adj = LoadAdjacentsRecursive(location, elem.parentGraph, x, y + 1);
                        elem.AddAdjacent("Down", adj);
                    }
                }
            }
            //Right
            if (location.getObjectAtTile(x + 1, y) != null && x + 1 < location.map.DisplayWidth)
            {
                if (LogisticItemNames.Contains(location.getObjectAtTile(x + 1, y).Name))
                {
                    Monitor.Log("Right: " + location.getObjectAtTile(x + 1, y).Name + (x + 1).ToString() + (y).ToString(), LogLevel.Info);
                    if (!elem.parentGraph.Elements.Contains(logisticMatrix[x + 1, y]))
                    {
                        SGElement adj = LoadAdjacentsRecursive(location, elem.parentGraph, x + 1, y);
                        elem.AddAdjacent("Right", adj);
                    }
                }
            }
            //Left
            if (location.getObjectAtTile(x - 1, y) != null && x - 1 >= 0)
            {
                if (LogisticItemNames.Contains(location.getObjectAtTile(x - 1, y).Name))
                {
                    Monitor.Log("Left: " + location.getObjectAtTile(x - 1, y).Name + (x - 1).ToString() + (y).ToString(), LogLevel.Info);
                    if (!elem.parentGraph.Elements.Contains(logisticMatrix[x - 1, y]))
                    {
                        SGElement adj = LoadAdjacentsRecursive(location, elem.parentGraph, x - 1, y);
                        elem.AddAdjacent("Left", adj);
                    }
                }
            }

            return elem;
        }

        private void LoadElemenToGraph(GameLocation location, int x, int y, SGraph lg)
        {
            SGElement[,] logisticMatrix;
            LocationsMatrix.TryGetValue(location, out logisticMatrix);
            lg.AddElement(logisticMatrix[x, y]);
            if (logisticMatrix[x, y] is OutPipe)
            {
                Monitor.Log("OUTPIPE", LogLevel.Info);
                lg.AddOutput((OutPipe)logisticMatrix[x, y]);
            }
            if (logisticMatrix[x, y] is InPipe)
            {
                Monitor.Log("INPIPE", LogLevel.Info);
                lg.AddInput((InPipe)logisticMatrix[x, y]);
            }
            if (logisticMatrix[x, y] is Pipe)
            {
                Monitor.Log("PIPE", LogLevel.Info);
                lg.AddConector((Pipe)logisticMatrix[x, y]);
            }
        }

        private void LoadElemenToGraph2(SGElement elem, SGraph lg)
        {
            lg.AddElement(elem);
            if (elem is OutPipe)
            {
                Monitor.Log("OUTPIPE", LogLevel.Info);
                lg.AddOutput((OutPipe)elem);
            }
            if (elem is InPipe)
            {
                Monitor.Log("INPIPE", LogLevel.Info);
                lg.AddInput((InPipe)elem);
            }
            if (elem is Pipe)
            {
                Monitor.Log("PIPE", LogLevel.Info);
                lg.AddConector((Pipe)elem);
            }
        }

        private void OnObjectListChanged(object sender, ObjectListChangedEventArgs e)
        {
            List<KeyValuePair<Vector2, StardewValley.Object>> addedObjects = e.Added.ToList();
            foreach(KeyValuePair< Vector2, StardewValley.Object > obj in addedObjects)
            {
                AddObject(obj);
            }

            List<KeyValuePair<Vector2, StardewValley.Object>> removedObjects = e.Removed.ToList();
            foreach (KeyValuePair<Vector2, StardewValley.Object> obj in removedObjects)
            {
                RemoveObject(obj);
            }
        }

        private void AddNewElement(SGElement newElem, SGraph lg)
        {
            SGElement[,] logisticMatrix;
            LocationsMatrix.TryGetValue(Game1.currentLocation, out logisticMatrix);
            newElem.parentGraph = lg;
            logisticMatrix[(int)newElem.Position.X, (int)newElem.Position.Y] = newElem;
            LoadElemenToGraph(newElem.Location, (int)newElem.Position.X, (int)newElem.Position.Y, lg);
        }


        private void AddObject(KeyValuePair<Vector2, StardewValley.Object> obj)
        {
            this.Monitor.Log("ADDING: " + obj.Key.ToString() + obj.Value.Name, LogLevel.Info);
            //Conector
            if (LogisticItemNames.Contains(obj.Value.Name))
            {
                SGElement[,] logisticMatrix;
                LocationsMatrix.TryGetValue(Game1.currentLocation, out logisticMatrix);
                SGElement newElem = ElementFactory.CreateElement(obj.Key, Game1.currentLocation, obj.Value);
                logisticMatrix[(int)newElem.Position.X, (int)newElem.Position.Y] = newElem;
                if (logisticMatrix[(int)newElem.Position.X, (int)newElem.Position.Y - 1] != null) 
                {
                    newElem.AddAdjacent("Up", logisticMatrix[(int)newElem.Position.X, (int)newElem.Position.Y - 1]);
                }
                if (logisticMatrix[(int)newElem.Position.X, (int)newElem.Position.Y + 1] != null)
                {
                    newElem.AddAdjacent("Down", logisticMatrix[(int)newElem.Position.X, (int)newElem.Position.Y + 1]);
                }
                if (logisticMatrix[(int)newElem.Position.X + 1, (int)newElem.Position.Y] != null)
                {
                    newElem.AddAdjacent("Right", logisticMatrix[(int)newElem.Position.X + 1, (int)newElem.Position.Y]);
                }
                if (logisticMatrix[(int)newElem.Position.X - 1, (int)newElem.Position.Y] != null)
                {
                    newElem.AddAdjacent("Left", logisticMatrix[(int)newElem.Position.X - 1, (int)newElem.Position.Y]);
                }

                List<SGraph> adjGraphs = newElem.Scan();
                Monitor.Log("Graphs adj: " + adjGraphs.Count.ToString(), LogLevel.Info);

                if (adjGraphs.Count == 0)
                {
                    Monitor.Log("New graph", LogLevel.Info);
                    LogisticGroup lg = new LogisticGroup();
                    AddNewElement(newElem, lg);
                    Monitor.Log(lg.Print(), LogLevel.Info);
                    Monitor.Log((lg == null).ToString(), LogLevel.Info);
                }
                else
                {
                    foreach (SGraph lg in adjGraphs)
                    {
                        Monitor.Log("Params: " + (lg == null).ToString() + (newElem.parentGraph == null).ToString(), LogLevel.Info);
                        if (lg != null && newElem.parentGraph == null)
                        {
                            Monitor.Log("Existing graph", LogLevel.Info);
                            //LoadAdjacents(Game1.currentLocation, lg, (int)obj.Key.X, (int)obj.Key.Y);
                            AddNewElement(newElem, lg);
                            Monitor.Log(lg.Print(), LogLevel.Info);
                        }
                        else if(lg != null && newElem.parentGraph != null)
                        {
                            //Segundo graph
                            Monitor.Log("Segundo graph", LogLevel.Info);
                            List<SGraph> orderedAdjGraphs = adjGraphs.OrderByDescending(s => s.Elements.Count).ToList();
                            MergeGraphs(orderedAdjGraphs);
                        }
                        else
                        {
                            Monitor.Log("No graph", LogLevel.Info);
                        }
                    }

                }
            }
        }

        //Añadir todos los items de la lista de
        //grafos al primer grafo (el mas largo)
        private void MergeGraphs(List<SGraph> graphs)
        {
            for(int i=1;i<graphs.Count;i++)
            {
                Monitor.Log("G size:" + graphs[i].Elements.Count.ToString(), LogLevel.Info);
                foreach(SGElement elem in graphs[i].Elements)
                {
                    elem.parentGraph = graphs[0];
                    LoadElemenToGraph2(elem, graphs[0]);
                }
            }
        }

        private void SplitGraphs(List<SGraph> graphs)
        {

            for (int i = 1; i < graphs.Count; i++)
            {
                foreach (SGElement elem in graphs[i].Elements)
                {
                    elem.parentGraph = graphs[0];
                    LoadElemenToGraph2(elem, graphs[0]);
                }
            }
        }

        private void RemoveObject(KeyValuePair<Vector2, StardewValley.Object> obj)
        {
            this.Monitor.Log("REMOVE: " + obj.Key.ToString() + obj.Value.Name, LogLevel.Info);
            //Conector
            if (LogisticItemNames.Contains(obj.Value.Name))
            {
                SGElement[,] logisticMatrix;
                LocationsMatrix.TryGetValue(Game1.currentLocation, out logisticMatrix);
                SGElement elem = logisticMatrix[(int)obj.Key.X, (int)obj.Key.Y];
                List<SGraph> adjGraphs = elem.Scan();
                logisticMatrix[(int)elem.Position.X, (int)elem.Position.Y] = null;
                elem.parentGraph.RemoveElement(elem);
                elem.RemoveAllAdjacents();
                //Split graphs
                //Hay que recorrerlos enteros para volver a construirlos

            }
        }

        private void OnOneSecondUpdateTicked(object sender, OneSecondUpdateTickedEventArgs e)
        {

        }

        private List<SGElement> LoadLocationElems(GameLocation location)
        {
            List<SGElement> locationElems = new List<SGElement>();
            //location.findNearestObject
            for (int x = 0; x < location.map.DisplayWidth; x++)
            {
                for (int y = 0; y < location.map.DisplayHeight; y++)
                {
                    Vector2 tile = new Vector2(x, y);
                    if (location.getObjectAtTile(x, y) != null)
                    {
                        StardewValley.Object obj = location.getObjectAtTile(x, y);

                        //Crear un nuevo objeto que tenga
                        //Un Vector2 con la pos, la location y un StardewObject
                        SGElement elem = new SGElement(tile, location, obj);
                        locationElems.Add(elem);
                    }
                }
            }

            return locationElems;
        }

        private string PrintLocationElems(List<SGElement> elems, GameLocation location)
        {
            StringBuilder stringBuffer = new StringBuilder();
            stringBuffer.Append("Elements: \n");
            foreach (SGElement elem in elems)
            {
                stringBuffer.Append(elem.Position + " " + elem.Obj.Name);
                for (int x = 0; x < location.map.DisplayWidth; x++)
                {
                    stringBuffer.AppendLine(x + " ");
                    for (int y = 0; y < location.map.DisplayHeight; y++)
                    {
                        if (elem.Position.X == x && elem.Position.Y == y)
                        {
                            stringBuffer.Append(elem.Obj.name);
                        }
                    }
                }
            }
            return stringBuffer.ToString();
        }
    }
}
