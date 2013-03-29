using System;
using System.Linq;
using System.Collections;
using TowerOffense.Entities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;


namespace TowerOffense
{
    /// <summary>
    /// TOGame - The main game type for TO
    /// </summary>
    public class TOGame : Microsoft.Xna.Framework.Game
    {
        //TO Objects
        TOWorldInfo WorldInfo = TOWorldInfo.Instance;

        GraphicsDeviceManager pGraphicDeviceManager;
        SpriteBatch pSpriteBatch;

        public TOGame()
        {
            pGraphicDeviceManager = new GraphicsDeviceManager(this);
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
            WorldInfo.RegisterGame(this);

            //Show The Mouse
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //Update Start

            WorldInfo.Update(aGameTime);

            //Update End

            base.Update(aGameTime);
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

            pSpriteBatch.End();

            //Drawing End

            base.Draw(aGameTime);
        }

        public SpriteBatch GetSpriteBatch()
        {
            return pSpriteBatch;
        }
    }
}
