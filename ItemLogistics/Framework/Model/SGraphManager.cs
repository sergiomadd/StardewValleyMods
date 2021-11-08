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

namespace ItemLogistics.Framework.Model
{
    public static class SGraphManager
    {
        public static void LoadNodeToGraph(GameLocation location, int x, int y, SGraph graph)
        {
            SGraphDB DataAccess = SGraphDB.GetSGraphDB();
            /*Refactor:
             * Move check for pipe type to logistic group
            */
            SGNode[,] locationMatrix;
            if (DataAccess.LocationMatrix.TryGetValue(location, out locationMatrix))
            {
                graph.AddNode(locationMatrix[x, y]);
                if (locationMatrix[x, y] is OutPipe)
                {
                    Console.Info("OUTPIPE");
                    graph.AddOutput((OutPipe)locationMatrix[x, y]);
                }
                if (locationMatrix[x, y] is InPipe)
                {
                    Console.Info("INPIPE");
                    graph.AddInput((InPipe)locationMatrix[x, y]);
                }
                if (locationMatrix[x, y] is Pipe)
                {
                    Console.Info("PIPE");
                    graph.AddConector((Pipe)locationMatrix[x, y]);
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
            Console.Info("ADDING: " + obj.Key.ToString() + obj.Value.Name);
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
                                    Console.Info(graph.Print());
                                }
                                else if (newNode.ParentGraph != null)
                                {
                                    MergeGraphs(orderedAdjGraphs);
                                }
                            }
                            else
                            {
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
                Console.Info("G size:" + graphs[i].Nodes.Count.ToString());
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
            Console.Info("REMOVE: " + obj.Key.ToString() + obj.Value.Name);
            if (DataAccess.ValidItemNames.Contains(obj.Value.Name))
            {
                SGNode[,] locationMatrix;
                if (DataAccess.LocationMatrix.TryGetValue(Game1.currentLocation, out locationMatrix))
                {
                    SGNode node = locationMatrix[(int)obj.Key.X, (int)obj.Key.Y];
                    if (node.ParentGraph != null)
                    {
                        List<SGraph> adjGraphs = node.Scan();
                        Console.Info("Graphs adj: " + adjGraphs.Count.ToString());
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
            Console.Info("Remaking");
            foreach (KeyValuePair<Side, SGNode> adj in node.Adjacents)
            {
                Console.Info((adj.Value == null).ToString());
                if (adj.Value != null)
                {
                    List<SGraph> graphList;
                    if (DataAccess.LocationGraphs.TryGetValue(Game1.currentLocation, out graphList))
                    {
                        if(graphList.Count == 0)
                        {
                            Console.Info("Old graph: " + adj.Value.ParentGraph.Print());
                            SGNode newNode = SGraphBuilder.BuildGraphRecursive(Game1.currentLocation, null, (int)adj.Value.Position.X, (int)adj.Value.Position.Y);
                            //Console.Info("New graph: " + newNode.ParentGraph.Print());
                        }
                        else
                        {
                            if (!graphList.Any(x => x.Contains(new Vector2(adj.Value.Position.X, adj.Value.Position.Y))))
                            {
                                Console.Info("Old graph: " + adj.Value.ParentGraph.Print());
                                SGNode newNode = SGraphBuilder.BuildGraphRecursive(Game1.currentLocation, null, (int)adj.Value.Position.X, (int)adj.Value.Position.Y);
                                // Console.Info("New graph: " + newNode.ParentGraph.Print());
                            }
                        }
                    }
                }
            }
        }

        public static void PrintLocationGraphs(GameLocation location)
        {
            SGraphDB DataAccess = SGraphDB.GetSGraphDB();
            List<SGraph> graphList;
            if (DataAccess.LocationGraphs.TryGetValue(location, out graphList))
            {
                foreach (SGraph graph in graphList)
                {
                    Console.Info(graph.Print());
                }
            }
        }
    }
}
