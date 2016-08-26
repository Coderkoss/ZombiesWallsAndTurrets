using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombiesWallsAndTurrets
{
    class Inventory
    {
        const int MINWIDTH = 40;
        const int MINHEIGHT = 40;
        const int NORMALWIDTH = 86;
        const int NORMALHEIGHT = 200;
        const int SLOTICONSIZE = 40;
        
        Rectangle background;
        bool maximized;
        int width, height;
        int minWidth, minHeight;
        List<Rectangle> slots;

        public List<Rectangle> Slots
        {
            get { return slots; }
        }
        public Rectangle Background
        {
            get { return background; }
        }
        public Inventory(int width,int height)
        {
            this.width = width;
            this.height = height;
            this.background = new Rectangle(Game1.SCREENWIDTHLOWRES - width,height / 4,width,height);
            this.minWidth = 40;
            this.minHeight = 40;
            this.slots = new List<Rectangle>();
            
        }
        public void Update(GameTime gameTime)
        {
            int backGroundXpos;
            int backGroundYpos;

            MaximizeInventory(out backGroundXpos, out backGroundYpos);
            if (Utility.Input.MouseInRect(background))
                maximized = true;            
            else
                maximized = false;
            //THIS IS ADDS A RECTANGLE TO THE SLOTS LIST
            slots.Add(new Rectangle(backGroundXpos, backGroundYpos, SLOTICONSIZE, SLOTICONSIZE));
            //background rect
            background = new Rectangle(backGroundXpos, backGroundYpos, width, height);
        }

        private void MaximizeInventory(out int backGroundXpos, out int backGroundYpos)
        {
            if (maximized)
            {
                width = NORMALWIDTH;
                height = NORMALHEIGHT;
                backGroundYpos = Game1.SCREENHEIGHTLOWRES / 8;
                backGroundXpos = Game1.SCREENWIDTHLOWRES - width;

            }
            else 
            {
                width = MINWIDTH;
                height = MINHEIGHT;
                backGroundYpos = Game1.SCREENHEIGHTLOWRES / 8;
                backGroundXpos = Game1.SCREENWIDTHLOWRES - width;
            }
                      
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Utility.RectClass.DrawBorder(Game1.pixel, spriteBatch, background, 1, Color.Yellow, Color.Gray * .5f);
           // if(slots.Count > 0)
           //     Utility.RectClass.DrawBorder(Game1.pixel, spriteBatch, Slots[0], 1, Color.Red, Color.Yellow);
        }
        
        
    }
}
