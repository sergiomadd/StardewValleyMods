using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemLogistics.Framework.Model;
using StardewValley;
using StardewValley.Buildings;
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
                    if (Game1.getFarm().getBuildingAt(new Vector2(x, y)) != null)
                    {
                        Printer.Info(Game1.getFarm().getBuildingAt(new Vector2(x, y)).buildingType.ToString());
                        Printer.Info(new Vector2(x, y).ToString());
                        BuildNetworkRecursive(location, null, x, y);
                    }
                    else if (location.getObjectAtTile(x, y) != null)
                    {
                        if (DataAccess.ValidNetworkItems.Contains(location.getObjectAtTile(x, y).name))
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
                if (DataAccess.ValidItems.Contains(location.getObjectAtTile(x, y).name))
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
                            //North
                            if (location.getObjectAtTile(x, y - 1) != null && y - 1 >= 0)
                            {
                                if (DataAccess.ValidNetworkItems.Contains(location.getObjectAtTile(x, y - 1).Name))
                                {
                                    if (!node.ParentNetwork.Nodes.Contains(matrix[x, y - 1]))
                                    {
                                        Node adj = BuildNetworkRecursive(location, node.ParentNetwork, x, y - 1);
                                        node.AddAdjacent(SideStruct.GetSides().North, adj);
                                    }
                                }
                                else if (DataAccess.ValidExtraNames.Contains(location.getObjectAtTile(x, y - 1).Name))
                                {
                                    Node adj = NodeFactory.CreateElement(new Vector2(x, y - 1), location, location.getObjectAtTile(x, y - 1));
                                    matrix[x, y - 1] = adj;
                                    node.AddAdjacent(SideStruct.GetSides().North, adj);
                                }
                            }
                            else if(Game1.getFarm().getBuildingAt(new Vector2(x, y - 1)) != null && y - 1 >= 0)
                            {
                                Printer.Info("ADDING BUILDING TO " + matrix[x, y].Name);
                                Printer.Info(Game1.getFarm().getBuildingAt(new Vector2(x, y - 1)).buildingType.ToString());
                                Printer.Info(new Vector2(x, y - 1).ToString());
                                Printer.Info((Game1.getFarm().getBuildingAt(new Vector2(x, y - 1)).GetType().Equals(typeof(ShippingBin)).ToString()));
                                if (Game1.getFarm().getBuildingAt(new Vector2(x, y - 1)).GetType().Equals(typeof(ShippingBin)))
                                {
                                    Node adj = NodeFactory.CreateElement(new Vector2(x, y - 1), location, Game1.getFarm().getBuildingAt(new Vector2(x, y - 1)));
                                    matrix[x, y - 1] = adj;
                                    node.AddAdjacent(SideStruct.GetSides().North, adj);
                                    Printer.Info("SHIPPING BIUN ADJ");
                                    Printer.Info(matrix[x, y - 1].Name);
                                }
                            }

                            //South
                            if (location.getObjectAtTile(x, y + 1) != null && y + 1 < location.map.DisplayHeight)
                            {
                                if (DataAccess.ValidNetworkItems.Contains(location.getObjectAtTile(x, y + 1).Name))
                                {
                                    if (!node.ParentNetwork.Nodes.Contains(matrix[x, y + 1]))
                                    {
                                        Node adj = BuildNetworkRecursive(location, node.ParentNetwork, x, y + 1);
                                        node.AddAdjacent(SideStruct.GetSides().South, adj);
                                    }
                                }
                                else if (DataAccess.ValidExtraNames.Contains(location.getObjectAtTile(x, y + 1).Name))
                                {
                                    Node adj = NodeFactory.CreateElement(new Vector2(x, y + 1), location, location.getObjectAtTile(x, y + 1));
                                    matrix[x, y + 1] = adj;
                                    node.AddAdjacent(SideStruct.GetSides().South, adj);
                                }
                            }
                            else if (Game1.getFarm().getBuildingAt(new Vector2(x, y + 1)) != null && y + 1 < location.map.DisplayHeight)
                            {
                                if (Game1.getFarm().getBuildingAt(new Vector2(x, y + 1)).buildingType.Equals(typeof(ShipBin)))
                                {
                                    Node adj = NodeFactory.CreateElement(new Vector2(x, y + 1), location, Game1.getFarm().getBuildingAt(new Vector2(x, y + 1)));
                                    matrix[x, y + 1] = adj;
                                    node.AddAdjacent(SideStruct.GetSides().South, adj);
                                }
                            }
                            //West
                            if (location.getObjectAtTile(x + 1, y) != null && x + 1 < location.map.DisplayWidth)
                            {
                                if (DataAccess.ValidNetworkItems.Contains(location.getObjectAtTile(x + 1, y).Name))
                                {
                                    if (!node.ParentNetwork.Nodes.Contains(matrix[x + 1, y]))
                                    {
                                        Node adj = BuildNetworkRecursive(location, node.ParentNetwork, x + 1, y);
                                        node.AddAdjacent(SideStruct.GetSides().West, adj);
                                    }
                                }
                                else if (DataAccess.ValidExtraNames.Contains(location.getObjectAtTile(x + 1, y).Name))
                                {
                                    Node adj = NodeFactory.CreateElement(new Vector2(x + 1, y), location, location.getObjectAtTile(x + 1, y));
                                    matrix[x + 1, y] = adj;
                                    node.AddAdjacent(SideStruct.GetSides().West, adj);
                                }
                            }
                            else if (Game1.getFarm().getBuildingAt(new Vector2(x + 1, y)) != null && x + 1 < location.map.DisplayWidth)
                            {
                                if (Game1.getFarm().getBuildingAt(new Vector2(x + 1, y)).buildingType.Equals(typeof(ShipBin)))
                                {
                                    Node adj = NodeFactory.CreateElement(new Vector2(x + 1, y), location, Game1.getFarm().getBuildingAt(new Vector2(x + 1, y)));
                                    matrix[x + 1, y] = adj;
                                    node.AddAdjacent(SideStruct.GetSides().West, adj);
                                }
                            }
                            //East
                            if (location.getObjectAtTile(x - 1, y) != null && x - 1 >= 0)
                            {
                                if (DataAccess.ValidNetworkItems.Contains(location.getObjectAtTile(x - 1, y).Name))
                                {
                                    if (!node.ParentNetwork.Nodes.Contains(matrix[x - 1, y]))
                                    {
                                        Node adj = BuildNetworkRecursive(location, node.ParentNetwork, x - 1, y);
                                        node.AddAdjacent(SideStruct.GetSides().East, adj);
                                    }
                                }
                                else if (DataAccess.ValidExtraNames.Contains(location.getObjectAtTile(x - 1, y).Name))
                                {
                                    Node adj = NodeFactory.CreateElement(new Vector2(x - 1, y), location, location.getObjectAtTile(x - 1, y));
                                    matrix[x-1, y] = adj;
                                    node.AddAdjacent(SideStruct.GetSides().East, adj);
                                }
                            }
                            else if (Game1.getFarm().getBuildingAt(new Vector2(x - 1, y)) != null && x - 1 >= 0)
                            {
                                if (Game1.getFarm().getBuildingAt(new Vector2(x - 1, y)).buildingType.Equals(typeof(ShipBin)))
                                {
                                    Node adj = NodeFactory.CreateElement(new Vector2(x - 1, y), location, Game1.getFarm().getBuildingAt(new Vector2(x - 1, y)));
                                    matrix[x - 1, y] = adj;
                                    node.AddAdjacent(SideStruct.GetSides().East, adj);
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
