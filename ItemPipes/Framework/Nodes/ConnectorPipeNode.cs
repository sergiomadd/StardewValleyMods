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
    public abstract class ConnectorPipeNode : PipeNode
    {
        public ConnectorPipeNode() : base()
        {

        }
        public ConnectorPipeNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {

        }

        public override bool AddAdjacent(Side side, Node node)
        {
            bool added = false;
            if (Adjacents[side] == null)
            {
                if (!(node is ConnectorPipeNode) || (node is ConnectorPipeNode && node.GetType().Equals(this.GetType())))
                {
                    added = true;
                    Adjacents[side] = node;
                    node.AddAdjacent(Sides.GetInverse(side), this);
                }
            }
            //Check to not connect to non-network nodes (chests)
            else if (Adjacents[side] != null &&
                Adjacents[side].Adjacents[Sides.GetInverse(side)] != null &&
                Adjacents[side].Adjacents[Sides.GetInverse(side)].ParentNetwork != null &&
                Adjacents[side].ParentNetwork != null &&
                Adjacents[side].Adjacents[Sides.GetInverse(side)].ParentNetwork != Adjacents[side].ParentNetwork)
            {
                added = true;
                Adjacents[side].Adjacents[Sides.GetInverse(side)] = this;
            }
            return added;
        }
    }
}
