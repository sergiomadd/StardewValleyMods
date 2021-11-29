using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemPipes.Framework.Model;
using ItemPipes.Framework.Objects;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;
using StardewModdingAPI.Events;

namespace ItemPipes.Framework.Util
{
    public static class Animator
    {
        public static Connector current;
        public static bool updated;
        public static void AnimatePath(List<Node> path)
        {
            foreach (Node node in path.ToList())
            {
                if (node != null && node is Connector)
                {
                    Connector conn = (Connector)node;
                    Animate(conn);
                }
            }
            /*Node[] nodes = path.ToArray();
            int cont = 0;
            updated = false;
            if (Globals.Debug) { Printer.Info("ANIMATING..."); }
            while (cont < nodes.Length)
            {
                Node node = nodes[cont];
                if (node != null && node is Connector)
                {
                    Connector conn = (Connector)node;
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

            }*/
                
        }

        private static void Animate(Connector conn)
        {
            conn.PassingItem = true;
            System.Threading.Thread.Sleep(500);
            conn.PassingItem = false;
        }

        private static void Animate2(Connector conn)
        {
            conn.PassingItem = true;
            current = conn;
            //Helper.GetHelper().Events.GameLoop.OneSecondUpdateTicked += OnOneSecondUpdateTicked;
        }
    }
}
