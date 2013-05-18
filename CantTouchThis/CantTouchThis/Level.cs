using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoGame.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace CantTouchThis
{
    public class Level
    {
        Texture2D[] tiles;
        Texture2D[] obstacles;
        int width = 20;
        int height = 16;
        int tileWidth = 54;
        int tileHeight = 45;

        public int LevelWidth { get { return width * tileWidth; } }
        public int LevelHeight { get { return height * tileHeight; } }

        public List<Item> itemList;

        private Random random = new Random();

        int[] groundLayer = new int[]
        {
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
        };
        int[] obstacleLayer = new int[]
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 2, 0,
            0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0,
            0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0
        };

        public Level(Texture2D[] tiles, Texture2D[] obstacles, Texture2D itemTexture, Player player, GraphicsDevice graphics)
        {
            this.tiles = tiles;
            this.obstacles = obstacles;
            itemList = new List<Item>();

            for (int i = 0; i < 10; i++)
            {
                SpawnItem(player, itemTexture, graphics.Viewport.Height);
            }

            
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Vector2 pos = Vector2.Zero;

            for (int i = 0; i < groundLayer.Length; i++)
            {
                if (groundLayer[i] > 0)
                {
                    pos.X = tileWidth * (i % width);
                    pos.Y = tileHeight * (float)(Math.Floor((double)i / width));
                    spriteBatch.Draw(this.tiles[groundLayer[i] - 1], pos, Color.White);
                }
            }

            for (int i = 0; i < obstacleLayer.Length; i++)
            {
                if (obstacleLayer[i] > 0)
                {
                    pos.X = tileWidth * (i % width);
                    pos.Y = (tileHeight * (float)(Math.Floor((double)i / width)))
                        - (tiles[obstacleLayer[i] - 1].Height - tileHeight);
                    spriteBatch.Draw(this.tiles[obstacleLayer[i] - 1], pos, Color.White);
                }
            }

            foreach(Item item in itemList)
            {
                item.Draw(spriteBatch, gameTime);
            }
        }

        private void GenObstacleLayer()
        {
            this.obstacleLayer = new int[groundLayer.Length];
            for (int i = 0; i < groundLayer.Length; i++)
            {

            }
        }

        public Rectangle? CheckCollision(Rectangle playerRect)
        {
            Rectangle? result = null;

            Rectangle obstacleRect = new Rectangle();
            for (int i = 0; i < obstacleLayer.Length; i++)
            {
                if (obstacleLayer[i] > 0)
                {
                    obstacleRect.X = tileWidth * (i % width);
                    obstacleRect.Y = (tileHeight * (int)(Math.Floor((double)i / width)))
                        - (tiles[obstacleLayer[i] - 1].Height - tileHeight);
                    obstacleRect.Width = tiles[obstacleLayer[i] - 1].Width;
                    obstacleRect.Height = tiles[obstacleLayer[i] - 1].Height;

                    if (playerRect.Intersects(obstacleRect))
                        return obstacleRect;
                }
            }

            return result;
        }

        public Item CheckItemCollision(Rectangle playerRect)
        {
            Item result = null;

            foreach (Item item in itemList)
            {
                if (item.CheckCollision(playerRect))
                {
                    result = item;
                    break;
                }
            }

            if (result != null)
                itemList.Remove(result);

            return result;
        }

        private int GetNewItemLocation(int maxYPosition, int screenHeight)
        {
            int maxYCol = maxYPosition / (screenHeight / height);
            bool isEmpty = false;
            int index = -1;

            while (true)
            {
                index = random.Next(maxYCol * width);
                isEmpty = obstacleLayer[index] == 0;
                if (isEmpty)
                    break;
            }

            return index;
        }

        public void SpawnItem(Player player, Texture2D texture, int screenHeight)
        {
            while (true)
            {
                int tileIndex = this.GetNewItemLocation((int)player.Position.Y, screenHeight);
                Vector2 pos = new Vector2(
                    (tileIndex % width) * tileWidth,
                    (tileIndex / width) * tileHeight);

                Rectangle? collision = CheckCollision(new Rectangle((int)pos.X, (int)pos.Y, texture.Width, texture.Height));
                
                if (!collision.HasValue)
                {
                    itemList.Add(new Item(texture, pos));
                    break;
                }
            }
            
        }
    }
}
