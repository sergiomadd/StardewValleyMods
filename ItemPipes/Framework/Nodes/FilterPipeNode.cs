using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;
using ItemPipes.Framework.Util;
using ItemPipes.Framework.Items;
using StardewValley.Menus;
using Netcode;


namespace ItemPipes.Framework.Nodes
{
    public class FilterPipeNode : InputNode
    {
        public FilterPipeNode() : base()
        {

        }
        public FilterPipeNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            ConnectedContainer = null;
            Priority = 3;
            if(obj != null && obj is FilterPipeItem)
            {
                Filter = (obj as FilterPipeItem).Filter.items;
            }
        }

        public override void UpdateFilter()
        {
            if (ConnectedContainer != null)
            {
                ConnectedContainer.UpdateFilter(Filter);
            }
        }
    }
}
