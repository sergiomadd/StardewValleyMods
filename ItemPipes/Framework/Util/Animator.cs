﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemPipes.Framework.Model;
using ItemPipes.Framework.Nodes;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;
using StardewModdingAPI.Events;

namespace ItemPipes.Framework.Util
{
    public static class Animator
    {
        public static ConnectorNode current;
        public static bool updated;
        public static void AnimateItemSending(List<Node> path)
        {
            foreach (Node node in path.ToList())
            {
                if (node != null && node is ConnectorNode)
                {
                    ConnectorNode conn = (ConnectorNode)node;
                    AnimateItemMovement(conn);
                }
            }
            /*
            Node[] nodes = path.ToArray();
            int cont = 0;
            updated = false;
            if (Globals.Debug) { Printer.Info("ANIMATING..."); }
            while (cont < nodes.Length)
            {
                Node node = nodes[cont];
                if (node != null && node is ConnectorNode)
                {
                    ConnectorNode conn = (ConnectorNode)node;
                    if (Globals.Debug) { conn.Print(); Printer.Info(conn.PassingItem.ToString()); }
                    if (!conn.PassingItem)
                    {
                        if (Globals.Debug) { Printer.Info($"[X] PASSING"); }
                        updated = false;
                        conn.PassingItem = true;
                        current = conn;
                    }
                    if (updated)
                    {
                        current.PassingItem = false;
                        cont++;
                    }
                }
                else
                {
                    cont++;
                }

            }
            */
        }

        private static void AnimateItemMovement(ConnectorNode conn)
        {
            conn.PassingItem = true;
            System.Threading.Thread.Sleep(500);
            conn.PassingItem = false;
        }

        public static void AnimateInputConnection(List<Node> path)
        {
            foreach (Node node in path.ToList())
            {
                if (node != null && node is ConnectorNode)
                {
                    ConnectorNode conn = (ConnectorNode)node;
                    AnimateInput(conn);
                }
            }
        }

        private static void AnimateInput(ConnectorNode conn)
        {
            conn.Connecting = true;
            System.Threading.Thread.Sleep(100);
            conn.Connecting = false;
        }


        public static void AnimateChangingPassable(List<Node> path)
        {
            foreach (Node node in path.ToList())
            {
                if (node != null)
                {
                    if(node.Passable)
                    {
                        node.Passable = false;
                    }
                    else
                    {
                        node.Passable = true;
                    }
                    System.Threading.Thread.Sleep(100);
                }
            }
        }

        /*
        private static void Animate2(Connector conn)
        {
            conn.PassingItem = true;
            current = conn;
            Helper.GetHelper().Events.GameLoop.OneSecondUpdateTicked += OnOneSecondUpdateTicked;
        }
        */
    }
}