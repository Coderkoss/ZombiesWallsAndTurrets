//#define debug
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace ZombiesWallsAndTurrets
{
    class Zombie:GameObject
    {
        #region Members
        public const int WIDTH = 80;
        public const int HEIGHT = 80;
        const float SPEED = .05f;
        const int FRAMES = 10;
        const int STARTHEALTH = 100;
        const int WALKTIMEINTERVAL = 75;
        const int HITTIMEINTERVAL = 50;
        float walkingAnimationTimer;
        float hitAnimationTimer;
        int WalkingFrame;
        int HitFrame;
        int health;
        float DeathAnmationTimer;
        bool displayingDamagePoints;
        int randomDamageOffset;
        int DeathFrame;
        Vector2 targetPosition;       
        Vector2 ScreenCenter;
        Vector2 velocity;
        public bool walkAnimating;
        public bool hitAnimating;
        bool alive;
        public Rectangle hitSourceRectangle;
        Vector2 bloodPosition;
        float groanTimer;
        int damageTaken;
        Vector2 damagePosition;
        float damageDisplayIncretment;
        

        public Vector2  DamagePosition 
        {
            get { return this.damagePosition; }
            set { this.damagePosition = value; }
        }
        public int DamageTaken 
        {
            set { damageTaken = value; }
            get { return damageTaken; }
        }
        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }
        public int Health 
        {
            get { return health; }
            set { health = value; }
        }
        public Vector2 BloodPosition 
        {
            get { return bloodPosition; }
            set { bloodPosition = value; }
        }
        public Rectangle BoundingBox 
        {
            get { return new Rectangle((int)this.Position.X - (int)((WIDTH / 2)  * scale) ,
                                       (int)this.Position.Y - (int)((HEIGHT / 2) * scale) ,
                                       (int)(WIDTH  * scale)  ,
                                       (int)(HEIGHT * scale)); }
        }
        public Vector2 Velocity 
        {
            get { return velocity; }
            set { velocity = value; }
        }
        public override Texture2D Texture
        {
            get {  return texture;  }
            set {  texture = value; }
        }
        public override Vector2 Position
        {
            get { return position; }
            set { position = value;}
        }
        public Vector2 TargetPosition
        {
            get { return targetPosition; }
            set { targetPosition = value; }
        }
        public override Rectangle SourceRectangle
        {
            get { return sourceRectangle; }
            set { sourceRectangle = value; }
        }
        public Rectangle HitSourceRectangle 
        {
            get { return hitSourceRectangle; }
            set { hitSourceRectangle = value; }
        }
        public override float Rotation
        {
            get{return rotation;  }
            set{rotation = value; }
        }
        public override Vector2 Origin
        {
            get{ return origin; }
            set{origin = value; }
        }
        public override Color Color
        {
            get{ return color; }
            set{color = value; }
        }
        public override float Scale
        {
            get{return scale; }
            set{scale = value; }
        }
        public Vector2 HealthBarPosition 
        {
            get{ return new Vector2(BoundingBox.X,BoundingBox.Y - 16);}            
        }
        public int ZombieHealthBarWidthBackground
        {
            get { return (int)(Game1.HealthBar.Width * (float)scale); }
        }
        public int ZombieHealthBarWidth 
        {
            get { return (int)(Game1.HealthBar.Width * this.Health / 100 * scale); }
        }
        #endregion
        public Zombie(Texture2D texture)
        {
            this.walkingAnimationTimer = 0;//done
            this.hitAnimationTimer = 0;//done
            this.WalkingFrame = 0;//done
            this.HitFrame = 0;//done
            this.health = STARTHEALTH;//done
            this.walkAnimating = true;//done
            this.hitAnimating = false;//done
            this.alive = true;//done
            this.texture = texture;//done
            this.DeathFrame = 0;
            this.DeathAnmationTimer = 50f;
            this.position = new Vector2(Game1.random.Next(-Game1.SCREENWIDTHLOWRES, Game1.SCREENWIDTHLOWRES * 2), Game1.random.Next(-Game1.SCREENHEIGHTLOWRES, Game1.SCREENHEIGHTLOWRES * 2));//done
            while (Game1.ScreenSize.Contains(new Point((int)this.Position.X, (int)this.Position.Y)))
            {
                this.Position = new Vector2(Game1.random.Next(-Game1.SCREENWIDTHLOWRES, Game1.SCREENWIDTHLOWRES * 2), Game1.random.Next(-Game1.SCREENHEIGHTLOWRES, Game1.SCREENHEIGHTLOWRES * 2));
            }
            
            this.sourceRectangle = new Rectangle(0, 0, WIDTH, HEIGHT);
            this.rotation = 0f;//done
            this.color = Color.White;//done
            this.scale = .5f;
            this.origin = new Vector2(30,36);
            ScreenCenter = new Vector2(Game1.SCREENWIDTHLOWRES / 2, Game1.SCREENHEIGHTLOWRES / 2);
            this.hitSourceRectangle = new Rectangle(0, 80, (int)(WIDTH / scale),(int)(HEIGHT / scale));
            this.targetPosition = new Vector2(HomeBase.Hitbox.Center.X,HomeBase.Hitbox.Center.Y);
            bloodPosition = this.position;            
            groanTimer = 2;
            
        }
        public override void Update(GameTime gametime)
        {            
            walkingAnimationTimer += (float)gametime.ElapsedGameTime.TotalMilliseconds;
            hitAnimationTimer += (float)gametime.ElapsedGameTime.TotalMilliseconds;
            if (alive)
            {
                this.sourceRectangle = new Rectangle(WIDTH * WalkingFrame, 0, WIDTH, HEIGHT);
                this.hitSourceRectangle = new Rectangle(WIDTH * HitFrame, 80, WIDTH, HEIGHT);

                if (Vector2.Distance(this.Position, targetPosition) <= 50)
                {
                    this.velocity = Vector2.Zero;
                    this.Position = new Vector2(0, 0);
                }
                else
                {
                    this.velocity = AIMovement.MoveTowards(this.Position, targetPosition) * SPEED * (float)gametime.ElapsedGameTime.TotalMilliseconds;
                    UpdateRotation();
                    WalkingAnimation(ref walkingAnimationTimer);
                    HitAmimation(ref hitAnimationTimer);
                }

                if (this.health <= 0)
                {
                    Alive = false;
                }
                this.Position += velocity;
                this.bloodPosition = position;

                //HIT POINT FLASH DISPLAY
                this.damagePosition = new Vector2(this.BoundingBox.X + randomDamageOffset, this.BoundingBox.Y + damageDisplayIncretment);
                if (displayingDamagePoints)
                {
                    damageDisplayIncretment -= 2f;
                    if (damageDisplayIncretment < -100f)
                    {
                        displayingDamagePoints = false;
                        damageDisplayIncretment = 0f;
                    }
                }
                // HIT POINT FLASH DISPLAY

                ZombieGroan(gametime);
            }
            else if (!alive)
            {
                DeathAnmationTimer -= (float)gametime.ElapsedGameTime.TotalMilliseconds;
                if (DeathAnmationTimer <= 0)
                {
                    DeathFrame++;
                    DeathAnmationTimer = 50f;
                }

                if (DeathFrame >= 11)
                {
                    DeathFrame = 11;
                }
                
            }
    
        }

        private void ZombieGroan(GameTime timer)
        {
            groanTimer -= (float)timer.ElapsedGameTime.TotalSeconds;
            if (groanTimer <= 0) 
            {
                Game1.ZombieGrowns[Game1.random.Next(0, Game1.ZombieGrowns.Count)].Play();
                groanTimer = 8;
            }            
        }   
       
        public override void Draw(SpriteBatch spriteBatch)
        {
#if debug
            RectClass.DrawBorder(Game1.pixel, spriteBatch, BoundingBox, 1, Color.Red, Color.Gray * 0.3f);
            //spriteBatch.DrawString(Game1.debugFont, damagePosition.ToString(), new Vector2(100, 100), Color.Black);
#endif           
            #region Zombie draw
            if (alive)
            {
                 #region Healthbar draw
                 spriteBatch.Draw(Game1.HealthBar, new Rectangle((int)HealthBarPosition.X, (int)HealthBarPosition.Y,ZombieHealthBarWidthBackground, Game1.HealthBar.Height), Color.Black);
                 spriteBatch.Draw(Game1.HealthBar, new Rectangle((int)HealthBarPosition.X, (int)HealthBarPosition.Y,ZombieHealthBarWidth, Game1.HealthBar.Height),Color.Red);
                 #endregion
                 spriteBatch.Draw(texture, bloodPosition , hitSourceRectangle, color,rotation, new Vector2(90,40), scale, SpriteEffects.None, 0f);
                 spriteBatch.Draw(texture, new Rectangle((int)this.position.X, (int)this.position.Y, (int)(WIDTH * scale), (int)(HEIGHT * scale)), sourceRectangle, Color.White, rotation, origin, SpriteEffects.None, 0f);
                if (displayingDamagePoints && Game1.ScreenSize.Contains((int)damagePosition.X, (int)damagePosition.Y))
                    spriteBatch.DrawString(Game1.debugFont, DamageTaken.ToString(), this.DamagePosition, Color.Black, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            }
            else if (!alive)
            {
                int Dwidth = 120;
                int Dheight = 80;
                Vector2 Dorgin = new Vector2(Dwidth /2,Dheight / 2);

                spriteBatch.Draw(Game1.SSZombieDeathAnmation, new Rectangle((int)this.position.X, (int)this.position.Y, (int)(Dwidth * scale), (int)(Dheight * scale)), 
                    new Rectangle(DeathFrame * Dwidth,0,Dwidth,Dheight), 
                    Color.White, 
                    rotation,
                    Dorgin  , 
                    SpriteEffects.None,
                    0f);
            }            
            #endregion            
        }
        private void UpdateRotation() 
        {            
            rotation = (float)Math.Atan2(targetPosition.Y - this.Position.Y, targetPosition.X - this.Position.X);
        }                            

        public void WalkingAnimation(ref float timer) 
        {
            if (walkAnimating)
            {
                if (timer > WALKTIMEINTERVAL)
                {
                    WalkingFrame++;
                    if (WalkingFrame >= 10)
                    {
                        WalkingFrame = 0;
                        //walkAnimating = false;
                    }                    
                    timer = 0;                    
                }                
            }
                     
        }
        public void HitAmimation(ref float timer) 
        {
            if (hitAnimating) 
            {
                if (timer > HITTIMEINTERVAL) 
                {
                    HitFrame++;
                    if (HitFrame >= 8)
                    {
                        HitFrame = 0;
                        hitAnimating = false;   
                    }
                    timer = 0;
                }
            }
        }
        public bool CheckProjectileCollision(Projectile projectile) 
        {
            if (this.BoundingBox.Intersects(projectile.BoundingBox))
                return true;            
            else 
                return false;            
        }
        public void TakeDamage(int damage) 
        {
            this.health -= damage;
        }
        public void hitPointDisplay() 
        {
            displayingDamagePoints = true;
            randomDamageOffset = Game1.random.Next(0,40);
        }
    }
}
