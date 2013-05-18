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
        public static float INITIAL_CONTROL_SPEED = 0.3f;
        public static float MIN_CONTROL_SPEED = 0.1f;
        public static float MAX_CONTROL_SPEED = 0.4f;

        public static int WOBBLE_DURATION = 650;
        public static int REFRESH_INTERVAL = 200;

        //TODO Fix initial null
        private Vector2 _Position;
        public Vector2 Position
        {
            get { return this._Position; }
            set
            {
                LastPlayerPosition = this._Position;
                this._Position = value;
            }
        }
        public Vector2 LastPlayerPosition { get; protected set; }

        public float Velocity { get; set; }
        public float Direction { get; set; }
        public int Width;
        public int Height;

        public List<Item> leftStack;
        public List<Item> rightStack;
        public float LeftControl { get; set; }
        public float RightControl { get; set; }

        protected bool refreshIntervalPassed = false;
        protected int refreshInterval { get; set; }
        
        public bool isMoving { get; set; }
        public bool facingUp { get; set; }
        /* true if wobbling, false if not
         */
        public bool Wobble { get; protected set; }
        protected int wobbleTimeout { get; set; }

        public Texture2D CurrentWalk
        {
            get
            {
                if (this.Wobble)
                {
                    if (this.facingUp) return BackWobble; else return FrontWobble;
                }
                else
                {
                    if (this.facingUp) return BackWalk; else return FrontWalk;
                }
            }
        }
        protected Texture2D FrontWalk { get; set; }
        protected Texture2D BackWalk { get; set; }
        protected Texture2D FrontWobble { get; set; }
        protected Texture2D BackWobble { get; set; }
        
        protected int MaxFrames { get; set; }
        private int _CurrentFrame;
        protected int CurrentFrame
        {
            get { return this._CurrentFrame; }
            set
            {
                if (CurrentFrame == (MaxFrames-1)) this._CurrentFrame = 0; else { this._CurrentFrame = value; }
            }
        }
        

        public Player(int width, int height)
        {
            Width = width;
            Height = height;
            LeftControl = INITIAL_CONTROL_SPEED;
            RightControl = INITIAL_CONTROL_SPEED;

            leftStack = new List<Item>();
            rightStack = new List<Item>();
            CurrentFrame = 0;
            MaxFrames = 6;
        }

        public void LoadContent(Texture2D frontWalk, Texture2D backWalk, Texture2D frontWobble, Texture2D backWobble )
        {
            FrontWalk = frontWalk;
            BackWalk = backWalk;
            FrontWobble = frontWobble;
            BackWobble = backWobble;

            //CurrentWalk = FrontWalk;
        }

        public void RegisterMovement(GameTime gameTime)
        {
            //Vector2 diff = LastPlayerPosition - Position;
            wobbleTimeout -= gameTime.ElapsedGameTime.Milliseconds;

            if (isMoving)
            {
                refreshInterval += gameTime.ElapsedGameTime.Milliseconds;
                //TODO and check what direction player is facing, modify Walk
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //refreshInterval += gameTime.ElapsedGameTime.Milliseconds; //Now done elsewhere
            if (refreshInterval > REFRESH_INTERVAL)
            {
                refreshInterval = 0;
                refreshIntervalPassed = true;
            }

            if (refreshIntervalPassed)
            {
                //Increment frame
                CurrentFrame++;
                
                //Update Orientation
                //if (facingUp) CurrentWalk = BackWalk; else CurrentWalk = FrontWalk;

                //Update wobble status
                if (wobbleTimeout <= 0)
                {
                    Wobble = false;
                    //if (CurrentWalk == FrontWobble) CurrentWalk = FrontWalk;
                    //if (CurrentWalk == BackWobble) CurrentWalk = BackWalk; 
                }
            }
            refreshIntervalPassed = false;

            spriteBatch.Draw(CurrentWalk, Position, 
                new Rectangle(CurrentFrame*Width, 36, Width, Height), 
                Color.White);
        }

        public void causeWobble()
        {
            //if (CurrentWalk == FrontWalk) CurrentWalk = FrontWobble;
            //if (CurrentWalk == BackWalk) CurrentWalk = BackWobble;
            Wobble = true;
            wobbleTimeout = WOBBLE_DURATION;
        }

        public void setPos(float x, float y) {
            Position = new Vector2(x, y);
        }

        public void AddItem(Item item)
        {
            //Check whether picked up by L or R scientist
            ////for now randomly choose
            int side = Game1.r.Next(1, 2);

            //Add item to corresponding stack
            if (side == 1)
            {
                leftStack.Add(item);
            }
            else
                rightStack.Add(item);

            //If stack full then stop?
        }

        public void DropItem()
        {
            //Check whether picked up by L or R scientist
            ////for now randomly choose
            int side = Game1.r.Next(1, 2);

            //Add item to corresponding stack
            if (side == 1 && leftStack.Count != 0)
            {
                leftStack.RemoveAt(0);
            }
            else if (rightStack.Count != 0)
                rightStack.RemoveAt(0);

            //If stack full then stop?
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
            int leftwgt = leftStack.Count;
            int rightwgt = rightStack.Count;

            /* Test only 
            leftwgt = 2;
            rightwgt = 5;
            /* end test only */



            if (leftwgt == rightwgt)
            {
                LeftControl = INITIAL_CONTROL_SPEED;
                RightControl = INITIAL_CONTROL_SPEED;
            }
            else
            {
                /* Difference is scaled  */
                float difference = (float)(
                    ((float)Math.Abs(leftwgt - rightwgt) / Math.Max(leftwgt, rightwgt)) 
                    /2.2);


                if (leftwgt >= rightwgt)
                {
                    LeftControl = INITIAL_CONTROL_SPEED + difference;
                    RightControl = (INITIAL_CONTROL_SPEED - difference) + MIN_CONTROL_SPEED;
                }
                else
                {
                    LeftControl = (INITIAL_CONTROL_SPEED - difference) + MIN_CONTROL_SPEED;
                    RightControl = INITIAL_CONTROL_SPEED + difference;
                }

                //Quickfix on feedback that he moves too fast
                LeftControl = (float)(LeftControl * 0.7);
                RightControl = (float)(RightControl * 0.7);
            }


        }

        public void RandomJumps()
        {
            float[] seeds = new float[] { 0, 0, 0, 0, 0, 0, 0.1f, 0.1f, 0.1f, 0.3f, 0.3f, 0.5f };
            //Random r = new Random();
            LeftControl += LeftControl * (float)seeds[Game1.r.Next(0, seeds.Length-1)];
            RightControl += RightControl * (float)seeds[Game1.r.Next(0, seeds.Length-1)];

        }
    }
}
