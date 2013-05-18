using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CantTouchThis
{
    public class Item
    {
        Texture2D texture;
        public Vector2 position { get; set; }

        public Rectangle GetBoundingBox 
        { 
            get 
            { 
                return new Rectangle((int)position.X, (int)position.Y, texture.Bounds.Width, texture.Bounds.Height); 
            } 
        }

        public Item(Texture2D texture, Vector2 position)
        {
            this.position = position;
            this.texture = texture;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 transform, int topBoundary, int botBoundary, GameTime gameTime)
        {
            Vector2 pos = position - transform;

            if (pos.Y >= topBoundary && pos.Y <= botBoundary)
            {
                spriteBatch.Draw(texture, pos, Color.White);
            }
        }

        public bool CheckCollision(Rectangle rect, Vector2 transform)
        {
            Rectangle temp = new Rectangle(
                this.GetBoundingBox.X, 
                this.GetBoundingBox.Y - (int)transform.Y, 
                this.GetBoundingBox.Width, 
                this.GetBoundingBox.Height);
            return rect.Intersects(temp);
        }
    }
}
