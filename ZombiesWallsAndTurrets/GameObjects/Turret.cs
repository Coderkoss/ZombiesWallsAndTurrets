#define debug
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utility;

namespace ZombiesWallsAndTurrets
{
    class Turret:GameObject
    {
        const int WIDTH = 80;
        const int HEIGHT = 80;
        const int BULLETFIRESPEED = 2000;//higher means slower
        const float SPEED = 5;
        float timer;
        public List<Projectile> projectiles;
        public Zombie target;
        float radius;
        public bool firing;
        public Rectangle BoundingBox
        {
            get { return new Rectangle((int)this.position.X - (int)(WIDTH * scale) / 2, (int)this.position.Y - (int)(HEIGHT * scale) / 2, (int)(WIDTH * scale), (int)(HEIGHT * scale)); }
        }
        public float Timer 
        {
            get { return timer; }
            set { value = timer; }
        }
        public override Texture2D Texture
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override float Rotation
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override Vector2 Origin
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override Color Color
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override float Scale
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public Zombie Target
        {
            get { return target; }
            set { target = value;}
        }
        
        public Turret(Texture2D texture)
        {
            this.texture = texture;
            this.position = new Vector2(Game1.SCREENWIDTHLOWRES / 2, Game1.SCREENHEIGHTLOWRES / 2);
            this.sourceRectangle = new Rectangle(WIDTH * 3, 0, WIDTH, HEIGHT);
            this.rotation = 0f;
            this.origin = new Vector2(WIDTH / 2, HEIGHT / 2);
            this.color = Color.White;
            this.scale = .5f;
            target = null;
            radius = 500f;
            projectiles = new List<Projectile>();
            Game1.WallPlaceSnd.Play();
            firing = false;
        }
        public override void Update(GameTime gametime)
        {            
            timer += (float)gametime.ElapsedGameTime.TotalMilliseconds;
            if (Timer >= BULLETFIRESPEED) 
            {
                timer = 0;
                if(firing)
                    FireProjectile();       
            }
               UpdateRotation(target);          
                UpdateBullets();            
        }
        private void PlayShotSound()
        {
            Game1.gunSound.Play(0.5f,0f,0f);
        }
        private void PlayBulletHitSnd() 
        {
            Game1.bulletHitSnd.Play();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Projectile item in this.projectiles)
            {
                item.Draw(spriteBatch);
            }
            spriteBatch.Draw(texture, position,sourceRectangle,color,rotation,origin,scale,SpriteEffects.None,0f);
#if debug
            RectClass.DrawBorder(Game1.pixel, spriteBatch, BoundingBox, 1, Color.Red, Color.Gray * 0.3f);
#endif            
        }
        private void UpdateRotation(Zombie target) 
        {
            if (target != null) 
            {
                float turn = (float)Math.Atan2(target.Position.Y - this.position.Y ,
                                         target.Position.X - this.position.X)  ;
                //rotation = (float)Math.Atan2(target.Position.Y - this.position.Y ,
                //                         target.Position.X - this.position.X)  ;
                rotation = MathHelper.Lerp(rotation, turn, 0.1f);

            }           
        }
        private void FireProjectile()
        {
            Projectile projectile = new Projectile(Game1.bulletSpriteSheet);
            projectile.Velcoity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation))  * SPEED;
            projectile.Position = this.position + projectile.Velcoity ;
            projectiles.Add(projectile);
            PlayShotSound();
        }
        private void UpdateBullets() 
        {
            foreach (Projectile item in projectiles)
            {
                item.Position += item.Velcoity;
                if (Vector2.Distance(item.Position, this.position) > 700)
                {
                    item.Alive = false;
                    item.Rotation = this.rotation;
                }
            }
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (!projectiles[i].Alive)
                {
                    projectiles.RemoveAt(i);
                    i--;                   
                }
            }
        }
        public void GetClosestEnemy(List<Zombie> zombies)
        {
            target = null;
            foreach (Zombie zombie in zombies)
            {
                if(IsInRange(zombie.Position))
                {
                    target = zombie;
                }
            }
            
                  
        }
        private bool IsInRange(Vector2 position) 
        {
            if (Vector2.Distance(this.position, position) <= radius) 
            {
                return true;
            }
            return false;
        }
    }
}
