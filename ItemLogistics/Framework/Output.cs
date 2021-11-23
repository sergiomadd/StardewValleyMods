using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemLogistics.Framework.Model;
using ItemLogistics.Framework.Objects;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;

namespace ItemLogistics.Framework
{
    public class Output : Node
    {
        public Dictionary<Input, List<Node>> ConnectedInputs { get; set; }
        public Container ConnectedContainer { get; set; }
        public List<Item> Filter { get; set; }

        public Output(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            ConnectedInputs = new Dictionary<Input, List<Node>>();
            ConnectedContainer = null;
            Filter = new List<Item>();
        }

        public override bool AddAdjacent(Side side, Node entity)
        {
            Printer.Info(entity.Obj.name);
            bool added = false;
            if (Adjacents[side] == null)
            {
                added = true;
                Adjacents[side] = entity;
                entity.AddAdjacent(Sides.GetInverse(side), this);
                if (ConnectedContainer == null && entity is Container)
                {
                    Printer.Info("Container es null");
                    ConnectedContainer = (Container)entity;
                }
                else
                {
                    Printer.Info("Container no es null.");
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
                if (ConnectedContainer != null && entity is Container)
                {
                    ConnectedContainer = null;
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

        public void ProcessExchanges()
        {

            if (ConnectedContainer != null && !ConnectedContainer.IsEmpty() && ConnectedInputs.Count > 0)
            {
                Printer.Info("Output empty? " + ConnectedContainer.IsEmpty().ToString());
                //mirar de sacar el item en cuanto empieza al exchange
                Printer.Info("NEW THREAD");
                Thread thread = new Thread(new ThreadStart(StartExchage));
                thread.Start();
            }
            else
            {
                //Printer.Info("No items to process or no inputs connected");
            }
        }

        public void StartExchage()
        {
            Printer.Info(ConnectedInputs.Count.ToString());
            Item item = null;
            int index = 0;
            Dictionary<Input, List<Node>> priorityInputs = ConnectedInputs;
            priorityInputs = priorityInputs.
                OrderByDescending(pair => pair.Key.Priority).
                ThenBy(pair => pair.Value.Count).
                ToDictionary(x => x.Key, x => x.Value);
            index = 0;
            while (index < priorityInputs.Count && item == null)
            {
                Input input = priorityInputs.Keys.ToList()[index];
                Printer.Info("INPUT");
                input.Print();
                if (input is PolymorphicPipe)
                {
                    //Printer.Info("Filter");
                    PolymorphicPipe poly = (PolymorphicPipe)input;
                    poly.UpdateFilter();
                }
                else if (input is FilterPipe)
                {
                    //Printer.Info("Filter");
                    FilterPipe filter = input as FilterPipe;
                    filter.UpdateFilter();
                }
                item = ConnectedContainer.CanSendItem(input.ConnectedContainer);
                Printer.Info("Can send: " + (item != null).ToString());
                if (item != null)
                {
                    List<Node> path = GetPath(input);
                    AnimatePath(path);
                    
                    if(!ConnectedContainer.SendItem(input.ConnectedContainer, item))
                    {
                        Printer.Info("CANT ENTER, REVERSE");
                        path.Reverse();
                        AnimatePath(path);
                    }
                    else
                    {
                        if(input is PolymorphicPipe)
                        {
                            PolymorphicPipe poly = (PolymorphicPipe)input;
                            poly.UpdateFilter();
                        }
                        else if (input is FilterPipe)
                        {
                            //Printer.Info("Filter");
                            FilterPipe filter = input as FilterPipe;
                            filter.UpdateFilter();
                        }
                    }
                    Printer.Info("END animation");

                }

                index++;
            }
        }

        public bool IsInputConnected(Input input)
        {
            bool connected = false;
            if (ConnectedInputs.Keys.Contains(input))
            {
                connected = true;
            }
            return connected;
        }

        public bool AddConnectedInput(Input input)
        {
            bool added = false;
            if (!ConnectedInputs.Keys.Contains(input))
            {
                Printer.Info("input container: "+ (input.ConnectedContainer == null).ToString());
                if (input.ConnectedContainer != null)
                {
                    added = true;
                    List<Node> path;
                    path = ConnectedContainer.GetPath(input.ConnectedContainer);
                    ConnectedInputs.Add(input, path);
                }  
            }
            return added;
        }

        public bool RemoveConnectedInput(Input input)
        {
            bool removed = false;
            if (ConnectedInputs.Keys.Contains(input))
            {
                removed = true;
                ConnectedInputs.Remove(input);
                Printer.Info("HAS STILL INPUT: "+ ConnectedInputs.Keys.Contains(input).ToString());
            }
            return removed;
        }
    }
}
