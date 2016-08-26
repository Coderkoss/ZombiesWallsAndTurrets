using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Utility;

namespace ZombiesWallsAndTurrets
{
    class BloodPoof:Animation
    {
        public BloodPoof(Texture2D texture,int TotalFrames,bool looping,int width,int height):base(texture,TotalFrames,looping,width,height)
        {
            this.animatingInterval = 30f;
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gametime)
        {
            base.Update(gametime);
            SingleAnimation();
            if (!this.Animating)
                this.alive = false;
        }
        
    }
}
