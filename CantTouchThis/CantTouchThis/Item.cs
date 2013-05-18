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

        public Item(Texture2D texture, Vector2 position)
        {
            this.position = position;
            this.texture = texture;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
