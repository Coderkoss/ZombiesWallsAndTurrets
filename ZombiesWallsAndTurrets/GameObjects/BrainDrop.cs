using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombiesWallsAndTurrets
{
    class BrainDrop:GameObject
    {
        const int WIDTH = 40;
        const int HEIGHT = 40;
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

        public BrainDrop(Texture2D texture,Vector2 position)
        {
            this.texture = texture;
            this.position = position;
            this.sourceRectangle = new Rectangle(0, 0, WIDTH, HEIGHT);
            this.rotation = 0f;
            this.origin = new Vector2(0, 0);
            this.color = Color.White;
            this.scale = .5f;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, SpriteEffects.None, 0f);
#if Debug
            RectClass.DrawBorder(Game1.pixel, spriteBatch, BoundingBox, 1, Color.Red, Color.Gray * 0.3f);
#endif
        }

        public override void Update(GameTime gametime)
        {
            this.scale = 5.0f;
        }
    }
}
