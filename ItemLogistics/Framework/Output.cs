using System;
using System.Collections.Generic;
using System.Threading;
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
            Printer.Info(entity.Obj.name);
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
                    else
                    {
                        Printer.Info("More than 1 container adjacent.");
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

        public override bool RemoveAdjacent(Side side, SGNode entity)
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
            foreach (KeyValuePair<Side, SGNode> adj in Adjacents.ToList())
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
            //Printer.Info(ConnectedInputs.Count.ToString());
            if (!ConnectedContainer.IsEmpty() && ConnectedInputs.Count > 0)
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
            bool sent = false;
            int index = 0;
            while (index < ConnectedInputs.Count && sent == false)
            {
                Input input = ConnectedInputs[index];
                Printer.Info("INPUT");
                input.Print();
                if (input is PolymorphicPipe)
                {
                    //Printer.Info("Filter");
                    PolymorphicPipe poly = (PolymorphicPipe)input;
                    poly.UpdateFilter();
                }
                sent = ConnectedContainer.CanSendItem(input.ConnectedContainer);
                Printer.Info("Can send: " + sent.ToString());
                if (sent)
                {
                    Item item = ConnectedContainer.GetItemToSend(input.ConnectedContainer);
                    List<SGNode> path = ConnectedContainer.GetPath(input.ConnectedContainer);
                    AnimatePath(path);
                    Printer.Info("END animation");
                    if(!ConnectedContainer.SendItem(input.ConnectedContainer, item))
                    {
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
                    }
                    
                }

                index++;
            }
        }

        public bool IsInputConnected(Input input)
        {
            bool connected = false;
            if (ConnectedInputs.Contains(input))
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
