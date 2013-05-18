#region Using Statements
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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Level currentLevel;

        Texture2D tile;
        Texture2D walk;
        Player player;

        bool invertYaxis = false;

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
            
            graphics.PreferredBackBufferWidth = 1080;
            graphics.PreferredBackBufferHeight = 720;
            
            graphics.ApplyChanges();

            player = new Player(93, 80); //Explicitly set to prototype walk texture params
            player.setPos(50, 50);

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

            Texture2D[] tiles = new Texture2D[]
            {
                Content.Load<Texture2D>(@"tile"),
                Content.Load<Texture2D>(@"obstacle")
            };
            currentLevel = new Level(tiles);

            player.LoadContent(Content.Load<Texture2D>(@"walk_front_colour"),
                Content.Load<Texture2D>(@"walk_back_colour"),
                Content.Load<Texture2D>(@"wobble front"),
                Content.Load<Texture2D>(@"wobble back"));

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

            // Read in input from controller
            UpdateInput(gameTime);

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

                if (currentState.ThumbSticks.Right != Vector2.Zero)
                {
                    Vector2 movementVector = currentState.ThumbSticks.Right;
                    if (!invertYaxis) movementVector.Y *= -1; // Y-Axis is inverted by default, correct if necessary
                    player.Position += movementVector * correctionRight;
                }




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
            }

            // Check for collisions
            Rectangle playerRect = new Rectangle((int)player.Position.X, (int)player.Position.Y, 90, 95);
            Rectangle? collision = currentLevel.CheckCollision(playerRect);
            if (collision.HasValue)
            {
                player.Position = lastPlayerPosition;
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

            currentLevel.Draw(spriteBatch, gameTime);

            //TODO fix these sprite frame coords
            spriteBatch.Draw(player.CurrentWalk, player.Position, new Rectangle(5, 36, 90, 95), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
