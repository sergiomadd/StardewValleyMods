﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewModdingAPI.Events;
using SObject = StardewValley.Object;
using StardewValley.Tools;
using System.Xml.Serialization;
using ItemPipes.Framework.Util;
using ItemPipes.Framework.Model;
using System.Threading;

namespace ItemPipes.Framework.Nodes
{
    public class PipeNode : Node
    {
        public Item StoredItem { get; set; }
        public int ItemTimer { get; set; }
        public bool PassingItem { get; set; }
        public bool Connecting { get; set; }
        public bool Broken { get; set; }

        public PipeNode() : base()
        {
            State = "default";
        }
        public PipeNode(Vector2 position, GameLocation location, StardewValley.Object obj) : base(position, location, obj)
        {
            State = "default";

            PassingItem = false;
            Connecting = false;
            Broken = false;
        }

        public bool CanConnectedWith(PipeNode target)
        {
            bool connected = false;
            List<PipeNode> path = GetPath(target);
            if (path.Count > 0 && path.Last().Equals(target))
            {
                connected = true;
            }
            return connected;
        }

        public List<PipeNode> GetPath(PipeNode target)
        {
            if (Globals.UltraDebug) { Printer.Info($"Getting path for {target.Print()}"); }
            List<PipeNode> path = new List<PipeNode>();
            path = GetPathRecursive(target, path);
            return path;
        }

        public List<PipeNode> GetPathRecursive(PipeNode target, List<PipeNode> path)
        {
            if (Globals.UltraDebug) { Printer.Info(Print()); }
            Node adj;
            if (path.Contains(target))
            {
                return path;
            }
            else
            {
                path.Add(this);
                adj = Adjacents[Sides.North];
                if (!path.Contains(target) && adj != null && adj is PipeNode && !path.Contains(adj))
                {
                    PipeNode adjPipe = (PipeNode)adj;
                    path = adjPipe.GetPathRecursive(target, path);
                }
                adj = Adjacents[Sides.South];
                if (!path.Contains(target) && adj != null && adj is PipeNode && !path.Contains(adj))
                {
                    PipeNode adjPipe = (PipeNode)adj;
                    path = adjPipe.GetPathRecursive(target, path);
                }
                adj = Adjacents[Sides.East];
                if (!path.Contains(target) && adj != null && adj is PipeNode && !path.Contains(adj))
                {
                    PipeNode adjPipe = (PipeNode)adj;
                    path = adjPipe.GetPathRecursive(target, path);
                }
                adj = Adjacents[Sides.West];
                if (!path.Contains(target) && adj != null && adj is PipeNode && !path.Contains(adj))
                {
                    PipeNode adjPipe = (PipeNode)adj;
                    path = adjPipe.GetPathRecursive(target, path);
                }
                if (!path.Contains(target))
                {
                    path.Remove(this);
                }
                return path;
            }
        }

        public virtual string GetState()
        {
            if (!Passable)
            {
                return State;
            }
            else
            {
                return State + "_passable";
            }
        }
        public PipeNode MoveItem(Item item, PipeNode target, int index, List<PipeNode> path)
        {
            PipeNode broken = null;
            DisplayItem(item);
            if (!this.Equals(target))
            {
                //Printer.Info($"[T{ Thread.CurrentThread.ManagedThreadId}] Path lenght: "+path.Count);
                //Printer.Info($"[T{Thread.CurrentThread.ManagedThreadId}] Index: " + index);
                if (index < path.Count-1)
                {
                    index++;
                    PipeNode nextNode = path[index];
                    if (Location.getObjectAtTile((int)nextNode.Position.X, (int)nextNode.Position.Y) != null)
                    {
                        //Printer.Info($"[T{Thread.CurrentThread.ManagedThreadId}] Index: " + index);
                        //Printer.Info($"[T{Thread.CurrentThread.ManagedThreadId}] Broken? " + Broken);
                        if (!Broken)
                        {
                            broken = nextNode.MoveItem(item, target, index, path);
                        }
                        else
                        {
                            //Printer.Info($"[T{Thread.CurrentThread.ManagedThreadId}] Broken? when true " + Broken);
                            broken = nextNode;
                            return broken;
                        }
                    }
                    else
                    {
                        broken = nextNode;
                        return broken;
                    }
                }
            }

            return broken;
        }

        public bool DisplayItem(Item item)
        {
            bool canLoad = false;
            if (StoredItem == null)
            {
                StoredItem = item;
                PassingItem = true;
                try
                {
                    System.Threading.Thread.Sleep(ItemTimer);
                }
                catch (ThreadInterruptedException exception)
                {
                }
                StoredItem = null;
                PassingItem = false;
            }
            return canLoad;
        }

        public PipeNode ConnectPipe(PipeNode target, int index, List<PipeNode> path)
        {
            PipeNode broken = null;
            if (!this.Equals(target))
            {
                DisplayConnection();
                if (index < path.Count - 1)
                {
                    index++;
                    PipeNode nextNode = path[index];
                    if (Location.getObjectAtTile((int)nextNode.Position.X, (int)nextNode.Position.Y) != null)
                    {
                        broken = nextNode.ConnectPipe(target, index, path);
                    }
                    else
                    {
                        broken = nextNode;
                        return broken;
                    }
                }
            }
            else
            {
                foreach(Node node in path)
                {
                    if (node is PipeNode)
                    {
                        PipeNode pipe = (PipeNode)node;
                        EndConnection(pipe);
                    }
                }
            }

            return broken;
        }

        public void DisplayConnection()
        {
            Connecting = true;
            System.Threading.Thread.Sleep(60);
        }

        public void EndConnection(PipeNode pipe)
        {
            pipe.Connecting = false;
            System.Threading.Thread.Sleep(10);
        }
        /*
        public void AddInvisibilizer(InvisibilizerNode invis)
        {
            ParentNetwork.Invisibilize(this, invis);
        }

        public void RemoveInvisibilizer(InvisibilizerNode invis)
        {
            ParentNetwork.Deinvisibilize(this, invis);
        }
        */
    }
}
