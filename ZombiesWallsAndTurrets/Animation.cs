using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombiesWallsAndTurrets
{
    class Animation:GameObject
    {
        protected int width, height;
        protected int frame;
        protected int TotalFrames;
        protected float animatingInterval;
        protected float timer;
        protected bool Animating;
        protected bool alive;
        protected bool looping;

        public bool Alive 
        {
            get { return alive; }
        }

        public Animation(Texture2D texture,int TotalFrames,bool looping, int width,int height)
        {
            this.texture = texture;
            this.position = Vector2.Zero;
            this.width = width;
            this.height = height;
            this.color = Color.White;
            this.rotation = 0f;
            this.origin = new Vector2(width /2 ,height / 2);
            this.scale = Game1.GameScale;
            this.TotalFrames = TotalFrames;
            this.sourceRectangle = new Rectangle(0, 0, width, height);
            this.looping = looping;
            this.alive = true;
            this.Animating = true;
            this.animatingInterval = 100f;
        }
        public override void Update(GameTime gametime)
        {
            timer += (float)gametime.ElapsedGameTime.TotalMilliseconds;
            this.sourceRectangle = new Rectangle(width * frame,0,width,height);
            
        }

        protected void SingleAnimation()
        {
            if (Animating && !looping)
            {
                if (timer > animatingInterval)
                {
                    frame++;
                    if (frame >= TotalFrames)
                    {
                        frame = 0;
                        Animating = false;                        
                    }
                    timer = 0; 
                }
            }
        }

        protected void LoopingAnimation()
        {
            if (Animating && looping)
            {
                if (timer > animatingInterval) 
                {
                    frame++;
                    if (frame >= TotalFrames)
                    {
                        frame = 0;
                    }
                    timer = 0;
                }
                
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (alive) 
            {
                spriteBatch.Draw(Texture,Position,SourceRectangle,Color,Rotation,Origin,Scale,SpriteEffects.None,0f);
            }
        }

        public override Vector2 Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;  
            }
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

        public override Rectangle SourceRectangle
        {
            get
            {
                return sourceRectangle;
            }
            set
            {
                this.sourceRectangle = value;    
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
                this.rotation = value;  
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
                this.origin = value;     
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
                this.color = value;    
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
    }
}
