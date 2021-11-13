﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemLogistics.Framework.Model;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;

namespace ItemLogistics.Framework
{
    public class Output : SGNode
    {
        public List<Input> ConnectedInputs { get; set; }
        public Container ConnectedContainer { get; set; }
        public List<Item> Filter { get; set; }

        public Output(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            ConnectedInputs = new List<Input>();
            ConnectedContainer = null;
            Filter = new List<Item>();
        }

        public override bool AddAdjacent(Side side, SGNode entity)
        {
            bool added = false;
            if (Adjacents[side] == null)
            {
                added = true;
                Adjacents[side] = entity;
                entity.AddAdjacent(Sides.GetInverse(side), this);
                try
                {
                    if (ConnectedContainer == null && entity is Container)
                    {
                        ConnectedContainer = (Container)entity;
                    }
                }
                catch(Exception e)
                {
                    Printer.Info("More than 1 container adjacent.");
                    Printer.Info(e.StackTrace);
                }
            }
            return added;
        }

        public void ProcessExchanges()
        {
            Printer.Info(ConnectedInputs.Count.ToString());
            if(!ConnectedContainer.IsEmpty() && ConnectedInputs.Count > 0)
            {
                foreach (Input input in ConnectedInputs)
                {
                    Printer.Info("INPUT");
                    StartExchage(input);
                }
            }
            else
            {
                Printer.Info("No items to process or no inputs connected");
            }
        }

        public void StartExchage(Input input)
        {
            //ConnectedContainer.SendItems(input.ConnectedContainer);
            ConnectedContainer.SendItem(input.ConnectedContainer);
        }

        public bool IsInputConnected(Input input)
        {
            bool connected = false;
            if (ConnectedInputs.Contains(input) && ParentGraph.Nodes.Contains(input))
            {
                connected = true;
            }
            return connected;
        }

        public bool AddConnectedInput(Input input)
        {
            bool added = false;
            if (!ConnectedInputs.Contains(input))
            {
                added = true;
                ConnectedInputs.Add(input);
            }
            return added;
        }

        public bool RemoveConnectedInput(Input input)
        {
            bool removed = false;
            if (ConnectedInputs.Contains(input))
            {
                removed = true;
                ConnectedInputs.Remove(input);
            }
            return removed;
        }
    }
}
