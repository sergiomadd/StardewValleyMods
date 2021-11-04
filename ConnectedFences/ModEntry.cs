using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using ConnectedFences.Patches;
using HarmonyLib;

namespace ConnectedFences
{
    class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            FencePatcher.Initialize(this.Monitor);

            var harmony = new Harmony(this.ModManifest.UniqueID);
            FencePatcher.Apply(harmony);
        }
    }
}
