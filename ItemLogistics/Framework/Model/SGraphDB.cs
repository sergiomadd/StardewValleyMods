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
    public class SGraphDB
    {
        private static SGraphDB mySGraphDB;
        public Dictionary<GameLocation, List<SGraph>> LocationGraphs { get; set; }
        public Dictionary<GameLocation, SGNode[,]> LocationMatrix  { get; set; }
        public List<string> ValidItemNames { get; set; }
        public List<string> ValidLocations { get; set; }
        private SGraphDB()
        {
            LocationGraphs = new Dictionary<GameLocation, List<SGraph>>();
            LocationMatrix  = new Dictionary<GameLocation, SGNode[,]>();

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
            ValidItemNames = new List<string>
                {
                "Pipe", "Extractor Pipe", "Inserter Pipe"
                };
        }

        public static SGraphDB GetSGraphDB()
        {
            if(mySGraphDB == null)
            {
                mySGraphDB = new SGraphDB();
            }
            return mySGraphDB;
        }

        public List<SGraph> GetGraphList(GameLocation location)
        {
            List<SGraph> graphList = null;
            foreach (KeyValuePair<GameLocation, List<SGraph>> pair in LocationGraphs)
            {
                if(pair.Key.Equals(location))
                {
                    graphList = pair.Value;
                }
            }
            return graphList;
        }

        public SGNode[,] GetMatrix(GameLocation location)
        {
            SGNode[,] graphMatrix = null;
            foreach (KeyValuePair<GameLocation, SGNode[,]> pair in LocationMatrix)
            {
                if (pair.Key.Equals(location))
                {
                    graphMatrix = pair.Value;
                }
            }
            return graphMatrix;
        }
    }
}
