using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace TowerOffense.Entities
{
    public class TOItem : TOActor
    {
        protected TOTower pTower = null;
        protected int pHealthModification;
        protected int pFireRateModification;
        protected int pDamageModification;
        protected Boolean pbPickedUp = false;



        public TOItem(Vector2 aPosition, Texture2D aTexture)
            : base(aPosition, aTexture)
        {
            pHealthModification = 0;
            pFireRateModification = 0;
            pDamageModification = 0;
        }

        public void PickUp()
        {
            pbPickedUp = true;
        }

        public void OnEquipped(TOTower aTower)
        {
            pTower = aTower;

            pTower.Health += pHealthModification;
            pTower.FireRate += pFireRateModification;
            pTower.Damage += pDamageModification;
        }

        public void OnUnEquipped()
        {
            pTower.Health -= pHealthModification;
            pTower.FireRate -= pFireRateModification;
            pTower.Damage -= pDamageModification;

            pTower = null;
        }

        public void Draw(GameTime aGameTime)
        {
            if (pTower == null || !pbPickedUp)
                base.Draw(aGameTime);
        }
    }
}
