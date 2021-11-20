using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemLogistics.Framework.Model;
using StardewValley;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework;

namespace ItemLogistics.Framework
{
    public static class LogisticGroupManager
    {
        public static void UpdateLocationGroups(GameLocation location)
        {
            SGraphDB DataAccess = SGraphDB.GetSGraphDB();
            List<LogisticGroup> groupList;
            if (DataAccess.LocationGroups.TryGetValue(location, out groupList))
            {
                foreach (LogisticGroup group in groupList)
                {
                    //Printer.Info("UPDATING");
                    group.Update();
                }
            }
        }


        public static void LoadNodeToGraph(GameLocation location, int x, int y, SGraph graph)
        {
            SGraphDB DataAccess = SGraphDB.GetSGraphDB();
            SGNode[,] locationMatrix;
            if (DataAccess.LocationMatrix.TryGetValue(location, out locationMatrix))
            {
                Printer.Info(x.ToString() + y.ToString());
                graph.AddNode(locationMatrix[x, y]);
                if (graph is LogisticGroup)
                {
                    LogisticGroup lg = (LogisticGroup)graph;
                    if (locationMatrix[x, y] is ExtractorPipe)
                    {
                        lg.AddOutput((ExtractorPipe)locationMatrix[x, y]);
                    }
                    else if (locationMatrix[x, y] is InserterPipe)
                    {
                        lg.AddInput((InserterPipe)locationMatrix[x, y]);
                    }
                    else if (locationMatrix[x, y] is PolymorphicPipe)
                    {
                        lg.AddInput((PolymorphicPipe)locationMatrix[x, y]);
                    }
                    else if (locationMatrix[x, y] is FilterPipe)
                    {
                        lg.AddInput((FilterPipe)locationMatrix[x, y]);
                    }
                    else if (locationMatrix[x, y] is Container)
                    {
                        lg.AddContainer((Container)locationMatrix[x, y]);
                    }
                    else if (locationMatrix[x, y] is Pipe)
                    {
                        lg.AddConnector((Pipe)locationMatrix[x, y]);
                    }
                }
            }
        }

        public static void AddNewElement(SGNode newElem, SGraph lg)
        {
            SGraphDB DataAccess = SGraphDB.GetSGraphDB();
            SGNode[,] logisticMatrix;
            if (DataAccess.LocationMatrix.TryGetValue(Game1.currentLocation, out logisticMatrix))
            {
                newElem.ParentGraph = lg;
                logisticMatrix[(int)newElem.Position.X, (int)newElem.Position.Y] = newElem;
                LoadNodeToGraph(newElem.Location, (int)newElem.Position.X, (int)newElem.Position.Y, lg);
            }
        }



        public static void AddObject(KeyValuePair<Vector2, StardewValley.Object> obj)
        {
            SGraphDB DataAccess = SGraphDB.GetSGraphDB();
            Printer.Info("ADDING: " + obj.Key.ToString() + obj.Value.Name);
            if (DataAccess.ValidItemNames.Contains(obj.Value.Name))
            {
                SGNode[,] locationMatrix;
                if (DataAccess.LocationMatrix.TryGetValue(Game1.currentLocation, out locationMatrix))
                {
                    SGNode newNode = SGNodeFactory.CreateElement(obj.Key, Game1.currentLocation, obj.Value);
                    int x = (int)newNode.Position.X;
                    int y = (int)newNode.Position.Y;
                    locationMatrix[x, y] = newNode;
                    if (locationMatrix[x, y - 1] != null)
                    {
                        newNode.AddAdjacent(SideStruct.GetSides().North, locationMatrix[x, y - 1]);
                    }
                    if (locationMatrix[x, y + 1] != null)
                    {
                        newNode.AddAdjacent(SideStruct.GetSides().South, locationMatrix[x, y + 1]);
                    }
                    if (locationMatrix[x + 1, y] != null)
                    {
                        newNode.AddAdjacent(SideStruct.GetSides().West, locationMatrix[x + 1, y]);
                    }
                    if (locationMatrix[x - 1, y] != null)
                    {
                        newNode.AddAdjacent(SideStruct.GetSides().East, locationMatrix[x - 1, y]);
                    }
                    newNode.Print();
                    if (newNode is Input)
                    {
                        Input input = (Input)newNode;
                    }
                    List<SGraph> adjGraphs = newNode.Scan();

                    if (adjGraphs.Count == 0)
                    {
                        SGraph graph = CreateLocationGraph(Game1.currentLocation);
                        AddNewElement(newNode, graph);
                    }
                    else
                    {
                        List<SGraph> orderedAdjGraphs = adjGraphs.OrderByDescending(s => s.Nodes.Count).ToList();
                        newNode.ParentGraph = orderedAdjGraphs[0];
                        AddNewElement(newNode, orderedAdjGraphs[0]);
                        MergeGraphs(orderedAdjGraphs);
                        List<LogisticGroup> logisticGroups;
                    }
                }
            }
        }

