using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace TowerOffense.Entities
{
    public class TOProjectile : TOActor
    {
        protected int pDamage;
        protected DateTime pBorn;
        protected int pLifeSpan;
        
        public TOProjectile(Vector2 aPosition, Texture2D aTexture)
            : base(aPosition, aTexture)
        {
            pDamage = 28;
            pVelocity = 2.8f;

            pBorn = DateTime.Now;
            pLifeSpan = 16;
        }

        public override void Update(GameTime aGameTime)
        {
            if (OffWorld())
            {
                bDeleteMe = true;
                WorldInfo.AllActors.Remove(this);
            }

            if (pBorn.AddSeconds(pLifeSpan) < DateTime.Now)
            {
                bDeleteMe = true;
                WorldInfo.AllActors.Remove(this);
            }
            else
            {
                //Update Velocity
                pPosition.X += (float)Math.Cos(pRotation) * pVelocity;
                pPosition.Y += (float)Math.Sin(pRotation) * pVelocity;

                TOBug aBug = CheckHitBug();

                if (aBug != null)
                {
                    aBug.Hit(this);
                    WorldInfo.DestroyActor(this);
                }
            }

        }

        

        public int Damage
        {
            get { return pDamage; }
        }

        protected TOBug CheckHitBug()
        {
            foreach (TOActor aActor in WorldInfo.AllActors)
            {
                if (aActor != null && aActor is TOBug)
                {
                    TOBug aBug = aActor as TOBug;

                    if (aBug.BoundingBox.Intersects(BoundingBox))
                        return aBug;
                }
            }

            return null;
        }
    }
}
