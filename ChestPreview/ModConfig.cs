using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;

namespace ChestPreview
{
    public class ModConfig
    {
        public int Range { get; set; }
        public string Size { get; set; }

        public ModConfig()
        {
            ResetToDefault();
        }

        public void RegisterModConfigMenu(IModHelper helper, IManifest manifest)
        {
            var configMenu = helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
                return;

            configMenu.Register(
                mod: manifest,
                reset: () => ResetToDefault(),
                save: () => helper.WriteConfig(this)
            );

            configMenu.AddNumberOption(
                mod: manifest,
                name: () => "Show preview range",
                tooltip: () => "0 or -1 for infite range. The amount of tiles away from the player where a preview should be shown.",
                getValue: () => Range,
                setValue: value => Range = value
            );
            configMenu.AddTextOption(
                mod: manifest,
                name: () => "Preview menu UI size",
                getValue: () => Size,
                setValue: value => Size = value,
                allowedValues: new string[] { "Small", "Medium", "Big" }
            );
        }

        public void ResetToDefault()
        {
            Range = -1;
            Size = "Medium";
        }


    }
}
