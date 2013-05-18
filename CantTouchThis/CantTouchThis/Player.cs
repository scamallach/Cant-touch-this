using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CantTouchThis
{
    public class Player
    {
        public Vector2 Position { get; protected set; }

        public float Velocity { get; set; }
        public float Direction { get; set; }
        public int Width;
        public int Height;

        public Player(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void setPos(int x, int y) {
            Position = new Vector2(x, y);
        }

    }
}
