//#define Debug
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Utility;

namespace ZombiesWallsAndTurrets
{
    class Wall:GameObject
    {
        const int WIDTH = 80;
        const int HEIGHT = 80;
        Dictionary<string, Rectangle> wallTypes;
        string type;
        public int index;

        public string Type 
        {
            get { return type; }
            set { type = value; }
        }
        public Rectangle BoundingBox
        {
             get { return new Rectangle((int)this.position.X + (int)(WIDTH * scale) / 3, (int)this.position.Y, (int)(WIDTH * scale) / 3, (int)(HEIGHT * scale)); }
        }
        public override Texture2D Texture
        {
            get
            {
                return this.texture;
            }
            set
            {
                this.texture = value;
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
        public Dictionary<string, Rectangle> WallTypes 
        {
            get { return wallTypes; }
        } 


        public Wall(Texture2D texture,string type)
        {
            InitilizeWallPieces();
            this.texture = texture;
            this.position = new Vector2(0, 0);
            this.type = type;
            this.sourceRectangle = wallTypes[this.type];
            this.rotation = 0f;
            this.origin = new Vector2(0,0);
            this.color = Color.White;
            this.scale = .5f;
            Game1.WallPlaceSnd.Play();
        }

        public override void Update(GameTime gametime)
        {          
            sourceRectangle = wallTypes[Type];
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, SpriteEffects.None, 0f);
#if Debug
            RectClass.DrawBorder(Game1.pixel, spriteBatch, BoundingBox, 1, Color.Red, Color.Gray * 0.3f);
#endif
        }
        private void InitilizeWallPieces() 
        {
            wallTypes = new Dictionary<string, Rectangle>();
            wallTypes.Add("NWCorner",new Rectangle(0,0,WIDTH,HEIGHT));
            wallTypes.Add("SWCorner", new Rectangle(0, 80, WIDTH, HEIGHT));
            wallTypes.Add("NECorner", new Rectangle(80, 0, WIDTH, HEIGHT));
            wallTypes.Add("SECorner", new Rectangle(80, 80, WIDTH, HEIGHT));
            wallTypes.Add("HPiece", new Rectangle(160, 80, WIDTH, HEIGHT));
            wallTypes.Add("VPiece", new Rectangle(160, 0, WIDTH, HEIGHT));
            wallTypes.Add("TTop", new Rectangle(240, 0, WIDTH, HEIGHT));
            wallTypes.Add("TBottom", new Rectangle(240, 80, WIDTH, HEIGHT));
            wallTypes.Add("TLeft", new Rectangle(320, 0, WIDTH, HEIGHT));
            wallTypes.Add("TRight", new Rectangle(320, 80, WIDTH, HEIGHT));
        }
        public override string ToString()
        {
            return type;
        }            
    }
}
