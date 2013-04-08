using System;
using System.Linq;
using System.Text;
using TowerOffense.Entities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TowerOffense.Level_Editor
{
    class TOMobSpawner
    {

        private int pSpawnsLeft = 0;
        private TOTile pSpawnZone = null;
        private DateTime pLastSpawn;

        public TOMobSpawner(TOTile aSpawnZone)
        {
            pSpawnZone = aSpawnZone;
        }

        public void StartWave(int aSpawnsLeft)
        {
            pSpawnsLeft = aSpawnsLeft;
        }

        public void Update(GameTime aGameTime)
        {
            if (DateTime.Now.Subtract(pLastSpawn).TotalSeconds >= 1 && pSpawnsLeft > 0)
            {
                pSpawnsLeft--;

                //Spawn Item
                TOBug aBug = new TOBug(pSpawnZone.Position, TOWorldInfo.Instance.GetGame().GetSprite("Bug_Generic"));

                //Set Path

                pLastSpawn = DateTime.Now;
            }
        }

        public Boolean DoneSpawning
        {
            get { return pSpawnsLeft == 0; }
        }
    }
}
