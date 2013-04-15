using System;
using System.Linq;
using System.Text;
using TowerOffense.Level_Editor;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace TowerOffense.Entities
{
    public class TOTower : TOActor
    {
        protected String pTag;

        protected int pHealth;
        protected int pFireRate;
        protected int pMaxFireRate;
        protected DateTime pLastFireTime;
        protected int pDamage;
        protected float Max_Range = TOTile.MAX_WIDTH * 4;

        protected List<TOItem> pUpgrades = new List<TOItem>();
        protected List<TOProjectile> pProjectiles = new List<TOProjectile>();

        protected SoundEffect pFireSound = null;

        public TOTower(Vector2 aPosition, Texture2D aTexture)
            : base(aPosition, aTexture)
        {
            pTag = "Standard Tower";
            pHealth = 100;
            pFireRate = 4;
            pMaxFireRate = 4;
            pDamage = 25;

            pLastFireTime = DateTime.Now.AddSeconds(-pFireRate);
        }

        public void ResetProjectiles()
        {
            pProjectiles.Clear();
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

        public override void Update(GameTime aGameTime)
        {
            GetClosestBug();

            if (pTarget != null && !pTarget.bDeleteMe)
            {
                for (int i = pProjectiles.Count - 1; i >= 0; i--)
                {
                    TOActor aActor = pProjectiles[i] as TOActor;
                    if (aActor == null || aActor.bDeleteMe)
                    {
                        WorldInfo.DestroyActor(aActor);
                        pProjectiles.RemoveAt(i);
                    }
                }
                base.Update(aGameTime);

                if (pLastFireTime.AddSeconds(pFireRate) < DateTime.Now && pProjectiles.Count < pMaxFireRate)
                {
                    float TrueRotation = (float)(pRotation - Math.PI/2);
                    Vector2 aStartPoint = new Vector2(CenterPoint.X + (float)Math.Cos(TrueRotation) * TOTile.MAX_WIDTH / 2, CenterPoint.Y + (float)Math.Sin(TrueRotation) * TOTile.MAX_WIDTH / 2);
                    TOProjectile aProjectile = new TOProjectile(aStartPoint, WorldInfo.GetGame().GetSprite("Projectile_Generic"));
                    aProjectile.Rotation = TrueRotation;

                    pProjectiles.Add(aProjectile);
                    pLastFireTime = DateTime.Now;

                    if (pFireSound == null)
                        pFireSound = WorldInfo.GetGame().GetSoundEffect("Tower_Shoot_Generic");

                    pFireSound.Play(1.0f, 0.0f, 0.0f);
                }
            }
        }

        private void GetClosestBug()
        {
            float Smallest_Distance = Max_Range;
            foreach (TOActor aActor in WorldInfo.AllActors)
            {
                if (aActor != null && aActor is TOBug)
                {
                    TOBug aBug = aActor as TOBug;

                    float Distance = Vector2.Distance(CenterPoint, aBug.CenterPoint);

                    if (Distance < Smallest_Distance)
                    {
                        Smallest_Distance = Distance;
                        pTarget = aBug;
                    }
                }
            }
        }
    }
}
