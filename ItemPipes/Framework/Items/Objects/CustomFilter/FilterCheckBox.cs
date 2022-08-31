using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewValley.Menus;
using ItemPipes.Framework.Items.CustomFilter;
using ItemPipes.Framework.Util;
using Microsoft.Xna.Framework;

namespace ItemPipes.Framework.Items.Objects.CustomFilter
{
    public class FilterCheckBox : OptionsCheckbox
    {
        public Filter parentFilter { get; set; }

        public FilterCheckBox(string label, int whichOption, Filter filter, int x = -1, int y = -1) : base(label, x, y, whichOption)
        {
            parentFilter = filter;

            bounds = new Rectangle(x, y, 36, 36);

        }

        public override void receiveLeftClick(int x, int y)
        {
            if (!base.greyedOut)
            {
                Game1.playSound("drumkit6");
                selected = this;
                this.isChecked = !this.isChecked;
                parentFilter.UpdateOption(this);
                selected = null;
            }
        }
    }
}
