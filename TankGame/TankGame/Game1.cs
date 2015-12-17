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
using Tanks_Client;

namespace TankGame
{

    public struct PlayerData
    {
        public Vector2 Position;
        public bool IsAlive;
        public Color Color;
        public float Angle;
        public float Power;
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice device;

        Texture2D backgroundTexture;

        int gridWidth;
        int gridHeight;

        Texture2D tank;
        PlayerData[] players;
        int numberOfPlayers = 5;

        //create textures for brick/water/stone/life/coin
        Texture2D brick;
        Texture2D water;
        Texture2D stone;
        Texture2D life;
        Texture2D coin;

        //will update the GUI at the initiation
        private string[,] map;

        //create parser to get msg parsed
        private MsgParser parser;

        //create ClientClass variable
        private ClientClass networkClient;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Armored Warfare";

            //initialize map
            map = new string[Constant.MAP_SIZE, Constant.MAP_SIZE];
            for (int i = 0; i < Constant.MAP_SIZE; i++)
            {
                for (int j = 0; j < Constant.MAP_SIZE; j++)
                    map[i, j] = Constant.EMPTY;
            }

            //instantiate message passer
            parser = new MsgParser();
            //instantiate network client
            networkClient = new ClientClass(parser);


            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            device = graphics.GraphicsDevice;
            backgroundTexture = Content.Load<Texture2D>("background");
            //foregroundTexture = Content.Load<Texture2D>("foreground");
            gridWidth = 500;
            gridHeight = 500;


            SetUpPlayers();

            //load the map content images brick/water/stone/life/coin
            brick = Content.Load<Texture2D>("brick");
            water = Content.Load<Texture2D>("water");
            stone = Content.Load<Texture2D>("stone");
            life = Content.Load<Texture2D>("life");
            coin = Content.Load<Texture2D>("coin");
            tank = Content.Load<Texture2D>("tank");
            networkClient.Sender("JOIN#");


                    
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            map = parser.getMap();
            

            startGame();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            DrawScenery();
            DrawPlayers();
            updateMap();
            spriteBatch.End();
            

            base.Draw(gameTime);
        }

        private void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, gridWidth, gridHeight);
            spriteBatch.Draw(backgroundTexture, screenRectangle, Color.White);

        }

        private void SetUpPlayers()
        {
            Color[] playerColors = new Color[5];
            playerColors[0] = Color.Red;
            playerColors[1] = Color.Green;
            playerColors[2] = Color.Blue;
            playerColors[3] = Color.Purple;
            playerColors[4] = Color.Orange;
  

            players = new PlayerData[numberOfPlayers];
            for (int i = 0; i < numberOfPlayers; i++)
            {
                players[i].IsAlive = false;
                players[i].Color = playerColors[i];
                players[i].Angle = MathHelper.ToRadians(90);
                players[i].Power = 100;
            }


        }

        private void DrawPlayers()
        {

            foreach (PlayerData player in players)
            {
                if (player.IsAlive)
                {
                    spriteBatch.Draw(tank, player.Position, player.Color);
                }
            }
        }

        private void updateMap()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {

                    Vector2 position = new Vector2(i * 50, j * 50);
                    
                    if (map[i, j] == Constant.WATER) spriteBatch.Draw(water, position, Color.White);
                    if (map[i, j] == Constant.STONE) spriteBatch.Draw(stone, position, Color.White);
                    if (map[i, j] == Constant.BRICK) spriteBatch.Draw(brick, position, Color.White);
                    if (map[i, j] == Constant.LIFE) spriteBatch.Draw(life, position, Color.White);
                    if (map[i, j] == Constant.COIN) spriteBatch.Draw(coin, position, Color.White);

                    //for (int k = 0; k < numberOfPlayers; k++)
                    //{
                    //    players[k].IsAlive = false;
                    //}

                    if (map[i, j] == Constant.PLAYER_0) { players[0].Position = position; players[0].IsAlive = true; };
                    if (map[i, j] == Constant.PLAYER_1) { players[1].Position = position; players[1].IsAlive = true; };
                    if (map[i, j] == Constant.PLAYER_2) { players[2].Position = position; players[2].IsAlive = true; };
                    if (map[i, j] == Constant.PLAYER_3) { players[3].Position = position; players[3].IsAlive = true; };
                    if (map[i, j] == Constant.PLAYER_4) { players[4].Position = position; players[4].IsAlive = true; };
 

                }
            }
        }

        private void startGame() {
            KeyboardState keybState = Keyboard.GetState();
            if (keybState.IsKeyDown(Keys.Space))
                networkClient.Sender("JOIN#");
                
        }

        private void updateMoves() {
            
            String nextCommand = "";

            if (nextCommand == Constant.UP) {
                networkClient.Sender("UP#");
            }
            if (nextCommand == Constant.DOWN) {
                networkClient.Sender("DOWN#");
            }
            if (nextCommand == Constant.LEFT) {
                networkClient.Sender("LEFT#");
            }
            if (nextCommand == Constant.RIGHT) {
                networkClient.Sender("RIGHT#");
            }
            if (nextCommand == Constant.SHOOT) {
                networkClient.Sender("SHOOT#");
            }
        }
    }
}
