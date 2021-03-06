﻿#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace CantTouchThis
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public static readonly Random r = new Random();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Level currentLevel;
        Texture2D stscreen;
        Texture2D tile;
        Player player;

        Texture2D lastSceneTexture;

        bool invertYaxis = false;

        public bool StartScreen { get; set; }

        bool endScreen = false;

        public SpriteFont Font { get; set; }

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.IsFixedTimeStep = false;

            StartScreen = true;

            graphics.PreferredBackBufferWidth = 1080;
            graphics.PreferredBackBufferHeight = 720;
            
            graphics.ApplyChanges();

            player = new Player(95, 95);//Explicitly set to prototype walk texture params
            /*player.setPos(
                (graphics.GraphicsDevice.Viewport.Width / 2) + (player.Width / 2) , 
                graphics.GraphicsDevice.Viewport.Height - player.Height); */
            player.Position = new Vector2(
                (graphics.GraphicsDevice.Viewport.Width / 2) + (player.Width / 2) , 
                720);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            tile = Content.Load<Texture2D>(@"tile");
            stscreen = Content.Load<Texture2D>(@"First scene");

            Texture2D[] tiles = new Texture2D[]
            {
                Content.Load<Texture2D>(@"tile"),
                Content.Load<Texture2D>(@"obstacle")
            };

            Texture2D[] obstacles = new Texture2D[]
            {
                Content.Load<Texture2D>("BeakerFinal"),
                Content.Load<Texture2D>("BlueChairBackFinal"),
                Content.Load<Texture2D>("BlueChairFrontFinal"),
                Content.Load<Texture2D>("BlueChairSideFinal"),
                Content.Load<Texture2D>("BlueChairSideFlipFinal"),
                Content.Load<Texture2D>("ColumnFinal"),
                Content.Load<Texture2D>(@"Desk type 1Final"),
                Content.Load<Texture2D>(@"Desk type 2Final"),
                Content.Load<Texture2D>(@"Desk type 2sideFinal"),
                Content.Load<Texture2D>("GreenChairBackFinal"),
                Content.Load<Texture2D>("GreenChairFrontFinal"),
                Content.Load<Texture2D>("GreenChairSideFinal"),
                Content.Load<Texture2D>("GreenChairSideFlipFinal"),
                Content.Load<Texture2D>("PilarFinal"),
                Content.Load<Texture2D>("PinkChairBackFinal"),
                Content.Load<Texture2D>("PinkChairFrontFinal"),
                Content.Load<Texture2D>("PinkChairSideFinal"),
                Content.Load<Texture2D>("PinkChairSideFlipFinal")
            };


            currentLevel = new Level(tiles, obstacles, Content.Load<Texture2D>("RatCageFinal_small"), player, GraphicsDevice);

            player.LoadContent(Content.Load<Texture2D>(@"walk_front_colour2"),
                Content.Load<Texture2D>(@"walk_back_colour"),
                Content.Load<Texture2D>(@"wobble front"),
                Content.Load<Texture2D>(@"wobble back2"));

            lastSceneTexture = Content.Load<Texture2D>(@"last scene");

            Font = Content.Load<SpriteFont>("SpriteFont1");

            //walk = Content.Load<Texture2D>(@"walk_back_colour");
            //walkFront = Content.Load<Texture2D>(@"walk_front_colour");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (StartScreen)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
                    StartScreen = false;
                //display startscreen 

            }
            else
            {
                // Read in input from controller
                UpdateInput(gameTime);
            }
            base.Update(gameTime);
        }

        protected void UpdateInput(GameTime gameTime)
        {
            // Get the game pad state.
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            Vector2 lastPlayerPosition = player.Position;

            if (currentState.IsConnected)
            {
                // Update player's weight/speed balance
                player.balancePlayer();

                // Seed input controls with current weight/speed balance
                float correctionLeft = player.LeftControl * gameTime.ElapsedGameTime.Milliseconds;
                float correctionRight = player.RightControl * gameTime.ElapsedGameTime.Milliseconds;

                Vector2 movementVector = Vector2.Zero;
                if (currentState.ThumbSticks.Left != Vector2.Zero)
                    movementVector = currentState.ThumbSticks.Left * correctionLeft;

                if (currentState.ThumbSticks.Right != Vector2.Zero)
                    movementVector += currentState.ThumbSticks.Right * correctionRight;

                if (!invertYaxis) movementVector.Y *= -1; // Y-Axis is inverted by default, correct if necessary

                // Check for collisions
                Vector2 newPos = player.Position + movementVector;
                Rectangle playerRect = new Rectangle((int)newPos.X, (int)newPos.Y, 93, 80);
                Rectangle? collision = currentLevel.CheckCollision(playerRect);
                if (collision != null) {
                    player.causeWobble();
                    player.DropItem();
                }

                if(!collision.HasValue)
                {
                    player.Position += new Vector2(movementVector.X, 0);

                    if (player.Position.Y > (graphics.GraphicsDevice.Viewport.Height / 2))
                        player.Position += new Vector2(0, movementVector.Y);
                    else
                    {
                        currentLevel.transform += new Vector2(0, movementVector.Y);
                        if (currentLevel.transform.Y < -300)
                            Win();
                    }
                    //Apply vector for bounceback

                    currentLevel.transform += new Vector2(0, movementVector.Y);

                    player.isMoving = (movementVector != Vector2.Zero);

                    if (movementVector.Y < 0) player.facingUp = true;
                    if (movementVector.Y > 0) player.facingUp = false;
                }

                HandleItemCollisions(playerRect);


                /* Reset condition */
                // Warp back to start with the A button
                if (currentState.Buttons.A == ButtonState.Pressed)
                {
                    player.setPos(50, 50);

                }

                /* Check bounds */
                if (player.Position.X < 0) player.setPos(0, player.Position.Y);
                if (player.Position.Y < 0) player.setPos(player.Position.X, 0);
                if (player.Position.X > graphics.PreferredBackBufferWidth - player.Width) player.setPos(graphics.PreferredBackBufferWidth - player.Width, player.Position.Y);
                if (player.Position.Y > graphics.PreferredBackBufferHeight - player.Height) player.setPos(player.Position.X, graphics.PreferredBackBufferHeight - player.Height);

                /* Tell player they're moving */
                player.RegisterMovement(gameTime);

            }
            /* Keyboard controls */
            else
            {
                if (keyboardState.IsKeyDown(Keys.Left)) player.setPos(player.Position.X - 5, player.Position.Y);
                if (keyboardState.IsKeyDown(Keys.Right)) player.setPos(player.Position.X + 5, player.Position.Y);
                if (keyboardState.IsKeyDown(Keys.Down)) player.setPos(player.Position.X, player.Position.Y + 5);
                if (keyboardState.IsKeyDown(Keys.Up)) player.setPos(player.Position.X, player.Position.Y - 5);

                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    player.setPos(50, 50);
                }


                /* Check bounds */
                if (player.Position.X < 0) player.setPos(0, player.Position.Y);
                if (player.Position.Y < 0) player.setPos(player.Position.X, 0);
                if (player.Position.X > graphics.PreferredBackBufferWidth - player.Width) player.setPos(graphics.PreferredBackBufferWidth - player.Width, player.Position.Y);
                if (player.Position.Y > graphics.PreferredBackBufferHeight - player.Height) player.setPos(player.Position.X, graphics.PreferredBackBufferHeight - player.Height);

                /* Tell player they're moving */
                player.RegisterMovement(gameTime);
            }

            
        }

        private void HandleItemCollisions(Rectangle playerRect)
        {
            // Check for item collisions
            Item itemCollision = currentLevel.CheckItemCollision(playerRect);
            if (itemCollision != null)
            {
                player.AddItem(itemCollision);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            if (endScreen)
            {
                spriteBatch.Draw(lastSceneTexture, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                // Draw the Score in the top-left of screen
                spriteBatch.DrawString(
                    Font,                          // SpriteFont
                    "Score: " + (player.leftStack.Count + player.rightStack.Count).ToString(),  // Text
                    new Vector2(500, 350),                      // Position
                    Color.White);  
            }
            else
            {
                if (StartScreen)
                { //draw startscreen
                    spriteBatch.Draw(stscreen, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                }
                else
                {
                    currentLevel.Draw(spriteBatch, gameTime);

                    //TODO fix these sprite frame coords
                    player.Draw(spriteBatch, gameTime);
                    //spriteBatch.Draw(player.CurrentWalk, player.Position, new Rectangle(5, 36, 90, 95), Color.White);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Win()
        {
            endScreen = true;
        }
    }
}
