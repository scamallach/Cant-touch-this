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
            graphics.PreferredBackBufferWidth = 1080;
            graphics.PreferredBackBufferHeight = 720;
            
            graphics.ApplyChanges();

            player = new Player(90, 95);//Explicitly set to prototype walk texture params
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

            walk = Content.Load<Texture2D>(@"gb_walk2");
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
            UpdateInput();

            base.Update(gameTime);
        }

        protected void UpdateInput()
        {
            // Get the game pad state.
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            Vector2 lastPlayerPosition = player.Position;

            if (currentState.IsConnected)
            {
                /* Ship velocity type controls 
                // Rotate the model using the left thumbstick, and scale it down
                modelRotation -= currentState.ThumbSticks.Left.X * 0.10f;

                // Create some velocity if the right trigger is down.
                Vector3 modelVelocityAdd = Vector3.Zero;

                // Find out what direction we should be thrusting, 
                // using rotation.
                modelVelocityAdd.X = -(float)Math.Sin(modelRotation);
                modelVelocityAdd.Z = -(float)Math.Cos(modelRotation);

                // Now scale our direction by how hard the trigger is down.
                modelVelocityAdd *= currentState.Triggers.Right;

                // Finally, add this vector to our velocity.
                modelVelocity += modelVelocityAdd;
                */

                

                // MOVEMENT PROFILES
                // Binary
                // NEXT Summation
                /*
                float maxSpeed = 0.1f;
                float changeInAngle = currentState.ThumbSticks.Left.X * maxSpeed;

                //player.Direction += changeInAngle;


                Vector2 direction = new Vector2((float)Math.Cos(changeInAngle),
                                (float)Math.Sin(changeInAngle));
                direction.Normalize();
                player.Position += direction * maxSpeed;
                */

                // Weighted
                /*// Explicit stepped
                if (currentState.ThumbSticks.Left.X < 0) player.setPos(player.Position.X - 5, player.Position.Y);
                if (currentState.ThumbSticks.Left.X > 0) player.setPos(player.Position.X + 5, player.Position.Y);
                if (currentState.ThumbSticks.Left.Y < 0) player.setPos(player.Position.X, player.Position.Y + 5);
                if (currentState.ThumbSticks.Left.Y > 0) player.setPos(player.Position.X, player.Position.Y - 5);
                */
                // Acceleration stepped
                if (currentState.ThumbSticks.Left.X < 0) player.setPos(player.Position.X - 5, player.Position.Y);
                if (currentState.ThumbSticks.Left.X > 0) player.setPos(player.Position.X + 5, player.Position.Y);

                if (currentState.ThumbSticks.Left.Y < 0) player.setPos(player.Position.X, player.Position.Y + 5);
                if (currentState.ThumbSticks.Left.Y > 0) player.setPos(player.Position.X, player.Position.Y - 5);
                

                // END MOVEMENT PROFILES

                /*
                // Set vibration feedback
                GamePad.SetVibration(PlayerIndex.One,
                    1.0f, 1.0f);
                    //currentState.Triggers.Right,
                    //currentState.Triggers.Right);
                */


                /* Reset condition */
                // Warp back to start with the A button
                if (currentState.Buttons.A == ButtonState.Pressed)
                {
                    player.setPos(50, 50);// Vector2.Zero;
                    //modelVelocity = Vector3.Zero;
                    //modelRotation = 0.0f;
                }

                /* Check bounds */
                if (player.Position.X < 0) player.setPos(0, player.Position.Y);
                if (player.Position.Y < 0) player.setPos(player.Position.X, 0);
                if (player.Position.X > graphics.PreferredBackBufferWidth - player.Width) player.setPos(graphics.PreferredBackBufferWidth - player.Width, player.Position.Y);
                if (player.Position.Y > graphics.PreferredBackBufferHeight - player.Height) player.setPos(player.Position.X, graphics.PreferredBackBufferHeight - player.Height);
 
            }
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

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            currentLevel.Draw(spriteBatch, gameTime);

            spriteBatch.Draw(walk, player.Position, new Rectangle(5, 36, 90, 95), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
