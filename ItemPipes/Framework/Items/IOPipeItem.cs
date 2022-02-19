using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Tools;
using ItemPipes.Framework.Model;
using ItemPipes.Framework.Util;
using ItemPipes.Framework.Factories;
using ItemPipes.Framework.Items.Objects;
using System.Xml.Serialization;

namespace ItemPipes.Framework.Items
{
    public abstract class IOPipeItem : PipeItem
    {
        [XmlIgnore]
        public Texture2D SignalTexture { get; set; }
        [XmlIgnore]
        public Texture2D OnSignal { get; set; }
        [XmlIgnore]
        public Texture2D OffSignal { get; set; }
        [XmlIgnore]
        public Texture2D UnconnectedSignal { get; set; }
        [XmlIgnore]
        public Texture2D NoChestSignal { get; set; }

        public IOPipeItem() : base()
        {
            LoadSignals();
        }
        public IOPipeItem(Vector2 position) : base(position)
        {
            LoadSignals();
        }

        public void LoadSignals()
        {
            OnSignal = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/on.png");
            OffSignal = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/off.png");
            UnconnectedSignal = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/unconnected.png");
            NoChestSignal = ModEntry.helper.Content.Load<Texture2D>($"assets/Pipes/nochest.png");
            SignalTexture = UnconnectedSignal;
        }

        public override bool performToolAction(Tool t, GameLocation location)
        {
            if (t is Pickaxe)
            {
                var who = t.getLastFarmerToUse();
                this.performRemoveAction(this.TileLocation, location);
                Debris deb = new Debris(this.getOne(), who.GetToolLocation(), new Vector2(who.GetBoundingBox().Center.X, who.GetBoundingBox().Center.Y));
                Game1.currentLocation.debris.Add(deb);
                Game1.currentLocation.objects.Remove(this.TileLocation);
                return false;
            }
            if (t is WrenchItem)
            {
                Printer.Info("WRENCH");
                ChangeSignal();
                return false;
            }
            return false;
        }
        public override Item getOne()
        {
            return ItemFactory.CreateItem(this);
        }

        public override bool clicked(Farmer who)
        {
            if(who.CurrentTool == null)
            {
                DataAccess DataAccess = DataAccess.GetDataAccess();
                if (DataAccess.IOPipeNames.Contains(this.Name))
                {
                    List<Node> nodes = DataAccess.LocationNodes[Game1.currentLocation];
                    Node node = nodes.Find(n => n.Position.Equals(this.TileLocation));
                    if(node is IOPipeNode)
                    {
                        IOPipeNode pipe = (IOPipeNode)node;
                        if (pipe != null)
                        {
                            Printer.Info($"{Name} is {pipe.Signal}");
                        }
                    }
                }
            }
            return false;
        }

        public void ChangeSignal()
        {
            DataAccess DataAccess = DataAccess.GetDataAccess();
            List<Node> nodes = DataAccess.LocationNodes[Game1.currentLocation];
            IOPipeNode pipe = (IOPipeNode)nodes.Find(n => n.Position.Equals(this.TileLocation));
            pipe.ChangeSignal();
            UpdateSignal(pipe.Signal);
        }

        public void UpdateSignal(string signal)
        {
            switch (signal)
            {
                case "on":
                    SignalTexture = OnSignal;
                    break;
                case "off":
                    SignalTexture = OffSignal;
                    break;
                case "unconnected":
                    SignalTexture = UnconnectedSignal;
                    break;
                case "nochest":
                    SignalTexture = NoChestSignal;
                    break;
            }
        }

        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1)
        {
            base.draw(spriteBatch, x, y);
            DataAccess DataAccess = DataAccess.GetDataAccess();
            if(DataAccess.LocationNodes.ContainsKey(Game1.currentLocation))
            {
                List<Node> nodes = DataAccess.LocationNodes[Game1.currentLocation];
                Node node = nodes.Find(n => n.Position.Equals(TileLocation));
                if (node != null && node is IOPipeNode)
                {
                    IOPipeNode IONode = (IOPipeNode)node;
                    if (IONode.Signal != null)
                    {
                        UpdateSignal(IONode.Signal);
                        float transparency = 1f;
                        if (IONode.Passable)
                        {
                            Passable = true;
                            transparency = 0.5f;
                        }
                        else
                        {
                            Passable = false;
                            transparency = 1f;
                        }
                        Rectangle srcRect = new Rectangle(0, 0, 16, 16);
                        spriteBatch.Draw(SignalTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64)), srcRect, Color.White * transparency, 0f, Vector2.Zero, 4f, SpriteEffects.None, ((float)(y * 64 + 32) / 10000f) + 0.002f);
                    }
                }
            }
        }

        public override Dictionary<int, int> GetNewDrawGuide()
        {
            Dictionary<int, int> DrawGuide = new Dictionary<int, int>();
            DrawGuide.Add(0, 0);
            DrawGuide.Add(1000, 1);
            DrawGuide.Add(1002, 1);
            DrawGuide.Add(1003, 2);
            DrawGuide.Add(1004, 3);
            DrawGuide.Add(500, 4);
            DrawGuide.Add(501, 4);
            DrawGuide.Add(503, 5);
            DrawGuide.Add(504, 6);
            DrawGuide.Add(100, 9);
            DrawGuide.Add(101, 7);
            DrawGuide.Add(102, 8);
            DrawGuide.Add(104, 9);
            DrawGuide.Add(10, 12);
            DrawGuide.Add(11, 10);
            DrawGuide.Add(12, 11);
            DrawGuide.Add(13, 12);
            DrawGuide.Add(1500, 13);
            DrawGuide.Add(1503, 13);
            DrawGuide.Add(1504, 14);
            DrawGuide.Add(1100, 15);
            DrawGuide.Add(1102, 15);
            DrawGuide.Add(1104, 16);
            DrawGuide.Add(1010, 17);
            DrawGuide.Add(1012, 17);
            DrawGuide.Add(1013, 18);
            DrawGuide.Add(600, 19);
            DrawGuide.Add(601, 19);
            DrawGuide.Add(604, 20);
            DrawGuide.Add(510, 21);
            DrawGuide.Add(511, 21);
            DrawGuide.Add(513, 22);
            DrawGuide.Add(110, 23);
            DrawGuide.Add(111, 23);
            DrawGuide.Add(112, 24);
            DrawGuide.Add(1600, 25);
            DrawGuide.Add(1510, 26);
            DrawGuide.Add(1110, 27);
            DrawGuide.Add(610, 28);
            DrawGuide.Add(1610, 29);
            return DrawGuide;
        }
    }
}
