using System;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;

namespace TowerOffense.Entities
{
    class TOWorldInfo
    {
        private ArrayList pActors = new ArrayList();
        private TOGame pGame = null;

        #region Singleton Instance
        private static TOWorldInfo pInstance;

        private TOWorldInfo()
        {}

        public static TOWorldInfo Instance
        {
            get
            {
                if (pInstance == null)
                    pInstance = new TOWorldInfo();
                return pInstance;
            }
        }


        #endregion

        public void RegisterGame(TOGame aGame)
        {
            pGame = aGame;
        }

        public void RegisterEntity(TOActor aEntity)
        {
            pActors.Add(aEntity);
        }

        public void Update(GameTime aGameTime)
        {
            foreach(TOActor Actor in pActors)
            {
                Actor.Update(aGameTime);
            }
        }

        public void Draw(GameTime aGameTime)
        {
            foreach (TOActor Actor in pActors)
            {
                Actor.Draw(aGameTime);
            }
        }

        public SpriteBatch GetSpriteBatch()
        {
            return pGame.GetSpriteBatch();
        }
    }
}