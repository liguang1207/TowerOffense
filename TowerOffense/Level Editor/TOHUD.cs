using System;
using System.Linq;
using System.Text;
using TowerOffense.Entities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace TowerOffense.Level_Editor
{
    public class TOHUD
    {
        private Texture2D TitleBar = null;
        private Rectangle TitleBar_Rectangle;

        private Texture2D WaveBar = null;
        private Rectangle WaveBar_Rectangle;

        private SpriteFont LargeFont = null;

        private Texture2D Instructions_LevelSetup = null;

        private Rectangle InformationArea_Rectangle;
        //private Texture2D SelectedActorPanelBackground = null;
        //private Texture2D SelectedActorInformation = null;

        private TOWorldInfo WorldInfo = TOWorldInfo.Instance;

        public TOHUD()
        {
            LargeFont = WorldInfo.GetGame().GetFont("Font_Large");
            int X = 10 * TOTile.MAX_WIDTH;

            TitleBar = WorldInfo.GetGame().GetSprite("HUD_GenericBackground");
            TitleBar_Rectangle = new Rectangle(X, 0, WorldInfo.GetGame().GetScreenWidth() - X, 32);

            WaveBar = WorldInfo.GetGame().GetSprite("HUD_GenericBackground");
            WaveBar_Rectangle = new Rectangle(X, WorldInfo.GetGame().GetScreenHeight() - 32, WorldInfo.GetGame().GetScreenWidth() - X, 32);

            Instructions_LevelSetup = WorldInfo.GetGame().GetSprite("Instructions_LevelSetup");

            InformationArea_Rectangle = new Rectangle(X, 32, WorldInfo.GetGame().GetScreenWidth() - X, WorldInfo.GetGame().GetScreenHeight() - 64);
        }

        public void Draw(GameTime aGameTime)
        {
            SpriteBatch aSpriteBatch = WorldInfo.GetSpriteBatch();

            //Header
            Point TitleBar_Center = TitleBar_Rectangle.Center;
            aSpriteBatch.Draw(TitleBar, TitleBar_Rectangle, Color.White);
            aSpriteBatch.DrawString(LargeFont, "Tower Offense", new Vector2(TitleBar_Center.X, TitleBar_Center.Y), Color.Black, 0, LargeFont.MeasureString("Tower Offense") / 2, 1.0f, SpriteEffects.None, 1f);

            //Wave Bar
            String GameStatus = GetGameStatus();
            Point WaveBar_Center = WaveBar_Rectangle.Center;
            aSpriteBatch.Draw(WaveBar, WaveBar_Rectangle, Color.White);
            aSpriteBatch.DrawString(LargeFont, GameStatus, new Vector2(WaveBar_Center.X, WaveBar_Center.Y), Color.Black, 0, LargeFont.MeasureString(GameStatus) / 2, 1.0f, SpriteEffects.None, 1f);

            //Panels
            GameState G = WorldInfo.GetGame().CurrentGameState;
            if (G == GameState.Game_Started)
            {
                aSpriteBatch.Draw(Instructions_LevelSetup, InformationArea_Rectangle, Color.White);
            }
            else if (G == GameState.Wave_Setup)
            {
                
            }
            else if (G == GameState.Wave_Running)
            {
                
            }
            else if (G == GameState.Wave_Over)
            {
                
            }
            else if (G == GameState.Game_Over)
            {
                
            }
        }

        public String GetGameStatus()
        {
            GameState G = WorldInfo.GetGame().CurrentGameState;

            if (G == GameState.Game_Started)
            {
                return "Game Setup";
            }
            else if (G == GameState.Wave_Setup)
            {
                return "Wave " + WorldInfo.GetGame().Wave + " Setup - " + WorldInfo.GetGame().Timer.TimeLeft;
            }
            else if (G == GameState.Wave_Running)
            {
                return "Wave " + WorldInfo.GetGame().Wave + " Running";
            }
            else if (G == GameState.Wave_Over)
            {
                return "Wave " + WorldInfo.GetGame().Wave + " Over";
            }
            else if (G == GameState.Game_Over)
            {
                return "Game Over";
            }

            return String.Empty;
        }
    }
}
