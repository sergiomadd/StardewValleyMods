using ItemPipes.Framework.Data;
using ItemPipes.Framework.Model;
using ItemPipes.Framework.Util;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ItemPipes.Framework
{
    public class DataAccess
    {
        private static DataAccess myDataAccess;
        public Dictionary<GameLocation, List<Network>> LocationNetworks { get; set; }
        public Dictionary<GameLocation, List<Node>> LocationNodes { get; set; }
        public List<int> ModItems { get; set; }
        public List<int> NetworkItems { get; set; }
        public List<string> Buildings { get; set; }

        public Dictionary<GameLocation, List<long>>  UsedNetworkIDs { get; set; }
        public List<Thread> Threads { get; set; }

        public Dictionary<string, Texture2D> Sprites { get; set; }
        public Dictionary<string, string> Recipes { get; set; }
        public Dictionary<string, string> FakeRecipes { get; set; }
        public List<string> ItemIDNames { get; set; }
        public Dictionary<string, string> ItemNames { get; set; }
        public Dictionary<string, int> ItemIDs { get; set; }
        public Dictionary<string, string> ItemDescriptions { get; set; }
        public List<Item> LostItems { get; set; }
        public Dictionary<string, string> Letters { get; set; }
        public Dictionary<string, string> Warnings { get; set; }


        public DataAccess()
        {
            LocationNetworks = new Dictionary<GameLocation, List<Network>>();
            LocationNodes = new Dictionary<GameLocation, List<Node>>();
            ModItems = new List<int>();
            NetworkItems = new List<int>();
            Buildings = new List<string>();
            Threads = new List<Thread>();
            UsedNetworkIDs = new Dictionary<GameLocation, List<long>>();
            Sprites = new Dictionary<string, Texture2D>();
            Recipes = new Dictionary<string, string>();
            FakeRecipes = new Dictionary<string, string>();
            ItemIDNames = new List<string>();
            ItemNames = new Dictionary<string, string>();
            ItemIDs = new Dictionary<string, int>();
            ItemDescriptions = new Dictionary<string, string>();
            LostItems = new List<Item>();

            Letters = new Dictionary<string, string>();
            Warnings = new Dictionary<string, string>();
        }

        public static DataAccess GetDataAccess()
        {
            if(myDataAccess == null)
            {
                myDataAccess = new DataAccess();
            }
            return myDataAccess;
        }

        public bool RemoveThread(Thread thread)
        {
            try
            {
                if (DataAccess.GetDataAccess().Threads.Contains(thread))
                {
                    DataAccess.GetDataAccess().Threads.Remove(thread);
                    thread.Abort();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                DataAccess.GetDataAccess().Threads.Clear();
                return true;
            }
        }

        public long GetNewNetworkID(GameLocation location)
        {
            List<long> IDs = UsedNetworkIDs[location];
            if(IDs.Count == 0)
            {
                IDs.Add(1);
                return 1;
            }
            else
            {
                long newID = IDs[IDs.Count - 1] + 1;
                IDs.Add(newID);
                return newID;
            }
        }

        public List<Network> GetNetworkList(GameLocation location)
        {
            List<Network> networkList = null;
            foreach (KeyValuePair<GameLocation, List<Network>> pair in LocationNetworks)
            {
                if(pair.Key.Equals(location))
                {
                    networkList = pair.Value;
                }
            }
            return networkList;
        }

        public void LoadConfig()
        {
            ModConfig config = null;
            try
            {
                config = ModEntry.helper.ReadConfig<ModConfig>();
                if (config == null)
                {
                    Printer.Error($"The config file seems to be empty or invalid. Data class returned null.");
                }
            }
            catch (Exception ex)
            {
                Printer.Error($"The config file seems to be missing or invalid.\n{ex}");
            }


            //Normal debug = only errors
            if (config.DebugMode)
            {
                Globals.Debug = true;
                Printer.Debug("Debug mode ENABLED");
            }
            else
            {
                Globals.Debug = false;
                Printer.Debug("Debug mode DISABLED");
            }
            //Ultra debug = all the prints like step by step
            if (config.UltraDebugMode)
            {
                Globals.UltraDebug = true;
                Printer.Debug("UltraDebug mode ENABLED");
            }
            else
            {
                Globals.UltraDebug = false;
                Printer.Debug("UltraDebug mode DISABLED");
            }
            if (config.ItemSending)
            {
                Globals.ItemSending = true;
                Printer.Debug("Item sending ENABLED");
            }
            else
            {
                Globals.ItemSending = false;
                Printer.Debug("Item sending DISABLED");
            }
            if (config.IOPipeStatePopup)
            {
                Globals.IOPipeStatePopup = true;
                Printer.Debug("IOPipe state bubble popup ENABLED");
            }
            else
            {
                Globals.IOPipeStatePopup = false;
                Printer.Debug("IOPipe state bubble popup DISABLED");
            }
        }

        public void LoadAssets()
        {
            LoadIDs();
            LoadLetters();
            LoadItems();
            LoadSprites();
            LoadRecipes();
            LoadWarnings();
        }

        public void LoadIDs()
        {
            string dataPath = "assets/Data/ItemIDsData.json";
            ItemIDs IDs = null;
            try
            {
                IDs = ModEntry.helper.Data.ReadJsonFile<ItemIDs>(dataPath);
                if (IDs == null)
                {
                    Printer.Error($"The {dataPath} file seems to be empty or invalid. Data class returned null.");
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                Printer.Error($"The {dataPath} file seems to be missing or invalid.\n{ex}");
            }
            ModItems = IDs.ModItems;
            NetworkItems = IDs.NetworkItems;
            Buildings = IDs.Buildings;
        }
        public void LoadLetters()
        {
            string dataPath = "assets/Data/LetterData.json";
            LettersData lettersData = null;
            try
            {
                lettersData = ModEntry.helper.Data.ReadJsonFile<LettersData>(dataPath);
                if (lettersData == null)
                {
                    Printer.Error($"The {dataPath} file seems to be empty or invalid. Data class returned null.");
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                Printer.Error($"The {dataPath} file seems to be missing or invalid.\n{ex}");
            }
            var currLang = LocalizedContentManager.CurrentLanguageCode;
            for (int i = 0; i < lettersData.lettersDataList.Count; i++)
            {
                if(lettersData.lettersDataList[i].letterDataDict["LangKey"].Equals(currLang.ToString()))
                {
                    Letters = lettersData.lettersDataList[i].letterDataDict;
                }
            }
            if(Letters.Count == 0)
            {
                Letters = lettersData.lettersDataList[0].letterDataDict;
                Printer.Warn($"Language {currLang.ToString()} not supported");
            }
        }
        
        public void LoadWarnings()
        {
            string dataPath = "assets/Data/WarningsData.json";
            WarningsData warningsData = null;
            try
            {
                warningsData = ModEntry.helper.Data.ReadJsonFile<WarningsData>(dataPath);
                if (warningsData == null)
                {
                    Printer.Error($"The {dataPath} file seems to be empty or invalid.");
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                Printer.Error($"The {dataPath} file seems to be missing or invalid.\n{ex}");
            }
            var currLang = LocalizedContentManager.CurrentLanguageCode;
            for (int i = 0; i < warningsData.warningsDataList.Count; i++)
            {
                if (warningsData.warningsDataList[i].warningsDataDict["LangKey"].Equals(currLang.ToString()))
                {
                    Warnings = warningsData.warningsDataList[i].warningsDataDict;
                }
            }
            if (Letters.Count == 0)
            {
                Warnings = warningsData.warningsDataList[0].warningsDataDict;
                Printer.Warn($"Language {currLang.ToString()} not supported");
            }
        }
        
        public void LoadRecipes()
        {
            string dataPath = "assets/Data/RecipeData.json";
            RecipeData recipes = null;
            try
            {
                recipes = ModEntry.helper.Data.ReadJsonFile<RecipeData>(dataPath);
                if (recipes == null)
                {
                    Printer.Error($"The {dataPath} file seems to be empty or invalid. Data class returned null.");
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                Printer.Error($"The {dataPath} file seems to be missing or invalid.\n{ex}");
            }
            Recipes = recipes.recipesData;
            FakeRecipes = recipes.fakeRecipesData;
        }

        public void LoadItems()
        {
            string dataPath = "assets/Data/ItemData.json";
            ItemsData items = null;
            try
            {
                items = ModEntry.helper.Data.ReadJsonFile<ItemsData>(dataPath);
                if (items == null)
                {
                    Printer.Error($"The {dataPath} file seems to be missing or invalid. Data class returned null.");
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                Printer.Error($"The {dataPath} file seems to be missing or invalid.\n{ex}");
            }
            ItemIDNames.Clear();
            ItemNames.Clear();
            ItemIDs.Clear();
            ItemDescriptions.Clear();
            var currLang = LocalizedContentManager.CurrentLanguageCode;
            for (int i=0;i< items.itemsDataList.Count; i++)
            {
                ItemIDNames.Add(items.itemsDataList[i].IDName);
                if(currLang != LocalizedContentManager.LanguageCode.en)
                {
                    if (items.itemsDataList[i].NameLocalization.ContainsKey(currLang.ToString())
                        && items.itemsDataList[i].NameLocalization[currLang.ToString()].Length > 0)
                    {
                        ItemNames.Add(items.itemsDataList[i].IDName, items.itemsDataList[i].NameLocalization[currLang.ToString()]);
                    }
                    else
                    {
                        ItemNames.Add(items.itemsDataList[i].IDName, items.itemsDataList[i].Name);
                    }
                    if (items.itemsDataList[i].DescriptionLocalization.ContainsKey(currLang.ToString())
                        && items.itemsDataList[i].DescriptionLocalization[currLang.ToString()].Length > 0)
                    {
                        ItemDescriptions.Add(items.itemsDataList[i].IDName, items.itemsDataList[i].DescriptionLocalization[currLang.ToString()]);
                    }
                    else
                    {
                        ItemDescriptions.Add(items.itemsDataList[i].IDName, items.itemsDataList[i].Description);
                    }
                }
                else
                {
                    ItemNames.Add(items.itemsDataList[i].IDName, items.itemsDataList[i].Name);
                    ItemDescriptions.Add(items.itemsDataList[i].IDName, items.itemsDataList[i].Description);
                }
                ItemIDs.Add(items.itemsDataList[i].IDName, items.itemsDataList[i].ID);
            }
        }


        public void LoadSprites()
        {
            Sprites.Clear();
            IModContentHelper helper = ModHelper.GetHelper();
            try
            {
                List<string> pipes = new List<string>
                {"IronPipe", "GoldPipe", "IridiumPipe", "ExtractorPipe", "GoldExtractorPipe",
                 "IridiumExtractorPipe", "InserterPipe", "PolymorphicPipe", "FilterPipe"};
                foreach (string name in pipes)
                {
                    if (!name.Contains("Iridium"))
                    {
                        Sprites.Add($"{name}_Item", helper.Load<Texture2D>($"assets/Pipes/{name}/{name}_Item.png"));
                        Sprites.Add($"{name}_default_Sprite", helper.Load<Texture2D>($"assets/Pipes/{name}/{name}_default_Sprite.png"));
                        Sprites.Add($"{name}_connecting_Sprite", helper.Load<Texture2D>($"assets/Pipes/{name}/{name}_connecting_Sprite.png"));
                        Sprites.Add($"{name}_item_Sprite", helper.Load<Texture2D>($"assets/Pipes/{name}/{name}_item_Sprite.png"));
                    }
                    else
                    {
                        Sprites.Add($"{name}_Item", helper.Load<Texture2D>($"assets/Pipes/{name}/1/{name}_Item.png"));

                        Sprites.Add($"{name}_Item1", helper.Load<Texture2D>($"assets/Pipes/{name}/1/{name}_Item.png"));
                        Sprites.Add($"{name}_default_Sprite1", helper.Load<Texture2D>($"assets/Pipes/{name}/1/{name}_default_Sprite.png"));
                        Sprites.Add($"{name}_connecting_Sprite1", helper.Load<Texture2D>($"assets/Pipes/{name}/1/{name}_connecting_Sprite.png"));
                        Sprites.Add($"{name}_item_Sprite1", helper.Load<Texture2D>($"assets/Pipes/{name}/1/{name}_item_Sprite.png"));

                        Sprites.Add($"{name}_Item2", helper.Load<Texture2D>($"assets/Pipes/{name}/2/{name}_Item.png"));
                        Sprites.Add($"{name}_default_Sprite2", helper.Load<Texture2D>($"assets/Pipes/{name}/2/{name}_default_Sprite.png"));
                        Sprites.Add($"{name}_connecting_Sprite2", helper.Load<Texture2D>($"assets/Pipes/{name}/2/{name}_connecting_Sprite.png"));
                        Sprites.Add($"{name}_item_Sprite2", helper.Load<Texture2D>($"assets/Pipes/{name}/2/{name}_item_Sprite.png"));

                        Sprites.Add($"{name}_Item3", helper.Load<Texture2D>($"assets/Pipes/{name}/3/{name}_Item.png"));
                        Sprites.Add($"{name}_default_Sprite3", helper.Load<Texture2D>($"assets/Pipes/{name}/3/{name}_default_Sprite.png"));
                        Sprites.Add($"{name}_connecting_Sprite3", helper.Load<Texture2D>($"assets/Pipes/{name}/3/{name}_connecting_Sprite.png"));
                        Sprites.Add($"{name}_item_Sprite3", helper.Load<Texture2D>($"assets/Pipes/{name}/3/{name}_item_Sprite.png"));
                    }
                }
                Sprites.Add("signal_on", helper.Load<Texture2D>($"assets/Pipes/on.png"));
                Sprites.Add("signal_off", helper.Load<Texture2D>($"assets/Pipes/off.png"));
                Sprites.Add("signal_unconnected", helper.Load<Texture2D>($"assets/Pipes/unconnected.png"));
                Sprites.Add("signal_nochest", helper.Load<Texture2D>($"assets/Pipes/nochest.png"));

                Sprites.Add("PIPO_Item", helper.Load<Texture2D>($"assets/Objects/PIPO/PIPO_off.png"));
                Sprites.Add("PIPO_on", helper.Load<Texture2D>($"assets/Objects/PIPO/PIPO_on.png"));
                Sprites.Add("PIPO_off", helper.Load<Texture2D>($"assets/Objects/PIPO/PIPO_off.png"));
                Sprites.Add("Wrench_Item", helper.Load<Texture2D>($"assets/Objects/Wrench/Wrench_Item.png"));

                Sprites.Add("nochest_state", helper.Load<Texture2D>($"assets/Misc/nochest_state.png"));
                Sprites.Add("nochest1_state", helper.Load<Texture2D>($"assets/Misc/nochest1_state.png"));
                //Sprites.Add("unconnected_state", ModEntry.helper.Content.Load<Texture2D>($"assets/Misc/unconnected_state.png"));
                Sprites.Add("unconnected1_state", helper.Load<Texture2D>($"assets/Misc/unconnected1_state.png"));
            }
            catch (Exception e)
            {
                Printer.Error("Can't load Item Pipes mod sprites!");
                Printer.Error(e.Message);
                Printer.Error(e.StackTrace);
            }
        }

        public void Reset()
        {
            LocationNodes.Clear();
            LocationNetworks.Clear();
            UsedNetworkIDs.Clear();
            Threads.Clear();
        }
    }
}
