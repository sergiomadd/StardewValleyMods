using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using MaddUtil;

namespace ChestPreview
{
    public class ModConfig
    {
        public bool Enabled { get; set; }
        public int Range { get; set; }
        public string Size { get; set; }
        public bool Connector { get; set; }
        public bool EnableKey { get; set; }
        public SButton Key { get; set; }
        public bool EnableMouse { get; set; }
        public string Mouse { get; set; }


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
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => Helpers.GetTranslationHelper().Get("config.enabled.name"),
                getValue: () => Enabled,
                setValue: value => Enabled = value
                );
            configMenu.AddNumberOption(
                mod: manifest,
                name: () => Helpers.GetTranslationHelper().Get("config.range.name"),
                tooltip: () => Helpers.GetTranslationHelper().Get("config.range.tooltip"),
                getValue: () => Range,
                setValue: value => Range = value
            );
            configMenu.AddTextOption(
                mod: manifest,
                name: () => Helpers.GetTranslationHelper().Get("config.size.name"),
                getValue: () => Size,
                setValue: value => Size = value,
                allowedValues: new string[] { "Small", "Medium", "Big", "Huge" },
                formatAllowedValue: value => GetTranslationSize(value)
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => Helpers.GetTranslationHelper().Get("config.connector.name"),
                getValue: () => Connector,
                setValue: value => Connector = value
                );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => Helpers.GetTranslationHelper().Get("config.enablekey.name"),
                getValue: () => EnableKey,
                setValue: value => EnableKey = value
                );
            configMenu.AddKeybind(
                mod: manifest,
                name: () => Helpers.GetTranslationHelper().Get("config.key.name"),
                tooltip: () => Helpers.GetTranslationHelper().Get("config.key.tooltip"),
                getValue: () => Key,
                setValue: value => Key = value
                );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => Helpers.GetTranslationHelper().Get("config.enablemouse.name"),
                getValue: () => EnableMouse,
                setValue: value => EnableMouse = value
                );
            configMenu.AddTextOption(
                mod: manifest,
                name: () => Helpers.GetTranslationHelper().Get("config.mouse.name"),
                tooltip: () => Helpers.GetTranslationHelper().Get("config.mouse.tooltip"),
                getValue: () => Mouse,
                setValue: value => Mouse = value,
                allowedValues: new string[] { "MouseLeft", "MouseRight", "MouseMiddle", "MouseX1", "MouseX2" },
                formatAllowedValue: value => GetTranslationMouse(value)
            );
        }

        public void ResetToDefault()
        {
            Enabled = true;
            Range = -1;
            Size = "Medium";
            Connector = true;
            EnableKey = false;
            Key = SButton.J;
            EnableMouse = false;
            Mouse = "MouseLeft";
        }

        public SButton GetMouseButton(string value)
        {
            SButton button = SButton.A;
            if (value.Equals("MouseLeft"))
            {
                button = SButton.MouseLeft;
            }
            else if (value.Equals("MouseRight"))
            {
                button = SButton.MouseRight;
            }
            else if (value.Equals("MouseMiddle"))
            {
                button = SButton.MouseMiddle;
            }
            else if (value.Equals("MouseX1"))
            {
                button = SButton.MouseX1;
            }
            else if (value.Equals("MouseX2"))
            {
                button = SButton.MouseX2;
            }
            return button;
        }

        public string GetTranslationMouse(string value)
        {
            string translated;
            if (value.Equals("MouseLeft"))
            {
                translated = Helpers.GetTranslationHelper().Get("config.mouse.left");
            }
            else if (value.Equals("MouseRight"))
            {
                translated = Helpers.GetTranslationHelper().Get("config.mouse.right");
            }
            else if (value.Equals("MouseMiddle"))
            {
                translated = Helpers.GetTranslationHelper().Get("config.mouse.middle");
            }
            else if (value.Equals("MouseX1"))
            {
                translated = Helpers.GetTranslationHelper().Get("config.mouse.x1");
            }
            else
            {
                translated = Helpers.GetTranslationHelper().Get("config.mouse.x2");
            }
            return translated;
        }

        public string GetTranslationSize(string value) 
        {
            string translated;
            if(value.Equals("Small"))
            {
                translated = Helpers.GetTranslationHelper().Get("config.size.small");
            }
            else if(value.Equals("Medium"))
            {
                translated = Helpers.GetTranslationHelper().Get("config.size.medium");
            }
            else if(value.Equals("Big"))
            {
                translated = Helpers.GetTranslationHelper().Get("config.size.big");
            }
            else if (value.Equals("Huge"))
            {
                translated = Helpers.GetTranslationHelper().Get("config.size.huge");
            }
            else
            {
                translated = Helpers.GetTranslationHelper().Get("config.size.medium");
            }
            return translated;
        }

    }
}
