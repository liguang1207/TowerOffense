using System;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using TowerOffense.Level_Editor;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;

namespace TowerOffense.Entities
{
    public class TOWorldInfo
    {
        private ArrayList pActors = new ArrayList();
        private TOGame pGame = null;
        private TOTile[,] pGameGrid = new TOTile[10, 10];
        private List<TOTile> pPath = new List<TOTile>();
        private Boolean pDebugMode = true;
        private double pMoney = 100.0;

        private Dictionary<String, Texture2D> Textures = new Dictionary<string, Texture2D>();

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

        public ArrayList AllActors
        {
            get { return pActors; }
        }

        public double Money
        {
            get { return pMoney; }
            set { pMoney = value; }
        }

        public List<TOTile> Path
        {
            get { return pPath; }
        }

        public void GetPath()
        {

        }

        public void DestroyActor(TOActor aActor)
        {
            pActors.Remove(aActor);
        }

        public void RegisterGame(TOGame aGame)
        {
            pGame = aGame;
        }

        public void AddTexture(String aKey, Texture2D aTexture)
        {
            Textures.Add(aKey, aTexture);
        }

        public Texture2D GetTexture(String aKey)
        {
            Texture2D T = null;
            Textures.TryGetValue(aKey, out T);
            return T;
        }


        public void RegisterEntity(TOActor aEntity)
        {
            pActors.Add(aEntity);
        }

        public void Update(GameTime aGameTime)
        {
            for (int i = 0; i < pActors.Count; i++)
            {
                TOActor aActor = pActors[i] as TOActor;

                if (aActor != null)
                {
                    aActor.Update(aGameTime);
                }
                else if (aActor == null || aActor.bDeleteMe)
                {
                    pActors.RemoveAt(i);
                }
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

        public TOGame GetGame()
        {
            return pGame;
        }

        public TOTile[,] GameGrid
        {
            get { return pGameGrid; }
            set { pGameGrid = value; }
        }

        public Boolean isDebugMode
        {
            get { return pDebugMode; }
            set { pDebugMode = value; }
        }
    }
}