#define Debug
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Utility;

namespace ZombiesWallsAndTurrets
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        enum GameState 
        {
            SplashScreen,
            StartScreen,
            Playing,
            Paused,
            GameOver
        }
        const int DIFFICULTYMULTIPILYER = 1;
        const int LEVELCHANGETIMER = 8;
        const int LEVELINITIALTIME = 5;
        public const int SCREENWIDTHLOWRES = 1280;
        public const int SCREENHEIGHTLOWRES = 720;
        public const int WORLDGRIDSIZE = 40;
        public static float GameScale = 1.0f;
        int frameRate = 0;
        int frameCounter = 0;
        int poorBulletDamage;
        int POORBULLETDAMAGEMIN = 6;
        int POORBULLETDAMAGEMAX = 12;
        bool created = false; 

        TimeSpan elaspedTime = TimeSpan.Zero;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        #region Game Object List
        List<Wall> Walls;
        List<Zombie> Zombies;
        List<Turret> Turrets;
        List<Rectangle> worldGrid;
        List<WorldCell> worldCells;
        List<Button> buttons;
        List<BloodPoof> bloodPoofs;
        List<Zombie> deadZombies;
        List<BrainDrop> brains;
        #endregion
        GameState currentGameState = GameState.SplashScreen;
        #region textures and fonts fields
        Texture2D zombieGameSpriteSheet;
        Texture2D zombieSpriteSheet;
        Texture2D turretSpriteSheet;
        public static SpriteFont ButtonTextFont;
        public static SpriteFont LevelChangeFont;
        public static SpriteFont debugFont;
        public static Texture2D buttonTextures;
        public static Texture2D WallSpriteSheet;
        public static Texture2D bulletSpriteSheet;
        public static Texture2D pixel;
        public static Texture2D grassBackground;
        public static Texture2D splashScreenBackground;
        public static Texture2D menubackground;
        public static Texture2D HealthBar;
        public static Texture2D bloodSplat;
        public static Texture2D SSZombieDeathAnmation;
        public static Texture2D brain;
        #endregion
        public static Random random;
        bool splashScreenMusicPlaying;
        bool DisplayLevelText;
        float colorlerp = 0.01f;
        Wall prevViewWall;
        int pieceSwitch;
        float musicInterval;
        int level;
        //Timers
        float levelChangeTime = 0;
        float GamePlayingInitialTime = 0;
        #region Sounds
        public static List<SoundEffect> ZombieGrowns;
        SoundEffect zombiegrown1;
        SoundEffect zombiegrown2;        
        SoundEffect zombiegrown4;
        SoundEffect zombiegrown5;
        SoundEffect zombiegrown6;
        SoundEffect zombiegrown7;        
        SoundEffect zombiegrown9;
        SoundEffect zombiegrown10;
        SoundEffect zombiegrown11;
        SoundEffect zombiegrown12;
        public static SoundEffect gunSound;
        public static SoundEffect bulletHitSnd;
        public static SoundEffect WallPlaceSnd;
        public static SoundEffect menuSelection;
        SoundEffect track1;
        SoundEffect SplashScreenMusic;
        #endregion
        Inventory invetory;
        //TODO:Check list  
        //Brains need to be added
        //Brains need to drop where zombies die
        //Brains need to be drop in a vary radius
        //Brains should bob up and down
        //Brains should be collected
        //Brains should go away after a set time
        //Brains should represent currency     
        //REfactor and comment code heavy
        //Start thinking about inventory system
        //inventory currency is going to be brains for now
        //tryout the unlocking inventory system
        //tryout the upgrade inventory system
        //Make game over state
        //make won game state
        //make tier zombies 
        //mAKE detail dirt around the base of the walls
        //make health bars for everything maybe
        //setup the zombie collisions with walls
        //zombie hitting walls sounds      
        //fix art on menu background
        //maybe setup bullet collisions with walls        
        //====================================================test shit==========================

        //=======================================================================================

        public static Rectangle ScreenSize
        {
            get { return new Rectangle(0,0,SCREENWIDTHLOWRES,SCREENHEIGHTLOWRES); }
        }
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            random = new Random();
            Walls = new List<Wall>();
            Zombies = new List<Zombie>();
            Turrets = new List<Turret>();
            worldGrid = new List<Rectangle>();
            deadZombies = new List<Zombie>();
            pieceSwitch = 0;
            musicInterval = 30;
            DisplayLevelText = false;   
            level = 1;
            invetory = new Inventory(120, 400);
        }        
        protected override void Initialize()
        {            
            graphics.PreferredBackBufferWidth = SCREENWIDTHLOWRES;
            graphics.PreferredBackBufferHeight = SCREENHEIGHTLOWRES;
            
            Window.AllowUserResizing = true;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            for (int x = 0; x < graphics.PreferredBackBufferWidth / WORLDGRIDSIZE; x++)
            {
                for (int y = 0; y < graphics.PreferredBackBufferHeight / WORLDGRIDSIZE; y++)
                {
                    worldGrid.Add(new Rectangle( x * WORLDGRIDSIZE,y*WORLDGRIDSIZE,WORLDGRIDSIZE,WORLDGRIDSIZE));
                }
            }
            worldCells = new List<WorldCell>();
            for (int i = 0; i < worldGrid.Count ; i++)
            {
                worldCells.Add( new WorldCell("null", worldGrid[i]));
            }

            bloodPoofs = new List<BloodPoof>();
            base.Initialize();
        }        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            #region sounds
            ZombieGrowns = new List<SoundEffect>();
            zombiegrown1 = Content.Load<SoundEffect>("Fx/growl2");
            zombiegrown2 = Content.Load<SoundEffect>("Fx/monster-zombie-scream");            
            zombiegrown4 = Content.Load<SoundEffect>("Fx/zmb-pain5");
            zombiegrown5 = Content.Load<SoundEffect>("Fx/zombie-21");
            zombiegrown6 = Content.Load<SoundEffect>("Fx/zombie-22");
            zombiegrown7 = Content.Load<SoundEffect>("Fx/zombie-attack6");            
            zombiegrown9 = Content.Load<SoundEffect>("Fx/zombie-growls6");
            zombiegrown10= Content.Load<SoundEffect>("Fx/zombie");
            zombiegrown11 = Content.Load<SoundEffect>("Fx/zombie02");
            menuSelection = Content.Load<SoundEffect>("Fx/menuSelection");
            zombiegrown12 = Content.Load<SoundEffect>("Fx/zombie1-");
            gunSound = Content.Load<SoundEffect>("Fx/GunShotSnd");
            SplashScreenMusic = Content.Load<SoundEffect>(@"Music/SplashScreen");
            bulletHitSnd = Content.Load<SoundEffect>("Fx/bulletSplash");
            splashScreenBackground = Content.Load<Texture2D>("Textures/ZombieWalls&TurretsBackground");
            WallPlaceSnd = Content.Load<SoundEffect>("Fx/WallPlaceSnd");
            track1 = Content.Load<SoundEffect>("Music/reverse-piano-45extended");
            ZombieGrowns.Add(zombiegrown1);
            ZombieGrowns.Add(zombiegrown2);            
            ZombieGrowns.Add(zombiegrown4);
            ZombieGrowns.Add(zombiegrown5);
            ZombieGrowns.Add(zombiegrown6);
            ZombieGrowns.Add(zombiegrown7);          
            ZombieGrowns.Add(zombiegrown9);
            ZombieGrowns.Add(zombiegrown10);
            ZombieGrowns.Add(zombiegrown11);
            ZombieGrowns.Add(zombiegrown12);
            splashScreenMusicPlaying = true;
            #endregion

            buttons = new List<Button>();
            brains = new List<BrainDrop>();
            buttonTextures = Content.Load<Texture2D>("Textures/ButtonTexture");
            ButtonTextFont = Content.Load<SpriteFont>("buttonFont");
            HealthBar = Content.Load<Texture2D>("Textures/healthBar");
            zombieGameSpriteSheet = Content.Load<Texture2D>("Textures/BasicSSZombie");
            zombieSpriteSheet = Content.Load<Texture2D>("Textures/ZombieSpriteSheet");
            turretSpriteSheet = Content.Load<Texture2D>("Textures/turretSpriteSheet");
            bulletSpriteSheet = Content.Load<Texture2D>("Textures/bulletSpriteSheet");
            bloodSplat = Content.Load<Texture2D>("Textures/bloodsplat");
            LevelChangeFont = Content.Load<SpriteFont>("SpriteFont1");
            menubackground = Content.Load<Texture2D>("Textures/ZWTMenuSelectionBackground");
            grassBackground = Content.Load<Texture2D>("Textures/grassBackgroundTopdown1");
            pixel = Content.Load<Texture2D>("Textures/Pixel");
            WallSpriteSheet = Content.Load<Texture2D>("Textures/WallsSpriteSheet");
            SSZombieDeathAnmation = Content.Load<Texture2D>("Textures/SSZombieDeath");
            prevViewWall = new Wall(WallSpriteSheet,"VPiece");
            brain = Content.Load<Texture2D>("Textures/brain");
            debugFont = Content.Load<SpriteFont>("Aierl");
            HomeBase.position = new Vector2(ScreenSize.Width / 2, ScreenSize.Height / 2);
            StartMenuCreate();
           
        }   
        protected override void UnloadContent()
        {
            
        }       
        protected override void Update(GameTime gameTime)
        {
            Input.Update();            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //Fps counter
            elaspedTime += gameTime.ElapsedGameTime;
            if (elaspedTime > TimeSpan.FromSeconds(1))
            {
                elaspedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
            //Fps counter
            switch (currentGameState)
            {
                case GameState.SplashScreen:
                    if (splashScreenMusicPlaying) 
                    {
                        SplashScreenMusic.Play();
                        splashScreenMusicPlaying = false;
                    }
                    colorlerp += 0.004f;
                    if (Input.LeftMouseButtonClick()) 
                    {
                        currentGameState = GameState.StartScreen;
                        SplashScreenMusic.Dispose();
                    }
                    break;
                case GameState.StartScreen:                  
                    foreach (Button button in buttons)
                    {
                        button.Update(gameTime);
                    }
                    if (buttons[0].ButtonSelected&&Input.LeftMouseButtonClick()) 
                    {
                        currentGameState = GameState.Playing;
                    }                                       
                    break;
                case GameState.Playing:
                    #region [Input]
                    for (int i = 0; i < worldCells.Count; i++)
                    {
                        WorldCell cell = worldCells[i];
                        cell.index = i;
                        CreateTurretWithClick(cell);
                        WallPreview(cell);
                        CreateWallWithClick(cell);
                    }
                    foreach (Wall item in Walls)
                    {
                        item.Update(gameTime);
                    }
                    prevViewWall.Update(gameTime);
                    #endregion                                                 
                    GamePlayingInitialTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    #region level Switch
                    while (GamePlayingInitialTime > LEVELINITIALTIME)
                    {
                        GamePlayingInitialTime = LEVELINITIALTIME;
                        createLevel(level);
                        if (Zombies.Count == 0)
                        {
                            DisplayLevelText = true;
                            levelChangeTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                            if (levelChangeTime >= LEVELCHANGETIMER || Input.KeyPressed(Keys.Space))
                            {                               
                                 created = false;
                                 level++;
                                 DisplayLevelText = false;
                                 levelChangeTime = 0;                               
                            }
                        }
                    }
                    #endregion
                    HomeBase.Update();
                    invetory.Update(gameTime);
                    CreateZombieWithClick();         
                    UpdateZombiesHits(gameTime);
                    RemoveDeadZombies();
                    Updateturrets(gameTime);
                    PlayGameMusic(gameTime);
                    UpdateBloodPoofs(gameTime);
                    UpdateBrainDrops(gameTime);
                    break;
                case GameState.Paused:
                    break;
                case GameState.GameOver:
                    break;
                default:
                    break;
            }
            base.Update(gameTime);
        }
        private void UpdateBloodPoofs(GameTime gametime)
        {
            foreach (BloodPoof item in bloodPoofs)
            {
                item.Update(gametime);
              
            }
            for (int i = bloodPoofs.Count - 1; i >= 0; i--)
            {
                if (!bloodPoofs[i].Alive) 
                {
                    bloodPoofs.Remove(bloodPoofs[i]);
                }
            }
            
        }
        private void createLevel(int level)
        {            
            if (created == false) 
            {
                for (int i = 0; i < level * DIFFICULTYMULTIPILYER; i++)
                {
                    CreateZombie();          
                }
                created = true; 
            }
        }
        private void CreateZombie() 
        {
            Zombie newZombie = new Zombie(zombieSpriteSheet);
            Zombies.Add(newZombie);
        }           
        private void PlayGameMusic(GameTime gameTime)
        {
            musicInterval -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (musicInterval <= 0) 
            {
                track1.Play(0.5f, 0f, 0f);
                musicInterval = random.Next(60, 200);
            }
        }
        private void CreateTurretWithClick(WorldCell worldcell)
        {
            if (Input.MouseInRect(worldcell.cellRect) && Input.KeyPressed(Keys.F1))
            {
                if (worldcell.occupiedType != "" &&
                    worldcell.occupiedType != "turret")
                {
                    Turret turret = new Turret(turretSpriteSheet);
                    turret.Position = new Vector2(worldcell.cellRect.X + turret.BoundingBox.Width / 2, worldcell.cellRect.Y + turret.BoundingBox.Height / 2);
                    Turrets.Add(turret);
                    worldcell.occupiedType = "turret";
                }
            }            
        }
        private void WallPreview(WorldCell worldCell)
        {
            if (Input.MouseInRect(worldCell.cellRect))
            {
                prevViewWall.Position = new Vector2(worldCell.cellRect.X, worldCell.cellRect.Y);
                if (Input.KeyPressed(Keys.F) && pieceSwitch == 0)
                {
                    prevViewWall.Type = "HPiece";
                    pieceSwitch = 1;
                    
                }
                else if (Input.KeyPressed(Keys.F) && pieceSwitch == 1)
                {
                    prevViewWall.Type = "VPiece";
                    pieceSwitch = 2;
                }
                else if (Input.KeyPressed(Keys.F) && pieceSwitch == 2)
                {
                    prevViewWall.Type = "SECorner";
                    pieceSwitch = 3;
                }
                else if (Input.KeyPressed(Keys.F) && pieceSwitch == 3)
                {
                    prevViewWall.Type = "NECorner";
                    pieceSwitch = 4;
                }
                else if (Input.KeyPressed(Keys.F) && pieceSwitch == 4)
                {
                    prevViewWall.Type = "SWCorner";
                    pieceSwitch = 5;
                }
                else if (Input.KeyPressed(Keys.F) && pieceSwitch == 5)
                {
                    prevViewWall.Type = "NWCorner";
                    pieceSwitch = 0;
                }
                
            }
            
        }
        private void CreateWallWithClick(WorldCell worldCell)
        {
            if (Input.MouseInRect(worldCell.cellRect) && Input.LeftMouseButtonClick()&&
                worldCell.occupiedType == "null")
            {                
                Vector2 cell = new Vector2(worldCell.cellRect.X, worldCell.cellRect.Y);
                Wall newWall = new Wall(WallSpriteSheet,"VPiece");
                newWall.index = worldCell.index;
                newWall.Position = cell;
                newWall.Type = prevViewWall.Type;                
                worldCell.occupiedType = newWall.Type;
                CorrectWallType(worldCell,newWall);
                Walls.Add(newWall);              
            }
            
        }
        private void CorrectWallType(WorldCell cell,Wall newWall)
        {
            WorldCell c = cell;
            Wall cw = newWall;
            if (c.occupiedType == "VPiece" && worldCells[c.index + 1].occupiedType == "HPiece")
            {
                worldCells[c.index + 1].occupiedType = "TTop";

                for (int i = 0; i < Walls.Count; i++)
                {
                    Wall wall = Walls[i];
                    if (wall.index == cw.index + 1)
                        wall.Type = "TTop";    
                    //done   1 down             
                }
            }
            if (c.occupiedType == "VPiece" && worldCells[c.index - 1].occupiedType == "HPiece")
            {
                worldCells[c.index - 1].occupiedType = "TBottom";
                for (int i = 0; i < Walls.Count; i++)
                {
                    Wall wall = Walls[i];
                    if (wall.index == cw.index - 1)
                        wall.Type = "TBottom";
                    //done 1 up
                }
            }
        }
        private void CreateZombieWithClick()
        {
            if (Input.RightMouseButtonClick())
            {
                Zombie newZombie = new Zombie(zombieSpriteSheet);
                newZombie.Position = new Vector2(Input.MousePositionByVector.X, Input.MousePositionByVector.Y);
                Zombies.Add(newZombie);
            }
        }
        private void UpdateZombiesHits(GameTime gameTime)
        {
            for (int i = 0; i < Zombies.Count; i++)
            {
                Zombies[i].Update(gameTime);
                
                for (int j = 0; j < Turrets.Count; j++)
                {
                    Turret turret = Turrets[j];
                    for (int y = 0; y < turret.projectiles.Count; y++)
                    {
                        if (Zombies[i].CheckProjectileCollision(turret.projectiles[y]))
                        {
                            poorBulletDamage = random.Next(POORBULLETDAMAGEMIN,POORBULLETDAMAGEMAX);
                            Zombies[i].hitAnimating = true;
                            Zombies[i].TakeDamage(poorBulletDamage);
                            Zombies[i].DamageTaken = poorBulletDamage;
                            Zombies[i].hitPointDisplay();
                            BloodPoof poof = new BloodPoof(Game1.bloodSplat, 4, false, 40, 40);
                            poof.Position = Zombies[i].Position;
                            bloodPoofs.Add(poof);
                            turret.projectiles.Remove(turret.projectiles[y]);

                            bulletHitSnd.Play();
                        }
                    }
                }
            }
            for (int i = 0; i < deadZombies.Count; i++)
            {
                deadZombies[i].Update(gameTime);
                
            }
        }
        private void UpdateBrainDrops(GameTime gameTime)
        {
            for (int i = 0; i < brains.Count; i++)
            {
                brains[i].Update(gameTime);
            }
        }
        private void RemoveDeadZombies()
        {
            for (int i = 0; i < Zombies.Count; i++)
            {
                if (!Zombies[i].Alive)
                {
                    brains.Add(new BrainDrop(Game1.brain,Zombies[i].Position));
                    deadZombies.Add(Zombies[i]);
                    Zombies.Remove(Zombies[i]);
                }
            }
        }
        private void Updateturrets(GameTime gameTime)
        {
            for (int i = 0; i < Turrets.Count; i++)
            {
                Turrets[i].Update(gameTime);
                Turrets[i].GetClosestEnemy(Zombies);
                if (Turrets[i].target != null)
                    Turrets[i].firing = true;
                else
                    Turrets[i].firing = false;
            }
        }               
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            frameCounter++;
            string fps = string.Format(" fps: {0}",frameRate);
            Window.Title = "ZombieWallsAndTurrets" + fps;

            spriteBatch.Begin();
            switch (currentGameState)
            {
                case GameState.SplashScreen:
                                      
                    spriteBatch.Draw(splashScreenBackground, Vector2.Zero,Color.Lerp(Color.Black, Color.White, colorlerp) );
                    break;
                case GameState.StartScreen:
                    spriteBatch.Draw(menubackground, Vector2.Zero, Color.White);
                    foreach (Button button in buttons)
                    {
                        button.Draw(spriteBatch);
                    }                                                                                 
                    break;
                case GameState.Playing:
                    spriteBatch.Draw(grassBackground, Vector2.Zero, Color.White);
                    HomeBase.Draw(spriteBatch);
                    foreach ( Turret item in Turrets)
                    {
                        item.Draw(spriteBatch);
                    }
                    foreach (Wall item in Walls)
                    {
                        item.Draw(spriteBatch);
                    }
                    foreach (Zombie item in Zombies)
                    {
                        item.Draw(spriteBatch);
                    }
                    foreach (Rectangle rect in worldGrid)
                    {
#if Debug     
                        Utility.RectClass.DrawBorder(pixel, spriteBatch, rect, 1, Color.Black, Color.Gray * 0.3f);     
#endif        
                    }
                    foreach (Zombie item in deadZombies)
                    {
                        item.Draw(spriteBatch);
                    }
                    foreach (BloodPoof obj in bloodPoofs)
                    {
                        obj.Draw(spriteBatch);
                    }
                    foreach (BrainDrop item in brains)
                    {
                        item.Draw(spriteBatch);
                    }
                    prevViewWall.Draw(spriteBatch);
                    if (DisplayLevelText)
                    {
                        spriteBatch.DrawString(LevelChangeFont, "level " + (level + 1), new Vector2(this.Window.ClientBounds.Width / 2 - LevelChangeFont.MeasureString("level").X / 2, this.Window.ClientBounds.Height / 2 - LevelChangeFont.MeasureString("level").Y / 2), Color.Red);
                        spriteBatch.DrawString(LevelChangeFont, "Press Space", new Vector2(this.Window.ClientBounds.Width / 2 - LevelChangeFont.MeasureString("Press Space").X / 2, this.Window.ClientBounds.Height / 2 - LevelChangeFont.MeasureString("Press Space").Y / 2 + 80), Color.Red);
                    }
                    else
                    {
                        spriteBatch.DrawString(LevelChangeFont, "", new Vector2(this.Window.ClientBounds.Width / 2 - LevelChangeFont.MeasureString("level").X / 2, this.Window.ClientBounds.Height / 2 - LevelChangeFont.MeasureString("level").Y / 2), Color.White);
                    }
                    invetory.Draw(spriteBatch);
#if Debug
                    DrawDebugInfo(new Vector2(0,0));     

#endif
                    break;
                case GameState.Paused:
                    break;
                case GameState.GameOver:
                    break;
                default:
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);           
        }                
        private void StartMenuCreate() 
        {
            buttons.Add(new Button(buttonTextures,0));
            buttons.Add(new Button(buttonTextures, 1));
            buttons.Add(new Button(buttonTextures, 2));
            buttons.Add(new Button(buttonTextures, 3));
            int buttonpos = 200;
            foreach (Button button in buttons)
            {
                button.Position = new Vector2(graphics.PreferredBackBufferWidth / 2 ,buttonpos);
                buttonpos += 50;                
            }
        }
        private void DrawDebugText(string text,string valuedisplay, Vector2 position)
        {
            int offset = 1;
            spriteBatch.DrawString(debugFont, text +":"+ valuedisplay, new Vector2(position.X + offset, position.Y + offset), Color.Red);
            spriteBatch.DrawString(debugFont, text +":"+ valuedisplay, new Vector2(position.X ,position.Y), Color.Black);

        }
        private void DrawDebugBackground(Vector2 position)
        {
            Utility.RectClass.DrawBorder(Game1.pixel, spriteBatch, new Rectangle((int)position.X,(int)position.Y, 160, 300), 2, Color.Red, Color.Gray * 0.7f);
        }
        private void DrawDebugInfo(Vector2 position)
        {
            int posIndex = 0;
            int offset = posIndex * 16;
            DrawDebugBackground(new Vector2(position.X,position.Y));
            DrawDebugText("Turrets",Turrets.Count.ToString(),new Vector2(position.X,position.Y + offset));
            posIndex++;
            offset = posIndex * 16;
            DrawDebugText("Zombies", Zombies.Count.ToString(), new Vector2(position.X,position.Y +offset));
            posIndex++;
            offset = posIndex * 16;
            DrawDebugText("Walls", Walls.Count.ToString(), new Vector2(position.X,position.Y + offset));
            posIndex++;
            offset = posIndex * 16;
            DrawDebugText("Dead Zombies", deadZombies.Count.ToString(), new Vector2(position.X, position.Y + offset));
            posIndex++;
            offset = posIndex * 16;
            DrawDebugText("brains", deadZombies.Count.ToString(), new Vector2(position.X, position.Y + offset));

        }
    }
}
