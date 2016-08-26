//#define debug
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utility;

namespace ZombiesWallsAndTurrets
{
    class Projectile:GameObject
    {
        
        const int WIDTH = 2;
        const int HEIGHT = 2;
        bool alive;
        Vector2 velocity;
        public Rectangle BoundingBox
        {            
            get { return new Rectangle((int)this.position.X, (int)this.position.Y, (int)(WIDTH * scale), (int)(HEIGHT * scale)); }
        }
        public bool Alive 
        {
            get { return alive; }
            set { alive = value; }
        }
        public Vector2 Velcoity 
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public override Texture2D Texture
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;
            }
        }

        public override Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public override Rectangle SourceRectangle
        {
            get
            {
                return sourceRectangle;
            }
            set
            {
                sourceRectangle = value;
            }
        }

        public override float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;   
            }
        }

        public override Vector2 Origin
        {
            get
            {
                return origin;
            }
            set
            {
                origin = value;
            }
        }

        public override Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        public override float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }

        public Projectile(Texture2D texture) 
        {
            this.texture = texture;
            this.position = Vector2.Zero;
            this.sourceRectangle = new Rectangle(0, 2, WIDTH, HEIGHT);
            this.rotation = 0f;
            this.origin = new Vector2(3, 1);
            this.color = Color.White;
            this.scale = 1.0f;
            this.velocity = Vector2.Zero;
            Alive = true;
        }
        public override void Update(GameTime gametime)
        {

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, SourceRectangle, Color, Rotation, Origin, Scale, SpriteEffects.None, 0f);
#if debug
            RectClass.DrawBorder(Game1.pixel, spriteBatch, BoundingBox, 2, Color.Red, Color.Blue * 0.3f);
#endif                        
        }        
    }
}
