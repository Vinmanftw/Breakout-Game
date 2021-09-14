using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Breakout
{
    
    enum GameStates { Start, Instructions, Game , SataliteCrash, Shop}
    enum ShopTabs { Targets, Paddles, Balls}
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    struct ButtonSpriteStruct
    {
        public Texture2D ButtonTexture;
        public Rectangle ButtonRectangle;
        public Color ButtonColor;
        public float X;
        public float Y;
        public int Cost;
        public float WidthFactor;

    }
    struct GameSpriteStruct
    {
        public Texture2D SpriteTexture;
        public Rectangle SpriteRectangle;
        public float X;
        public float Y;
        public float XSpeed;       
        public float YSpeed;
        public float WidthFactor;
        public float TicksToCrossScreen;
        public bool Visible;
    }
    public class BreakOutCode : Game
    {
        GameSpriteStruct Harris, Paddle;
        ButtonSpriteStruct Targets,Paddles,Balls,FishEye,SkaterDude,Board,Hydra,frag,Sixty,Satellite,SpaceShuttle,Astroids,XWING,TIEFIGHT,FALCON;
        GameSpriteStruct[] targets;
        MouseState mouseState;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        SoundEffect M60, Flyover,jungle, StarwarsSong,Xwingfire,Xwingfire3,TieFighterEx,TieFighterFlyBy, TieFighterFlyBy4;
        SoundEffectInstance M60Instance,Xwingfire3Instance;
        
        

        GameStates curGameState = GameStates.Start;
        ShopTabs curShopTab = ShopTabs.Targets;
        Texture2D targetsTexture, startButtonTexture,PlayAgainTexture,MainMenuTexture, instructionsButtonTexture, BackTexture, BreakoutInt,BreakoutScreen ,CliffSpace, EarthComet, Galexy1, Galexy2, MilkyWay, Sattelite,SatelliteCrashTexture,Store,ShopS,Nam1,Nam2,Nam3,Nam4,Nam5,Nam6,Nam7 ,Huey,Nam2f,Nam3f,Nam7f,Nam6f ,TIEfighterZoom, TIEfighterZoom2, TIEfighterZoom3, Empire1;
        Rectangle    backgroundRect, startButtonRect, instructionsButtonRect, BackRec, instructionsRec,PlayAgainRec,MainMenuRec,ShopRec,TopImageRec,MidImageRec,BottomImageRec,BBottomImageRec,HueyRec,ZoomRec, ZoomRec2, ZoomRec3;
        Texture2D AstroidS, BoardS, FishEye1, FishEye2, Frag, Hind, SatelliteS, Sixty1,Sixty2, SkaterDudeS, SpaceShuttleS,TIEFighter,Falcon, xWing;
        
        void RandomBackGround()
        {
            Random rnd = new Random();
            int nextValue = rnd.Next(0, 5);
            if (curTargetTexture == TIEFighter && curPaddleTexture == Falcon && curBallTexture == xWing)
            {
                RandomBackTexture = Empire1;
            }
        
            else if (curTargetTexture ==Hind && (curPaddleTexture ==Sixty1 ||curPaddleTexture ==Sixty2 )&&curBallTexture == Frag)
            {
                int NextValue = rnd.Next(0, 4);
                if (NextValue == 0)
                {
                    RandomBackTexture = Nam2;
                    frontNam = Nam2f;
                }
                else if (NextValue == 1)
                {
                    RandomBackTexture = Nam3;
                    frontNam = Nam3f;
                }
                else if (NextValue == 2)
                {
                    RandomBackTexture = Nam7;
                    frontNam = Nam7f;
                }
                else if (NextValue == 3)
                {
                    RandomBackTexture = Nam6;
                    frontNam = Nam6f;
                }
                


            }
            else if(nextValue == 0) 
            {
                RandomBackTexture = CliffSpace;
            }
            else if (nextValue == 1)
            {
                RandomBackTexture = EarthComet;
            }
            else if (nextValue == 2)
            {
                RandomBackTexture = Galexy1;
            }
            else if (nextValue == 3)
            {
                RandomBackTexture = Galexy2;
            }
            else if (nextValue == 4)
            {
                RandomBackTexture = MilkyWay;
            }
            else 
            {
                RandomBackTexture = Sattelite;
            }
        }
        

        float displayWidth;
        float displayHeight;     
        float minDisplayX,minDisplayY = 0;
        float maxDisplayY = 1500;

        float targetHeight;
        float targetStepFactor = 0.1f;
        float targetHeightLimit = 750;

        int numberOfTargets = 20;
        int Score;
        int HighScore;
        int Lives;
        int Points;

        
        bool PointsVisible;
        bool inCollision;
        Texture2D frontNam;
        Texture2D RandomBackTexture;
        Texture2D curPaddleTexture;
        Texture2D curBallTexture;
        Texture2D curTargetTexture;
        Color BackColor = Color.White;
        Color startColor = Color.White;
        Color PlayAgainColor = Color.White;
        Color MainMenuColor = Color.White;
        Color instructionsColor = Color.White;
        Color ShopColor = Color.White;
        
        void setupSprite(ref GameSpriteStruct sprite,Texture2D Skin ,float widthFactor, float ticksToCrossScreen,float initialX,float initialY,bool initialVisibility)
        {
            sprite.WidthFactor = widthFactor;
            sprite.TicksToCrossScreen = ticksToCrossScreen;
            sprite.SpriteRectangle.Width = (int)((displayWidth * widthFactor) + 0.5f);
            float aspectRatio = (float)Skin.Width / sprite.SpriteTexture.Height; 
            sprite.SpriteRectangle.Height = (int)((sprite.SpriteRectangle.Width / aspectRatio) + 0.5f);
            sprite.X = initialX;
            sprite.Y = initialY;
            sprite.XSpeed = displayWidth / ticksToCrossScreen;
            sprite.YSpeed = sprite.XSpeed;
            sprite.Visible = initialVisibility;
        }
        void setupButtons(ref ButtonSpriteStruct button,float widthFactor,int initialX,int initialY,int cost,Color buttonColor)
        {
            button.WidthFactor = widthFactor;            
            button.ButtonRectangle.Width = (int)((displayWidth * widthFactor) + 0.5f);
            float aspectRatio = (float)button.ButtonTexture.Width / button.ButtonTexture.Height;
            button.ButtonRectangle.Height = (int)((button.ButtonRectangle.Width / aspectRatio) + 0.5f);
            button.ButtonRectangle.X = initialX;
            button.ButtonRectangle.Y = initialY;
            button.Cost = cost;
            button.ButtonColor = buttonColor;
        }
        void resetTargetDisplay()
        {
            targetHeight = targetHeight +
                    (displayHeight * targetStepFactor);
            
            

            if (targetHeight > targetHeightLimit)
            {
                targetHeight = minDisplayY;
            }

            for (int i = 0; i < numberOfTargets; i++)
            {
                targets[i].Visible = true;
                targets[i].Y = targetHeight;
            }
        }
        void UpdateVariables()
        {
            Lives = 3;
            Score = 0;
            HueyRec.X = 0;
            HueySetup();
        }
        
        public BreakOutCode()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        
        
       

        protected override void Initialize()
        {
            targetsTexture = Content.Load<Texture2D>("HairlessPFP");
            Paddle.SpriteTexture = Content.Load<Texture2D>("tvinRight");
            Harris.SpriteTexture = Content.Load<Texture2D>("HarrisBack");
            Targets.ButtonTexture = Content.Load<Texture2D>("TargetsSkinsB");
            Paddles.ButtonTexture = Content.Load<Texture2D>("PaddleSkins");
            Balls.ButtonTexture = Content.Load<Texture2D>("SatelliteSkinsB");
            FishEye.ButtonTexture = Content.Load<Texture2D>("FishEyeB");
            Board.ButtonTexture = Content.Load<Texture2D>("BoardB");
            SkaterDude.ButtonTexture = Content.Load<Texture2D>("SkaterDudeB");
            Hydra.ButtonTexture = Content.Load<Texture2D>("HindB");
            Sixty.ButtonTexture = Content.Load<Texture2D>("SixtyB");
            frag.ButtonTexture = Content.Load<Texture2D>("FragB");
            Satellite.ButtonTexture = Content.Load<Texture2D>("SatelliteB");
            SpaceShuttle.ButtonTexture = Content.Load<Texture2D>("SpaceShuttleB");
            Astroids.ButtonTexture = Content.Load<Texture2D>("AstroidB");
            FALCON.ButtonTexture = Content.Load<Texture2D>("Falcon");
            Falcon= Content.Load<Texture2D>("FalconS");
            TIEFIGHT.ButtonTexture = Content.Load<Texture2D>("TIE-Fight");
            TIEFighter = Content.Load<Texture2D>("TIEfighter");
            XWING.ButtonTexture = Content.Load<Texture2D>("X-Wing");
            xWing = Content.Load<Texture2D>("X wing");
            AstroidS = Content.Load<Texture2D>("Astroid");
            BoardS = Content.Load<Texture2D>("BoardBB");
            FishEye1 = Content.Load<Texture2D>("FishEye1");
            FishEye2 = Content.Load<Texture2D>("FishEye2");
            Frag = Content.Load<Texture2D>("frag");
            Hind = Content.Load<Texture2D>("Hind");
            SatelliteS = Content.Load<Texture2D>("SatelliteS");
            Sixty1 = Content.Load<Texture2D>("sixty1");
            Sixty2 = Content.Load<Texture2D>("sixty2");
            SkaterDudeS = Content.Load<Texture2D>("SkaterDude2.0");
            SpaceShuttleS = Content.Load<Texture2D>("SpaceShuttle");

            Huey = Content.Load<Texture2D>("Huey1");
            TIEfighterZoom = Content.Load<Texture2D>("ZOOM");
            TIEfighterZoom2 = Content.Load<Texture2D>("ZOOM");
            TIEfighterZoom3 = Content.Load<Texture2D>("ZOOM");

            font = Content.Load<SpriteFont>("Score");
            displayWidth = GraphicsDevice.Viewport.Width;
            displayHeight = GraphicsDevice.Viewport.Height;
            curPaddleTexture = Paddle.SpriteTexture;
            curBallTexture = Harris.SpriteTexture;
            curTargetTexture = targetsTexture;

            

            Lives = 3;

            // TODO: Add your initialization logic here
            
            
            SoundEffect.DistanceScale = 0.4f;
            SoundEffect.MasterVolume = .2f;
            ZoomRec3 = new Rectangle(-480, 390, 352, 198);
            ZoomRec2 = new Rectangle(-320, 210, 320, 180);
            ZoomRec = new Rectangle(0, 300, 288, 162);  
            HueyRec = new Rectangle(0, 300, 320, 180);
            ShopRec = new Rectangle(GraphicsDevice.Viewport.Width / 2 - 125, GraphicsDevice.Viewport.Height - 200, 250,100);
            MainMenuRec = new Rectangle(0, GraphicsDevice.Viewport.Height - 425, 450, 325);
            PlayAgainRec = new Rectangle(GraphicsDevice.Viewport.Width - 450, GraphicsDevice.Viewport.Height - 325, 450,225);            
            instructionsRec = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            backgroundRect = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            startButtonRect = new Rectangle(GraphicsDevice.Viewport.Width-300, GraphicsDevice.Viewport.Height - 190, 300, 90);
            instructionsButtonRect = new Rectangle(0, GraphicsDevice.Viewport.Height - 200, 450, 100);
            BackRec = new Rectangle(0, GraphicsDevice.Viewport.Height - 100, 250, 100);
            base.Initialize();
        }
        void Shop()
        {
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboard = Keyboard.GetState();
            mouseState = Mouse.GetState();
            if (ShopRec.Contains(new Point(mouseState.X, mouseState.Y)))
            {
                ShopColor = Color.Yellow;
                if (mouseState.LeftButton == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Enter) || pad1.Buttons.A == ButtonState.Pressed)
                {
                    ShopColor = Color.Red;
                    curGameState = GameStates.Shop;
                    IsMouseVisible = true;
                }
            }
            else
            {
                ShopColor = Color.White;
            }
        }
        void TabNavigation(ref ButtonSpriteStruct button,int use)
        {
            mouseState = Mouse.GetState();
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboard = Keyboard.GetState();
            if (button.ButtonRectangle.Contains(new Point(mouseState.X, mouseState.Y)))
            {
                button.ButtonColor = Color.LightGreen;
                if (mouseState.LeftButton == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Enter) || pad1.Buttons.A == ButtonState.Pressed)
                {
                    button.ButtonColor = Color.Green;


                    if (use == 1 && curShopTab != ShopTabs.Targets)
                    {
                        curShopTab = ShopTabs.Targets;
                    }
                    else if (use == 2 && curShopTab != ShopTabs.Paddles)
                    {
                        curShopTab = ShopTabs.Paddles;
                    }
                    else if (use == 3 && curShopTab != ShopTabs.Balls)
                    {
                        curShopTab = ShopTabs.Balls;
                    }
                    IsMouseVisible = true;
                }               
            }
            else
            {
                button.ButtonColor = Color.LightBlue;
            }
        }
        void ShopUpdateButtons(ref ButtonSpriteStruct button,int use,Texture2D Skin)
        {
            mouseState = Mouse.GetState();
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboard = Keyboard.GetState();
            if (button.ButtonRectangle.Contains(new Point(mouseState.X, mouseState.Y)))
            {
                if (Points >= button.Cost)
                {
                    button.ButtonColor = Color.LightGreen;
                    if (mouseState.LeftButton == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Enter) || pad1.Buttons.A == ButtonState.Pressed)
                    {
                        button.ButtonColor = Color.Green;
                        Points -= button.Cost;
                        button.Cost = 0;
                        if (use == 1)
                        {
                            curTargetTexture = Skin;
                        }
                        else if (use == 2)
                        {
                            curPaddleTexture = Skin;
                        }
                        else if (use == 3)
                        {
                            curBallTexture = Skin;
                        }
                        IsMouseVisible = true;
                    }
                }
                else
                {
                    button.ButtonColor = Color.Yellow;
                }
                
                
            }
            else
            {
                button.ButtonColor = Color.White;
            }
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            targets = new GameSpriteStruct[numberOfTargets];
            float targetSpacing = displayWidth / numberOfTargets;
            
            setupSprite(ref Harris,curBallTexture, 0.05f, 200.0f, displayWidth / 2, displayHeight / 2,true);
            setupSprite(ref Paddle,curPaddleTexture , 0.15f, 120.0f, displayWidth / 10, 1300, true);
            //yes what is below is the code you need to activate to enable free skins
            /*
            setupButtons(ref Targets, 0.25f, 250, 250, 0, Color.White);
            setupButtons(ref FishEye, 0.25f, 750, 500, 0, Color.White);
            setupButtons(ref Hydra, 0.25f, 750, 750, 0, Color.White);
            setupButtons(ref Astroids, 0.25f, 750, 1000, 0, Color.White);
            setupButtons(ref TIEFIGHT, 0.25f, 750, 1250, 0, Color.White);
            setupButtons(ref Paddles, 0.25f, 750, 250, 0, Color.White);
            setupButtons(ref Board, 0.255f, 750, 500, 0, Color.White);
            setupButtons(ref Sixty, 0.25f, 750, 750, 0, Color.White);
            setupButtons(ref SpaceShuttle, 0.25f, 750, 1000, 0, Color.White);
            setupButtons(ref FALCON, 0.25f, 750, 1250, 0, Color.White);
            setupButtons(ref Balls, 0.25f, 1250, 250, 0, Color.White);
            setupButtons(ref SkaterDude, 0.25f, 750, 500, 0, Color.White);
            setupButtons(ref frag, 0.25f, 750, 750, 0, Color.White);
            setupButtons(ref Satellite, 0.25f, 750, 1000, 0, Color.White);
            setupButtons(ref XWING, 0.25f, 750, 1250, 0, Color.White);
            */
            setupButtons(ref Targets, 0.25f, 250, 250, 0, Color.White);
            setupButtons(ref FishEye, 0.25f, 750, 500, 300, Color.White);
            setupButtons(ref Hydra, 0.25f, 750, 750, 600, Color.White);
            setupButtons(ref Astroids, 0.25f, 750, 1000, 900, Color.White);
            setupButtons(ref TIEFIGHT, 0.25f, 750, 1250, 1200, Color.White);
            setupButtons(ref Paddles, 0.25f, 750, 250, 0, Color.White);
            setupButtons(ref Board, 0.255f, 750, 500, 300, Color.White);
            setupButtons(ref Sixty, 0.25f, 750, 750, 600, Color.White);
            setupButtons(ref SpaceShuttle, 0.25f, 750, 1000, 900, Color.White);
            setupButtons(ref FALCON, 0.25f, 750, 1250, 1200, Color.White);
            setupButtons(ref Balls, 0.25f, 1250, 250, 0, Color.White);
            setupButtons(ref SkaterDude, 0.25f, 750, 500, 300, Color.White);
            setupButtons(ref frag, 0.25f, 750, 750, 600, Color.White);
            setupButtons(ref Satellite, 0.25f, 750, 1000, 900, Color.White);
            setupButtons(ref XWING, 0.25f, 750, 1250, 1200, Color.White);
            
            TopImageRec = new Rectangle(250, 500, 500, 250);
            MidImageRec = new Rectangle(250, 750, 500, 250);
            BottomImageRec = new Rectangle(250, 1000, 500, 250);
            BBottomImageRec = new Rectangle(250, 1250, 500, 250);
            for (int i = 0; i < numberOfTargets; i++)
            {
                
                targets[i].SpriteTexture = curTargetTexture;
                setupSprite(ref targets[i],curTargetTexture ,0.05f, 0, i * targetSpacing, 0 + targets[i].SpriteRectangle.Height, true);
            }


            Nam1 = Content.Load<Texture2D>("Nam1");
            Nam2 = Content.Load<Texture2D>("Nam2");
            Nam2f = Content.Load<Texture2D>("Nam2f");
            Nam3 = Content.Load<Texture2D>("Nam3");
            Nam3f = Content.Load<Texture2D>("Nam3f");
            Nam4 = Content.Load<Texture2D>("Nam4");
            Nam5 = Content.Load<Texture2D>("Nam5");
            Nam6 = Content.Load<Texture2D>("Nam6");
            Nam6f = Content.Load<Texture2D>("Nam6f");
            Nam7 = Content.Load<Texture2D>("Nam7");
            Nam7f = Content.Load<Texture2D>("Nam7f");

            M60 = Content.Load<SoundEffect>("FrFinished");
            M60Instance = M60.CreateInstance();
            jungle = Content.Load<SoundEffect>("jungle19sW");
            Flyover = Content.Load<SoundEffect>("heuyflyOverWav");

            StarwarsSong = Content.Load<SoundEffect>("StarwarsSong");
            TieFighterEx = Content.Load<SoundEffect>("TIEfighterexplode");
            TieFighterFlyBy = Content.Load<SoundEffect>("TIEfighterflyby1");
            TieFighterFlyBy4 = Content.Load<SoundEffect>("TIEfighterflyby4");
            Xwingfire = Content.Load<SoundEffect>("XWingfire");
            Xwingfire3 = Content.Load<SoundEffect>("XWingfire3");

            Empire1 = Content.Load<Texture2D>("Empire's Arrival");
            CliffSpace = Content.Load<Texture2D>("CliffSpace");
            EarthComet = Content.Load<Texture2D>("EarthComet");
            Galexy1 = Content.Load<Texture2D>("Galexy1");
            Galexy2 = Content.Load<Texture2D>("Galexy2");
            MilkyWay = Content.Load<Texture2D>("MilkyWay");
            Sattelite = Content.Load<Texture2D>("Sattelite");
            Store = Content.Load<Texture2D>("Store");
            ShopS = Content.Load<Texture2D>("Shop");
            BreakoutScreen = Content.Load<Texture2D>("BreakoutScreen");
            BackTexture = Content.Load<Texture2D>("Backbuttons");
            BreakoutInt = Content.Load<Texture2D>("BreakoutInt");
            startButtonTexture = Content.Load<Texture2D>("StartButton");
            instructionsButtonTexture = Content.Load<Texture2D>("InstructionsButton");
            MainMenuTexture = Content.Load<Texture2D>("MainMenu");
            PlayAgainTexture = Content.Load<Texture2D>("PlayAgain");
            SatelliteCrashTexture = Content.Load<Texture2D>("SatelliteCrash");
            // TODO: use this.Content to load your game content here
        }
        void ProblemSolve()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            targets = new GameSpriteStruct[numberOfTargets];
            float targetSpacing = displayWidth / numberOfTargets;
            
            for (int i = 0; i < numberOfTargets; i++)
            {
                targets[i].SpriteTexture = curTargetTexture;
                setupSprite(ref targets[i], curTargetTexture, 0.05f, 0, i * targetSpacing, 0 + targets[i].SpriteRectangle.Height, true);
            }
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        void moveAndWrapRect(ref Rectangle rect, int width, bool right, int speed,bool backgroundSound)
        {
            if(backgroundSound == true)
            {


                //traveling right
                if (right)
                {
                    rect.X += speed;


                    if (curTargetTexture == Hind && (curPaddleTexture == Sixty1 || curPaddleTexture == Sixty2) && curBallTexture == Frag)
                    {
                        if (rect.X > width + (GraphicsDevice.Viewport.Width / 11 / 60) * 480)
                        {
                            rect.X = 0 - rect.Width;
                            //set sprite completely off screen to left 
                            Flyover.Play();
                            jungle.Play();


                        }

                    }
                    if (curTargetTexture == TIEFighter && curPaddleTexture == Falcon && curBallTexture == xWing)
                    {
                        if (rect.X > width + (GraphicsDevice.Viewport.Width / 1 / 60) * 1080)
                        {
                            rect.X = 0 - rect.Width;
                            //set sprite completely off screen to left 
                            TieFighterFlyBy.Play();
                            StarwarsSong.Play();


                        }
                    }
                    // and the sprite is completely off the screen to the right
                }                                        
            }
            else
            {
                if (right)
                {
                    rect.X += speed;


                    if (curTargetTexture == Hind && (curPaddleTexture == Sixty1 || curPaddleTexture == Sixty2) && curBallTexture == Frag)
                    {
                        if (rect.X > width + (GraphicsDevice.Viewport.Width / 11 / 60) * 480)
                        {
                            rect.X = 0 - rect.Width;
                            //set sprite completely off screen to left 
                            Flyover.Play();
                            


                        }

                    }
                    if (curTargetTexture == TIEFighter && curPaddleTexture == Falcon && curBallTexture == xWing)
                    {
                        if (rect.X > width + (GraphicsDevice.Viewport.Width / 1 / 60) * 1080)
                        {
                            rect.X = 0 - rect.Width;
                            //set sprite completely off screen to left 
                            TieFighterFlyBy.Play();
                            


                        }
                    }
                    // and the sprite is completely off the screen to the right
                }
            }
            
        }
        void HueyMovement()
        {
            moveAndWrapRect(ref HueyRec, 2000, true, GraphicsDevice.Viewport.Width / 11 / 60,true);
            
        }
        void XwingMovement()
        {
            moveAndWrapRect(ref ZoomRec, 2000, true, GraphicsDevice.Viewport.Width / 1 / 60,true);
            moveAndWrapRect(ref ZoomRec2, 2000, true, GraphicsDevice.Viewport.Width / 1 / 60, false);
            moveAndWrapRect(ref ZoomRec3, 2000, true, GraphicsDevice.Viewport.Width / 1 / 60, false);
        }
      
        void HueySetup()
        {

            if (curTargetTexture == Hind && (curPaddleTexture == Sixty1 || curPaddleTexture == Sixty2) && curBallTexture == Frag)
            {

                jungle.Play();
                
                

                Flyover.Play();
                
            }
            if(curTargetTexture == TIEFighter && curPaddleTexture == Falcon && curBallTexture == xWing)
            {
                StarwarsSong.Play();
                TieFighterFlyBy4.Play();
            }
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
             
            // TODO: Add your update logic here
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboard = Keyboard.GetState();
            mouseState = Mouse.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            pointDisplay();
            switch (curGameState)
            {
                case GameStates.Start:
                    WelcomeScreen();
                    Shop();
                    IsMouseVisible = true;
                    break;
                case GameStates.Game:
                    if (curTargetTexture == Hind && (curPaddleTexture == Sixty1 || curPaddleTexture == Sixty2) && curBallTexture == Frag)
                        HueyMovement();
                    if (curTargetTexture == TIEFighter && curPaddleTexture == Falcon && curBallTexture == xWing)
                        XwingMovement();
                    UpdateHarris();
                    HarrisUpdate2();
                    if (Lives <= 0)
                    {
                        curGameState = GameStates.SataliteCrash;
                        ProblemSolve();
                        if (Score > HighScore)
                        {
                            HighScore = Score;
                            Points += 200;
                        }
                        Points += Score;
                    }
                    UpdatePaddle();
                    UpdateTarget();
                    
                    


                    break;
                case GameStates.Instructions:
                    IsMouseVisible = true;
                    Instructions();
                    break;
                case GameStates.SataliteCrash:
                    IsMouseVisible = true;
                    SataliteDown();
                    MediaPlayer.IsMuted=true;
                    Shop();
                    break;
                case GameStates.Shop:
                    IsMouseVisible = true;
                    Back();
                    switch (curShopTab)
                    {
                        case ShopTabs.Targets:
                            TabNavigation(ref Targets, 1);
                            TabNavigation(ref Paddles, 2);
                            TabNavigation(ref Balls, 3);
                            ShopUpdateButtons(ref FishEye, 1,FishEye1);
                            ShopUpdateButtons(ref Hydra , 1, Hind );
                            ShopUpdateButtons(ref Astroids, 1, AstroidS);
                            ShopUpdateButtons(ref TIEFIGHT, 1, TIEFighter);
                            break;

                        case ShopTabs.Paddles:
                            TabNavigation(ref Targets, 1);
                            TabNavigation(ref Paddles, 2);
                            TabNavigation(ref Balls, 3);
                            ShopUpdateButtons(ref Board, 2, BoardS);
                            ShopUpdateButtons(ref Sixty, 2, Sixty1);
                            ShopUpdateButtons(ref SpaceShuttle , 2, SpaceShuttleS);
                            ShopUpdateButtons(ref FALCON, 2, Falcon);
                            break;

                        case ShopTabs.Balls:
                            TabNavigation(ref Targets, 1);
                            TabNavigation(ref Paddles, 2);
                            TabNavigation(ref Balls, 3);
                            ShopUpdateButtons(ref SkaterDude, 3, SkaterDudeS);
                            ShopUpdateButtons(ref frag, 3, Frag);
                            ShopUpdateButtons(ref Satellite, 3, SatelliteS);
                            ShopUpdateButtons(ref XWING, 3, xWing);
                            break;
                    }

                    break;


            }
            base.Update(gameTime);
        }
        private void WelcomeScreen()
        {
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboard = Keyboard.GetState();
            mouseState = Mouse.GetState();
            if (instructionsButtonRect.Contains(new Point(mouseState.X, mouseState.Y)) || keyboard.IsKeyDown(Keys.Left) || pad1.DPad.Left == ButtonState.Pressed)
            {
                instructionsColor = Color.Yellow;
                if (mouseState.LeftButton == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Enter) || pad1.Buttons.A == ButtonState.Pressed)
                {
                    instructionsColor = Color.Red;
                    curGameState = GameStates.Instructions;
                    IsMouseVisible = true;
                }
            }
            else
            {
                instructionsColor = Color.White;
            }

            if (startButtonRect.Contains(new Point(mouseState.X, mouseState.Y)) || keyboard.IsKeyDown(Keys.Right) || pad1.DPad.Right == ButtonState.Pressed)
            {
                startColor = Color.Yellow;
                if (mouseState.LeftButton == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Enter) || pad1.Buttons.A == ButtonState.Pressed)
                {
                    RandomBackGround();
                    UpdateTarget();
                    HueySetup();
                    startColor = Color.Red;
                    curGameState = GameStates.Game;
                    IsMouseVisible = false;


                }
            }
            else
            {
                startColor = Color.White;
            }

        }

        void UpdateHarris()
        {
            //harris movement
            
            Harris.X = Harris.X + Harris.XSpeed;
            Harris.Y = Harris.Y + Harris.YSpeed;

            Harris.SpriteRectangle.X = (int)(Harris.X + 0.5f);
            Harris.SpriteRectangle.Y = (int)(Harris.Y + 0.5f);
            //keeps Harris where he belongs 
            if (Harris.SpriteRectangle.X <= 0 || Harris.SpriteRectangle.Width + Harris.X >= displayWidth)
            {
                if (Harris.X <= minDisplayX)
                {
                    Harris.XSpeed = Math.Abs(Harris.XSpeed);
                }
                else if (Harris.SpriteRectangle.Width + Harris.X >= displayWidth)
                {
                    Harris.XSpeed = Math.Abs(Harris.XSpeed) * -1;
                }

            }
            else if (Harris.SpriteRectangle.Y <= 0 || Harris.SpriteRectangle.Height + Harris.Y >= displayHeight)
            {
                if (Harris.Y <= minDisplayY)
                {
                    Harris.YSpeed = Math.Abs(Harris.YSpeed);
                }
                else if (Harris.SpriteRectangle.Height + Harris.Y >= displayHeight)
                {
                    Harris.YSpeed = Math.Abs(Harris.YSpeed) * -1;
                    if (Lives > 0)
                    {
                        Lives--;
                    }
                }
            }
            //Makes Harris bounce off paddle

        }
        void HarrisUpdate2()
        {
            inCollision = false;
            if (Harris.SpriteRectangle.Intersects(Paddle.SpriteRectangle) && !inCollision)
            {
                inCollision = true;
                if (curPaddleTexture == Sixty1)
                {
                    Harris.YSpeed *= -1;
                    curPaddleTexture = Sixty2;
                    M60Instance.Play();
                    
                    if (Harris.Y < Paddle.Y)
                    {
                        if (Harris.Y < Paddle.Y)
                        {
                            Harris.Y = Paddle.Y - Harris.SpriteRectangle.Height;
                        }
                    }
                    else if (Harris.Y > Paddle.Y + Paddle.SpriteRectangle.Height)
                    {
                        if (Harris.Y > Paddle.Y + Paddle.SpriteRectangle.Height)
                        {
                            Harris.Y = Paddle.Y + Harris.SpriteRectangle.Height;
                        }
                    }
                }
                else
                {
                    Harris.YSpeed *= -1;
                    if (Harris.Y < Paddle.Y)
                    {
                        if (Harris.Y < Paddle.Y)
                        {
                            Harris.Y = Paddle.Y - Harris.SpriteRectangle.Height;
                        }
                    }
                    else if (Harris.Y > Paddle.Y + Paddle.SpriteRectangle.Height)
                    {
                        if (Harris.Y > Paddle.Y + Paddle.SpriteRectangle.Height)
                        {
                            Harris.Y = Paddle.Y + Harris.SpriteRectangle.Height;
                        }
                    }
                }
            }
            else
            {
                if (M60Instance.State == SoundState.Stopped && curPaddleTexture == Sixty2)
                {
                    curPaddleTexture = Sixty1;
                }

                inCollision = false;

            }




                //prevents harris from getting stuck on the paddle
                
        }
        private void UpdateTarget()
        {
            // sets everything up for targets 
            bool noTargets = true;
            for (int i = 0; i < numberOfTargets; i++)
            {
                targets[i].SpriteRectangle.X = (int)targets[i].X;
                targets[i].SpriteRectangle.Y = (int)targets[i].Y;
                
                if (targets[i].Visible)
                {
                    noTargets = false;
                    if (Harris.SpriteRectangle.Intersects(targets[i].SpriteRectangle))
                    {
                        targets[i].Visible = false;
                        Harris.YSpeed = Harris.YSpeed * -1;
                        Score += 10;
                        
                        break;

                    }
                    
                }
                
            }
            //makes the targets move doen screen when all eliminated
            if (noTargets)
            {
                resetTargetDisplay();
                Harris.Y = Paddle.Y - Harris.SpriteRectangle.Height;
                
                Harris.X = Paddle.X + Harris.SpriteRectangle.Width / 2 - Paddle.SpriteRectangle.Width / 2;


            }
        }
        void pointDisplay()
        {
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboard = Keyboard.GetState();
            
            if (keyboard.IsKeyDown(Keys.LeftControl) || pad1.Buttons.X == ButtonState.Pressed)
            {
                PointsVisible = true; 
            }
            else
            {
                PointsVisible = false;
            }
        }
        void UpdatePaddle()
        {
            GamePadState gamePad1 = GamePad.GetState(PlayerIndex.One);
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboard = Keyboard.GetState();
            //establsihes game to movement with left thumbstick for paddle 
            Paddle.X = Paddle.X +
            (Paddle.XSpeed * pad1.ThumbSticks.Left.X);
            Paddle.Y = Paddle.Y -
                (Paddle.YSpeed * gamePad1.ThumbSticks.Left.Y);
            //establishes AUTO PILOT 
            if (keyboard.IsKeyDown(Keys.RightControl) || pad1.Buttons.A == ButtonState.Pressed)
            {
                Paddle.X = Harris.X - Paddle.SpriteRectangle.Width / 2 + Harris.SpriteRectangle.Width / 2;
            }
            //keeps paddle in the screen
            if (Paddle.X < minDisplayX)
            {
                Paddle.X = minDisplayX;
            }
            else if (Paddle.X > displayWidth - Paddle.SpriteRectangle.Width)
            {
                Paddle.X = displayWidth - Paddle.SpriteRectangle.Width;
            }
            else if (Paddle.Y < displayHeight - Paddle.SpriteRectangle.Height * 5)
            {
                Paddle.Y = displayHeight - Paddle.SpriteRectangle.Height * 5;
            }
            else if (Paddle.Y > displayHeight - Paddle.SpriteRectangle.Height)
            {
                Paddle.Y = displayHeight - Paddle.SpriteRectangle.Height;
            }
            Paddle.SpriteRectangle.X = (int)Paddle.X;
            Paddle.SpriteRectangle.Y = (int)Paddle.Y;
        }
        void Back()
        {
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboard = Keyboard.GetState();
            mouseState = Mouse.GetState();
            if (BackRec.Contains(new Point(mouseState.X, mouseState.Y)) || keyboard.IsKeyDown(Keys.Right) || pad1.DPad.Right == ButtonState.Pressed)
            {
                BackColor = Color.Yellow;
                if (mouseState.LeftButton == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Enter) || pad1.Buttons.A == ButtonState.Pressed)
                {
                    BackColor = Color.Red;
                    curGameState = GameStates.Start;
                    IsMouseVisible = true;
                }
            }
            else
            {
                BackColor = Color.White;
            }
        }
        private void Instructions()
        {
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboard = Keyboard.GetState();
            mouseState = Mouse.GetState();
            if (BackRec.Contains(new Point(mouseState.X, mouseState.Y)) || keyboard.IsKeyDown(Keys.Right) || pad1.DPad.Right == ButtonState.Pressed)
            {
                BackColor = Color.Yellow;
                if (mouseState.LeftButton == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Enter) || pad1.Buttons.A == ButtonState.Pressed)
                {
                    BackColor = Color.Red;
                    curGameState = GameStates.Start;
                    IsMouseVisible = true;
                }
            }
            else
            {
                BackColor = Color.White;
            }

        }
        /// <summary>
        void drawText(string text, Color textColor, float x, float y)
        {
            Vector2 nowVector = new Vector2(x, y);
            //establishes shadow
            Color drawColor = new Color(0, 0, 0, 10);
            //shadow
            for (int Layer = 0; Layer < 25; Layer++)
            {
                spriteBatch.DrawString(font, text, nowVector, drawColor);
                nowVector.X = nowVector.X + 1;
                nowVector.Y = nowVector.Y + 1;
            }
            //extruded text
            for (int Layer = 0; Layer < 10; Layer++)
            {
                spriteBatch.DrawString(font, text, nowVector, Color.LightGray);
                nowVector.X = nowVector.X + 1;
                nowVector.Y = nowVector.Y + 1;
            }
            //the first layer of text
            spriteBatch.DrawString(font, text, nowVector, textColor);
        }
        void SataliteDown()
        {
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboard = Keyboard.GetState();
            mouseState = Mouse.GetState();
            if (MainMenuRec.Contains(new Point(mouseState.X, mouseState.Y)) || keyboard.IsKeyDown(Keys.Left) || pad1.DPad.Left == ButtonState.Pressed)
            {
                MainMenuColor = Color.Yellow;
                if (mouseState.LeftButton == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Enter) || pad1.Buttons.A == ButtonState.Pressed)
                {
                    UpdateVariables();                    
                    MainMenuColor = Color.Red;
                    curGameState = GameStates.Start;
                    IsMouseVisible = true;
                }
            }
            else
            {
                MainMenuColor = Color.White;
            }

            if (PlayAgainRec.Contains(new Point(mouseState.X, mouseState.Y)) || keyboard.IsKeyDown(Keys.Right) || pad1.DPad.Right == ButtonState.Pressed)
            {
                PlayAgainColor = Color.Yellow;
                if (mouseState.LeftButton == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Enter))
                {
                    RandomBackGround();
                    UpdateVariables();
                    
                    PlayAgainColor = Color.Red;
                    curGameState = GameStates.Game;
                    IsMouseVisible = false;


                }
            }
            else
            {
                PlayAgainColor = Color.White;
            }

        }
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            
            if (curGameState == GameStates.Start)//sets up starting screen
            {
                spriteBatch.Draw(BreakoutScreen, backgroundRect, Color.White);
                spriteBatch.Draw(startButtonTexture, startButtonRect, startColor);
                spriteBatch.Draw(instructionsButtonTexture, instructionsButtonRect, instructionsColor);
                spriteBatch.Draw(ShopS, ShopRec, ShopColor);
            }
            else if (curGameState == GameStates.Instructions)//shows instructions
            {
                spriteBatch.Draw(BreakoutInt, instructionsRec, Color.White);
                spriteBatch.Draw(BackTexture, BackRec, BackColor);
            }

            else if (curGameState == GameStates.Game)//draws game
            {
                spriteBatch.Draw(RandomBackTexture, backgroundRect, Color.White);
                if (curTargetTexture == Hind && (curPaddleTexture == Sixty1 || curPaddleTexture == Sixty2) && curBallTexture == Frag)
                {
                    spriteBatch.Draw(Huey, HueyRec, Color.White);
                    spriteBatch.Draw(frontNam, backgroundRect, Color.White);
                }
                else if (curTargetTexture == TIEFighter && curPaddleTexture == Falcon && curBallTexture == xWing)
                {
                    spriteBatch.Draw(TIEfighterZoom, ZoomRec2, Color.White);
                    spriteBatch.Draw(TIEfighterZoom2, ZoomRec, Color.White);
                    spriteBatch.Draw(TIEfighterZoom3, ZoomRec3, Color.White);
                }
                    

                spriteBatch.Draw(curPaddleTexture, Paddle.SpriteRectangle, Color.White);
                spriteBatch.Draw(curBallTexture, Harris.SpriteRectangle, Color.White);
                //Score
                drawText("Score : " + Score.ToString() +" Lives : " + Lives.ToString(),Color.White,50,1300);
                for (int i = 0; i < numberOfTargets; i++)
                {
                    if (targets[i].Visible)
                    {
                        spriteBatch.Draw(curTargetTexture, targets[i].SpriteRectangle, Color.White);
                    }
                }
                
            }
            else if (curGameState == GameStates.SataliteCrash)
            {
                spriteBatch.Draw(SatelliteCrashTexture, backgroundRect, Color.White);
                spriteBatch.Draw(PlayAgainTexture, PlayAgainRec, PlayAgainColor);
                spriteBatch.Draw(MainMenuTexture, MainMenuRec, MainMenuColor);
                spriteBatch.Draw(ShopS, ShopRec, ShopColor);
                if (Score >= HighScore)
                {
                    drawText("NEW HIGH SCORE", Color.Gold, 750, 650);
                    drawText(HighScore.ToString(), Color.Gold, 750, 725);
                }
                else
                {
                    drawText("SCORE "+Score, Color.White, 750, 650);
                    drawText("High Score "+HighScore.ToString(), Color.White, 750, 725);
                }

            }
            else if (curGameState == GameStates.Shop)
            {
                spriteBatch.Draw(Store, backgroundRect, Color.White);
                spriteBatch.Draw(BackTexture, BackRec, BackColor);
                if (curShopTab == ShopTabs.Targets)
                {
                    spriteBatch.Draw(Targets.ButtonTexture, Targets.ButtonRectangle, Targets.ButtonColor);
                    spriteBatch.Draw(Paddles.ButtonTexture, Paddles.ButtonRectangle, Paddles.ButtonColor);
                    spriteBatch.Draw(Balls.ButtonTexture, Balls.ButtonRectangle, Balls.ButtonColor);
                    spriteBatch.Draw(FishEye.ButtonTexture, FishEye.ButtonRectangle, FishEye.ButtonColor);
                    spriteBatch.Draw(FishEye1, TopImageRec, Color.White);
                    drawText("Cost" + FishEye.Cost, Color.White, 1250, 500);
                    spriteBatch.Draw(Hydra.ButtonTexture, Hydra.ButtonRectangle, Hydra.ButtonColor);
                    spriteBatch.Draw(Hind, MidImageRec, Color.White);
                    drawText("Cost" + Hydra.Cost, Color.White, 1250, 750);
                    spriteBatch.Draw(Astroids.ButtonTexture, Astroids.ButtonRectangle, Astroids.ButtonColor);
                    spriteBatch.Draw(AstroidS, BottomImageRec, Color.White);
                    drawText("Cost" + Astroids.Cost, Color.White, 1250, 1000);
                    spriteBatch.Draw(TIEFIGHT.ButtonTexture, TIEFIGHT.ButtonRectangle, TIEFIGHT.ButtonColor);
                    spriteBatch.Draw(TIEFighter , BBottomImageRec, Color.White);
                    drawText("Cost" + TIEFIGHT.Cost, Color.White, 1250, 1250);
                }
                else if(curShopTab == ShopTabs.Paddles)
                {
                    spriteBatch.Draw(Targets.ButtonTexture, Targets.ButtonRectangle, Targets.ButtonColor);
                    spriteBatch.Draw(Paddles.ButtonTexture, Paddles.ButtonRectangle, Paddles.ButtonColor);
                    spriteBatch.Draw(Balls.ButtonTexture, Balls.ButtonRectangle, Balls.ButtonColor);
                    spriteBatch.Draw(Board.ButtonTexture, Board.ButtonRectangle, Board.ButtonColor);
                    spriteBatch.Draw(BoardS, TopImageRec, Color.White);
                    drawText("Cost" + Board.Cost, Color.White, 1250, 500);
                    spriteBatch.Draw(Sixty.ButtonTexture, Sixty.ButtonRectangle, Sixty.ButtonColor);
                    spriteBatch.Draw(Sixty1, MidImageRec, Color.White);
                    drawText("Cost" + Sixty.Cost, Color.White, 1250, 750);
                    spriteBatch.Draw(SpaceShuttle.ButtonTexture, SpaceShuttle.ButtonRectangle, SpaceShuttle.ButtonColor);
                    spriteBatch.Draw(SpaceShuttleS, BottomImageRec, Color.White);
                    drawText("Cost" + SpaceShuttle.Cost, Color.White, 1250, 1000);
                    spriteBatch.Draw(FALCON.ButtonTexture, FALCON.ButtonRectangle, FALCON.ButtonColor);
                    spriteBatch.Draw(Falcon, BBottomImageRec, Color.White);
                    drawText("Cost" + FALCON.Cost, Color.White, 1250, 1250);
                }
                else if(curShopTab == ShopTabs.Balls)
                {
                    spriteBatch.Draw(Targets.ButtonTexture, Targets.ButtonRectangle, Targets.ButtonColor);
                    spriteBatch.Draw(Paddles.ButtonTexture, Paddles.ButtonRectangle, Paddles.ButtonColor);
                    spriteBatch.Draw(Balls.ButtonTexture, Balls.ButtonRectangle, Balls.ButtonColor);
                    spriteBatch.Draw(SkaterDude.ButtonTexture, SkaterDude.ButtonRectangle, SkaterDude.ButtonColor);
                    spriteBatch.Draw(SkaterDudeS, TopImageRec, Color.White);
                    drawText("Cost" + SkaterDude.Cost, Color.White, 1250, 500);
                    spriteBatch.Draw(frag.ButtonTexture, frag.ButtonRectangle, frag.ButtonColor);
                    spriteBatch.Draw(Frag, MidImageRec, Color.White);
                    drawText("Cost" + frag.Cost, Color.White, 1250, 750);
                    spriteBatch.Draw(Satellite.ButtonTexture, Satellite.ButtonRectangle, Satellite.ButtonColor);
                    spriteBatch.Draw(SatelliteS, BottomImageRec, Color.White);
                    drawText("Cost" + Satellite.Cost, Color.White, 1250, 1000);
                    spriteBatch.Draw(XWING.ButtonTexture, XWING.ButtonRectangle, XWING.ButtonColor);
                    spriteBatch.Draw(xWing, BBottomImageRec, Color.White);
                    drawText("Cost" + XWING.Cost, Color.White, 1250, 1250);
                }
            }
                if (PointsVisible == true)
            {
                drawText("Points : " + Points, Color.White, 1500, 1300);
            }
            spriteBatch.End();
        base.Draw(gameTime);
        }
    }
}
