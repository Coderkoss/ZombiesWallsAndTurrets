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
    class Button:GameObject
    {
        int width;
        int height;
        bool menuSelectionsndPlaying;
        bool buttonSelected;
        
        public bool ButtonSelected 
        {
            get { return buttonSelected; }
        }
        public Button(Texture2D texture,int buttonIndex) 
        {            
            this.texture = texture;
            this.width = this.texture.Width ;
            this.height = this.texture.Height/ 5;
            this.buttonSelected = false;    
            this.position = new Vector2(444,400);
            //Start
            //Options
            //Quit
            //Load
            //blank
            switch (buttonIndex)
            {
                case 0:
                    this.sourceRectangle = new Rectangle(0,0,this.width,this.height);
                    break;
                case 1:
                    this.sourceRectangle = new Rectangle(0, 36, this.width, this.height);
                    break;
                case 2:
                    this.sourceRectangle = new Rectangle(0, 72, this.width, this.height);
                    break;
                case 3:
                    this.sourceRectangle = new Rectangle(0, 106, this.width, this.height);
                    break;
                case 4:
                    this.sourceRectangle = new Rectangle(0, 180, this.width, this.height);
                    break;
                default:                
                    this.sourceRectangle = new Rectangle(0, 225, this.width, this.height);                    
                    break;
            }
            this.rotation = 0f;
            this.origin = new Vector2(this.width/2,this.height /2) ;
            this.color = Color.White;
            this.scale = 1.0f;
            
        }

        public override void Update(GameTime gametime)
        {
            if (Input.MouseInRect(Bounds))
            {
                buttonSelected = true;
                scale = MathHelper.Lerp(1.08f, 1.0f, 0.01f);
                if (!menuSelectionsndPlaying) 
                {
                    Game1.menuSelection.Play();
                    menuSelectionsndPlaying = true;
                }
                
            }
            else
            {
                buttonSelected = false; 
                scale = MathHelper.Lerp(1.0f, 1.08f, 0.01f);
                menuSelectionsndPlaying = false;
            }           
        }
        public override void Draw(SpriteBatch spriteBatch)
        {   
          //spriteBatch.Draw(this.texture,new Rectangle((int)this.position.X,(int)this.position.Y,this.width,this.height),Color.Red);//1
          // spriteBatch.Draw(this.texture, this.position, Color.Purple);//2
          // spriteBatch.Draw(this.texture, new Rectangle((int)this.position.X, (int)this.position.Y, this.width, this.height), Color.Blue);//3
          // spriteBatch.Draw(this.texture, this.position, sourceRectangle, Color.Green);//4
          // spriteBatch.Draw(this.texture, new Rectangle((int)this.position.X, (int)this.position.Y, this.width, this.height), sourceRectangle, Color.Orange, rotation, origin, SpriteEffects.None, 0.0f);//5
          // spriteBatch.Draw(this.texture, position, sourceRectangle, Color.White, rotation, origin, scale, SpriteEffects.None, 0.0f);//6
           spriteBatch.Draw(this.texture, this.position, sourceRectangle, Color.White, rotation, origin, new Vector2(scale, scale), SpriteEffects.None, 0.0f);//7
           
#if Debug
            RectClass.DrawBorder(Game1.pixel, spriteBatch, Bounds, 1, Color.Red, Color.Gray * 0.5f);
#endif
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
                return this.position;
            }
            set
            {
                this.position = value;  
            }
        }

        public override Rectangle SourceRectangle
        {
            get
            {
                return this.sourceRectangle;
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
                return this.rotation;
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
                return this.origin;
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
                return this.color;
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
                return this.scale;
            }
            set
            {
                this.scale = value;
            }
        }

        public Rectangle Bounds 
        {
            get { return new Rectangle(
                (int)position.X  - (int)(origin.X * scale),  
                (int)position.Y  - (int)(origin.Y * scale),
                (int)(width * scale),
                (int)(height * scale) );
            }            
        }
        public int Width 
        {
            get { return width; }
            set { width = value; }
        }
    }
}
