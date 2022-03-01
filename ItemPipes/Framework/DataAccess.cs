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
using ItemPipes.Framework.Util;
using ItemPipes.Framework.Model;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;

namespace ItemPipes.Framework
{
    public class DataAccess
    {
        private static DataAccess myDataAccess;
        public Dictionary<GameLocation, List<Network>> LocationNetworks { get; set; }
        public Dictionary<GameLocation, List<Node>> LocationNodes { get; set; }
        public List<string> ModItems { get; set; }
        public List<string> NetworkItems { get; set; }
        public List<string> PipeNames { get; set; }
        public List<string> IOPipeNames { get; set; }
        public List<string> ExtraNames { get; set; }
        public List<string> Buildings { get; set; }
        public List<string> Locations { get; set; }

        public List<int> UsedNetworkIDs { get; set; }
        public List<Thread> Threads { get; set; }

        public Dictionary<string, Texture2D> Sprites { get; set; }


        public DataAccess()
        {
            LocationNetworks = new Dictionary<GameLocation, List<Network>>();
            LocationNodes = new Dictionary<GameLocation, List<Node>>();
            ModItems = new List<string>();
            NetworkItems = new List<string>();
            PipeNames = new List<string>();
            IOPipeNames = new List<string>();
            ExtraNames = new List<string>();
            Buildings = new List<string>();
            Locations = new List<string>();
            Threads = new List<Thread>();

            /*ValidLocations = new List<string>
                { 
                "FarmHouse", "Farm", "FarmCave", "Beach", "Mountain", 
                "Forest", "RailRoad", "Greenhouse", "Tunnel", "Cellar", "Cellar2", "Cellar3", "Cellar4"
                };
            */

            UsedNetworkIDs = new List<int>();
            Sprites = new Dictionary<string, Texture2D>();
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

        public void LoadSprites()
        {
            try
            {
                List<string> pipes = new List<string>
                {"IronPipe", "GoldPipe", "IridiumPipe", "ExtractorPipe", "GoldExtractorPipe",
                 "IridiumExtractorPipe", "InserterPipe", "PolymorphicPipe", "FilterPipe"};
                foreach (string name in pipes)
                {
                    if (!name.Contains("Iridium"))
                    {
                        Sprites.Add($"{name}_Item", ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{name}/{name}_Item.png"));
                        Sprites.Add($"{name}_default_Sprite", ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{name}/{name}_default_Sprite.png"));
                        Sprites.Add($"{name}_connecting_Sprite", ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{name}/{name}_connecting_Sprite.png"));
                        Sprites.Add($"{name}_item_Sprite", ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{name}/{name}_item_Sprite.png"));
                    }
                    else
                    {
                        Sprites.Add($"{name}_Item1", ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{name}/1/{name}_Item.png"));
                        Sprites.Add($"{name}_default_Sprite1", ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{name}/1/{name}_default_Sprite.png"));
                        Sprites.Add($"{name}_connecting_Sprite1", ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{name}/1/{name}_connecting_Sprite.png"));
                        Sprites.Add($"{name}_item_Sprite1", ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{name}/1/{name}_item_Sprite.png"));

                        Sprites.Add($"{name}_Item2", ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{name}/2/{name}_Item.png"));
                        Sprites.Add($"{name}_default_Sprite2", ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{name}/2/{name}_default_Sprite.png"));
                        Sprites.Add($"{name}_connecting_Sprite2", ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{name}/2/{name}_connecting_Sprite.png"));
                        Sprites.Add($"{name}_item_Sprite2", ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{name}/2/{name}_item_Sprite.png"));

                        Sprites.Add($"{name}_Item3", ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{name}/3/{name}_Item.png"));
                        Sprites.Add($"{name}_default_Sprite3", ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{name}/3/{name}_default_Sprite.png"));
                        Sprites.Add($"{name}_connecting_Sprite3", ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{name}/3/{name}_connecting_Sprite.png"));
                        Sprites.Add($"{name}_item_Sprite3", ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/{name}/3/{name}_item_Sprite.png"));
                    }
                }

                Sprites.Add("PPM_on", ModEntry.helper.Content.Load<Texture2D>($"assets/Objects/PPM/PPM_on.png"));
                Sprites.Add("PPM_off", ModEntry.helper.Content.Load<Texture2D>($"assets/Objects/PPM/PPM_off.png"));
                Sprites.Add("Wrench", ModEntry.helper.Content.Load<Texture2D>($"assets/Objects/Wrench/Wrench_Item.png"));
            }
            catch (Exception e)
            {
                Printer.Info("Can't load Item Pipes mod sprites!");
                Printer.Info(e.Message);
                Printer.Info(e.StackTrace);
            }
        }
    }
}
