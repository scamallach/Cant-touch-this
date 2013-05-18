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
        public static float INITIAL_CONTROL_SPEED = 0.2f;
        public static float MIN_CONTROL_SPEED = 0.1f;
        public static float MAX_CONTROL_SPEED = 0.4f;

        public Vector2 Position { get; set; }

        public float Velocity { get; set; }
        public float Direction { get; set; }
        public int Width;
        public int Height;


        public List<Item> leftStack;
        public List<Item> rightStack;


        public float LeftControl { get; set; }
        public float RightControl { get; set; }

        public Texture2D CurrentWalk { get; protected set; }
        protected Texture2D FrontWalk { get; set; }
        protected Texture2D BackWalk { get; set; }
        protected Texture2D FrontWobble { get; set; }
        protected Texture2D BackWobble { get; set; }


        public Player(int width, int height)
        {
            Width = width;
            Height = height;
            LeftControl = INITIAL_CONTROL_SPEED;
            RightControl = INITIAL_CONTROL_SPEED;

            leftStack = new List<Item>();
            rightStack = new List<Item>();
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

        public void balancePlayer()
        {
            /* Design decisions to change here:
             * -whether we want players to try and even out the balance
             * -whether we want players to try to get to the end quicker or get more rats
             * etc
             */

            // Find each weight from 0 to 4, add 1 always
            //eg: float result = (float)((Math.Abs(1 - 3) / 3) * 0.4) + 0.1f;
            if (leftStack.Count == 0 && rightStack.Count == 0)
            {
                LeftControl = MIN_CONTROL_SPEED;
                RightControl = MIN_CONTROL_SPEED;
            }
            else
            {
                float difference = (float)(
                    ((Math.Abs(leftStack.Count - rightStack.Count) / Math.Max(leftStack.Count, rightStack.Count))
                    * MAX_CONTROL_SPEED)
                    + MIN_CONTROL_SPEED);

                if (leftStack.Count >= rightStack.Count)
                {
                    LeftControl = difference;
                    RightControl = MIN_CONTROL_SPEED;
                }
                else
                {
                    LeftControl = MIN_CONTROL_SPEED;
                    RightControl = difference;
                }
            }
        }

    }
}
