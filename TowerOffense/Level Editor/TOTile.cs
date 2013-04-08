using System;
using System.Linq;
using TowerOffense.Entities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;


namespace TowerOffense.Level_Editor
{
    public enum TowerState
    {
        Open = 1,
        Wall = 2,
        Spawn = 3,
        EndZone = 4,
        Path = 5
    }

    public class TOTile : TOActor
    {
        public Vector2 pCenter;

        public int heu;
        public int cost;
        public int total;

        private Boolean pBlocked = false;
        private Boolean pEndZone = false;
        private Boolean pPath = false;
        private Boolean pClosed = false;
        private Boolean pOpen = true;

        public TowerState CurrentState = TowerState.Open;

        public Boolean Blocked
        {
            get { return pBlocked; }
            set { pBlocked = value; }
        }

        public Boolean EndZone
        {
            get { return pEndZone; }
            set { pEndZone = value; }
        }

        public Boolean Path
        {
            get { return pPath; }
            set { pPath = value; }
        }

        public Boolean Closed
        {
            get { return pClosed; }
            set { pClosed = value; }
        }

        public Boolean Open
        {
            get { return pOpen; }
            set { pOpen = value; }
        }


        public TOTile parentTile;

        private TOActor pTower;

        public TOTile(Vector2 aPosition, Texture2D aTexture)
            : base(aPosition, aTexture)
        {
            pPosition = aPosition;
        }

        public override void Draw(GameTime aGameTime)
        {
            SpriteBatch aSpriteBatch = WorldInfo.GetSpriteBatch();

            if (pTower == null)
            {
                if(CurrentState == TowerState.Open)
                    pTexture = WorldInfo.GetTexture("Tile_Default");
                else if (CurrentState == TowerState.Wall)
                    pTexture = WorldInfo.GetTexture("Tile_Blocked");
                else if (CurrentState == TowerState.Spawn)
                    pTexture = WorldInfo.GetTexture("Tile_Spawn");
                else if (CurrentState == TowerState.EndZone)
                    pTexture = WorldInfo.GetTexture("Tile_EndZone");
                else if (CurrentState == TowerState.Path)
                    pTexture = WorldInfo.GetTexture("Tile_Path");

                aSpriteBatch.Draw(pTexture, BoundingBox, Color.White);
            }
            else if(pTower != null)
            {
                pTower.Draw(aGameTime);
            }
            
        }

        public TOActor Tower
        {
            get { return pTower; }
            set { pTower = value; }
        }
    }
}
