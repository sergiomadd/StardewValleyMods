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
    public static class SGraphBuilder
    {
        public static void BuildLocationGraphs(GameLocation location)
        {
            SGraphDB DataAccess = SGraphDB.GetSGraphDB();
            for (int x = 0; x < location.map.DisplayWidth; x++)
            {
                for (int y = 0; y < location.map.DisplayHeight; y++)
                {
                    if (location.getObjectAtTile(x, y) != null)
                    {
                        if (DataAccess.ValidItemNames.Contains(location.getObjectAtTile(x, y).name))
                        {
                            BuildGraphRecursive(location, null, x, y);
                        }
                    }
                }
            }
        }

        public static SGNode BuildGraphRecursive(GameLocation location, SGraph inGraph, int x, int y)
        {
            //Revisar metodo, refactor checks si null y si moditem

            //Check si location es valida
            SGraphDB DataAccess = SGraphDB.GetSGraphDB();
            SGNode node = null;
            SGNode[,] locationMatrix;
            Console.Info("IN: " + x.ToString() + y.ToString());
            if (location.getObjectAtTile(x, y) != null)
            {
                if (DataAccess.ValidItemNames.Contains(location.getObjectAtTile(x, y).name))
                {
                    if (DataAccess.LocationMatrix.TryGetValue(location, out locationMatrix))
                    {
                        if (locationMatrix[x, y] == null)
                        {
                            locationMatrix[x, y] = SGNodeFactory.CreateElement(new Vector2(x, y), location, location.getObjectAtTile(x, y));
                        }
                        node = locationMatrix[x, y];
                        if (inGraph == null)
                        {
                            node.ParentGraph = CreateLocationGraph(location);
                        }
                        else
                        {
                            node.ParentGraph = inGraph;
                        }
                        SGraphManager.LoadNodeToGraph(location, x, y, node.ParentGraph);


                        Console.Info("SOURCE: " + location.getObjectAtTile(x, y).Name + x.ToString() + (y).ToString());
                        //Up
                        if (location.getObjectAtTile(x, y - 1) != null && y - 1 >= 0)
                        {
                            if (DataAccess.ValidItemNames.Contains(location.getObjectAtTile(x, y - 1).Name))
                            {
                                if (!node.ParentGraph.Nodes.Contains(locationMatrix[x, y - 1]))
                                {
                                    SGNode adj = BuildGraphRecursive(location, node.ParentGraph, x, y - 1);
                                    node.AddAdjacent(SideStruct.GetSides().North, adj);
                                }
                            }
                        }

                        //Down
                        if (location.getObjectAtTile(x, y + 1) != null && y + 1 < location.map.DisplayHeight)
                        {
                            if (DataAccess.ValidItemNames.Contains(location.getObjectAtTile(x, y + 1).Name))
                            {
                                if (!node.ParentGraph.Nodes.Contains(locationMatrix[x, y + 1]))
                                {
                                    SGNode adj = BuildGraphRecursive(location, node.ParentGraph, x, y + 1);
                                    node.AddAdjacent(SideStruct.GetSides().South, adj);
                                }
                            }
                        }
                        //Right
                        if (location.getObjectAtTile(x + 1, y) != null && x + 1 < location.map.DisplayWidth)
                        {
                            if (DataAccess.ValidItemNames.Contains(location.getObjectAtTile(x + 1, y).Name))
                            {
                                if (!node.ParentGraph.Nodes.Contains(locationMatrix[x + 1, y]))
                                {
                                    SGNode adj = BuildGraphRecursive(location, node.ParentGraph, x + 1, y);
                                    node.AddAdjacent(SideStruct.GetSides().West, adj);
                                }
                            }
                        }
                        //Left
                        if (location.getObjectAtTile(x - 1, y) != null && x - 1 >= 0)
                        {
                            if (DataAccess.ValidItemNames.Contains(location.getObjectAtTile(x - 1, y).Name))
                            {
                                if (!node.ParentGraph.Nodes.Contains(locationMatrix[x - 1, y]))
                                {
                                    SGNode adj = BuildGraphRecursive(location, node.ParentGraph, x - 1, y);
                                    node.AddAdjacent(SideStruct.GetSides().East, adj);
                                }
                            }
                        }
                    }
                }
            }
            return node;
        }

        private static SGraph CreateLocationGraph(GameLocation location)
        {
            SGraphDB DataAccess = SGraphDB.GetSGraphDB();
            SGraph newSGraph = new SGraph();
            List<SGraph> graphList;
            if (DataAccess.LocationGraphs.TryGetValue(location, out graphList))
            {
                if (!graphList.Contains(newSGraph))
                {
                    graphList.Add(newSGraph);
                }
            }
            return newSGraph;
        }
    }
}
