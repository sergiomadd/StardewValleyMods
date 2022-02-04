using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemPipes.Framework.Model;
using ItemPipes.Framework.Nodes;
using ItemPipes.Framework.Util;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;


namespace ItemPipes.Framework.Nodes
{
    public class InvisibilizerNode : Node
    {
        public List<Network> AdjNetworks { get; set; }
        public InvisibilizerNode() { }
        public InvisibilizerNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            State = "off";
            AdjNetworks = new List<Network>();
        }

        public void ChangeState()
        {
            if(State.Equals("on"))
            {
                State = "off";
                foreach(Network network in AdjNetworks)
                {
                    if (network != null)
                    {
                        network.Deinvisibilize(this);
                    }
                }
            }
            else
            {
                State = "on";
                foreach (Network network in AdjNetworks)
                {
                    if(network != null)
                    {
                        network.Invisibilize(this);
                    }
                }
            }
        }
    }
}
