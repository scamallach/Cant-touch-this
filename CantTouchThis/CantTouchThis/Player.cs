using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CantTouchThis
{
    public class Player
    {
        public Vector2 Position { get; set; }

        public float Velocity { get; set; }
        public float Direction { get; set; }
        public int Width;
        public int Height;


        public List<Item> leftStack;
        public List<Item> rightStack;

        public Texture2D CurrentWalk { get; protected set; }
        protected Texture2D FrontWalk { get; set; }
        protected Texture2D BackWalk { get; set; }
        protected Texture2D FrontWobble { get; set; }
        protected Texture2D BackWobble { get; set; }

        //public PlayerL 


        public Player(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void LoadContent(Texture2D frontWalk, Texture2D backWalk, Texture2D frontWobble, Texture2D backWobble )
        {
            FrontWalk = frontWalk;
            BackWalk = backWalk;
            FrontWobble = frontWobble;
            BackWobble = backWobble;

            CurrentWalk = FrontWalk;
        }

        public void setPos(float x, float y) {
            Position = new Vector2(x, y);
        }

    }
}
