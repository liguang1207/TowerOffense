using System;
using System.Linq;
using System.Collections;
using TowerOffense.Entities;
using Microsoft.Xna.Framework;
using TowerOffense.Level_Editor;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;


namespace TowerOffense
{
    public enum GameState
    {
        Game_Started = 1,
        Wave_Setup = 2,
        Wave_Running = 3,
        Wave_Over = 4,
        Game_Over = 5
    }

    /// <summary>
    /// TOGame - The main game type for TO
    /// </summary>
    public class TOGame : Microsoft.Xna.Framework.Game
    {
        //TO Objects
        TOWorldInfo WorldInfo = TOWorldInfo.Instance;

        GraphicsDeviceManager pGraphicDeviceManager;
        SpriteBatch pSpriteBatch;

        public GameState CurrentGameState = GameState.Game_Started;

        public TOHUD HUD = null;
        public TOCountdownTimer Timer = null;

        public int Wave = 1;
        public int SpawnZoneCount = 0;
        public int EndZoneCount = 0;

        public Boolean bCanClick = true;
        public Boolean bCanType = true;

        public TOGame()
        {
            pGraphicDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Go Full Screen
            //this.pGraphicDeviceManager.IsFullScreen = true;

            pGraphicDeviceManager.PreferredBackBufferWidth = 1024;
            pGraphicDeviceManager.PreferredBackBufferHeight = TOActor.MAX_WIDTH * 10;
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
            WorldInfo.RegisterGame(this);

            //Show The Mouse
            this.IsMouseVisible = true;

            //Load Sprites For Tiles
            Texture2D DefaultTile = GetSprite("Tile_Default");
            WorldInfo.AddTexture("Tile_Default", DefaultTile);
            WorldInfo.AddTexture("Tile_Blocked", GetSprite("Tile_Blocked"));
            WorldInfo.AddTexture("Tile_EndZone", GetSprite("Tile_EndZone"));
            WorldInfo.AddTexture("Tile_Spawn", GetSprite("Tile_Spawn"));
            WorldInfo.AddTexture("Tile_Path", GetSprite("Tile_Path"));

            //Initialize Game Grid
            TOTile[,] aGameGrid = WorldInfo.GameGrid;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    aGameGrid[i, j] = new TOTile(new Vector2(TOTile.MAX_WIDTH * i, TOTile.MAX_WIDTH * j), DefaultTile);
                }
            }

            HUD = new TOHUD();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            pSpriteBatch = new SpriteBatch(GraphicsDevice);
            
            // TODO: use this.Content to load your game content here
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
        protected override void Update(GameTime aGameTime)
        {
            // Allows the game to exit
            KeyboardState K = Keyboard.GetState();
            if (K.IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            else if(K.IsKeyDown(Keys.Enter))
            {
                if (bCanType && CurrentGameState == GameState.Game_Started)
                {
                    CurrentGameState = GameState.Wave_Setup;
                    bCanType = false;
                }
                else
                    bCanType = true;
            }

            //Update Start

            MouseState M = Mouse.GetState();
            TOActor T = null;

            if (bCanClick && M.LeftButton == ButtonState.Pressed)
            {
                Vector2 ClickedAt = new Vector2(M.X, M.Y);
                T = GetTOActorNear(ClickedAt);
                bCanClick = false;
            }
            else if (!bCanClick && M.LeftButton == ButtonState.Released)
            {
                bCanClick = true;
            }


            if (CurrentGameState == GameState.Game_Started)
            {
                if (T != null && T is TOTile)
                {
                    TOTile TTile = (TOTile)T;
                    if (M.LeftButton == ButtonState.Pressed)
                    {
                        if (TTile.CurrentState == TowerState.Open)
                            TTile.CurrentState = TowerState.Wall;
                        else if (TTile.CurrentState == TowerState.Wall)
                        {
                            if (SpawnZoneCount < 3)
                            {
                                SpawnZoneCount++;
                                TTile.CurrentState = TowerState.Spawn;
                            }
                            else
                            {
                                if (EndZoneCount < 2)
                                {
                                    EndZoneCount++;
                                    TTile.CurrentState = TowerState.EndZone;
                                }
                                else
                                {
                                    TTile.CurrentState = TowerState.Open;
                                }
                            }
                            
                        }
                        else if (TTile.CurrentState == TowerState.Spawn)
                        {
                            SpawnZoneCount--;
                            if (EndZoneCount < 2)
                            {
                                EndZoneCount++;
                                TTile.CurrentState = TowerState.EndZone;
                            }
                            else
                            {
                                TTile.CurrentState = TowerState.Open;
                            }
                        }

                        else if (TTile.CurrentState == TowerState.EndZone)
                        {
                            EndZoneCount--;
                            TTile.CurrentState = TowerState.Open;
                        }
                            
                    }
                }
            }
            else if (CurrentGameState == GameState.Wave_Setup)
            {
                if (Timer == null)
                {
                    Timer = new TOCountdownTimer(90);
                }
                else if (Timer.Running)
                {
                    Timer.Update(aGameTime);
                }
                else if (Timer.Completed)
                {
                    Timer.Stop();
                    CurrentGameState = GameState.Wave_Running;
                }

                if (T != null && T is TOTile)
                {
                    TOTile TTile = (TOTile)T;
                    if (TTile.CurrentState == TowerState.Wall && TTile.Tower == null && WorldInfo.Money > 10)
                    {
                        TOTower TTower = new TOTower(TTile.Position, GetSprite("Tower_Generic"));
                        TTile.Tower = TTower;

                        WorldInfo.Money -= 10;
                    }
                }
            }
            else if (CurrentGameState == GameState.Wave_Running)
            {
                //Get Path
                //Get
            }
            else
            {
                WorldInfo.Update(aGameTime);
            }

           

            //Update End

            base.Update(aGameTime);
        }

        public TOActor GetTOActorNear(Vector2 aPosition)
        {
            float Min_Distance = 99999;
            TOActor aClosestActor = null;

            foreach (TOActor aActor in WorldInfo.AllActors)
            {
                float Distance = (aPosition - aActor.CenterPoint).Length();
                if (Distance < Min_Distance && Distance < TOActor.MAX_WIDTH)
                {
                    Min_Distance = Distance;
                    aClosestActor = aActor;
                }
            }
            return aClosestActor;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime aGameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Drawing Start

            pSpriteBatch.Begin();

            WorldInfo.Draw(aGameTime);

            HUD.Draw(aGameTime);

            pSpriteBatch.End();

            //Drawing End

            base.Draw(aGameTime);
        }

        public SpriteBatch GetSpriteBatch()
        {
            return pSpriteBatch;
        }

        public Texture2D GetSprite(String TextureName)
        {
            return Content.Load<Texture2D>(TextureName);
        }

        public SpriteFont GetFont(String FontName)
        {
            return Content.Load<SpriteFont>(FontName);
        }

        public int GetScreenWidth()
        {
            return GraphicsDevice.Viewport.Width;
        }

        public int GetScreenHeight()
        {
            return GraphicsDevice.Viewport.Height;
        }
    }
}
