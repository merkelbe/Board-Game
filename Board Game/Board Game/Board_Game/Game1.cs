using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BoardGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        KeyFilter keyFilter;
        List<Keys> pressedKeys;
        MouseFilter mouseFilter;

        KeyboardState keyboardState;
        MouseState mouseState; 

        int WINDOW_WIDTH = 1200;
        int WINDOW_HEIGHT = 700;

        Sprite grassHexSprite;
        Sprite desertHexSprite;
        Sprite waterHexSprite;
        Sprite deepWaterHexSprite;
        Sprite mountainHexSprite;
        Sprite woodHexSprite;
        Sprite cityHexSprite;
        Sprite selectedHexSprite;

        GameBoard gameBoard;
        List<ClickInfo> currentClickInputs;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Set resolution and make mouse visible
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;

            gameBoard = new GameBoard(GameBoard.BoardSpaceOffsetType.ToggleWithUpStart, 50,26, 418, 362);
            currentClickInputs = new List<ClickInfo>();

            keyFilter = new KeyFilter();
            mouseFilter = new MouseFilter();
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
            this.IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            grassHexSprite = new Sprite(Content, "grassHex");
            desertHexSprite = new Sprite(Content, "desertHex");
            waterHexSprite = new Sprite(Content, "waterHex");
            deepWaterHexSprite = new Sprite(Content, "deepWaterHex");
            mountainHexSprite = new Sprite(Content, "mountainHex");
            woodHexSprite = new Sprite(Content, "woodHex");
            cityHexSprite = new Sprite(Content, "cityHex");
            selectedHexSprite = new Sprite(Content, "selectedHex");

            SetUpGameBoard(ref gameBoard);
            spriteFont = Content.Load<SpriteFont>("Arial");
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            #region commented code for reference
            //EXTINCTION_COUNT_UP += gameTime.ElapsedGameTime.Milliseconds;
            //if (EXTINCTION_COUNT_UP > EXTINCTION_WAIT_TIME)
            //{
            //    List<Genome> currentPopulation = new List<Genome>();
            //    foreach (SmartTank smartTank in SmartTanks)
            //    {
            //        currentPopulation.Add(smartTank.Genome);
            //    }
            //    List<Genome> updatedPopulation = m.GeneticAlgorithm(currentPopulation);
            //    for(int i=0; i < SmartTanks.Count; i++)
            //    {
            //        SmartTanks[i].Genome = updatedPopulation[i];
            //    }
            //    EXTINCTION_COUNT_UP -= EXTINCTION_WAIT_TIME;
            //    generationNumber++;
            //}
            #endregion
            
            mouseState = Mouse.GetState();
            mouseFilter.Update(mouseState, ref currentClickInputs);
            if(currentClickInputs.Count > 0)
            {
                gameBoard.HandleClickInputs(currentClickInputs);
                currentClickInputs.RemoveAt(0);
            }

            keyboardState = Keyboard.GetState();
            pressedKeys = keyFilter.GetPressedKeys(keyboardState);
            gameBoard.ProcessPressedKeys(pressedKeys, keyboardState);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            gameBoard.Draw(spriteBatch);

            #region Outdated code.  Commented for reference.
            //foreach (Mine mine in Mines)
            //{
            //    mine.Draw(spriteBatch);
            //}

            //foreach (SmartTank smartTank in SmartTanks)
            //{
            //    smartTank.Draw(spriteBatch);
            //}

            //spriteBatch.DrawString(spriteFont, String.Format("Generation: {0}", generationNumber), new Vector2((float)(WINDOW_WIDTH * (0f / 6f)), (float)(WINDOW_HEIGHT * (0f / 6f))), Color.White);
            //spriteBatch.DrawString(spriteFont, String.Format("Game Speed: {0}", gameSpeed.Speed), new Vector2((float)(WINDOW_WIDTH * (5f / 6f)), (float)(WINDOW_HEIGHT * (0f / 6f))), Color.White);
            #endregion

            spriteBatch.DrawString(spriteFont, "X: " + mouseState.X + ", Y: " + mouseState.Y, new Vector2((float)(WINDOW_WIDTH * (5f / 6f)), (float)(WINDOW_HEIGHT * (0f / 6f))), Color.White);
            //spriteBatch.DrawString(spriteFont, "Current timer: " + mouseFilter.LeftButtonInfo.currentTimer, new Vector2((float)(WINDOW_WIDTH * (5f / 6f)), (float)(WINDOW_HEIGHT * (1f / 6f))), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        internal void SetUpGameBoard(ref GameBoard _gameBoard)
        {
            gameBoard.ClearBoardOfSpaces();

            gameBoard.AddSpace(new BoardSpace(0, 0, woodHexSprite,selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT,0,0));
            gameBoard.AddSpace(new BoardSpace(0, 0, woodHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 0, 1));
            gameBoard.AddSpace(new BoardSpace(0, 0, woodHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 0, 2));
            gameBoard.AddSpace(new BoardSpace(0, 0, woodHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 0, 3));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 0, 4));
            gameBoard.AddSpace(new BoardSpace(0, 0, waterHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 0, 5));
            gameBoard.AddSpace(new BoardSpace(0, 0, deepWaterHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 0, 6));
            gameBoard.AddSpace(new BoardSpace(0, 0, deepWaterHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 0, 7));
            gameBoard.AddSpace(new BoardSpace(0, 0, deepWaterHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 0, 8));

            gameBoard.AddSpace(new BoardSpace(0, 0, woodHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 1, 0));
            gameBoard.AddSpace(new BoardSpace(0, 0, woodHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 1, 1));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 1, 2));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 1, 3));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 1, 4));
            gameBoard.AddSpace(new BoardSpace(0, 0, waterHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 1, 5));
            gameBoard.AddSpace(new BoardSpace(0, 0, deepWaterHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 1, 6));
            gameBoard.AddSpace(new BoardSpace(0, 0, deepWaterHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 1, 7));

            gameBoard.AddSpace(new BoardSpace(0, 0, woodHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 2, 0));
            gameBoard.AddSpace(new BoardSpace(0, 0, woodHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 2, 1));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 2, 2));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 2, 3));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 2, 4));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 2, 5));
            gameBoard.AddSpace(new BoardSpace(0, 0, waterHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 2, 6));
            gameBoard.AddSpace(new BoardSpace(0, 0, waterHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 2, 7));
            gameBoard.AddSpace(new BoardSpace(0, 0, deepWaterHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 2, 8));

            gameBoard.AddSpace(new BoardSpace(0, 0, woodHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 3, 0));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 3, 1));
            gameBoard.AddSpace(new BoardSpace(0, 0, cityHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 3, 2));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 3, 3));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 3, 4));
            gameBoard.AddSpace(new BoardSpace(0, 0, cityHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 3, 5));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 3, 6));
            gameBoard.AddSpace(new BoardSpace(0, 0, waterHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 3, 7));

            gameBoard.AddSpace(new BoardSpace(0, 0, woodHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 4, 0));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 4, 1));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 4, 2));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 4, 3));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 4, 4));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 4, 5));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 4, 6));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 4, 7));
            gameBoard.AddSpace(new BoardSpace(0, 0, waterHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 4, 8));

            gameBoard.AddSpace(new BoardSpace(0, 0, woodHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 5, 0));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 5, 1));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 5, 2));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 5, 3));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 5, 4));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 5, 5));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 5, 6));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 5, 7));

            gameBoard.AddSpace(new BoardSpace(0, 0, woodHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 6, 0));
            gameBoard.AddSpace(new BoardSpace(0, 0, cityHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 6, 1));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 6, 2));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 6, 3));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 6, 4));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 6, 5));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 6, 6));
            gameBoard.AddSpace(new BoardSpace(0, 0, cityHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 6, 7));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 6, 8));

            gameBoard.AddSpace(new BoardSpace(0, 0, waterHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 7, 0));
            gameBoard.AddSpace(new BoardSpace(0, 0, waterHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 7, 1));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 7, 2));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 7, 3));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 7, 4));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 7, 5));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 7, 6));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 7, 7));

            gameBoard.AddSpace(new BoardSpace(0, 0, desertHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 8, 0));
            gameBoard.AddSpace(new BoardSpace(0, 0, desertHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 8, 1));
            gameBoard.AddSpace(new BoardSpace(0, 0, waterHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 8, 2));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 8, 3));
            gameBoard.AddSpace(new BoardSpace(0, 0, mountainHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 8, 4));
            gameBoard.AddSpace(new BoardSpace(0, 0, mountainHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 8, 5));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 8, 6));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 8, 7));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 8, 8));

            gameBoard.AddSpace(new BoardSpace(0, 0, desertHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 9, 0));
            gameBoard.AddSpace(new BoardSpace(0, 0, waterHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 9, 1));
            gameBoard.AddSpace(new BoardSpace(0, 0, cityHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 9, 2));
            gameBoard.AddSpace(new BoardSpace(0, 0, mountainHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 9, 3));
            gameBoard.AddSpace(new BoardSpace(0, 0, mountainHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 9, 4));
            gameBoard.AddSpace(new BoardSpace(0, 0, cityHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 9, 5));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 9, 6));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 9, 7));

            gameBoard.AddSpace(new BoardSpace(0, 0, desertHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 10, 0));
            gameBoard.AddSpace(new BoardSpace(0, 0, desertHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 10, 1));
            gameBoard.AddSpace(new BoardSpace(0, 0, waterHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 10, 2));
            gameBoard.AddSpace(new BoardSpace(0, 0, waterHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 10, 3));
            gameBoard.AddSpace(new BoardSpace(0, 0, mountainHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 10, 4));
            gameBoard.AddSpace(new BoardSpace(0, 0, mountainHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 10, 5));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 10, 6));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 10, 7));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 10, 8));

            gameBoard.AddSpace(new BoardSpace(0, 0, desertHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 11, 0));
            gameBoard.AddSpace(new BoardSpace(0, 0, desertHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 11, 1));
            gameBoard.AddSpace(new BoardSpace(0, 0, desertHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 11, 2));
            gameBoard.AddSpace(new BoardSpace(0, 0, mountainHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 11, 3));
            gameBoard.AddSpace(new BoardSpace(0, 0, mountainHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 11, 4));
            gameBoard.AddSpace(new BoardSpace(0, 0, mountainHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 11, 5));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 11, 6));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 11, 7));

            gameBoard.AddSpace(new BoardSpace(0, 0, desertHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 12, 0));
            gameBoard.AddSpace(new BoardSpace(0, 0, desertHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 12, 1));
            gameBoard.AddSpace(new BoardSpace(0, 0, desertHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 12, 2));
            gameBoard.AddSpace(new BoardSpace(0, 0, desertHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 12, 3));
            gameBoard.AddSpace(new BoardSpace(0, 0, mountainHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 12, 4));
            gameBoard.AddSpace(new BoardSpace(0, 0, mountainHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 12, 5));
            gameBoard.AddSpace(new BoardSpace(0, 0, mountainHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 12, 6));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 12, 7));
            gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, selectedHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT, 12, 8));

            //gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT), 13, 0);
            //gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT), 13, 1);
            //gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT), 13, 2);
            //gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT), 13, 3);
            //gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT), 13, 4);
            //gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT), 13, 5);
            //gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT), 13, 6);
            //gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT), 13, 7);

            //gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT), 14, 0);
            //gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT), 14, 1);
            //gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT), 14, 2);
            //gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT), 14, 3);
            //gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT), 14, 4);
            //gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT), 14, 5);
            //gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT), 14, 6);
            //gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT), 14, 7);
            //gameBoard.AddSpace(new BoardSpace(0, 0, grassHexSprite, WINDOW_WIDTH, WINDOW_HEIGHT), 14, 8);
        }
    }
}
