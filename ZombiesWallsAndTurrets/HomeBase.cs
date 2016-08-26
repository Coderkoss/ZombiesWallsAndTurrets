using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombiesWallsAndTurrets
{
    public static class HomeBase
    {
        public static Vector2 position;
        public static Rectangle hitbox;
        static int width, height;
        public static Rectangle Hitbox 
        {
            get { return new Rectangle((int)position.X,(int)position.Y,width,height); }
        }
        static HomeBase() 
        {
            width = 40;
            height = 40;           
        }
        public static void Update()
        {
            if (Utility.Input.KeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                position.Y += 2;
            }
            else if (Utility.Input.KeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                position.Y -= 2;
            }
        }       
        public static void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(Game1.pixel, Hitbox, Color.Black * 0.4f);
        }
    }
}
