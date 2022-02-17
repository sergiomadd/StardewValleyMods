﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemPipes.Framework.Model;
using Microsoft.Xna.Framework;
using StardewValley;
using ItemPipes.Framework.Nodes;
using ItemPipes.Framework.Util;


namespace ItemPipes.Framework
{
    public abstract class ConnectorNode : PipeNode
    {
        public ConnectorNode() : base()
        {

        }
        public ConnectorNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {

        }

        public override string GetState()
        {
            if(!Passable)
            {
                if (PassingItem)
                {
                    return "item";
                }
                else if (Connecting)
                {
                    return "connecting";
                }
                else
                {
                    return "default";
                }
            }
            else
            {
                if (PassingItem)
                {
                    return "item_passable";
                }
                else if (Connecting)
                {
                    return "connecting_passable";
                }
                else
                {
                    return "default_passable";
                }
            }
        }

        public override bool AddAdjacent(Side side, Node node)
        {
            bool added = false;
            Printer.Info("THIS "+ this.Print()+" ADJACENT: "+node.GetType() + " " + node.Print());
            if (Adjacents[side] == null)
            {
                if(!(node is ConnectorNode) || (node is ConnectorNode && node.GetType().Equals(this.GetType())))
                {
                    added = true;
                    Adjacents[side] = node;
                    node.AddAdjacent(Sides.GetInverse(side), this);
                }
            }
            return added;
        }
    }
}