        private static void MergeGraphs(List<SGraph> graphs)
        {
            Printer.Info(graphs.Count.ToString());
            for (int i = 1; i < graphs.Count; i++)
            {
                Printer.Info("G size:" + graphs[i].Nodes.Count.ToString());
                foreach (SGNode elem in graphs[i].Nodes)
                {
                    elem.ParentGraph = graphs[0];
                    LoadNodeToGraph(Game1.currentLocation, (int)elem.Position.X, (int)elem.Position.Y, graphs[0]);
                }
            }
        }

        public static void RemoveObject(KeyValuePair<Vector2, StardewValley.Object> obj)
        {
            SGraphDB DataAccess = SGraphDB.GetSGraphDB();
            Printer.Info("REMOVE: " + obj.Key.ToString() + obj.Value.Name);
            if (DataAccess.ValidItemNames.Contains(obj.Value.Name))
            {
                SGNode[,] locationMatrix;
                if (DataAccess.LocationMatrix.TryGetValue(Game1.currentLocation, out locationMatrix))
                {
                    SGNode node = locationMatrix[(int)obj.Key.X, (int)obj.Key.Y];
                    if (node.ParentGraph != null)
                    {
                        List<SGraph> adjGraphs = node.Scan();
                        node.ParentGraph.RemoveNode(node);
                        List<LogisticGroup> logisticGroups;
                        locationMatrix[(int)node.Position.X, (int)node.Position.Y] = null;
                        if (adjGraphs.Count > 0)
                        {
                            RemakeGraphs(node);
                        }
                        if (node is Input)
                        {
                            Input input = (Input)node;
                            input.RemoveAllAdjacents();
                        }
                    }
                }
            }
        }

        public static void RemakeGraphs(SGNode node)
        {
            SGraphDB DataAccess = SGraphDB.GetSGraphDB();
            Printer.Info("Remaking");
            foreach (KeyValuePair<Side, SGNode> adj in node.Adjacents)
            {
                if (adj.Value != null)
                {
                    adj.Value.Print();
                    List<LogisticGroup> logisticGroups;
                    if (DataAccess.LocationGroups.TryGetValue(Game1.currentLocation, out logisticGroups))
                    {
                        logisticGroups.Remove((LogisticGroup)adj.Value.ParentGraph);
                    }
                    if (adj.Value.ParentGraph != null)
                    {
                        adj.Value.ParentGraph.Delete();
                    }
                    SGNode newNode = LogisticGroupBuilder.BuildGraphRecursive(Game1.currentLocation, null, (int)adj.Value.Position.X, (int)adj.Value.Position.Y);
                }
            }
        }

        public static SGraph CreateLocationGraph(GameLocation location)
        {
            SGraphDB DataAccess = SGraphDB.GetSGraphDB();
            LogisticGroup newLG = new LogisticGroup();
            List<LogisticGroup> groupList;
            if (DataAccess.LocationGroups.TryGetValue(location, out groupList))
            {
                if (!groupList.Contains(newLG))
                {
                    groupList.Add(newLG);
                }
            }
            return newLG;
        }

        public static void PrintLocationGroups(GameLocation location)
        {
            SGraphDB DataAccess = SGraphDB.GetSGraphDB();
            List<LogisticGroup> groupList;
            if (DataAccess.LocationGroups.TryGetValue(location, out groupList))
            {
                Printer.Info($"NUMBER OF GROUPS: {groupList.Count}");
                foreach (LogisticGroup group in groupList)
                {
                    Printer.Info(group.Print());
                }
            }
        }
    }
}
