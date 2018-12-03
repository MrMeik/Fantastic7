using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Timers;
using System.IO;

namespace Fantastic7
{
    public enum GameState
    {
        mainMenu,
        running,
        paused,
        shop,
        death,
        leaderboard
    };

    public class SpriteBatchPlus : SpriteBatch
    {
        private Texture2D _defaultTexture;
        public SpriteBatchPlus(GraphicsDevice graphicsDevice) : base(graphicsDevice){}
        public void setDefaultTexture(Texture2D defaultTexture) { _defaultTexture = defaultTexture; }
        public Texture2D defaultTexture() { return _defaultTexture; }
    }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatchPlus spriteBatch;
        public static GameState gs;
        Room rm;
        //Texture2D plainText;
        Map currMap;
        const int WIDTH = 1280;
        const int HEIGHT = 720;
        SpriteFont mfont;
        SpriteFont sfont;
        SpriteFont guiFont;
        SSprite deathMenuScore;
        GGUI mainMenu;
        GGUI pauseMenu;
        GGUI shopMenu;
        GGUI deathMenu;
        GGUI scoreMenu;
        SSprite[] highscoresSprites;
        SSprite[] achievementSprites;
        int[] loadedHighscores;
        public static int[] loadedAchievements;
        bool updateScoreMenu = false;
        MenuControls MenuControls;
        PlayControls PlayControls;
        EventHandler EventHandler;
        SpriteLoader spriteLoader;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = HEIGHT;
            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.IsFullScreen = false;
            graphics.PreferMultiSampling = false;
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
            // TODO: Add your initialization logic here
            gs = GameState.mainMenu;
            spriteLoader = new SpriteLoader(this);
            Components.Add(spriteLoader);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatchPlus(GraphicsDevice);
            Texture2D plainText = new Texture2D(GraphicsDevice, 1, 1);
            plainText.SetData(new[] { Color.White });
            spriteBatch.setDefaultTexture(plainText);
            //currentTime = 0;
            //goalTime = 35;
            Keys[] MenuControlList = { Keys.Escape, Keys.Enter, Keys.W, Keys.S, Keys.Up, Keys.Down };
            MenuControls = new MenuControls(MenuControlList);

            Keys[] playControlList = { Keys.Escape, Keys.W, Keys.A, Keys.S, Keys.D, Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.Space };
            PlayControls = new PlayControls(playControlList);

         



            //Creates Test Room
            /*GSprite[] roomSprites = { new NSprite(new Rectangle(0, 0, WIDTH, HEIGHT), Color.Gray),
                new NSprite(new Rectangle(100, 100, WIDTH - 200, HEIGHT - 200), Color.LightGray)};
            rm = new Room(roomSprites);

            //rm.addObject(new Entity(new Vector2(500, 500), new NSprite(new Rectangle(500, 500, 50, 50), Color.Wheat)));*/


            //Imports Font
            mfont = Content.Load<SpriteFont>("main");
            sfont = Content.Load<SpriteFont>("second");
            guiFont = Content.Load<SpriteFont>("guiFont");
            int mHeight = (int)mfont.MeasureString("M").Y;
            int sHeight = (int)sfont.MeasureString("M").Y;


            //Score tracking system setup
            highscoresSprites = new SSprite[10];
            achievementSprites = new SSprite[3];

            loadedHighscores = new int[10];
            loadedAchievements = new int[3];

            for (int i = 0; i < 10; i++) loadedHighscores[i] = -1;
            for (int i = 0; i < 3; i++) loadedAchievements[i] = -2;

