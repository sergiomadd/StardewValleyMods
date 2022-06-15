using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemPipes.Framework.Model;
using ItemPipes.Framework.Nodes.ObjectNodes;
using ItemPipes.Framework.Util;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;


namespace ItemPipes.Framework.Nodes.ObjectNodes
{
    public class PIPONode : Node
    {
        public List<Network> AdjNetworks { get; set; }
        public PIPONode() { }
        public PIPONode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            State = "off";
            AdjNetworks = new List<Network>();
        }

        public bool ChangeState()
        {
            if(State.Equals("on"))
            {
                State = "off";
                Passable = false;
                ParentNetwork.Deinvisibilize(this);
                return false;
            }
            else
            {
                State = "on";
                Passable = true;
                ParentNetwork.Invisibilize(this);
                return true;
            }
        }
    }
}
