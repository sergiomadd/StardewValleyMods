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
    public static class NetworkBuilder
    {
        public static void BuildLocationNetworks(GameLocation location)
        {
            DataAccess DataAccess = DataAccess.GetDataAccess();
            for (int x = 0; x < location.map.DisplayWidth; x++)
            {
                for (int y = 0; y < location.map.DisplayHeight; y++)
                {
                    if (location.getObjectAtTile(x, y) != null)
                    {
                        if (DataAccess.ValidItemNames.Contains(location.getObjectAtTile(x, y).name))
                        {
                            BuildNetworkRecursive(location, null, x, y);
                        }
                    }
                }
            }
        }

        public static Node BuildNetworkRecursive(GameLocation location, Network inNetwork, int x, int y)
        {
            DataAccess DataAccess = DataAccess.GetDataAccess();
            Node node = null;
            Node[,] matrix;
            if (location.getObjectAtTile(x, y) != null)
            {
                if (DataAccess.ValidItemNames.Contains(location.getObjectAtTile(x, y).name))
                {
                    if (DataAccess.LocationMatrix.TryGetValue(location, out matrix))
                    {
                        if (matrix[x, y] == null)
                        {
                            matrix[x, y] = NodeFactory.CreateElement(new Vector2(x, y), location, location.getObjectAtTile(x, y));
                        }
                        node = matrix[x, y];
                        if(node.ParentNetwork == null)
                        {
                            if (inNetwork == null)
                            {
                                node.ParentNetwork = NetworkManager.CreateLocationNetwork(location);
                            }
                            else
                            {
                                node.ParentNetwork = inNetwork;
                            }
                            NetworkManager.LoadNodeToNetwork(location, x, y, node.ParentNetwork);
                            //Up
                            if (location.getObjectAtTile(x, y - 1) != null && y - 1 >= 0)
                            {
                                if (DataAccess.ValidItemNames.Contains(location.getObjectAtTile(x, y - 1).Name))
                                {
                                    if (!node.ParentNetwork.Nodes.Contains(matrix[x, y - 1]))
                                    {
                                        Node adj = BuildNetworkRecursive(location, node.ParentNetwork, x, y - 1);
                                        node.AddAdjacent(SideStruct.GetSides().North, adj);
                                    }
                                }
                            }

                            //Down
                            if (location.getObjectAtTile(x, y + 1) != null && y + 1 < location.map.DisplayHeight)
                            {
                                if (DataAccess.ValidItemNames.Contains(location.getObjectAtTile(x, y + 1).Name))
                                {
                                    if (!node.ParentNetwork.Nodes.Contains(matrix[x, y + 1]))
                                    {
                                        Node adj = BuildNetworkRecursive(location, node.ParentNetwork, x, y + 1);
                                        node.AddAdjacent(SideStruct.GetSides().South, adj);
                                    }
                                }
                            }
                            //Right
                            if (location.getObjectAtTile(x + 1, y) != null && x + 1 < location.map.DisplayWidth)
                            {
                                if (DataAccess.ValidItemNames.Contains(location.getObjectAtTile(x + 1, y).Name))
                                {
                                    if (!node.ParentNetwork.Nodes.Contains(matrix[x + 1, y]))
                                    {
                                        Node adj = BuildNetworkRecursive(location, node.ParentNetwork, x + 1, y);
                                        node.AddAdjacent(SideStruct.GetSides().West, adj);
                                    }
                                }
                            }
                            //Left
                            if (location.getObjectAtTile(x - 1, y) != null && x - 1 >= 0)
                            {
                                if (DataAccess.ValidItemNames.Contains(location.getObjectAtTile(x - 1, y).Name))
                                {
                                    if (!node.ParentNetwork.Nodes.Contains(matrix[x - 1, y]))
                                    {
                                        Node adj = BuildNetworkRecursive(location, node.ParentNetwork, x - 1, y);
                                        node.AddAdjacent(SideStruct.GetSides().East, adj);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return node;
        }
    }
}