            string input = "-1";
            try
            {
                StreamReader sr = new StreamReader("sts.txt");
                for(int i = 0; i < 10; i++)
                {
                    input = sr.ReadLine();
                    int.TryParse(input, out loadedHighscores[i]);
                }

                for(int i = 0; i < 3; i++)
                {
                    input = sr.ReadLine();
                    int.TryParse(input, out loadedAchievements[i]);
                }
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            for (int i = 0; i < 10; i++) highscoresSprites[i] = new SSprite(i + 1 + ". " + loadedHighscores[i], sfont, new Vector2(50, mHeight * 2 + 10 + i * (sHeight + 10)), Color.Azure);
            achievementSprites[0] = new SSprite( "Levels Completed: " + loadedAchievements[0], sfont, new Vector2(400, mHeight * 2 + 10 + 0 * (sHeight + 10)), Color.Azure);
            achievementSprites[1] = new SSprite("Chargers Killed: " + loadedAchievements[1], sfont, new Vector2(400, mHeight * 2 + 10 + 1 * (sHeight + 10)), Color.Azure);
            achievementSprites[2] = new SSprite("Rangers Killed: " + loadedAchievements[2], sfont, new Vector2(400, mHeight * 2 + 10 + 2 * (sHeight + 10)), Color.Azure);






            //Creates Main Menu
            GSprite[] gs = { new NSprite(new Rectangle(0, 0, WIDTH, HEIGHT), Color.SandyBrown),
                new NSprite(new Rectangle(0, 0, WIDTH, mHeight * 2), Color.SaddleBrown),
                new SSprite("Maze Crawler", mfont, new Vector2(25,mHeight / 2), Color.Azure)};

            MenuOption[] mo = { new MenuOption(new SSprite("Start Game", sfont, new Vector2(50, mHeight * 2 + sHeight), Color.Azure)),
                new MenuOption(new SSprite("Highscores and Achievements", sfont, new Vector2(50, mHeight * 2 + sHeight * 3), Color.Azure)),
                new MenuOption(new SSprite("Quit Game", sfont, new Vector2(50,mHeight * 2 + sHeight * 5), Color.Azure))};

            mainMenu = new GGUI(gs, mo, Color.Azure);

            

            //Creates Pause Menu
            GSprite[] pgs = { new NSprite(new Rectangle(WIDTH / 4, HEIGHT / 8, WIDTH / 2, HEIGHT / 2), Color.SandyBrown),
                new NSprite(new Rectangle(WIDTH / 4, HEIGHT / 8, WIDTH / 2, mHeight * 2), Color.SaddleBrown),
                new SSprite("Pause Menu", mfont, new Vector2(WIDTH / 2 - mfont.MeasureString("Pause Menu").X / 2, HEIGHT / 8 + mHeight / 2), Color.Azure)};

            MenuOption[] pmo = { new MenuOption(new SSprite("Resume", sfont, new Vector2(WIDTH / 2 - mfont.MeasureString("Pause Menu").X / 4, HEIGHT / 8 + mHeight * 2 + sHeight/2), Color.Azure)),
                new MenuOption(new SSprite("Setting", sfont, new Vector2(WIDTH / 2 - mfont.MeasureString("Pause Menu").X / 4, HEIGHT / 8 + mHeight * 2 + sHeight * 2), Color.Azure)),
                new MenuOption(new SSprite("Quit", sfont, new Vector2(WIDTH / 2 - mfont.MeasureString("Pause Menu").X / 4, HEIGHT / 8 + mHeight * 2 + sHeight * 3.5f), Color.Azure))};

            pauseMenu = new GGUI(pgs, pmo, Color.Azure);

            //Creates Shop Menu
            GSprite[] sgs = { new NSprite(new Rectangle(WIDTH / 4, HEIGHT / 8, WIDTH / 2, HEIGHT *2 / 3 ), Color.SandyBrown),
                new NSprite(new Rectangle(WIDTH / 4, HEIGHT / 8, WIDTH / 2, mHeight * 2), Color.SaddleBrown),
                new SSprite("Ye Old Shope", mfont, new Vector2(WIDTH / 2 - mfont.MeasureString("Ye Old Shoppe").X / 2, HEIGHT / 8 + mHeight / 2), Color.Azure)};

            MenuOption[] smo = { new MenuOption(new SSprite("Weapon Damage (10C)", sfont, new Vector2(WIDTH / 2 - mfont.MeasureString("Ye Old Shope").X / 4, HEIGHT / 8 + mHeight * 2 + sHeight/2), Color.Azure)),
                new MenuOption(new SSprite("Max Health (10C)", sfont, new Vector2(WIDTH / 2 - mfont.MeasureString("Ye Old Shope").X / 4, HEIGHT / 8 + mHeight * 2 + sHeight * 2), Color.Azure)),
                new MenuOption(new SSprite("Quit", sfont, new Vector2(WIDTH / 2 - mfont.MeasureString("Ye Old Shope").X / 4, HEIGHT / 8 + mHeight * 2 + sHeight * 3.5f), Color.Azure))};


            shopMenu = new GGUI(sgs, smo, Color.Azure);

            //Creates Death Menu

            deathMenuScore = new SSprite("Your Score: ", sfont, new Vector2(WIDTH / 2 - mfont.MeasureString("You Died").X / 4, HEIGHT / 8 + mHeight * 2 + sHeight * 3.5f), Color.Azure);

            GSprite[] dgs = { new NSprite(new Rectangle(WIDTH / 4, HEIGHT / 8, WIDTH / 2, HEIGHT / 2), Color.SandyBrown),
                new NSprite(new Rectangle(WIDTH / 4, HEIGHT / 8, WIDTH / 2, mHeight * 2), Color.SaddleBrown),
                new SSprite("You Died", mfont, new Vector2(WIDTH / 2 - mfont.MeasureString("You Died").X / 2, HEIGHT / 8 + mHeight / 2), Color.Azure),
                deathMenuScore};

            MenuOption[] dmo = { new MenuOption(new SSprite("Main Menu", sfont, new Vector2(WIDTH / 2 - mfont.MeasureString("You Died").X / 4, HEIGHT / 8 + mHeight * 2 + sHeight/2), Color.Azure)),
                new MenuOption(new SSprite("Quit", sfont, new Vector2(WIDTH / 2 - mfont.MeasureString("You Died").X / 4, HEIGHT / 8 + mHeight * 2 + sHeight * 2), Color.Azure))};

            deathMenu = new GGUI(dgs, dmo, Color.Azure);

            //Creates Leaderboard plus AH system

            GSprite[] scoregs = new GSprite[16];
            scoregs[0] = new NSprite(new Rectangle(0, 0, WIDTH, HEIGHT), Color.SandyBrown);
            scoregs[1] = new NSprite(new Rectangle(0, 0, WIDTH, mHeight * 2), Color.SaddleBrown);
            scoregs[2] = new SSprite("Highscores and Acheivements", mfont, new Vector2(25,mHeight / 2), Color.Azure);
            for (int i = 0; i < 10; i++) scoregs[3 + i] = highscoresSprites[i];
            for (int i = 0; i < 3; i++) scoregs[13 + i] = achievementSprites[i];

            MenuOption[] scoremo = { new MenuOption(new SSprite("Return" , sfont, new Vector2(50,HEIGHT - sHeight*2), Color.Azure))};

            scoreMenu = new GGUI(scoregs, scoremo, Color.Azure);


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected void newGame()
        {
            gs = GameState.running;
            currMap = new Map(guiFont);
            currMap.GenerateMap();
            EventHandler = new EventHandler(currMap);
        }

        public void updateScore(int score)
        {
            for(int i = 9; i >= 0; i--)
            {
                if(score >= loadedHighscores[i])
                {
                    for (int j = 9; j > i; j--)
                    {
                        loadedHighscores[j] = loadedHighscores[j - 1];
                    }
                    loadedHighscores[i] = score;
                    break;
                }
            }

            for (int i = 0; i < 10; i++) highscoresSprites[i].setText(i + 1 + ". " + loadedHighscores[i]);

            achievementSprites[0].setText("Levels Completed: " + loadedAchievements[0]);
            achievementSprites[1].setText("Chargers Killed: " + loadedAchievements[1]);
            achievementSprites[2].setText("Rangers Killed: " + loadedAchievements[2]);
            
        }

        public void gameExit()
        {
            try
            {
                StreamWriter sr = new StreamWriter("sts.txt", false);

                updateScore(0);

                for (int i = 0; i < 10; i++) sr.WriteLine(loadedHighscores[i]);
                for (int i = 0; i < 3; i++) sr.WriteLine(loadedAchievements[i]);
                sr.Close();

            }
            catch(Exception e)
            {

            }
            Exit();
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            

            switch (gs)
            {
                case GameState.mainMenu:


                    MenuControls.update(gameTime);
                    

                    if (MenuControls.getExit()) gameExit();
                    if (MenuControls.getSelect())
                    {
                        switch (mainMenu.getIndex())
                        {
                            case 0:
                                newGame();
                                break;
                            case 1:
                                gs = GameState.leaderboard;
                                break;
                            case 2:
                                gameExit();
                                break;
                        }
                    }
                    if (MenuControls.getNextKey()) mainMenu.nextOption();
                    if (MenuControls.getPrevKey()) mainMenu.previousOption();
                        
                        
                    break;
                case GameState.leaderboard:

                    MenuControls.update(gameTime);

                    if (MenuControls.getExit())
                    {
                        gs = GameState.mainMenu;
                        updateScoreMenu = false;
                    }
                    if (MenuControls.getSelect())
                    {
                        gs = GameState.mainMenu;
                        updateScoreMenu = false;
                    }

                    if (!updateScoreMenu)
                    {
                        updateScoreMenu = true;

                    }

                    break;
                case GameState.running:

                    if(updateScoreMenu) { updateScoreMenu = false; }
                    PlayControls.update(gameTime);

                    //Poll inputs

                    if (PlayControls.getPause()) gs = GameState.paused;

                    Vector2 playerMovement = new Vector2(0, 0);
                    if (PlayControls.getMoveDown())
                    {
                        playerMovement.Y += currMap.player.movementSpeed;
                        currMap.player.direction = CollisionHandler.Direction.South;
                    }
                    if (PlayControls.getMoveUp())
                    {
                        playerMovement.Y -= currMap.player.movementSpeed;
                        currMap.player.direction = CollisionHandler.Direction.North;
                    }
                    if (PlayControls.getMoveRight())
                    {
                        playerMovement.X += currMap.player.movementSpeed;
                        currMap.player.direction = CollisionHandler.Direction.East;
                    }
                    if (PlayControls.getMoveLeft())
                    {
                        playerMovement.X -= currMap.player.movementSpeed;
                        currMap.player.direction = CollisionHandler.Direction.West;
                    }
                    if (PlayControls.getShootKey())
                    {
                        currMap.player._mainweapon.IsUsing = true;
                    }
                    else currMap.player._mainweapon.IsUsing = false;
                    if (playerMovement.X != 0 || playerMovement.Y != 0)
                    {                      
                        currMap.player.move(playerMovement * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    }


                    //End inputs

                    EventHandler.handle(gameTime);
                    currMap.update(gameTime);
                    
                    break;
                case GameState.paused:


                    MenuControls.update(gameTime);

                    //Poll inputs

                    if (MenuControls.getNextKey()) pauseMenu.nextOption();
                    if (MenuControls.getPrevKey()) pauseMenu.previousOption();
                    if (MenuControls.getSelect()) gs = GameState.mainMenu;
                    if (MenuControls.getExit()) gs = GameState.running;
                    //End inputs
                    break;

                case GameState.shop:
                    MenuControls.update(gameTime);

                    //Poll inputs

                    if (MenuControls.getNextKey()) shopMenu.nextOption();
                    if (MenuControls.getPrevKey()) shopMenu.previousOption();
                    if (MenuControls.getSelect())
                    {
                        switch (shopMenu.getIndex())
                        {
                            case 0:
                                if (currMap.hud.Currency >= 10)
                                {
                                    currMap.player._mainweapon._Damage += 5;
                                    currMap.hud.Currency -= 10;
                                }
                                break;

                            case 1:
                                if (currMap.hud.Currency >= 10)
                                {
                                    currMap.player.increaseMax(5);
                                    currMap.hud.Currency -= 10;
                                }
                                break;

                            case 2:
                                gs = GameState.running;
                                break;
                        }
                    }
                    if (MenuControls.getExit()) gs = GameState.running;
                    //End inputs

                    currMap.hud.update(gameTime);

                    break;

                case GameState.death:
                    MenuControls.update(gameTime);

                    if (!updateScoreMenu)
                    {
                        updateScore(currMap.hud.Score);
                        updateScoreMenu = true;
                    }
                    deathMenuScore.setText("Your Score: " + currMap.hud.Score);

                    //Poll inputs

                    if (MenuControls.getNextKey()) deathMenu.nextOption();
                    if (MenuControls.getPrevKey()) deathMenu.previousOption();
                    if (MenuControls.getSelect())
                    {
                        switch (deathMenu.getIndex())
                        {
                            case 0:
                                gs = GameState.mainMenu;
                                break;

                            case 1:
                                gameExit();
                                break;
                        }
                    }
                    if (MenuControls.getExit()) gs = GameState.mainMenu;
                    break;
                default: break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            //Draws objects depending on the state of the game
            switch (gs)
            {
                case GameState.mainMenu:
                    mainMenu.draw(spriteBatch,1);
                    break;
                case GameState.leaderboard:
                    scoreMenu.draw(spriteBatch, 1);
                    break;
                case GameState.paused:
                    pauseMenu.draw(spriteBatch, 1);
                    break;
                case GameState.running:
                    currMap.draw(spriteBatch, 1);
                    break;
                case GameState.shop:
                    currMap.draw(spriteBatch, 1);
                    shopMenu.draw(spriteBatch, 1);
                    break;
                case GameState.death:
                    currMap.draw(spriteBatch, 1);
                    deathMenu.draw(spriteBatch, 1);
                    break;
                default: break;
            }

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
