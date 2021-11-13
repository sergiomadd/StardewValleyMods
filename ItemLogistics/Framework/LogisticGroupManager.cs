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
                    Printer.Info("UPDATING");
                    group.Update();
                }
            }
        }


        public static void LoadNodeToGraph(GameLocation location, int x, int y, SGraph graph)
        {
            SGraphDB DataAccess = SGraphDB.GetSGraphDB();
            /*Refactor:
             * Move check for pipe type to logistic group
             * Create logisticGroupManager for loading node types
            */
            SGNode[,] locationMatrix;
            if (DataAccess.LocationMatrix.TryGetValue(location, out locationMatrix))
            {
                graph.AddNode(locationMatrix[x, y]);
                if (graph is LogisticGroup)
                {
                    LogisticGroup lg = (LogisticGroup)graph;
                    if (locationMatrix[x, y] is ExtractorPipe)
                    {
                        Printer.Info("OUTPIPE");
                        lg.AddOutput((ExtractorPipe)locationMatrix[x, y]);
                    }
                    else if (locationMatrix[x, y] is Input)
                    {
                        Printer.Info("INPIPE");
                        lg.AddInput((Input)locationMatrix[x, y]);
                    }
                    else if (locationMatrix[x, y] is Input)
                    {
                        Printer.Info("CHEST");
                        lg.AddContainer((Container)locationMatrix[x, y]);
                    }
                    else if (locationMatrix[x, y] is Pipe)
                    {
                        Printer.Info("PIPE");
                        lg.AddConnector((Pipe)locationMatrix[x, y]);
                    }
                    Printer.Info(lg.Containers.Count.ToString());
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

                    List<SGraph> adjGraphs = newNode.Scan();

                    if (adjGraphs.Count == 0)
                    {
                        SGraph graph = new SGraph();
                        AddNewElement(newNode, graph);
                    }
                    else
                    {
                        List<SGraph> orderedAdjGraphs = adjGraphs.OrderByDescending(s => s.Nodes.Count).ToList();
                        foreach (SGraph graph in adjGraphs)
                        {
                            if (graph != null)
                            {
                                if (newNode.ParentGraph == null)
                                {
                                    AddNewElement(newNode, orderedAdjGraphs[0]);
                                    Printer.Info(graph.Print());
                                }
                                else if (newNode.ParentGraph != null)
                                {
                                    MergeGraphs(orderedAdjGraphs);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void MergeGraphs(List<SGraph> graphs)
        {
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
                        Printer.Info("Graphs adj: " + adjGraphs.Count.ToString());
                        node.ParentGraph.RemoveNode(node);
                        List<SGraph> locationGraphs;
                        if (DataAccess.LocationGraphs.TryGetValue(node.Location, out locationGraphs))
                        {
                            locationGraphs.Remove(node.ParentGraph);
                        }
                        if (adjGraphs.Count > 0)
                        {
                            RemakeGraphs(node);
                        }
                        node.RemoveAllAdjacents();
                        locationMatrix[(int)node.Position.X, (int)node.Position.Y] = null;
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
                Printer.Info((adj.Value == null).ToString());
                if (adj.Value != null)
                {
                    List<SGraph> graphList;
                    if (DataAccess.LocationGraphs.TryGetValue(Game1.currentLocation, out graphList))
                    {
                        if (graphList.Count == 0)
                        {
                            Printer.Info("Old graph: " + adj.Value.ParentGraph.Print());
                            SGNode newNode = SGraphBuilder.BuildGraphRecursive(Game1.currentLocation, null, (int)adj.Value.Position.X, (int)adj.Value.Position.Y);
                            //Console.Info("New graph: " + newNode.ParentGraph.Print());
                        }
                        else
                        {
                            if (!graphList.Any(x => x.ContainsVector2(new Vector2(adj.Value.Position.X, adj.Value.Position.Y))))
                            {
                                Printer.Info("Old graph: " + adj.Value.ParentGraph.Print());
                                SGNode newNode = SGraphBuilder.BuildGraphRecursive(Game1.currentLocation, null, (int)adj.Value.Position.X, (int)adj.Value.Position.Y);
                                // Console.Info("New graph: " + newNode.ParentGraph.Print());
                            }
                        }
                    }
                }
            }
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
