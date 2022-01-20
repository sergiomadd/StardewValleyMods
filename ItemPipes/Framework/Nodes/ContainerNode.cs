﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;
using ItemPipes.Framework.Model;
using ItemPipes.Framework.Nodes;
using ItemPipes.Framework.Util;
using Netcode;

namespace ItemPipes.Framework
{
    public class ContainerNode : PipeNode
    {
        public string Type { get; set; }
        public OutputNode Output { get; set; }
        public InputNode Input { get; set; }
        public List<string> Filter { get; set; }

        public ContainerNode() { }
        public ContainerNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            Type = "";
            Output = null;
            Input = null;
            Filter = new List<string>();
        }
        public override bool AddAdjacent(Side side, Node entity)
        {
            bool added = false;
            if (Adjacents[side] == null)
            {
                added = true;
                Adjacents[side] = entity;
                entity.AddAdjacent(Sides.GetInverse(side), this);
                if (Output == null && entity is OutputNode)
                {
                    Output = (OutputNode)entity;
                    if (Globals.Debug) { Printer.Info($"[?] OUTPUT ADDED"); }
                }
                else if (Input == null && entity is InputNode)
                {
                    Input = (InputNode)entity;
                    if (Globals.Debug) { Printer.Info($"[?] INPUT ADDED"); }
                }
            }
            return added;
        }

        public override bool RemoveAdjacent(Side side, Node entity)
        {
            bool removed = false;
            if (Adjacents[side] != null)
            {
                removed = true;
                if (Output != null && entity is OutputNode)
                {
                    Output = null;
                    if (Globals.Debug) { Printer.Info($"[?] OUTPUT REMOVED"); }
                }
                else if (Input != null && entity is InputNode)
                {
                    Input = null;
                    if (Globals.Debug) { Printer.Info($"[?] INPUT REMOVED"); }
                }
                Adjacents[side] = null;
                entity.RemoveAdjacent(Sides.GetInverse(side), this);
            }
            return removed;
        }

        public override bool RemoveAllAdjacents()
        {
            bool removed = false;
            foreach (KeyValuePair<Side, Node> adj in Adjacents.ToList())
            {
                if (adj.Value != null)
                {
                    removed = true;
                    RemoveAdjacent(adj.Key, adj.Value);
                    Adjacents[adj.Key] = null;
                }
            }
            return removed;
        }

        public virtual bool IsEmpty()
        {
            return false;
        }

        public virtual List<string> UpdateFilter(NetObjectList<Item> filteredItems)
        {
            return null;
        }
    }
}
