using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ZombiesWallsAndTurrets
{
    abstract class GameObject
    {
        protected Texture2D texture;
        protected Vector2 position;
        protected Rectangle sourceRectangle;
        protected Color color;
        protected float rotation;
        protected Vector2 origin;
        protected float scale;

        public abstract Vector2 Position
        {
            get;
            set;
        }
        public abstract Texture2D Texture
        {
            get;
            set;
        }
        public abstract Rectangle SourceRectangle
        {
            get;
            set;
        }
        public abstract float Rotation
        {
            get;
            set;
        }
        public abstract Vector2 Origin
        {
            get;
            set;
        }
        public abstract Color Color
        {
            get;
            set;
        }
        public abstract float Scale
        {
            get;
            set;
        }
       
        public abstract void Update(GameTime gametime);
        public abstract void Draw(SpriteBatch spriteBatch);
        
    }
}
