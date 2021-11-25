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

namespace ItemLogistics.Framework
{
    public class DataAccess
    {
        private static DataAccess myDataAccess;
        public Dictionary<GameLocation, List<Network>> LocationNetworks { get; set; }
        public Dictionary<GameLocation, Node[,]> LocationMatrix  { get; set; }
        public List<string> ValidNetworkItems { get; set; }
        public List<string> ValidLocations { get; set; }
        public List<string> ValidIOPipeNames { get; set; }
        public List<string> ValidPipeNames { get; set; }
        public List<string> ValidExtraNames { get; set; }
        public List<string> ValidItems { get; set; }
        public List<string> ValidBuildings { get; set; }


        public List<int> UsedNetworkIDs { get; set; }




        private DataAccess()
        {
            LocationNetworks = new Dictionary<GameLocation, List<Network>>();
            LocationMatrix  = new Dictionary<GameLocation, Node[,]>();
            ValidNetworkItems = new List<string>();
            ValidLocations = new List<string>();
            ValidIOPipeNames = new List<string>();
            ValidPipeNames = new List<string>();
            ValidExtraNames = new List<string>();
            ValidItems = new List<string>();
            ValidBuildings = new List<string>();
            /*ValidLocations = new List<string>
                { 
                "FarmHouse", "Farm", "FarmCave", "Beach", "Mountain", 
                "Forest", "RailRoad", "Greenhouse", "Tunnel", "Cellar", "Cellar2", "Cellar3", "Cellar4"
                };
            */

            UsedNetworkIDs = new List<int>();
        }

        public static DataAccess GetDataAccess()
        {
            if(myDataAccess == null)
            {
                myDataAccess = new DataAccess();
            }
            return myDataAccess;
        }

        public int GetNewNetworkID()
        {
            if(UsedNetworkIDs.Count == 0)
            {
                UsedNetworkIDs.Add(1);
                return 1;
            }
            else
            {
                int newID = UsedNetworkIDs[UsedNetworkIDs.Count - 1] + 1;
                UsedNetworkIDs.Add(newID);
                return newID;
            }
        }

        public List<Network> GetNetworkList(GameLocation location)
        {
            List<Network> graphList = null;
            foreach (KeyValuePair<GameLocation, List<Network>> pair in LocationNetworks)
            {
                if(pair.Key.Equals(location))
                {
                    graphList = pair.Value;
                }
            }
            return graphList;
        }

        public Node[,] GetMatrix(GameLocation location)
        {
            Node[,] matrix = null;
            foreach (KeyValuePair<GameLocation, Node[,]> pair in LocationMatrix)
            {
                if (pair.Key.Equals(location))
                {
                    matrix = pair.Value;
                }
            }
            return matrix;
        }
    }
}
