using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace TowerOffense.Entities
{
    public class TOTower : TOActor
    {
        protected String pTag;

        protected int pHealth;
        protected int pFireRate;
        protected int pDamage;
        

        protected List<TOItem> pUpgrades = new List<TOItem>();

        public TOTower(Vector2 aPosition, Texture2D aTexture)
            : base(aPosition, aTexture)
        {
            pTag = "Standard Tower";
            pHealth = 100;
            pFireRate = 1;
            pDamage = 25;
        }

        public void EquipItem(TOItem aItem)
        {
            aItem.OnEquipped(this);
            pUpgrades.Add(aItem);
        }

        public void UnEquipitem(TOItem aItem)
        {
            aItem.OnUnEquipped();
            pUpgrades.Remove(aItem);
            aItem = null;
        }

        public int Health
        {
            get { return pHealth; }
            set { pHealth = value; }
        }

        public int FireRate
        {
            get { return pFireRate; }
            set { pFireRate = value; }
        }

        public int Damage
        {
            get { return pDamage; }
            set { pDamage = value; }
        }
    }
}
