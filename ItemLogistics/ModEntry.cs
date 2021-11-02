using System;
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
    class ModEntry : Mod
    {
        public Dictionary<GameLocation, List<LogisticGroup>> LogisticLocations { get; set; }
        public Dictionary<GameLocation, SGElement[,]> LocationsMatrix { get; set; }
        public List<string> LogisticItemNames { get; set; }
        public List<string> ValidLocations { get; set; }
        public SGElementFactory ElementFactory { get; set; }

        public override void Entry(IModHelper helper)
        {
            LogisticLocations = new Dictionary<GameLocation, List<LogisticGroup>>();
            LocationsMatrix = new Dictionary<GameLocation, SGElement[,]>();
            

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

        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            foreach(GameLocation location in Game1.locations)
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
                            LoadAdjacents(location, null, x, y);
                        }
                    }  
                }
            }
        }

        private SGElement LoadAdjacents(GameLocation location, SGraph lg, int x, int y)
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
                elem.parentGraph.AddElement(elem);
                LoadPipeType(location, x, y);
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
                            lg.AddElement(elem);
                            LoadPipeType(location, x, y);
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
                        LoadPipeType(location, x, y);
                        SGElement adj = LoadAdjacents(location, elem.parentGraph, x, y - 1);
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
                        SGElement adj = LoadAdjacents(location, elem.parentGraph, x, y + 1);
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
                        SGElement adj = LoadAdjacents(location, elem.parentGraph, x + 1, y);
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
                        SGElement adj = LoadAdjacents(location, elem.parentGraph, x - 1, y);
                        elem.AddAdjacent("Left", adj);
                    }
                }
            }

            return elem;
        }

        private void LoadPipeType(GameLocation location, int x, int y)
        {
            SGElement[,] logisticMatrix;
            LocationsMatrix.TryGetValue(location, out logisticMatrix);
            if (logisticMatrix[x, y] is OutPipe)
            {
                Monitor.Log("OUTPIPE", LogLevel.Info);
                logisticMatrix[x, y].parentGraph.AddOutput((OutPipe)logisticMatrix[x, y]);
            }
            if (logisticMatrix[x, y] is InPipe)
            {
                Monitor.Log("INPIPE", LogLevel.Info);
                logisticMatrix[x, y].parentGraph.AddInput((InPipe)logisticMatrix[x, y]);
            }
            if (logisticMatrix[x, y] is Pipe)
            {
                Monitor.Log("PIPE", LogLevel.Info);
                logisticMatrix[x, y].parentGraph.AddConector((Pipe)logisticMatrix[x, y]);
            }
        }



        private LogisticGroup FindLogisticGroup(GameLocation location)
        {
            LogisticGroup group = new LogisticGroup(); ;
            int id = 0;
            //Sweeper
            /*
            while (true)
            {
                //si el objeto encontrado es uno de 
                //los del mod
                if(obj is Mod.obj)
                {
                    //Empezar el grafo
                    UpdateObject(obj);




                }
                //Cuando acabe se añade +1 a id
                //Acaba cuando una linea y una columna
                //No han tenido objetos del grafo
                group.ID = id;
                id++;

            }
            */

            return group;
        }



        private void OnObjectListChanged(object sender, ObjectListChangedEventArgs e)
        {
            List<KeyValuePair<Vector2, StardewValley.Object>> objects = e.Added.ToList();
            foreach(KeyValuePair< Vector2, StardewValley.Object > obj in objects)
            {
                UpdateObject(obj);
            }
        }
        
        private void UpdateObject(KeyValuePair<Vector2, StardewValley.Object> obj)
        {
            this.Monitor.Log("CHANGE: " + obj.Key.ToString() + Game1.currentLocation.ToString(), LogLevel.Info);
            //Conector
            if (obj.Value.name.Equals("Wood Fence"))
            {
                /*
                this.Monitor.Log("CONECTOR", LogLevel.Info);
                SGElement newElem = ElementFactory.CreateElement(obj.Key, Game1.currentLocation, obj.Value);
                List<SGraph> adjGraphs = newElem.Scan();
                if (adjGraphs.Count == 0)
                {
                    LogisticGroup lg = new LogisticGroup();
                    lg.AddConector(newElem);
                }
                else if (adjGraphs.Count == 1)
                {
                    adjGraphs
                    LoadAdjacents(Game1.currentLocation, SGraph lg, int x, int y)
                }
                else
                {
                    List<SGUnit> conections = pipe.Adjacents.Values.ToList();
                    foreach (SGUnit connection in conections)
                    {
                        connection.ParentGraph.AddConector(pipe);
                        connection.ParentGraph.Conectors.Add(pipe);
                        pipe.AddAdjacent("Up", connection);
                    }

                }
                */
            }
            //Out
            else if (obj.Value.Name.Equals("Stone Fence"))
            {
                /*this.Monitor.Log("OUTPUT", LogLevel.Info);
                OutPipe output = new OutPipe();
                if (!output.Scan())
                {
                    //Other Storage Managwre not found
                    LogisticGroup lg = new LogisticGroup();
                    lg.AddOutput(output);
                }
                else
                {
                    //GetConections = scan()
                    List<SGUnit> conections = output.Adjacents.Values.ToList();
                    foreach (SGUnit connection in conections)
                    {
                        connection.ParentGraph.AddOutput(output);
                        output.AddAdjacent("Up", connection);
                    }
                }*/
            }
            //In Brick Floor
            else if (obj.Value.Name.Equals("Chest"))
            {
                /*Container newContainer = new Container(obj.Key, Game1.currentLocation);
                if (newContainer.ScanAround())
                {
                    //Other Storage Managwre found
                    Pipe pipe = newContainer.GetLGAround();
                    LogisticGroup lg = pipe.ParentGraph;
                    newContainer.ParentGraph = lg;
                    lg.AddNode(obj.Key, Game1.currentLocation, pipe);
                }*/
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
